using Paway.Helper;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using System;
namespace Paway.WPF
{
    // 嵌套类型：排序比较器
    public sealed partial class PagedCollectionView
    {
        #region Private Classes

        /// <summary>
        /// Creates a comparer class that takes in a CultureInfo as a parameter,
        /// which it will use when comparing strings.
        /// </summary>
        private class CultureSensitiveComparer : IComparer<object>
        {
            /// <summary>
            /// Creates a comparer which will respect the CultureInfo
            /// that is passed in when comparing strings.
            /// </summary>
            public CultureSensitiveComparer() : base() { }

            #region IComparer<object> Members

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to or greater than the other.
            /// </summary>
            /// <param name="x">first item to compare</param>
            /// <param name="y">second item to compare</param>
            /// <returns>Negative number if x is less than y, zero if equal, and a positive number if x is greater than y</returns>
            /// <remarks>
            /// Compares the 2 items using the specified CultureInfo for string and using the default object comparer for all other objects.
            /// </remarks>
            public int Compare(object x, object y)
            {
                if (x == null)
                {
                    if (y != null)
                    {
                        return -1;
                    }

                    return 0;
                }

                if (y == null)
                {
                    return 1;
                }

                // at this point x and y are not null
                if (x.GetType() == typeof(string) && y.GetType() == typeof(string))
                {
                    return (x as string).Compare(y as string);
                }
                else
                {
                    return Comparer<object>.Default.Compare(x, y);
                }
            }

            #endregion
        }

        /// <summary>
        /// Used to keep track of Defer calls on the PagedCollectionView, which
        /// will prevent the user from calling Refresh() on the view. In order
        /// to allow refreshes again, the user will have to call IDisposable.Dispose,
        /// to end the Defer operation.
        /// </summary>
        private class DeferHelper : IDisposable
        {
            /// <summary>
            /// Private reference to the CollectionView that created this DeferHelper
            /// </summary>
            private PagedCollectionView collectionView;

            /// <summary>
            /// Initializes a new instance of the DeferHelper class
            /// </summary>
            /// <param name="collectionView">CollectionView that created this DeferHelper</param>
            public DeferHelper(PagedCollectionView collectionView)
            {
                this.collectionView = collectionView;
            }

            /// <summary>
            /// Cleanup method called when done using this class
            /// </summary>
            public void Dispose()
            {
                if (collectionView != null)
                {
                    collectionView.EndDefer();
                    collectionView = null;
                }

                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// A simple monitor class to help prevent re-entrant calls
        /// </summary>
        private class SimpleMonitor : IDisposable
        {
            /// <summary>
            /// Whether the monitor is entered
            /// </summary>
            private bool entered;

            /// <summary>
            /// Gets a value indicating whether we have been entered or not
            /// </summary>
            public bool Busy
            {
                get { return entered; }
            }

            /// <summary>
            /// Sets a value indicating that we have been entered
            /// </summary>
            /// <returns>Boolean value indicating whether we were already entered</returns>
            public bool Enter()
            {
                if (entered)
                {
                    return false;
                }

                entered = true;
                return true;
            }

            /// <summary>
            /// Cleanup method called when done using this class
            /// </summary>
            public void Dispose()
            {
                entered = false;
                GC.SuppressFinalize(this);
            }
        }

        /// <summary>
        /// IEnumerator generated using the new item taken into account
        /// </summary>
        private class NewItemAwareEnumerator : IEnumerator
        {
            private enum Position
            {
                /// <summary>
                /// Whether the position is before the new item
                /// </summary>
                BeforeNewItem,

                /// <summary>
                /// Whether the position is on the new item that is being created
                /// </summary>
                OnNewItem,

                /// <summary>
                /// Whether the position is after the new item
                /// </summary>
                AfterNewItem
            }

            /// <summary>
            /// Initializes a new instance of the NewItemAwareEnumerator class.
            /// </summary>
            /// <param name="collectionView">The PagedCollectionView we are creating the enumerator for</param>
            /// <param name="baseEnumerator">The baseEnumerator that we pass in</param>
            /// <param name="newItem">The new item we are adding to the collection</param>
            public NewItemAwareEnumerator(PagedCollectionView collectionView, IEnumerator baseEnumerator, object newItem)
            {
                _collectionView = collectionView;
                _timestamp = collectionView.Timestamp;
                _baseEnumerator = baseEnumerator;
                _newItem = newItem;
            }

            /// <summary>
            /// Implements the MoveNext function for IEnumerable
            /// </summary>
            /// <returns>Whether we can move to the next item</returns>
            public bool MoveNext()
            {
                if (_timestamp != _collectionView.Timestamp)
                {
                    //throw new InvalidOperationException(PagedCollectionViewResources.EnumeratorVersionChanged);
                }

                switch (_position)
                {
                    case Position.BeforeNewItem:
                        if (_baseEnumerator.MoveNext() &&
                                    (_newItem == null || _baseEnumerator.Current != _newItem
                                            || _baseEnumerator.MoveNext()))
                        {
                            // advance base, skipping the new item
                        }
                        else if (_newItem != null)
                        {
                            // if base has reached the end, move to new item
                            _position = Position.OnNewItem;
                        }
                        else
                        {
                            return false;
                        }

                        return true;
                }

                // in all other cases, simply advance base, skipping the new item
                _position = Position.AfterNewItem;
                return _baseEnumerator.MoveNext() &&
                    (_newItem == null
                        || _baseEnumerator.Current != _newItem
                        || _baseEnumerator.MoveNext());
            }

            /// <summary>
            /// Gets the Current value for IEnumerable
            /// </summary>
            public object Current
            {
                get
                {
                    return (_position == Position.OnNewItem) ? _newItem : _baseEnumerator.Current;
                }
            }

            /// <summary>
            /// Implements the Reset function for IEnumerable
            /// </summary>
            public void Reset()
            {
                _position = Position.BeforeNewItem;
                _baseEnumerator.Reset();
            }

            /// <summary>
            /// CollectionView that we are creating the enumerator for
            /// </summary>
            private readonly PagedCollectionView _collectionView;

            /// <summary>
            /// The Base Enumerator that we are passing in
            /// </summary>
            private readonly IEnumerator _baseEnumerator;

            /// <summary>
            /// The position we are appending items to the enumerator
            /// </summary>
            private Position _position;

            /// <summary>
            /// Reference to any new item that we want to add to the collection
            /// </summary>
            private readonly object _newItem;

            /// <summary>
            /// Timestamp to let us know whether there have been updates to the collection
            /// </summary>
            private readonly int _timestamp;
        }

        /// <summary>
        /// IComparer class to sort by class property value (using reflection).
        /// </summary>
        internal class SortFieldComparer : IComparer
        {
            #region Constructors

            internal SortFieldComparer()
            {
            }

            /// <summary>
            /// Create a comparer, using the SortDescription and a Type;
            /// tries to find a reflection PropertyInfo for each property name
            /// </summary>
            /// <param name="collectionView">CollectionView that contains list of property names and direction to sort by</param>
            public SortFieldComparer(ICollectionView collectionView)
            {
                _sortFields = collectionView.SortDescriptions;
                _fields = CreatePropertyInfo(_sortFields);
                _comparer = new CultureSensitiveComparer();
            }

            #endregion

            #region Public Methods

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to or greater than the other.
            /// </summary>
            /// <param name="x">first item to compare</param>
            /// <param name="y">second item to compare</param>
            /// <returns>Negative number if x is less than y, zero if equal, and a positive number if x is greater than y</returns>
            /// <remarks>
            /// Compares the 2 items using the list of property names and directions.
            /// </remarks>
            public int Compare(object x, object y)
            {
                int result = 0;

                // compare both objects by each of the properties until property values don't match
                for (int k = 0; k < _fields.Length; k++)
                {
                    // if the property type is not yet determined, try
                    // obtaining it from the objects
                    Type propertyType = _fields[k].PropertyType;
                    if (propertyType == null)
                    {
                        if (x != null)
                        {
                            _fields[k].PropertyType = x.GetType().GetNestedPropertyType(_fields[k].PropertyPath);
                            propertyType = _fields[k].PropertyType;
                        }

                        if (_fields[k].PropertyType == null && y != null)
                        {
                            _fields[k].PropertyType = y.GetType().GetNestedPropertyType(_fields[k].PropertyPath);
                            propertyType = _fields[k].PropertyType;
                        }
                    }

                    object v1 = _fields[k].GetValue(x);
                    object v2 = _fields[k].GetValue(y);

                    // this will handle the case with string comparisons
                    if (propertyType == typeof(string))
                    {
                        result = _comparer.Compare(v1, v2);
                    }
                    else
                    {
                        // try to also set the value for the comparer if this was
                        // not already calculated
                        IComparer comparer = _fields[k].Comparer;
                        if (propertyType != null && comparer == null)
                        {
                            _fields[k].Comparer = (typeof(Comparer<>).MakeGenericType(propertyType).GetProperty("Default")).GetValue(null, null) as IComparer;
                            comparer = _fields[k].Comparer;
                        }

                        result = (comparer != null) ? comparer.Compare(v1, v2) : 0 /*both values equal*/;
                    }

                    if (_fields[k].Descending)
                    {
                        result = -result;
                    }

                    if (result != 0)
                    {
                        break;
                    }
                }

                return result;
            }

            /// <summary>
            /// Steps through the given list using the comparer to find where
            /// to insert the specified item to maintain sorted order
            /// </summary>
            /// <param name="x">Item to insert into the list</param>
            /// <param name="list">List where we want to insert the item</param>
            /// <returns>Index where we should insert into</returns>
            public int FindInsertIndex(object x, IList list)
            {
                int min = 0;
                int max = list.Count - 1;
                int index;

                // run a binary search to find the right index
                // to insert into.
                while (min <= max)
                {
                    index = (min + max) / 2;

                    int result = Compare(x, list[index]);
                    if (result == 0)
                    {
                        return index;
                    }
                    else if (result > 0)
                    {
                        min = index + 1;
                    }
                    else
                    {
                        max = index - 1;
                    }
                }

                return min;
            }

            #endregion

            #region Private Methods

            private static SortPropertyInfo[] CreatePropertyInfo(SortDescriptionCollection sortFields)
            {
                SortPropertyInfo[] fields = new SortPropertyInfo[sortFields.Count];
                for (int k = 0; k < sortFields.Count; k++)
                {
                    // remember PropertyPath and Direction, used when actually sorting
                    fields[k].PropertyPath = sortFields[k].PropertyName;
                    fields[k].Descending = (sortFields[k].Direction == ListSortDirection.Descending);
                }

                return fields;
            }

            #endregion

            #region Private Fields

            private struct SortPropertyInfo
            {
                internal IComparer Comparer;
                internal bool Descending;
                internal string PropertyPath;
                internal Type PropertyType;

                internal object GetValue(object o)
                {
                    object value;
                    if (string.IsNullOrEmpty(PropertyPath))
                    {
                        value = (PropertyType == o.GetType()) ? o : null;
                    }
                    else
                    {
                        value = InvokePath(o, PropertyPath, PropertyType);
                    }

                    return value;
                }
            }

            private readonly SortPropertyInfo[] _fields;
            private readonly SortDescriptionCollection _sortFields;
            private readonly IComparer<object> _comparer;

            #endregion
        }

        #endregion Private Classes
    
    }
}
