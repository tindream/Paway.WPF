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
    // 属性、私有属性与索引器
    public sealed partial class PagedCollectionView
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the view supports AddNew.
        /// </summary>
        public bool CanAddNew
        {
            get
            {
                return !IsEditingItem &&
                    (SourceList != null && !SourceList.IsFixedSize && CanConstructItem);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the view supports the notion of "pending changes"
        /// on the current edit item.  This may vary, depending on the view and the particular
        /// item.  For example, a view might return true if the current edit item
        /// implements IEditableObject, or if the view has special knowledge about
        /// the item that it can use to support rollback of pending changes.
        /// </summary>
        public bool CanCancelEdit
        {
            get { return _editItem is System.ComponentModel.IEditableObject; }
        }

        /// <summary>
        /// Gets a value indicating whether the PageIndex value is allowed to change or not.
        /// </summary>
        public bool CanChangePage
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether we support filtering with this ICollectionView.
        /// </summary>
        public bool CanFilter
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether this view supports grouping.
        /// When this returns false, the rest of the interface is ignored.
        /// </summary>
        public bool CanGroup
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether the view supports Remove and RemoveAt.
        /// </summary>
        public bool CanRemove
        {
            get
            {
                return !IsEditingItem && !IsAddingNew &&
                    (SourceList != null && !SourceList.IsFixedSize);
            }
        }

        /// <summary>
        /// Gets a value indicating whether we support sorting with this ICollectionView.
        /// </summary>
        public bool CanSort
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the number of records in the view after
        /// filtering, sorting, and paging.
        /// </summary>
        public int Count
        {
            get
            {
                EnsureCollectionInSync();
                VerifyRefreshNotDeferred();

                // if we have paging
                if (PageSize > 0 && PageIndex > -1)
                {
                    if (IsGrouping && !_isUsingTemporaryGroup)
                    {
                        return _group.ItemCount;
                    }
                    else
                    {
                        return Math.Max(0, Math.Min(PageSize, InternalCount - (_pageSize * PageIndex)));
                    }
                }
                else
                {
                    if (IsGrouping)
                    {
                        if (_isUsingTemporaryGroup)
                        {
                            return _temporaryGroup.ItemCount;
                        }
                        else
                        {
                            return _group.ItemCount;
                        }
                    }
                    else
                    {
                        return InternalCount;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets Culture to use during sorting.
        /// </summary>
        public CultureInfo Culture
        {
            get
            {
                return _culture;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (_culture != value)
                {
                    _culture = value;
                    OnPropertyChanged(nameof(Culture));
                }
            }
        }

        /// <summary>
        /// Gets the new item when an AddNew transaction is in progress
        /// Otherwise it returns null.
        /// </summary>
        public object CurrentAddItem
        {
            get
            {
                return _newItem;
            }

            private set
            {
                if (_newItem != value)
                {
                    Debug.Assert(value == null || _newItem == null, "Old and new _newItem values are unexpectedly non null");
                    _newItem = value;
                    OnPropertyChanged(nameof(IsAddingNew));
                    OnPropertyChanged(nameof(CurrentAddItem));
                }
            }
        }

        /// <summary>
        /// Gets the affected item when an EditItem transaction is in progress
        /// Otherwise it returns null.
        /// </summary>
        public object CurrentEditItem
        {
            get
            {
                return _editItem;
            }

            private set
            {
                if (_editItem != value)
                {
                    Debug.Assert(value == null || _editItem == null, "Old and new _editItem values are unexpectedly non null");
                    bool oldCanCancelEdit = CanCancelEdit;
                    _editItem = value;
                    OnPropertyChanged(nameof(IsEditingItem));
                    OnPropertyChanged(nameof(CurrentEditItem));
                    if (oldCanCancelEdit != CanCancelEdit)
                    {
                        OnPropertyChanged(nameof(CanCancelEdit));
                    }
                }
            }
        }

        /// <summary>
        /// Gets the "current item" for this view
        /// </summary>
        public object CurrentItem
        {
            get
            {
                VerifyRefreshNotDeferred();
                return _currentItem;
            }
        }

        /// <summary>
        /// Gets the ordinal position of the CurrentItem within the
        /// (optionally sorted and filtered) view.
        /// </summary>
        public int CurrentPosition
        {
            get
            {
                VerifyRefreshNotDeferred();
                return _currentPosition;
            }
        }

        /// <summary>
        /// Gets or sets the Filter, which is a callback set by the consumer of the ICollectionView
        /// and used by the implementation of the ICollectionView to determine if an
        /// item is suitable for inclusion in the view.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// Simpler implementations do not support filtering and will throw a NotSupportedException.
        /// Use <seealso cref="CanFilter"/> property to test if filtering is supported before
        /// assigning a non-null value.
        /// </exception>
        public Predicate<object> Filter
        {
            get
            {
                return _filter;
            }

            set
            {
                if (IsAddingNew || IsEditingItem)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringAddOrEdit, "Filter"));
                }

                if (!CanFilter)
                {
                    throw new NotSupportedException(CannotFilter);
                }

                if (_filter != value)
                {
                    _filter = value;
                    RefreshOrDefer();
                    OnPropertyChanged(nameof(Filter));
                }
            }
        }

        /// <summary>
        /// Gets the description of grouping, indexed by level.
        /// </summary>
        public ObservableCollection<GroupDescription> GroupDescriptions
        {
            get
            {
                return _group?.GroupDescriptions;
            }
        }

        /// <summary>
        /// Gets the top-level groups, constructed according to the descriptions
        /// given in GroupDescriptions.
        /// </summary>
        public ReadOnlyObservableCollection<object> Groups
        {
            get
            {
                if (!IsGrouping)
                {
                    return null;
                }

                CollectionViewGroupRoot group = RootGroup;
                return group?.Items;
            }
        }

        /// <summary>
        /// Gets a value indicating whether an "AddNew" transaction is in progress.
        /// </summary>
        public bool IsAddingNew
        {
            get { return _newItem != null; }
        }

        /// <summary>
        /// Gets a value indicating whether currency is beyond the end (End-Of-File).
        /// </summary>
        /// <returns>Whether IsCurrentAfterLast</returns>
        public bool IsCurrentAfterLast
        {
            get
            {
                VerifyRefreshNotDeferred();
                return CheckFlag(CollectionViewFlags.IsCurrentAfterLast);
            }
        }

        /// <summary>
        /// Gets a value indicating whether currency is before the beginning (Beginning-Of-File).
        /// </summary>
        /// <returns>Whether IsCurrentBeforeFirst</returns>
        public bool IsCurrentBeforeFirst
        {
            get
            {
                VerifyRefreshNotDeferred();
                return CheckFlag(CollectionViewFlags.IsCurrentBeforeFirst);
            }
        }

        /// <summary>
        /// Gets a value indicating whether an EditItem transaction is in progress.
        /// </summary>
        public bool IsEditingItem
        {
            get { return _editItem != null; }
        }

        /// <summary>
        /// Gets a value indicating whether the resulting (filtered) view is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                EnsureCollectionInSync();
                return InternalCount == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether a page index change is in process or not.
        /// </summary>
        public bool IsPageChanging
        {
            get
            {
                return CheckFlag(CollectionViewFlags.IsPageChanging);
            }

            private set
            {
                if (CheckFlag(CollectionViewFlags.IsPageChanging) != value)
                {
                    SetFlag(CollectionViewFlags.IsPageChanging, value);
                    OnPropertyChanged(nameof(IsPageChanging));
                }
            }
        }

        /// <summary>
        /// Gets the minimum number of items known to be in the source collection
        /// that verify the current filter if any
        /// </summary>
        public int ItemCount
        {
            get
            {
                return InternalList.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this view needs to be refreshed.
        /// </summary>
        public bool NeedsRefresh
        {
            get { return CheckFlag(CollectionViewFlags.NeedsRefresh); }
        }

        /// <summary>
        /// Gets or sets whether to include a placeholder for a new item, and if so,
        /// where to put it. Only the value NewItemPlaceholderPosition.None is supported.
        /// </summary>
        public NewItemPlaceholderPosition NewItemPlaceholderPosition
        {
            get
            {
                return NewItemPlaceholderPosition.None;
            }
            set
            {
                if (value != NewItemPlaceholderPosition.None)
                {
                    //throw new ArgumentException(
                    //    string.Format(CultureInfo.InvariantCulture,
                    //        InvalidEnumArgument,
                    //        "value",
                    //        value.ToString(),
                    //        typeof(NewItemPlaceholderPosition).Name));
                }
            }
        }

        /// <summary>
        /// Gets the current page we are on. (zero based)
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }
        }

        /// <summary>
        /// Gets or sets the number of items to display on a page. If the
        /// PageSize = 0, then we are not paging, and will display all items
        /// in the collection. Otherwise, we will have separate pages for
        /// the items to display.
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                if (value < 0)
                {
                    //原来把消息文本当成 paramName 传入，异常信息里参数名是整句英文，排查时误导
                    throw new ArgumentOutOfRangeException(nameof(value), InvalidPageSize);
                }

                // if the Refresh is currently deferred, cache the desired PageSize
                // and set the flag so that once the defer is over, we can then
                // update the PageSize.
                if (IsRefreshDeferred)
                {
                    // set cached value and flag so that we update the PageSize on EndDefer
                    _cachedPageSize = value;
                    SetFlag(CollectionViewFlags.IsUpdatePageSizeDeferred, true);
                    return;
                }

                // to see whether or not to fire an OnPropertyChanged
                int oldCount = Count;

                if (_pageSize != value)
                {
                    // Remember current currency values for upcoming OnPropertyChanged notifications
                    object oldCurrentItem = CurrentItem;
                    int oldCurrentPosition = CurrentPosition;
                    bool oldIsCurrentAfterLast = IsCurrentAfterLast;
                    bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

                    // Check if there is a current edited or new item so changes can be committed first.
                    if (CurrentAddItem != null || CurrentEditItem != null)
                    {
                        // Check with the ICollectionView.CurrentChanging listeners if it's OK to
                        // change the currency. If not, then we can't fire the event to allow them to
                        // commit their changes. So, we will not be able to change the PageSize.
                        if (!OkToChangeCurrent())
                        {
                            throw new InvalidOperationException(ChangingPageSizeNotAllowedDuringAddOrEdit);
                        }

                        // Currently CommitNew()/CommitEdit()/CancelNew()/CancelEdit() can't handle committing or
                        // cancelling an item that is no longer on the current page. That's acceptable and means that
                        // the potential this._newItem or this._editItem needs to be committed before this PageSize change.
                        // The reason why we temporarily reset currency here is to give a chance to the bound
                        // controls to commit or cancel their potential edits/addition. The DataForm calls ForceEndEdit()
                        // for example as a result of changing currency.
                        SetCurrentToPosition(-1);
                        RaiseCurrencyChanges(true /*fireChangedEvent*/, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);

                        // If the bound controls did not successfully end their potential item editing/addition, we
                        // need to throw an exception to show that the PageSize change failed.
                        if (CurrentAddItem != null || CurrentEditItem != null)
                        {
                            throw new InvalidOperationException(ChangingPageSizeNotAllowedDuringAddOrEdit);
                        }
                    }

                    _pageSize = value;
                    OnPropertyChanged(nameof(PageSize));

                    if (_pageSize == 0)
                    {
                        // update the groups for the current page
                        PrepareGroups();

                        // if we are not paging
                        MoveToPage(-1);
                    }
                    else if (_pageIndex != 0)
                    {
                        if (!CheckFlag(CollectionViewFlags.IsMoveToPageDeferred))
                        {
                            // if the temporaryGroup was not created yet and is out of sync
                            // then create it so that we can use it as a refernce while paging.
                            if (IsGrouping && _temporaryGroup.ItemCount != InternalList.Count)
                            {
                                PrepareTemporaryGroups();
                            }

                            MoveToFirstPage();
                        }
                    }
                    else if (IsGrouping)
                    {
                        // if the temporaryGroup was not created yet and is out of sync
                        // then create it so that we can use it as a refernce while paging.
                        if (_temporaryGroup.ItemCount != InternalList.Count)
                        {
                            // update the groups that get created for the
                            // entire collection as well as the current page
                            PrepareTemporaryGroups();
                        }

                        // update the groups for the current page
                        PrepareGroupsForCurrentPage();
                    }

                    // if the count has changed
                    if (Count != oldCount)
                    {
                        OnPropertyChanged(nameof(Count));
                    }

                    // reset currency values
                    ResetCurrencyValues(oldCurrentItem, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);

                    // send a notification that our collection has been updated
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Reset));

                    // now raise currency changes at the end
                    RaiseCurrencyChanges(false, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);
                }
            }
        }

        /// <summary>
        /// Gets the Sort criteria to sort items in collection.
        /// </summary>
        /// <remarks>
        /// <p>
        /// Clear a sort criteria by assigning SortDescription.Empty to this property.
        /// One or more sort criteria in form of <seealso cref="SortDescription"/>
        /// can be used, each specifying a property and direction to sort by.
        /// </p>
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// Simpler implementations do not support sorting and will throw a NotSupportedException.
        /// Use <seealso cref="CanSort"/> property to test if sorting is supported before adding
        /// to SortDescriptions.
        /// </exception>
        public SortDescriptionCollection SortDescriptions
        {
            get
            {
                if (_sortDescriptions == null)
                {
                    SetSortDescriptions(new SortDescriptionCollection());
                }

                return _sortDescriptions;
            }
        }

        /// <summary>
        /// Gets the source of the IEnumerable collection we are using for our view.
        /// </summary>
        public IEnumerable SourceCollection
        {
            get { return _sourceCollection; }
        }

        /// <summary>
        /// Gets the total number of items in the view before paging is applied.
        /// </summary>
        public int TotalItemCount
        {
            get
            {
                return InternalList.Count;
            }
        }

        #endregion Public Properties

        ////------------------------------------------------------
        ////
        ////  Private Properties
        ////
        ////------------------------------------------------------

        #region Private Properties

        /// <summary>
        /// Gets a value indicating whether we have a valid ItemConstructor of the correct type
        /// </summary>
        private bool CanConstructItem
        {
            get
            {
                if (!_itemConstructorIsValid)
                {
                    EnsureItemConstructor();
                }

                return _itemConstructor != null;
            }
        }

        /// <summary>
        /// Gets the private count without taking paging or
        /// placeholders into account
        /// </summary>
        private int InternalCount
        {
            get { return InternalList.Count; }
        }

        /// <summary>
        /// Gets the InternalList
        /// </summary>
        private IList InternalList
        {
            get { return _internalList; }
        }

        /// <summary>
        /// Gets a value indicating whether CurrentItem and CurrentPosition are
        /// up-to-date with the state and content of the collection.
        /// </summary>
        private bool IsCurrentInSync
        {
            get
            {
                if (IsCurrentInView)
                {
                    return GetItemAt(CurrentPosition).Equals(CurrentItem);
                }
                else
                {
                    return CurrentItem == null;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current item is in the view
        /// </summary>
        private bool IsCurrentInView
        {
            get
            {
                VerifyRefreshNotDeferred();

                // Calling IndexOf will check whether the specified currentItem
                // is within the (paged) view.
                return IndexOf(CurrentItem) >= 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not we have grouping
        /// taking place in this collection.
        /// </summary>
        private bool IsGrouping
        {
            get { return _isGrouping; }
        }

        /// <summary>
        /// Gets a value indicating whether there
        /// is still an outstanding DeferRefresh in
        /// use.  If at all possible, derived classes
        /// should not call Refresh if IsRefreshDeferred
        /// is true.
        /// </summary>
        private bool IsRefreshDeferred
        {
            get { return _deferLevel > 0; }
        }

        /// <summary>
        /// Gets whether the current page is empty and we need
        /// to move to a previous page.
        /// </summary>
        private bool NeedToMoveToPreviousPage
        {
            get { return (PageSize > 0 && Count == 0 && PageIndex != 0 && PageCount == PageIndex); }
        }

        /// <summary>
        /// Gets a value indicating whether we are on the last local page
        /// </summary>
        private bool OnLastLocalPage
        {
            get
            {
                if (PageSize == 0)
                {
                    return false;
                }

                Debug.Assert(PageCount > 0, "Unexpected PageCount <= 0");

                // if we have no items (PageCount==1) or there is just one page
                if (PageCount == 1)
                {
                    return true;
                }

                return (PageIndex == PageCount - 1);
            }
        }

        /// <summary>
        /// Gets the number of pages we currently have
        /// </summary>
        private int PageCount
        {
            get { return (_pageSize > 0) ? Math.Max(1, (int)Math.Ceiling((double)ItemCount / _pageSize)) : 0; }
        }

        /// <summary>
        /// Gets the root of the Group that we expose to the user
        /// </summary>
        private CollectionViewGroupRoot RootGroup
        {
            get
            {
                return _isUsingTemporaryGroup ? _temporaryGroup : _group;
            }
        }

        /// <summary>
        /// Gets the SourceCollection as an IList
        /// </summary>
        private IList SourceList
        {
            get { return SourceCollection as IList; }
        }

        /// <summary>
        /// Gets Timestamp used by the NewItemAwareEnumerator to determine if a
        /// collection change has occurred since the enumerator began.  (If so,
        /// MoveNext should throw.)
        /// </summary>
        private int Timestamp
        {
            get { return _timestamp; }
        }

        /// <summary>
        /// Gets a value indicating whether a private copy of the data
        /// is needed for sorting, filtering, and paging. We want any deriving
        /// classes to also be able to access this value to see whether or not
        /// to use the default source collection, or the internal list.
        /// </summary>
        private bool UsesLocalArray
        {
            get { return SortDescriptions.Count > 0 || Filter != null || _pageSize > 0 || GroupDescriptions.Count > 0; }
        }

        #endregion Private Properties

        ////------------------------------------------------------
        ////
        ////  Indexers
        ////
        ////------------------------------------------------------

        #region Indexers

        /// <summary>
        /// Return the item at the specified index
        /// </summary>
        /// <param name="index">Index of the item we want to retrieve</param>
        /// <returns>The item at the specified index</returns>
        public object this[int index]
        {
            get { return GetItemAt(index); }
        }

        #endregion Indexers

        ////------------------------------------------------------
        ////
        ////  Public Methods
        ////
        ////------------------------------------------------------
    }
}
