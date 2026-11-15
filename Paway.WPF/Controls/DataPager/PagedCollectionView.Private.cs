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
    // 私有实现方法
    public sealed partial class PagedCollectionView
    {
        #region Private Methods

        /// <summary>
        /// Fix up CurrentPosition and CurrentItem after a collection change
        /// </summary>
        /// <param name="newCurrentItem">Item that we want to set currency to</param>
        /// <param name="index">Index of item involved in the collection change</param>
        private void AdjustCurrencyForAdd(object newCurrentItem, int index)
        {
            if (newCurrentItem != null)
            {
                int newItemIndex = IndexOf(newCurrentItem);

                // if we already have the correct currency set, we don't
                // want to unnecessarily fire events
                if (newItemIndex >= 0 && (newItemIndex != CurrentPosition || !IsCurrentInSync))
                {
                    OnCurrentChanging();
                    SetCurrent(newCurrentItem, newItemIndex);
                }

                return;
            }

            if (Count == 1)
            {
                if (CurrentItem != null || CurrentPosition != -1)
                {
                    // fire current changing notification
                    OnCurrentChanging();
                }

                // added first item; set current at BeforeFirst
                SetCurrent(null, -1);
            }
            else if (index <= CurrentPosition)
            {
                // fire current changing notification
                OnCurrentChanging();

                // adjust current index if insertion is earlier
                int newPosition = CurrentPosition + 1;
                if (newPosition >= Count)
                {
                    // if currency was on last item and it got shifted up,
                    // keep currency on last item.
                    newPosition = Count - 1;
                }

                SetCurrent(GetItemAt(newPosition), newPosition);
            }
        }

        /// <summary>
        /// Fix up CurrentPosition and CurrentItem after a collection change
        /// </summary>
        /// <param name="newCurrentItem">Item that we want to set currency to</param>
        /// <param name="index">Index of item involved in the collection change</param>
        private void AdjustCurrencyForEdit(object newCurrentItem, int index)
        {
            if (newCurrentItem != null && IndexOf(newCurrentItem) >= 0)
            {
                OnCurrentChanging();
                SetCurrent(newCurrentItem, IndexOf(newCurrentItem));
                return;
            }

            if (index <= CurrentPosition)
            {
                // fire current changing notification
                OnCurrentChanging();

                // adjust current index if insertion is earlier
                int newPosition = CurrentPosition + 1;
                if (newPosition < Count)
                {
                    // CurrentItem might be out of sync if underlying list is not INCC
                    // or if this Add is the result of a Replace (Rem + Add)
                    SetCurrent(GetItemAt(newPosition), newPosition);
                }
                else
                {
                    SetCurrent(null, Count);
                }
            }
        }

        /// <summary>
        /// Fix up CurrentPosition and CurrentItem after a collection change
        /// The index can be -1 if the item was removed from a previous page
        /// </summary>
        /// <param name="index">Index of item involved in the collection change</param>
        private void AdjustCurrencyForRemove(int index)
        {
            if (index < 0) return;
            // adjust current index if deletion is earlier
            if (index < CurrentPosition)
            {
                // fire current changing notification
                OnCurrentChanging();

                SetCurrent(CurrentItem, CurrentPosition - 1);
            }

            // adjust current index if > Count
            if (CurrentPosition >= Count)
            {
                // fire current changing notification
                OnCurrentChanging();

                SetCurrentToPosition(Count - 1);
            }

            // make sure that current position and item are in sync
            if (!IsCurrentInSync)
            {
                // fire current changing notification
                OnCurrentChanging();

                SetCurrentToPosition(CurrentPosition);
            }
        }

        /// <summary>
        /// Returns true if specified flag in flags is set.
        /// </summary>
        /// <param name="flags">Flag we are checking for</param>
        /// <returns>Whether the specified flag is set</returns>
        private bool CheckFlag(CollectionViewFlags flags)
        {
            return (_flags & flags) != 0;
        }

        /// <summary>
        /// Called to complete the page move operation to set the
        /// current page index.
        /// </summary>
        /// <param name="pageIndex">Final page index</param>
        private void CompletePageMove(int pageIndex)
        {
            Debug.Assert(_pageIndex != pageIndex, "Unexpected this._pageIndex == pageIndex");

            // to see whether or not to fire an OnPropertyChanged
            int oldCount = Count;
            object oldCurrentItem = CurrentItem;
            int oldCurrentPosition = CurrentPosition;
            bool oldIsCurrentAfterLast = IsCurrentAfterLast;
            bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

            _pageIndex = pageIndex;

            // update the groups
            if (IsGrouping && PageSize > 0)
            {
                PrepareGroupsForCurrentPage();
            }

            // update currency
            if (Count >= 1)
            {
                SetCurrent(GetItemAt(0), 0);
            }
            else
            {
                SetCurrent(null, -1);
            }

            IsPageChanging = false;
            OnPropertyChanged(nameof(PageIndex));
            RaisePageChanged();

            // if the count has changed
            if (Count != oldCount)
            {
                OnPropertyChanged(nameof(Count));
            }

            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset));

            // Always raise CurrentChanged since the calling method MoveToPage(pageIndex) raised CurrentChanging.
            RaiseCurrencyChanges(true /*fireChangedEvent*/, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);
        }

        /// <summary>
        /// Convert a value for the index passed in to the index it would be
        /// relative to the InternalIndex property.
        /// </summary>
        /// <param name="index">Index to convert</param>
        /// <returns>Value for the InternalIndex</returns>
        private int ConvertToInternalIndex(int index)
        {
            Debug.Assert(index > -1, "Unexpected index == -1");
            if (PageSize > 0)
            {
                return (_pageSize * PageIndex) + index;
            }
            else
            {
                return index;
            }
        }

        /// <summary>
        /// Copy all items from the source collection to the internal list for processing.
        /// </summary>
        private void CopySourceToInternalList()
        {
            _internalList = new List<object>();

            IEnumerator enumerator = SourceCollection.GetEnumerator();

            while (enumerator.MoveNext())
            {
                _internalList.Add(enumerator.Current);
            }
        }

        /// <summary>
        /// Common functionality used by CommitNew, CancelNew, and when the
        /// new item is removed by Remove or Refresh.
        /// </summary>
        /// <param name="cancel">Whether we canceled the add</param>
        /// <returns>The new item we ended adding</returns>
        private object EndAddNew(bool cancel)
        {
            object newItem = CurrentAddItem;

            CurrentAddItem = null;    // leave "adding-new" mode

            if (newItem is System.ComponentModel.IEditableObject ieo)
            {
                if (cancel)
                {
                    ieo.CancelEdit();
                }
                else
                {
                    ieo.EndEdit();
                }
            }

            return newItem;
        }

        /// <summary>
        /// Subtracts from the deferLevel counter and calls Refresh() if there are no other defers
        /// </summary>
        private void EndDefer()
        {
            --_deferLevel;

            if (_deferLevel == 0)
            {
                if (CheckFlag(CollectionViewFlags.IsUpdatePageSizeDeferred))
                {
                    SetFlag(CollectionViewFlags.IsUpdatePageSizeDeferred, false);
                    PageSize = _cachedPageSize;
                }

                if (CheckFlag(CollectionViewFlags.IsMoveToPageDeferred))
                {
                    SetFlag(CollectionViewFlags.IsMoveToPageDeferred, false);
                    MoveToPage(_cachedPageIndex);
                    _cachedPageIndex = -1;
                }

                if (CheckFlag(CollectionViewFlags.NeedsRefresh))
                {
                    Refresh();
                }
            }
        }

        /// <summary>
        /// Makes sure that the ItemConstructor is set for the correct type
        /// </summary>
        private void EnsureItemConstructor()
        {
            if (!_itemConstructorIsValid)
            {
                Type itemType = GetItemType(true);
                if (itemType != null)
                {
                    _itemConstructor = itemType.GetConstructor(Type.EmptyTypes);
                    _itemConstructorIsValid = true;
                }
            }
        }

        /// <summary>
        ///  If the IEnumerable has changed, bring the collection up to date.
        ///  (This isn't necessary if the IEnumerable is also INotifyCollectionChanged
        ///  because we keep the collection in sync incrementally.)
        /// </summary>
        private void EnsureCollectionInSync()
        {
            // if the IEnumerable is not a INotifyCollectionChanged
            if (_pollForChanges)
            {
                try
                {
                    _trackingEnumerator.MoveNext();
                }
                catch (InvalidOperationException)
                {
                    // When the collection has been modified, calling MoveNext()
                    // on the enumerator throws an InvalidOperationException, stating
                    // that the collection has been modified. Therefore, we know when
                    // to update our internal collection.
                    _trackingEnumerator = SourceCollection.GetEnumerator();
                    RefreshOrDefer();
                }
            }
        }

        /// <summary>
        /// Helper function used to determine the type of an item
        /// </summary>
        /// <param name="useRepresentativeItem">Whether we should use a representative item</param>
        /// <returns>The type of the items in the collection</returns>
        private Type GetItemType(bool useRepresentativeItem)
        {
            Type collectionType = SourceCollection.GetType();
            Type[] interfaces = collectionType.GetInterfaces();

            // Look for IEnumerable<T>.  All generic collections should implement
            // this.  We loop through the interface list, rather than call
            // GetInterface(IEnumerableT), so that we handle an ambiguous match
            // (by using the first match) without an exception.
            for (int i = 0; i < interfaces.Length; i++)
            {
                Type interfaceType = interfaces[i];
                if (interfaceType.Name == typeof(IEnumerable<>).Name)
                {
                    // found IEnumerable<>, extract T
                    Type[] typeParameters = interfaceType.GetGenericArguments();
                    if (typeParameters.Length == 1)
                    {
                        return typeParameters[0];
                    }
                }
            }

            // No generic information found.  Use a representative item instead.
            if (useRepresentativeItem)
            {
                // get type of a representative item
                object item = GetRepresentativeItem();
                if (item != null)
                {
                    return item.GetType();
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a representative item from the collection
        /// </summary>
        /// <returns>An item that can represent the collection</returns>
        private object GetRepresentativeItem()
        {
            if (IsEmpty)
            {
                return null;
            }

            IEnumerator enumerator = GetEnumerator();
            while (enumerator.MoveNext())
            {
                object item = enumerator.Current;

                // Since this collection view does not support a NewItemPlaceholder,
                // simply return the first non-null item.
                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// Return index of item in the internal list.
        /// </summary>
        /// <param name="item">The item we are checking</param>
        /// <returns>Integer value on where in the InternalList the object is located</returns>
        private int InternalIndexOf(object item)
        {
            return InternalList.IndexOf(item);
        }

        /// <summary>
        /// Return item at the given index in the internal list.
        /// </summary>
        /// <param name="index">The index we are checking</param>
        /// <returns>The item at the specified index</returns>
        private object InternalItemAt(int index)
        {
            if (index >= 0 && index < InternalList.Count)
            {
                return InternalList[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Ask listeners (via ICollectionView.CurrentChanging event) if it's OK to change currency
        /// </summary>
        /// <returns>False if a listener cancels the change, True otherwise</returns>
        private bool OkToChangeCurrent()
        {
            CurrentChangingEventArgs args = new CurrentChangingEventArgs();
            OnCurrentChanging(args);
            return !args.Cancel;
        }

        /// <summary>
        ///     Notify listeners that this View has changed
        /// </summary>
        /// <remarks>
        ///     CollectionViews (and sub-classes) should take their filter/sort/grouping/paging
        ///     into account before calling this method to forward CollectionChanged events.
        /// </remarks>
        /// <param name="args">
        ///     The NotifyCollectionChangedEventArgs to be passed to the EventHandler
        /// </param>
        private void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            unchecked
            {
                // invalidate enumerators because of a change
                ++_timestamp;
            }

            if (CollectionChanged != null)
            {
                if (args.Action != NotifyCollectionChangedAction.Add || PageSize == 0 || args.NewStartingIndex < Count)
                {
                    CollectionChanged(this, args);
                }
            }

            // Collection changes change the count unless an item is being
            // replaced within the collection.
            if (args.Action != NotifyCollectionChangedAction.Replace)
            {
                OnPropertyChanged(nameof(Count));
            }

            bool listIsEmpty = IsEmpty;
            if (listIsEmpty != CheckFlag(CollectionViewFlags.CachedIsEmpty))
            {
                SetFlag(CollectionViewFlags.CachedIsEmpty, listIsEmpty);
                OnPropertyChanged(nameof(IsEmpty));
            }
        }

        /// <summary>
        /// Raises the CurrentChanged event
        /// </summary>
        private void OnCurrentChanged()
        {
            if (CurrentChanged != null && _currentChangedMonitor.Enter())
            {
                using (_currentChangedMonitor)
                {
                    CurrentChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Raise a CurrentChanging event that is not cancelable.
        /// This is called by CollectionChanges (Add, Remove, and Refresh) that
        /// affect the CurrentItem.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// This CurrentChanging event cannot be canceled.
        /// </exception>
        private void OnCurrentChanging()
        {
            OnCurrentChanging(uncancelableCurrentChangingEventArgs);
        }

        /// <summary>
        /// Raises the CurrentChanging event
        /// </summary>
        /// <param name="args">
        ///     CancelEventArgs used by the consumer of the event.  args.Cancel will
        ///     be true after this call if the CurrentItem should not be changed for
        ///     any reason.
        /// </param>
        /// <exception cref="InvalidOperationException">
        ///     This CurrentChanging event cannot be canceled.
        /// </exception>
        private void OnCurrentChanging(CurrentChangingEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (_currentChangedMonitor.Busy)
            {
                if (args.IsCancelable)
                {
                    args.Cancel = true;
                }

                return;
            }

            CurrentChanging?.Invoke(this, args);
        }

        /// <summary>
        /// GroupBy changed handler
        /// </summary>
        /// <param name="sender">CollectionViewGroup whose GroupBy has changed</param>
        /// <param name="e">Arguments for the NotifyCollectionChanged event</param>
        private void OnGroupByChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsAddingNew || IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringAddOrEdit, "Grouping"));
            }

            RefreshOrDefer();
        }

        /// <summary>
        /// GroupDescription changed handler
        /// </summary>
        /// <param name="sender">CollectionViewGroup whose GroupDescription has changed</param>
        /// <param name="e">Arguments for the GroupDescriptionChanged event</param>
        private void OnGroupDescriptionChanged(object sender, EventArgs e)
        {
            if (IsAddingNew || IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringAddOrEdit, "Grouping"));
            }

            // we want to make sure that the data is refreshed before we try to move to a page
            // since the refresh would take care of the filtering, sorting, and grouping.
            RefreshOrDefer();

            if (PageSize > 0)
            {
                if (IsRefreshDeferred)
                {
                    // set cached value and flag so that we move to first page on EndDefer
                    _cachedPageIndex = 0;
                    SetFlag(CollectionViewFlags.IsMoveToPageDeferred, true);
                }
                else
                {
                    MoveToFirstPage();
                }
            }
        }

        /// <summary>
        /// Raises a PropertyChanged event.
        /// </summary>
        /// <param name="e">PropertyChangedEventArgs for this change</param>
        private void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Helper to raise a PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Property name for the property that changed</param>
        private void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets up the ActiveComparer for the CollectionViewGroupRoot specified
        /// </summary>
        /// <param name="groupRoot">The CollectionViewGroupRoot</param>
        private void PrepareGroupingComparer(CollectionViewGroupRoot groupRoot)
        {
            if (groupRoot == _temporaryGroup || PageSize == 0)
            {
                if (groupRoot.ActiveComparer is CollectionViewGroupInternal.ListComparer listComparer)
                {
                    listComparer.ResetList(InternalList);
                }
                else
                {
                    groupRoot.ActiveComparer = new CollectionViewGroupInternal.ListComparer(InternalList);
                }
            }
            else if (groupRoot == _group)
            {
                // create the new comparer based on the current _temporaryGroup
                groupRoot.ActiveComparer = new CollectionViewGroupInternal.CollectionViewGroupComparer(_temporaryGroup);
            }
        }

        /// <summary>
        /// Use the GroupDescriptions to place items into their respective groups.
        /// This assumes that there is no paging, so we just group the entire collection
        /// of items that the CollectionView holds.
        /// </summary>
        private void PrepareGroups()
        {
            // we should only use this method if we aren't paging
            Debug.Assert(PageSize == 0, "Unexpected PageSize != 0");

            _group.Clear();
            _group.Initialize();

            _group.IsDataInGroupOrder = CheckFlag(CollectionViewFlags.IsDataInGroupOrder);

            // set to false so that we access internal collection items
            // instead of the group items, as they have been cleared
            _isGrouping = false;

            if (_group.GroupDescriptions.Count > 0)
            {
                for (int num = 0, count = _internalList.Count; num < count; num++)
                {
                    object item = _internalList[num];
                    if (item != null && (!IsAddingNew || !Equals(CurrentAddItem, item)))
                    {
                        _group.AddToSubgroups(item, true /*loading*/);
                    }
                }

                if (IsAddingNew)
                {
                    _group.InsertSpecialItem(_group.Items.Count, CurrentAddItem, true);
                }
            }

            _isGrouping = _group.GroupBy != null;

            // now we set the value to false, so that subsequent adds will insert
            // into the correct groups.
            _group.IsDataInGroupOrder = false;

            // reset the grouping comparer
            PrepareGroupingComparer(_group);
        }

        /// <summary>
        /// Use the GroupDescriptions to place items into their respective groups.
        /// Because of the fact that we have paging, it is possible that we are only
        /// going to need a subset of the items to be displayed. However, before we
        /// actually group the entire collection, we can't display the items in the
        /// correct order. We therefore want to just create a temporary group with
        /// the entire collection, and then using this data we can create the group
        /// that is exposed with just the items we need.
        /// </summary>
        private void PrepareTemporaryGroups()
        {
            _temporaryGroup = new CollectionViewGroupRoot(this, CheckFlag(CollectionViewFlags.IsDataInGroupOrder));

            foreach (GroupDescription gd in _group.GroupDescriptions)
            {
                _temporaryGroup.GroupDescriptions.Add(gd);
            }

            _temporaryGroup.Initialize();

            // set to false so that we access internal collection items
            // instead of the group items, as they have been cleared
            _isGrouping = false;

            if (_temporaryGroup.GroupDescriptions.Count > 0)
            {
                for (int num = 0, count = _internalList.Count; num < count; num++)
                {
                    object item = _internalList[num];
                    if (item != null && (!IsAddingNew || !Equals(CurrentAddItem, item)))
                    {
                        _temporaryGroup.AddToSubgroups(item, true /*loading*/);
                    }
                }

                if (IsAddingNew)
                {
                    _temporaryGroup.InsertSpecialItem(_temporaryGroup.Items.Count, CurrentAddItem, true);
                }
            }

            _isGrouping = _temporaryGroup.GroupBy != null;

            // reset the grouping comparer
            PrepareGroupingComparer(_temporaryGroup);
        }

        /// <summary>
        /// Update our Groups private accessor to point to the subset of data
        /// covered by the current page, or to display the entire group if paging is not
        /// being used.
        /// </summary>
        private void PrepareGroupsForCurrentPage()
        {
            _group.Clear();
            _group.Initialize();

            // set to indicate that we will be pulling data from the temporary group data
            _isUsingTemporaryGroup = true;

            // since we are getting our data from the temporary group, it should
            // already be in group order
            _group.IsDataInGroupOrder = true;
            _group.ActiveComparer = null;

            if (GroupDescriptions.Count > 0)
            {
                for (int num = 0, count = Count; num < count; num++)
                {
                    object item = GetItemAt(num);
                    if (item != null && (!IsAddingNew || !Equals(CurrentAddItem, item)))
                    {
                        _group.AddToSubgroups(item, true /*loading*/);
                    }
                }

                if (IsAddingNew)
                {
                    _group.InsertSpecialItem(_group.Items.Count, CurrentAddItem, true);
                }
            }

            // set flag to indicate that we do not need to access the temporary data any longer
            _isUsingTemporaryGroup = false;

            // now we set the value to false, so that subsequent adds will insert
            // into the correct groups.
            _group.IsDataInGroupOrder = false;

            // reset the grouping comparer
            PrepareGroupingComparer(_group);

            _isGrouping = _group.GroupBy != null;
        }

        /// <summary>
        /// Create, filter and sort the local index array.
        /// called from Refresh(), override in derived classes as needed.
        /// </summary>
        /// <param name="enumerable">new IEnumerable to associate this view with</param>
        /// <returns>new local array to use for this view</returns>
        private IList PrepareLocalArray(IEnumerable enumerable)
        {
            Debug.Assert(enumerable != null, "Input list to filter/sort should not be null");

            // filter the collection's array into the local array
            List<object> localList = new List<object>();

            foreach (object item in enumerable)
            {
                if (Filter == null || PassesFilter(item))
                {
                    localList.Add(item);
                }
            }

            // sort the local array
            if (!CheckFlag(CollectionViewFlags.IsDataSorted) && SortDescriptions.Count > 0)
            {
                localList = SortList(localList);
            }

            return localList;
        }

        /// <summary>
        /// Process an Add operation from an INotifyCollectionChanged event handler.
        /// </summary>
        /// <param name="addedItem">Item added to the source collection</param>
        /// <param name="addIndex">Index item was added into</param>
        private void ProcessAddEvent(object addedItem, int addIndex)
        {
            // item to fire remove notification for if necessary
            object removeNotificationItem = null;
            if (PageSize > 0 && !IsGrouping)
            {
                removeNotificationItem = (Count == PageSize) ?
                    GetItemAt(PageSize - 1) : null;
            }

            // process the add by filtering and sorting the item
            ProcessInsertToCollection(
                addedItem,
                addIndex);

            // next check if we need to add an item into the current group
            bool needsGrouping = false;
            if (Count == 1 && GroupDescriptions.Count > 0)
            {
                // if this is the first item being added
                // we want to setup the groups with the
                // correct element type comparer
                if (PageSize > 0)
                {
                    PrepareGroupingComparer(_temporaryGroup);
                }

                PrepareGroupingComparer(_group);
            }

            if (IsGrouping)
            {
                int leafIndex = -1;

                if (PageSize > 0)
                {
                    _temporaryGroup.AddToSubgroups(addedItem, false /*loading*/);
                    leafIndex = _temporaryGroup.LeafIndexOf(addedItem);
                }

                // if we are not paging, we should just be able to add the item.
                // otherwise, we need to validate that it is within the current page.
                if (PageSize == 0 || (PageIndex + 1) * PageSize > leafIndex)
                {
                    needsGrouping = true;

                    int pageStartIndex = PageIndex * PageSize;

                    // if the item was inserted on a previous page
                    if (pageStartIndex > leafIndex && PageSize > 0)
                    {
                        addedItem = _temporaryGroup.LeafAt(pageStartIndex);
                    }

                    // if we're grouping and have more items than the
                    // PageSize will allow, remove the last item
                    if (PageSize > 0 && _group.ItemCount == PageSize)
                    {
                        removeNotificationItem = _group.LeafAt(PageSize - 1);
                        _group.RemoveFromSubgroups(removeNotificationItem);
                    }
                }
            }

            // if we are paging, we may have to fire another notification for the item
            // that needs to be removed for the one we added on this page.
            if (PageSize > 0 && !OnLastLocalPage &&
               (((IsGrouping && removeNotificationItem != null) ||
               (!IsGrouping && (PageIndex + 1) * PageSize > InternalIndexOf(addedItem)))))
            {
                if (removeNotificationItem != null && removeNotificationItem != addedItem)
                {
                    AdjustCurrencyForRemove(PageSize - 1);

                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Remove,
                            removeNotificationItem,
                            PageSize - 1));
                }
            }

            // if we need to add the item into the current group
            // that will be displayed
            if (needsGrouping)
            {
                _group.AddToSubgroups(addedItem, false /*loading*/);
            }

            int addedIndex = IndexOf(addedItem);

            // if the item is within the current page
            if (addedIndex >= 0)
            {
                object oldCurrentItem = CurrentItem;
                int oldCurrentPosition = CurrentPosition;
                bool oldIsCurrentAfterLast = IsCurrentAfterLast;
                bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

                AdjustCurrencyForAdd(null, addedIndex);

                // fire add notification
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Add,
                        addedItem,
                        addedIndex));

                RaiseCurrencyChanges(false, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);
            }
            else if (PageSize > 0)
            {
                // otherwise if the item was added into a previous page
                int internalIndex = IsGrouping ? _group.LeafIndexOf(addedItem) : InternalIndexOf(addedItem);
                if (internalIndex < ConvertToInternalIndex(0))
                {
                    // fire add notification for item pushed in
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Add,
                            GetItemAt(0),
                            0));
                }
            }
        }

        /// <summary>
        /// Process CollectionChanged event on source collection
        /// that implements INotifyCollectionChanged.
        /// </summary>
        /// <param name="args">
        /// The NotifyCollectionChangedEventArgs to be processed.
        /// </param>
        private void ProcessCollectionChanged(NotifyCollectionChangedEventArgs args)
        {
            // if we do not want to handle the CollectionChanged event, return
            if (!CheckFlag(CollectionViewFlags.ShouldProcessCollectionChanged))
            {
                return;
            }

            if (args.Action == NotifyCollectionChangedAction.Reset)
            {
                // if we have no items now, clear our own internal list
                if (!SourceCollection.GetEnumerator().MoveNext())
                {
                    _internalList.Clear();
                }

                // calling Refresh, will fire the collectionchanged event
                RefreshOrDefer();
                return;
            }

            //Move 与多元素变更没有增量维护实现：原代码只处理 "单个 Add/Remove/Replace"，
            //Move 之后 _internalList 与源集合顺序永久错位，后续所有 Index/GetItemAt/分页偏移全部错误，
            //多元素事件也只取 NewItems[0]。这里统一降级为整表刷新，保证顺序与源集合一致。
            if (args.Action == NotifyCollectionChangedAction.Move ||
                (args.NewItems != null && args.NewItems.Count > 1) ||
                (args.OldItems != null && args.OldItems.Count > 1))
            {
                RefreshOrDefer();
                OnPropertyChanged(nameof(ItemCount));
                return;
            }

            object addedItem = args.NewItems?[0];
            object removedItem = args.OldItems?[0];

            // fire notifications for removes
            if (args.Action == NotifyCollectionChangedAction.Remove ||
                args.Action == NotifyCollectionChangedAction.Replace)
            {
                ProcessRemoveEvent(removedItem, args.Action == NotifyCollectionChangedAction.Replace);
            }

            // fire notifications for adds
            if ((args.Action == NotifyCollectionChangedAction.Add ||
                args.Action == NotifyCollectionChangedAction.Replace) &&
                (Filter == null || PassesFilter(addedItem)))
            {
                ProcessAddEvent(addedItem, args.NewStartingIndex);
            }

            if (args.Action != NotifyCollectionChangedAction.Replace)
            {
                OnPropertyChanged(nameof(ItemCount));
            }
        }

        /// <summary>
        /// Process a Remove operation from an INotifyCollectionChanged event handler.
        /// </summary>
        /// <param name="removedItem">Item removed from the source collection</param>
        /// <param name="isReplace">Whether this was part of a Replace operation</param>
        private void ProcessRemoveEvent(object removedItem, bool isReplace)
        {
            int internalRemoveIndex;
            if (IsGrouping)
            {
                internalRemoveIndex = PageSize > 0 ? _temporaryGroup.LeafIndexOf(removedItem) :
                    _group.LeafIndexOf(removedItem);
            }
            else
            {
                internalRemoveIndex = InternalIndexOf(removedItem);
            }

            int removeIndex = IndexOf(removedItem);

            // remove the item from the collection
            _internalList.Remove(removedItem);

            // only fire the remove if it was removed from either the current page, or a previous page
            bool needToRemove = (PageSize == 0 && removeIndex >= 0) || (internalRemoveIndex < (PageIndex + 1) * PageSize);

            if (IsGrouping)
            {
                if (PageSize > 0)
                {
                    _temporaryGroup.RemoveFromSubgroups(removedItem);
                }

                if (needToRemove)
                {
                    _group.RemoveFromSubgroups(removeIndex >= 0 ? removedItem : _group.LeafAt(0));
                }
            }

            if (needToRemove)
            {
                object oldCurrentItem = CurrentItem;
                int oldCurrentPosition = CurrentPosition;
                bool oldIsCurrentAfterLast = IsCurrentAfterLast;
                bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

                AdjustCurrencyForRemove(removeIndex);

                // fire remove notification
                // if we removed from current page, remove from removeIndex,
                // if we removed from previous page, remove first item (index=0)
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Remove,
                        removedItem,
                        Math.Max(0, removeIndex)));

                RaiseCurrencyChanges(false, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);

                // if we removed all items from the current page,
                // move to the previous page. we do not need to
                // fire additional notifications, as moving the page will
                // trigger a reset.
                if (NeedToMoveToPreviousPage && !isReplace)
                {
                    MoveToPreviousPage();
                    return;
                }

                // if we are paging, we may have to fire another notification for the item
                // that needs to replace the one we removed on this page.
                if (PageSize > 0 && Count == PageSize)
                {
                    // we first need to add the item into the current group
                    if (IsGrouping)
                    {
                        object newItem = _temporaryGroup.LeafAt((PageSize * (PageIndex + 1)) - 1);
                        if (newItem != null)
                        {
                            _group.AddToSubgroups(newItem, false /*loading*/);
                        }
                    }

                    // fire the add notification
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Add,
                            GetItemAt(PageSize - 1),
                            PageSize - 1));
                }
            }
        }

        /// <summary>
        /// Handles adding an item into the collection, and applying sorting, filtering, grouping, paging.
        /// </summary>
        /// <param name="item">Item to insert in the collection</param>
        /// <param name="index">Index to insert item into</param>
        private void ProcessInsertToCollection(object item, int index)
        {
            // first check to see if it passes the filter
            if (Filter == null || PassesFilter(item))
            {
                if (SortDescriptions.Count > 0)
                {
                    // create the SortFieldComparer to use
                    SortFieldComparer sortFieldComparer = new SortFieldComparer(this);

                    // check if the item would be in sorted order if inserted into the specified index
                    // otherwise, calculate the correct sorted index
                    if (index < 0 || /* if item was not originally part of list */
                        (index > 0 && (sortFieldComparer.Compare(item, InternalItemAt(index - 1)) < 0)) || /* item has moved up in the list */
                        ((index < InternalList.Count - 1) && (sortFieldComparer.Compare(item, InternalItemAt(index)) > 0))) /* item has moved down in the list */
                    {
                        index = sortFieldComparer.FindInsertIndex(item, _internalList);
                    }
                }

                // make sure that the specified insert index is within the valid range
                // otherwise, just add it to the end. the index can be set to an invalid
                // value if the item was originally not in the collection, on a different
                // page, or if it had been previously filtered out.
                if (index < 0 || index > _internalList.Count)
                {
                    index = _internalList.Count;
                }

                _internalList.Insert(index, item);
            }
        }

        /// <summary>
        /// Raises Currency Change events
        /// </summary>
        /// <param name="fireChangedEvent">Whether to fire the CurrentChanged event even if the parameters have not changed</param>
        /// <param name="oldCurrentItem">CurrentItem before processing changes</param>
        /// <param name="oldCurrentPosition">CurrentPosition before processing changes</param>
        /// <param name="oldIsCurrentBeforeFirst">IsCurrentBeforeFirst before processing changes</param>
        /// <param name="oldIsCurrentAfterLast">IsCurrentAfterLast before processing changes</param>
        private void RaiseCurrencyChanges(bool fireChangedEvent, object oldCurrentItem, int oldCurrentPosition, bool oldIsCurrentBeforeFirst, bool oldIsCurrentAfterLast)
        {
            // fire events for currency changes
            if (fireChangedEvent || CurrentItem != oldCurrentItem || CurrentPosition != oldCurrentPosition)
            {
                OnCurrentChanged();
            }

            if (CurrentItem != oldCurrentItem)
            {
                OnPropertyChanged(nameof(CurrentItem));
            }

            if (CurrentPosition != oldCurrentPosition)
            {
                OnPropertyChanged(nameof(CurrentPosition));
            }

            if (IsCurrentAfterLast != oldIsCurrentAfterLast)
            {
                OnPropertyChanged(nameof(IsCurrentAfterLast));
            }

            if (IsCurrentBeforeFirst != oldIsCurrentBeforeFirst)
            {
                OnPropertyChanged(nameof(IsCurrentBeforeFirst));
            }
        }

        /// <summary>
        /// Raises the PageChanged event
        /// </summary>
        private void RaisePageChanged()
        {
            PageChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the PageChanging event
        /// </summary>
        /// <param name="newPageIndex">Index of the requested page</param>
        /// <returns>True if the event is cancelled (e.Cancel was set to True), False otherwise</returns>
        private bool RaisePageChanging(int newPageIndex)
        {
            EventHandler<PageChangingEventArgs> handler = PageChanging;
            if (handler != null)
            {
                PageChangingEventArgs pageChangingEventArgs = new PageChangingEventArgs(newPageIndex);
                handler(this, pageChangingEventArgs);
                return pageChangingEventArgs.Cancel;
            }

            return false;
        }

        /// <summary>
        /// Will call RefreshOverride and clear the NeedsRefresh flag
        /// </summary>
        private void RefreshInternal()
        {
            RefreshOverride();
            SetFlag(CollectionViewFlags.NeedsRefresh, false);
        }

        /// <summary>
        /// Refresh, or mark that refresh is needed when defer cycle completes.
        /// </summary>
        private void RefreshOrDefer()
        {
            if (IsRefreshDeferred)
            {
                SetFlag(CollectionViewFlags.NeedsRefresh, true);
            }
            else
            {
                RefreshInternal();
            }
        }

        /// <summary>
        /// Re-create the view, using any SortDescriptions.
        /// Also updates currency information.
        /// </summary>
        private void RefreshOverride()
        {
            object oldCurrentItem = CurrentItem;
            int oldCurrentPosition = CurrentPosition;
            bool oldIsCurrentAfterLast = IsCurrentAfterLast;
            bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

            // set IsGrouping to false
            _isGrouping = false;

            // force currency off the collection (gives user a chance to save dirty information)
            OnCurrentChanging();

            // if there's no sort/filter/paging/grouping, just use the collection's array
            if (UsesLocalArray)
            {
                try
                {
                    // apply filtering/sorting through the PrepareLocalArray method
                    _internalList = PrepareLocalArray(_sourceCollection);

                    // apply grouping
                    if (PageSize == 0)
                    {
                        PrepareGroups();
                    }
                    else
                    {
                        PrepareTemporaryGroups();
                        PrepareGroupsForCurrentPage();
                    }
                }
                catch (TargetInvocationException e)
                {
                    // If there's an exception while invoking PrepareLocalArray,
                    // we want to unwrap it and throw its inner exception
                    if (e.InnerException != null)
                    {
                        throw e.InnerException;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                CopySourceToInternalList();
            }

            // check if PageIndex is still valid after filter/sort
            if (PageSize > 0 &&
                PageIndex > 0 &&
                PageIndex >= PageCount)
            {
                MoveToPage(PageCount - 1);
            }

            // reset currency values
            ResetCurrencyValues(oldCurrentItem, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);

            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset));

            // now raise currency changes at the end
            RaiseCurrencyChanges(false, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);
        }

        /// <summary>
        /// Set currency back to the previous value it had if possible. If the item is no longer in view
        /// then either use the first item in the view, or if the list is empty, use null.
        /// </summary>
        /// <param name="oldCurrentItem">CurrentItem before processing changes</param>
        /// <param name="oldIsCurrentBeforeFirst">IsCurrentBeforeFirst before processing changes</param>
        /// <param name="oldIsCurrentAfterLast">IsCurrentAfterLast before processing changes</param>
        private void ResetCurrencyValues(object oldCurrentItem, bool oldIsCurrentBeforeFirst, bool oldIsCurrentAfterLast)
        {
            if (oldIsCurrentBeforeFirst || IsEmpty)
            {
                SetCurrent(null, -1);
            }
            else if (oldIsCurrentAfterLast)
            {
                SetCurrent(null, Count);
            }
            else
            {
                // try to set currency back to old current item
                // if there are duplicates, use the position of the first matching item
                int newPosition = IndexOf(oldCurrentItem);

                // if the old current item is no longer in view
                if (newPosition < 0)
                {
                    // if we are adding a new item, set it as the current item, otherwise, set it to null
                    newPosition = 0;

                    if (newPosition < Count)
                    {
                        SetCurrent(GetItemAt(newPosition), newPosition);
                    }
                    else if (!IsEmpty)
                    {
                        SetCurrent(GetItemAt(0), 0);
                    }
                    else
                    {
                        SetCurrent(null, -1);
                    }
                }
                else
                {
                    SetCurrent(oldCurrentItem, newPosition);
                }
            }
        }

        /// <summary>
        /// Set CurrentItem and CurrentPosition, no questions asked!
        /// </summary>
        /// <remarks>
        /// CollectionViews (and sub-classes) should use this method to update
        /// the Current values.
        /// </remarks>
        /// <param name="newItem">New CurrentItem</param>
        /// <param name="newPosition">New CurrentPosition</param>
        private void SetCurrent(object newItem, int newPosition)
        {
            int count = (newItem != null) ? 0 : (IsEmpty ? 0 : Count);
            SetCurrent(newItem, newPosition, count);
        }

        /// <summary>
        /// Set CurrentItem and CurrentPosition, no questions asked!
        /// </summary>
        /// <remarks>
        /// This method can be called from a constructor - it does not call
        /// any virtuals.  The 'count' parameter is substitute for the real Count,
        /// used only when newItem is null.
        /// In that case, this method sets IsCurrentAfterLast to true if and only
        /// if newPosition >= count.  This distinguishes between a null belonging
        /// to the view and the dummy null when CurrentPosition is past the end.
        /// </remarks>
        /// <param name="newItem">New CurrentItem</param>
        /// <param name="newPosition">New CurrentPosition</param>
        /// <param name="count">Numbers of items in the collection</param>
        private void SetCurrent(object newItem, int newPosition, int count)
        {
            if (newItem != null)
            {
                // non-null item implies position is within range.
                // We ignore count - it's just a placeholder
                SetFlag(CollectionViewFlags.IsCurrentBeforeFirst, false);
                SetFlag(CollectionViewFlags.IsCurrentAfterLast, false);
            }
            else if (count == 0)
            {
                // empty collection - by convention both flags are true and position is -1
                SetFlag(CollectionViewFlags.IsCurrentBeforeFirst, true);
                SetFlag(CollectionViewFlags.IsCurrentAfterLast, true);
                newPosition = -1;
            }
            else
            {
                // null item, possibly within range.
                SetFlag(CollectionViewFlags.IsCurrentBeforeFirst, newPosition < 0);
                SetFlag(CollectionViewFlags.IsCurrentAfterLast, newPosition >= count);
            }

            _currentItem = newItem;
            _currentPosition = newPosition;
        }

        /// <summary>
        /// Just move it. No argument check, no events, just move current to position.
        /// </summary>
        /// <param name="position">Position to move the current item to</param>
        private void SetCurrentToPosition(int position)
        {
            if (position < 0)
            {
                SetFlag(CollectionViewFlags.IsCurrentBeforeFirst, true);
                SetCurrent(null, -1);
            }
            else if (position >= Count)
            {
                SetFlag(CollectionViewFlags.IsCurrentAfterLast, true);
                SetCurrent(null, Count);
            }
            else
            {
                SetFlag(CollectionViewFlags.IsCurrentBeforeFirst | CollectionViewFlags.IsCurrentAfterLast, false);
                SetCurrent(GetItemAt(position), position);
            }
        }

        /// <summary>
        /// Sets the specified Flag(s)
        /// </summary>
        /// <param name="flags">Flags we want to set</param>
        /// <param name="value">Value we want to set these flags to</param>
        private void SetFlag(CollectionViewFlags flags, bool value)
        {
            if (value)
            {
                _flags |= flags;
            }
            else
            {
                _flags &= ~flags;
            }
        }

        /// <summary>
        /// Set new SortDescription collection; re-hook collection change notification handler
        /// </summary>
        /// <param name="descriptions">SortDescriptionCollection to set the property value to</param>
        private void SetSortDescriptions(SortDescriptionCollection descriptions)
        {
            if (_sortDescriptions != null)
            {
                ((INotifyCollectionChanged)_sortDescriptions).CollectionChanged -= new NotifyCollectionChangedEventHandler(SortDescriptionsChanged);
            }

            _sortDescriptions = descriptions;

            if (_sortDescriptions != null)
            {
                Debug.Assert(_sortDescriptions.Count == 0, "must be empty SortDescription collection");
                ((INotifyCollectionChanged)_sortDescriptions).CollectionChanged += new NotifyCollectionChangedEventHandler(SortDescriptionsChanged);
            }
        }

        /// <summary>
        /// SortDescription was added/removed, refresh PagedCollectionView
        /// </summary>
        /// <param name="sender">Sender that triggered this handler</param>
        /// <param name="e">NotifyCollectionChangedEventArgs for this change</param>
        private void SortDescriptionsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsAddingNew || IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringAddOrEdit, "Sorting"));
            }

            // we want to make sure that the data is refreshed before we try to move to a page
            // since the refresh would take care of the filtering, sorting, and grouping.
            RefreshOrDefer();

            if (PageSize > 0)
            {
                if (IsRefreshDeferred)
                {
                    // set cached value and flag so that we move to first page on EndDefer
                    _cachedPageIndex = 0;
                    SetFlag(CollectionViewFlags.IsMoveToPageDeferred, true);
                }
                else
                {
                    MoveToFirstPage();
                }
            }

            OnPropertyChanged(nameof(SortDescriptions));
        }

        /// <summary>
        /// Sort the List based on the SortDescriptions property.
        /// </summary>
        /// <param name="list">List of objects to sort</param>
        /// <returns>The sorted list</returns>
        private List<object> SortList(List<object> list)
        {
            Debug.Assert(list != null, "Input list to sort should not be null");

            IEnumerable<object> seq = list;
            IComparer<object> comparer = new CultureSensitiveComparer();

            foreach (SortDescription sort in SortDescriptions)
            {
                string propertyPath = sort.PropertyName;
                Type propertyType = null;

                // find the propertyType from the first non-null instance
                // in the list
                foreach (object item in list)
                {
                    if (item != null)
                    {
                        propertyType = item.GetType().GetNestedPropertyType(propertyPath);
                        break;
                    }
                }

                IOrderedEnumerable<object> orderedEnum = seq as IOrderedEnumerable<object>;

                switch (sort.Direction)
                {
                    case ListSortDirection.Ascending:
                        if (orderedEnum != null)
                        {
                            // thenby
                            seq = orderedEnum.ThenBy(item => InvokePath(item, propertyPath, propertyType), comparer);
                        }
                        else
                        {
                            // orderby
                            seq = seq.OrderBy(item => InvokePath(item, propertyPath, propertyType), comparer);
                        }

                        break;
                    case ListSortDirection.Descending:
                        if (orderedEnum != null)
                        {
                            // thenby
                            seq = orderedEnum.ThenByDescending(item => InvokePath(item, propertyPath, propertyType), comparer);
                        }
                        else
                        {
                            // orderby
                            seq = seq.OrderByDescending(item => InvokePath(item, propertyPath, propertyType), comparer);
                        }

                        break;
                    default:
                        break;
                }
            }

            return seq.ToList();
        }

        /// <summary>
        /// Helper to validate that we are not in the middle of a DeferRefresh
        /// and throw if that is the case.
        /// </summary>
        private void VerifyRefreshNotDeferred()
        {
            // If the Refresh is being deferred to change filtering or sorting of the
            // data by this PagedCollectionView, then PagedCollectionView will not reflect the correct
            // state of the underlying data.
            if (IsRefreshDeferred)
            {
                throw new InvalidOperationException(NoCheckOrChangeWhenDeferred);
            }
        }

        #endregion Private Methods

        ////------------------------------------------------------
        ////
        ////  Private Classes
        ////
        ////------------------------------------------------------
    }
}
