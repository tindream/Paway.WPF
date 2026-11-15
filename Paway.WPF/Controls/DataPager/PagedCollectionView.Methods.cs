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
    // 公开方法与静态方法
    public sealed partial class PagedCollectionView
    {
        #region Public Methods

        /// <summary>
        /// Add a new item to the underlying collection.  Returns the new item.
        /// After calling AddNew and changing the new item as desired, either
        /// CommitNew or CancelNew" should be called to complete the transaction.
        /// </summary>
        /// <returns>The new item we are adding</returns>
        public object AddNew()
        {
            EnsureCollectionInSync();
            VerifyRefreshNotDeferred();

            if (IsEditingItem)
            {
                // Implicitly close a previous EditItem
                CommitEdit();
            }

            // Implicitly close a previous AddNew
            CommitNew();

            // Checking CanAddNew will validate that we have the correct itemConstructor
            if (!CanAddNew)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedForView, "AddNew"));
            }

            object newItem = null;

            if (_itemConstructor != null)
            {
                newItem = _itemConstructor.Invoke(null);
            }

            try
            {
                // temporarily disable the CollectionChanged event
                // handler so filtering, sorting, or grouping
                // doesn't get applied yet
                SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, false);

                SourceList?.Add(newItem);
            }
            finally
            {
                SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, true);
            }

            // Modify our _trackingEnumerator so that it shows that our collection is "up to date"
            // and will not refresh for now.
            _trackingEnumerator = _sourceCollection.GetEnumerator();

            int addIndex;
            int removeIndex = -1;

            // Adjust index based on where it should be displayed in view.
            if (PageSize > 0)
            {
                // if the page is full (Count==PageSize), then replace last item (Count-1).
                // otherwise, we just append at end (Count).
                addIndex = Count - ((Count == PageSize) ? 1 : 0);

                // if the page is full, remove the last item to make space for the new one.
                removeIndex = (Count == PageSize) ? addIndex : -1;
            }
            else
            {
                // for non-paged lists, we want to insert the item
                // as the last item in the view
                addIndex = Count;
            }

            // if we need to remove an item from the view due to paging
            if (removeIndex > -1)
            {
                object removeItem = GetItemAt(removeIndex);
                if (IsGrouping)
                {
                    _group.RemoveFromSubgroups(removeItem);
                }

                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Remove,
                        removeItem,
                        removeIndex));
            }

            // add the new item to the internal list
            _internalList.Insert(ConvertToInternalIndex(addIndex), newItem);
            OnPropertyChanged(nameof(ItemCount));

            object oldCurrentItem = CurrentItem;
            int oldCurrentPosition = CurrentPosition;
            bool oldIsCurrentAfterLast = IsCurrentAfterLast;
            bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

            AdjustCurrencyForAdd(null, addIndex);

            if (IsGrouping)
            {
                _group.InsertSpecialItem(_group.Items.Count, newItem, false);
                if (PageSize > 0)
                {
                    _temporaryGroup.InsertSpecialItem(_temporaryGroup.Items.Count, newItem, false);
                }
            }

            // fire collection changed.
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add,
                    newItem,
                    addIndex));

            RaiseCurrencyChanges(false, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);

            // set the current new item
            CurrentAddItem = newItem;

            MoveCurrentTo(newItem);

            // if the new item is editable, call BeginEdit on it
            if (newItem is IEditableObject editableObject)
            {
                editableObject.BeginEdit();
            }

            return newItem;
        }

        /// <summary>
        /// Complete the transaction started by <seealso cref="EditItem"/>.
        /// The pending changes (if any) to the item are discarded.
        /// </summary>
        public void CancelEdit()
        {
            if (IsAddingNew)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringTransaction, "CancelEdit", "AddNew"));
            }
            else if (!CanCancelEdit)
            {
                throw new InvalidOperationException(CancelEditNotSupported);
            }

            VerifyRefreshNotDeferred();

            if (CurrentEditItem == null)
            {
                return;
            }

            object editItem = CurrentEditItem;
            CurrentEditItem = null;

            if (editItem is System.ComponentModel.IEditableObject ieo)
            {
                ieo.CancelEdit();
            }
            else
            {
                throw new InvalidOperationException(CancelEditNotSupported);
            }
        }

        /// <summary>
        /// Complete the transaction started by AddNew. The new
        /// item is removed from the collection.
        /// </summary>
        public void CancelNew()
        {
            if (IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringTransaction, "CancelNew", "EditItem"));
            }

            VerifyRefreshNotDeferred();

            if (CurrentAddItem == null)
            {
                return;
            }

            // get index of item before it is removed
            int index = IndexOf(CurrentAddItem);

            // remove the new item from the underlying collection
            try
            {
                // temporarily disable the CollectionChanged event
                // handler so filtering, sorting, or grouping
                // doesn't get applied yet
                SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, false);

                SourceList?.Remove(CurrentAddItem);
            }
            finally
            {
                SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, true);
            }

            // Modify our _trackingEnumerator so that it shows that our collection is "up to date"
            // and will not refresh for now.
            _trackingEnumerator = _sourceCollection.GetEnumerator();

            // fire the correct events
            if (CurrentAddItem != null)
            {
                object newItem = EndAddNew(true);

                int addIndex = -1;

                // Adjust index based on where it should be displayed in view.
                if (PageSize > 0 && !OnLastLocalPage)
                {
                    // if there is paging and we are not on the last page, we need
                    // to bring in an item from the next page.
                    addIndex = Count - 1;
                }

                // remove the new item from the internal list
                InternalList.Remove(newItem);

                if (IsGrouping)
                {
                    _group.RemoveSpecialItem(_group.Items.Count - 1, newItem, false);
                    if (PageSize > 0)
                    {
                        _temporaryGroup.RemoveSpecialItem(_temporaryGroup.Items.Count - 1, newItem, false);
                    }
                }

                OnPropertyChanged(nameof(ItemCount));

                object oldCurrentItem = CurrentItem;
                int oldCurrentPosition = CurrentPosition;
                bool oldIsCurrentAfterLast = IsCurrentAfterLast;
                bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

                AdjustCurrencyForRemove(index);

                // fire collection changed.
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Remove,
                        newItem,
                        index));

                RaiseCurrencyChanges(false, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);

                // if we need to add an item into the view due to paging
                if (addIndex > -1)
                {
                    int internalIndex = ConvertToInternalIndex(addIndex);
                    object addItem;
                    if (IsGrouping)
                    {
                        addItem = _temporaryGroup.LeafAt(internalIndex);
                        _group.AddToSubgroups(addItem, false /*loading*/);
                    }
                    else
                    {
                        addItem = InternalItemAt(internalIndex);
                    }

                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Add,
                            addItem,
                            IndexOf(addItem)));
                }
            }
        }

        /// <summary>
        /// Complete the transaction started by <seealso cref="EditItem"/>.
        /// The pending changes (if any) to the item are committed.
        /// </summary>
        public void CommitEdit()
        {
            if (IsAddingNew)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringTransaction, "CommitEdit", "AddNew"));
            }

            VerifyRefreshNotDeferred();

            if (CurrentEditItem == null)
            {
                return;
            }

            object editItem = CurrentEditItem;
            CurrentEditItem = null;

            if (editItem is System.ComponentModel.IEditableObject ieo)
            {
                ieo.EndEdit();
            }

            if (UsesLocalArray)
            {
                // first remove the item from the array so that we can insert into the correct position
                int removeIndex = IndexOf(editItem);
                int internalRemoveIndex = InternalIndexOf(editItem);
                _internalList.Remove(editItem);

                // check whether to restore currency to the item being edited
                object restoreCurrencyTo = (editItem == CurrentItem) ? editItem : null;

                if (removeIndex >= 0 && IsGrouping)
                {
                    // we can't just call RemoveFromSubgroups, as the group name
                    // for the item may have changed during the edit.
                    _group.RemoveItemFromSubgroupsByExhaustiveSearch(editItem);
                    if (PageSize > 0)
                    {
                        _temporaryGroup.RemoveItemFromSubgroupsByExhaustiveSearch(editItem);
                    }
                }

                object oldCurrentItem = CurrentItem;
                int oldCurrentPosition = CurrentPosition;
                bool oldIsCurrentAfterLast = IsCurrentAfterLast;
                bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

                // only adjust currency and fire the event if we actually removed the item
                if (removeIndex >= 0)
                {
                    AdjustCurrencyForRemove(removeIndex);

                    // raise the remove event so we can next insert it into the correct place
                    OnCollectionChanged(
                        new NotifyCollectionChangedEventArgs(
                            NotifyCollectionChangedAction.Remove,
                            editItem,
                            removeIndex));
                }

                // check to see that the item will be added back in
                bool passedFilter = PassesFilter(editItem);

                // if we removed all items from the current page,
                // move to the previous page. we do not need to
                // fire additional notifications, as moving the page will
                // trigger a reset.
                if (NeedToMoveToPreviousPage && !passedFilter)
                {
                    MoveToPreviousPage();
                    return;
                }

                // next process adding it into the correct location
                ProcessInsertToCollection(editItem, internalRemoveIndex);

                int pageStartIndex = PageIndex * PageSize;
                int nextPageStartIndex = pageStartIndex + PageSize;

                if (IsGrouping)
                {
                    int leafIndex = -1;
                    if (passedFilter && PageSize > 0)
                    {
                        _temporaryGroup.AddToSubgroups(editItem, false /*loading*/);
                        leafIndex = _temporaryGroup.LeafIndexOf(editItem);
                    }

                    // if we are not paging, we should just be able to add the item.
                    // otherwise, we need to validate that it is within the current page.
                    if (passedFilter && (PageSize == 0 ||
                       (pageStartIndex <= leafIndex && nextPageStartIndex > leafIndex)))
                    {
                        _group.AddToSubgroups(editItem, false /*loading*/);
                        int addIndex = IndexOf(editItem);
                        AdjustCurrencyForEdit(restoreCurrencyTo, addIndex);
                        OnCollectionChanged(
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Add,
                                editItem,
                                addIndex));
                    }
                    else if (PageSize > 0)
                    {
                        int addIndex = -1;
                        if (passedFilter && leafIndex < pageStartIndex)
                        {
                            // if the item was added to an earlier page, then we need to bring
                            // in the item that would have been pushed down to this page
                            addIndex = pageStartIndex;
                        }
                        else if (!OnLastLocalPage && removeIndex >= 0)
                        {
                            // if the item was added to a later page, then we need to bring in the
                            // first item from the next page
                            addIndex = nextPageStartIndex - 1;
                        }

                        object addItem = _temporaryGroup.LeafAt(addIndex);
                        if (addItem != null)
                        {
                            _group.AddToSubgroups(addItem, false /*loading*/);
                            addIndex = IndexOf(addItem);
                            AdjustCurrencyForEdit(restoreCurrencyTo, addIndex);
                            OnCollectionChanged(
                                new NotifyCollectionChangedEventArgs(
                                    NotifyCollectionChangedAction.Add,
                                    addItem,
                                    addIndex));
                        }
                    }
                }
                else
                {
                    // if we are still within the view
                    int addIndex = IndexOf(editItem);
                    if (addIndex >= 0)
                    {
                        AdjustCurrencyForEdit(restoreCurrencyTo, addIndex);
                        OnCollectionChanged(
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Add,
                                editItem,
                                addIndex));
                    }
                    else if (PageSize > 0)
                    {
                        // calculate whether the item was inserted into the previous page
                        bool insertedToPreviousPage = PassesFilter(editItem) &&
                            (InternalIndexOf(editItem) < ConvertToInternalIndex(0));
                        addIndex = insertedToPreviousPage ? 0 : Count - 1;

                        // don't fire the event if we are on the last page
                        // and we don't have any items to bring in.
                        if (insertedToPreviousPage || (!OnLastLocalPage && removeIndex >= 0))
                        {
                            AdjustCurrencyForEdit(restoreCurrencyTo, addIndex);
                            OnCollectionChanged(
                                new NotifyCollectionChangedEventArgs(
                                    NotifyCollectionChangedAction.Add,
                                    GetItemAt(addIndex),
                                    addIndex));
                        }
                    }
                }

                // now raise currency changes at the end
                RaiseCurrencyChanges(true, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);
            }
            else if (!Contains(editItem))
            {
                // if the item did not belong to the collection, add it
                InternalList.Add(editItem);
            }
        }

        /// <summary>
        /// Complete the transaction started by AddNew. We follow the WPF
        /// convention in that the view's sort, filter, and paging
        /// specifications (if any) are applied to the new item.
        /// </summary>
        public void CommitNew()
        {
            if (IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringTransaction, "CommitNew", "EditItem"));
            }

            VerifyRefreshNotDeferred();

            if (CurrentAddItem == null)
            {
                return;
            }

            // End the AddNew transaction
            object newItem = EndAddNew(false);

            // keep track of the current item
            object previousCurrentItem = CurrentItem;

            // Modify our _trackingEnumerator so that it shows that our collection is "up to date"
            // and will not refresh for now.
            _trackingEnumerator = _sourceCollection.GetEnumerator();

            if (UsesLocalArray)
            {
                // first remove the item from the array so that we can insert into the correct position
                int removeIndex = Count - 1;
                int internalIndex = _internalList.IndexOf(newItem);
                _internalList.Remove(newItem);

                if (IsGrouping)
                {
                    _group.RemoveSpecialItem(_group.Items.Count - 1, newItem, false);
                    if (PageSize > 0)
                    {
                        _temporaryGroup.RemoveSpecialItem(_temporaryGroup.Items.Count - 1, newItem, false);
                    }
                }

                object oldCurrentItem = CurrentItem;
                int oldCurrentPosition = CurrentPosition;
                bool oldIsCurrentAfterLast = IsCurrentAfterLast;
                bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

                AdjustCurrencyForRemove(removeIndex);

                // raise the remove event so we can next insert it into the correct place
                OnCollectionChanged(
                    new NotifyCollectionChangedEventArgs(
                        NotifyCollectionChangedAction.Remove,
                        newItem,
                        removeIndex));

                // check to see that the item will be added back in
                bool passedFilter = PassesFilter(newItem);

                // next process adding it into the correct location
                ProcessInsertToCollection(newItem, internalIndex);

                int pageStartIndex = PageIndex * PageSize;
                int nextPageStartIndex = pageStartIndex + PageSize;

                if (IsGrouping)
                {
                    int leafIndex = -1;
                    if (passedFilter && PageSize > 0)
                    {
                        _temporaryGroup.AddToSubgroups(newItem, false /*loading*/);
                        leafIndex = _temporaryGroup.LeafIndexOf(newItem);
                    }

                    // if we are not paging, we should just be able to add the item.
                    // otherwise, we need to validate that it is within the current page.
                    if (passedFilter && (PageSize == 0 ||
                       (pageStartIndex <= leafIndex && nextPageStartIndex > leafIndex)))
                    {
                        _group.AddToSubgroups(newItem, false /*loading*/);
                        int addIndex = IndexOf(newItem);

                        // adjust currency to either the previous current item if possible
                        // or to the item at the end of the list where the new item was.
                        if (previousCurrentItem != null)
                        {
                            if (Contains(previousCurrentItem))
                            {
                                AdjustCurrencyForAdd(previousCurrentItem, addIndex);
                            }
                            else
                            {
                                AdjustCurrencyForAdd(GetItemAt(Count - 1), addIndex);
                            }
                        }

                        OnCollectionChanged(
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Add,
                                newItem,
                                addIndex));
                    }
                    else
                    {
                        if (!passedFilter && (PageSize == 0 || OnLastLocalPage))
                        {
                            AdjustCurrencyForRemove(removeIndex);
                        }
                        else if (PageSize > 0)
                        {
                            int addIndex = -1;
                            if (passedFilter && leafIndex < pageStartIndex)
                            {
                                // if the item was added to an earlier page, then we need to bring
                                // in the item that would have been pushed down to this page
                                addIndex = pageStartIndex;
                            }
                            else if (!OnLastLocalPage)
                            {
                                // if the item was added to a later page, then we need to bring in the
                                // first item from the next page
                                addIndex = nextPageStartIndex - 1;
                            }

                            object addItem = _temporaryGroup.LeafAt(addIndex);
                            if (addItem != null)
                            {
                                _group.AddToSubgroups(addItem, false /*loading*/);
                                addIndex = IndexOf(addItem);

                                // adjust currency to either the previous current item if possible
                                // or to the item at the end of the list where the new item was.
                                if (previousCurrentItem != null)
                                {
                                    if (Contains(previousCurrentItem))
                                    {
                                        AdjustCurrencyForAdd(previousCurrentItem, addIndex);
                                    }
                                    else
                                    {
                                        AdjustCurrencyForAdd(GetItemAt(Count - 1), addIndex);
                                    }
                                }

                                OnCollectionChanged(
                                    new NotifyCollectionChangedEventArgs(
                                        NotifyCollectionChangedAction.Add,
                                        addItem,
                                        addIndex));
                            }
                        }
                    }
                }
                else
                {
                    // if we are still within the view
                    int addIndex = IndexOf(newItem);
                    if (addIndex >= 0)
                    {
                        AdjustCurrencyForAdd(newItem, addIndex);
                        OnCollectionChanged(
                            new NotifyCollectionChangedEventArgs(
                                NotifyCollectionChangedAction.Add,
                                newItem,
                                addIndex));
                    }
                    else
                    {
                        if (!passedFilter && (PageSize == 0 || OnLastLocalPage))
                        {
                            AdjustCurrencyForRemove(removeIndex);
                        }
                        else if (PageSize > 0)
                        {
                            bool insertedToPreviousPage = InternalIndexOf(newItem) < ConvertToInternalIndex(0);
                            addIndex = insertedToPreviousPage ? 0 : Count - 1;

                            // don't fire the event if we are on the last page
                            // and we don't have any items to bring in.
                            if (insertedToPreviousPage || !OnLastLocalPage)
                            {
                                AdjustCurrencyForAdd(null, addIndex);
                                OnCollectionChanged(
                                    new NotifyCollectionChangedEventArgs(
                                        NotifyCollectionChangedAction.Add,
                                        GetItemAt(addIndex),
                                        addIndex));
                            }
                        }
                    }
                }

                // we want to fire the current changed event, even if we kept
                // the same current item and position, since the item was
                // removed/added back to the collection
                RaiseCurrencyChanges(true, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);
            }
        }

        /// <summary>
        /// Return true if the item belongs to this view.  No assumptions are
        /// made about the item. This method will behave similarly to IList.Contains().
        /// If the caller knows that the item belongs to the
        /// underlying collection, it is more efficient to call PassesFilter.
        /// </summary>
        /// <param name="item">The item we are checking to see whether it is within the collection</param>
        /// <returns>Boolean value of whether or not the collection contains the item</returns>
        public bool Contains(object item)
        {
            EnsureCollectionInSync();
            VerifyRefreshNotDeferred();
            return IndexOf(item) >= 0;
        }

        /// <summary>
        /// Enter a Defer Cycle.
        /// Defer cycles are used to coalesce changes to the ICollectionView.
        /// </summary>
        /// <returns>IDisposable used to notify that we no longer need to defer, when we dispose</returns>
        public IDisposable DeferRefresh()
        {
            if (IsAddingNew || IsEditingItem)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringAddOrEdit, "DeferRefresh"));
            }

            ++_deferLevel;
            return new DeferHelper(this);
        }

        /// <summary>
        /// Begins an editing transaction on the given item.  The transaction is
        /// completed by calling either CommitEdit or CancelEdit.  Any changes made
        /// to the item during the transaction are considered "pending", provided
        /// that the view supports the notion of "pending changes" for the given item.
        /// </summary>
        /// <param name="item">Item we want to edit</param>
        public void EditItem(object item)
        {
            VerifyRefreshNotDeferred();

            if (IsAddingNew)
            {
                if (object.Equals(item, CurrentAddItem))
                {
                    // EditItem(newItem) is a no-op
                    return;
                }

                // implicitly close a previous AddNew
                CommitNew();
            }

            // implicitly close a previous EditItem transaction
            CommitEdit();

            CurrentEditItem = item;

            if (item is System.ComponentModel.IEditableObject ieo)
            {
                ieo.BeginEdit();
            }
        }

        /// <summary>
        /// Implementation of IEnumerable.GetEnumerator().
        /// This provides a way to enumerate the members of the collection
        /// without changing the currency.
        /// </summary>
        /// <returns>IEnumerator for the collection</returns>
        public IEnumerator GetEnumerator()
        {
            EnsureCollectionInSync();
            VerifyRefreshNotDeferred();

            if (IsGrouping)
            {
                CollectionViewGroupRoot group = RootGroup;
                return group?.GetLeafEnumerator();
            }

            // if we are paging
            if (PageSize > 0)
            {
                List<object> list = new List<object>();

                // if we are in the middle of asynchronous load
                if (PageIndex < 0)
                {
                    return list.GetEnumerator();
                }

                for (int index = _pageSize * PageIndex;
                    index < Math.Min(_pageSize * (PageIndex + 1), InternalList.Count);
                    index++)
                {
                    list.Add(InternalList[index]);
                }

                return new NewItemAwareEnumerator(this, list.GetEnumerator(), CurrentAddItem);
            }
            else
            {
                return new NewItemAwareEnumerator(this, InternalList.GetEnumerator(), CurrentAddItem);
            }
        }

        /// <summary>
        /// Interface Implementation for GetEnumerator()
        /// </summary>
        /// <returns>IEnumerator that we get from our internal collection</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Retrieve item at the given zero-based index in this PagedCollectionView, after the source collection
        /// is filtered, sorted, and paged.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown if index is out of range
        /// </exception>
        /// <param name="index">Index of the item we want to retrieve</param>
        /// <returns>Item at specified index</returns>
        public object GetItemAt(int index)
        {
            EnsureCollectionInSync();
            VerifyRefreshNotDeferred();

            // for indicies larger than the count
            if (index >= Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (IsGrouping)
            {
                CollectionViewGroupRoot group = RootGroup;
                return group?.LeafAt(_isUsingTemporaryGroup ? ConvertToInternalIndex(index) : index);
            }

            if (IsAddingNew && UsesLocalArray && index == Count - 1)
            {
                return CurrentAddItem;
            }

            return InternalItemAt(ConvertToInternalIndex(index));
        }

        /// <summary>
        /// Return the index where the given item appears, or -1 if doesn't appear.
        /// </summary>
        /// <param name="item">Item we are searching for</param>
        /// <returns>Index of specified item</returns>
        public int IndexOf(object item)
        {
            EnsureCollectionInSync();
            VerifyRefreshNotDeferred();

            if (IsGrouping)
            {
                CollectionViewGroupRoot group = RootGroup;
                return group != null ? group.LeafIndexOf(item) : -1;
            }

            if (IsAddingNew && object.Equals(item, CurrentAddItem) && UsesLocalArray)
            {
                return Count - 1;
            }

            int internalIndex = InternalIndexOf(item);

            if (PageSize > 0 && internalIndex != -1)
            {
                if ((internalIndex >= (PageIndex * _pageSize)) &&
                    (internalIndex < ((PageIndex + 1) * _pageSize)))
                {
                    return internalIndex - (PageIndex * _pageSize);
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return internalIndex;
            }
        }

        /// <summary>
        /// Move to the given item.
        /// </summary>
        /// <param name="item">Item we want to move the currency to</param>
        /// <returns>Whether the operation was successful</returns>
        public bool MoveCurrentTo(object item)
        {
            VerifyRefreshNotDeferred();

            // if already on item, don't do anything
            if (object.Equals(CurrentItem, item))
            {
                // also check that we're not fooled by a false null currentItem
                if (item != null || IsCurrentInView)
                {
                    return IsCurrentInView;
                }
            }

            // if the item is not found IndexOf() will return -1, and
            // the MoveCurrentToPosition() below will move current to BeforeFirst
            // The IndexOf function takes into account paging, filtering, and sorting
            return MoveCurrentToPosition(IndexOf(item));
        }

        /// <summary>
        /// Move to the first item.
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        public bool MoveCurrentToFirst()
        {
            VerifyRefreshNotDeferred();

            return MoveCurrentToPosition(0);
        }

        /// <summary>
        /// Move to the last item.
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        public bool MoveCurrentToLast()
        {
            VerifyRefreshNotDeferred();

            int index = Count - 1;

            return MoveCurrentToPosition(index);
        }

        /// <summary>
        /// Move to the next item.
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        public bool MoveCurrentToNext()
        {
            VerifyRefreshNotDeferred();

            int index = CurrentPosition + 1;

            if (index <= Count)
            {
                return MoveCurrentToPosition(index);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Move CurrentItem to this index
        /// </summary>
        /// <param name="position">Position we want to move the currency to</param>
        /// <returns>True if the resulting CurrentItem is an item within the view; otherwise False</returns>
        public bool MoveCurrentToPosition(int position)
        {
            VerifyRefreshNotDeferred();

            // We want to allow the user to set the currency to just
            // beyond the last item. EnumerableCollectionView in WPF
            // also checks (position > this.Count) though the ListCollectionView
            // looks for (position >= this.Count).
            if (position < -1 || position > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

            if ((position != CurrentPosition || !IsCurrentInSync)
                && OkToChangeCurrent())
            {
                bool oldIsCurrentAfterLast = IsCurrentAfterLast;
                bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

                SetCurrentToPosition(position);
                OnCurrentChanged();

                if (IsCurrentAfterLast != oldIsCurrentAfterLast)
                {
                    OnPropertyChanged(nameof(IsCurrentAfterLast));
                }

                if (IsCurrentBeforeFirst != oldIsCurrentBeforeFirst)
                {
                    OnPropertyChanged(nameof(IsCurrentBeforeFirst));
                }

                OnPropertyChanged(nameof(CurrentPosition));
                OnPropertyChanged(nameof(CurrentItem));
            }

            return IsCurrentInView;
        }

        /// <summary>
        /// Move to the previous item.
        /// </summary>
        /// <returns>Whether the operation was successful</returns>
        public bool MoveCurrentToPrevious()
        {
            VerifyRefreshNotDeferred();

            int index = CurrentPosition - 1;

            if (index >= -1)
            {
                return MoveCurrentToPosition(index);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Moves to the first page.
        /// </summary>
        /// <returns>Whether or not the move was successful.</returns>
        public bool MoveToFirstPage()
        {
            return MoveToPage(0);
        }

        /// <summary>
        /// Moves to the last page.
        /// The move is only attempted when TotalItemCount is known.
        /// </summary>
        /// <returns>Whether or not the move was successful.</returns>
        public bool MoveToLastPage()
        {
            if (TotalItemCount != -1 && PageSize > 0)
            {
                return MoveToPage(PageCount - 1);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Moves to the page after the current page we are on.
        /// </summary>
        /// <returns>Whether or not the move was successful.</returns>
        public bool MoveToNextPage()
        {
            return MoveToPage(_pageIndex + 1);
        }

        /// <summary>
        /// Requests a page move to page <paramref name="pageIndex"/>.
        /// </summary>
        /// <param name="pageIndex">Index of the target page</param>
        /// <returns>Whether or not the move was successfully initiated.</returns>
        public bool MoveToPage(int pageIndex)
        {
            // Boundary checks for negative pageIndex
            if (pageIndex < -1)
            {
                return false;
            }

            // if the Refresh is deferred, cache the requested PageIndex so that we
            // can move to the desired page when EndDefer is called.
            if (IsRefreshDeferred)
            {
                // set cached value and flag so that we move to the page on EndDefer
                _cachedPageIndex = pageIndex;
                SetFlag(CollectionViewFlags.IsMoveToPageDeferred, true);
                return false;
            }

            // check for invalid pageIndex
            if (pageIndex == -1 && PageSize > 0)
            {
                return false;
            }

            // Check if the target page is out of bound, or equal to the current page
            if (pageIndex >= PageCount || _pageIndex == pageIndex)
            {
                return false;
            }

            // Check with the ICollectionView.CurrentChanging listeners if it's OK to move
            // on to another page
            if (!OkToChangeCurrent())
            {
                return false;
            }

            if (RaisePageChanging(pageIndex) && pageIndex != -1)
            {
                // Page move was cancelled. Abort the move, but only if the target index isn't -1.
                return false;
            }

            // Check if there is a current edited or new item so changes can be committed first.
            if (CurrentAddItem != null || CurrentEditItem != null)
            {
                // Remember current currency values for upcoming OnPropertyChanged notifications
                object oldCurrentItem = CurrentItem;
                int oldCurrentPosition = CurrentPosition;
                bool oldIsCurrentAfterLast = IsCurrentAfterLast;
                bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

                // Currently CommitNew()/CommitEdit()/CancelNew()/CancelEdit() can't handle committing or
                // cancelling an item that is no longer on the current page. That's acceptable and means that
                // the potential this._newItem or this._editItem needs to be committed before this page move.
                // The reason why we temporarily reset currency here is to give a chance to the bound
                // controls to commit or cancel their potential edits/addition. The DataForm calls ForceEndEdit()
                // for example as a result of changing currency.
                SetCurrentToPosition(-1);
                RaiseCurrencyChanges(true /*fireChangedEvent*/, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);

                // If the bound controls did not successfully end their potential item editing/addition, the
                // page move needs to be aborted.
                if (CurrentAddItem != null || CurrentEditItem != null)
                {
                    // Since PageChanging was raised and not cancelled, a PageChanged notification needs to be raised
                    // even though the PageIndex actually did not change.
                    RaisePageChanged();

                    // Restore original currency
                    Debug.Assert(CurrentItem == null, "Unexpected this.CurrentItem != null");
                    Debug.Assert(CurrentPosition == -1, "Unexpected this.CurrentPosition != -1");
                    Debug.Assert(IsCurrentBeforeFirst, "Unexpected this.IsCurrentBeforeFirst == false");
                    Debug.Assert(!IsCurrentAfterLast, "Unexpected this.IsCurrentAfterLast == true");

                    SetCurrentToPosition(oldCurrentPosition);
                    RaiseCurrencyChanges(false /*fireChangedEvent*/, null /*oldCurrentItem*/, -1 /*oldCurrentPosition*/,
                        true /*oldIsCurrentBeforeFirst*/, false /*oldIsCurrentAfterLast*/);

                    return false;
                }

                // Finally raise a CurrentChanging notification for the upcoming currency change
                // that will occur in CompletePageMove(pageIndex).
                OnCurrentChanging();
            }

            IsPageChanging = true;
            CompletePageMove(pageIndex);

            return true;
        }

        /// <summary>
        /// Moves to the page before the current page we are on.
        /// </summary>
        /// <returns>Whether or not the move was successful.</returns>
        public bool MoveToPreviousPage()
        {
            return MoveToPage(_pageIndex - 1);
        }

        /// <summary>
        /// Return true if the item belongs to this view.  The item is assumed to belong to the
        /// underlying DataCollection;  this method merely takes filters into account.
        /// It is commonly used during collection-changed notifications to determine if the added/removed
        /// item requires processing.
        /// Returns true if no filter is set on collection view.
        /// </summary>
        /// <param name="item">The item to compare against the Filter</param>
        /// <returns>Whether the item passes the filter</returns>
        public bool PassesFilter(object item)
        {
            if (Filter != null)
            {
                return Filter(item);
            }

            return true;
        }

        /// <summary>
        /// Re-create the view, using any SortDescriptions and/or Filters.
        /// </summary>
        public void Refresh()
        {
            if (this is IEditableCollectionView ecv && (ecv.IsAddingNew || ecv.IsEditingItem))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringAddOrEdit, "Refresh"));
            }

            RefreshInternal();
        }

        /// <summary>
        /// Remove the given item from the underlying collection. It
        /// needs to be in the current filtered, sorted, and paged view
        /// to call this.
        /// </summary>
        /// <param name="item">Item we want to remove</param>
        public void Remove(object item)
        {
            int index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
            }
        }

        /// <summary>
        /// Remove the item at the given index from the underlying collection.
        /// The index is interpreted with respect to the view (filtered, sorted,
        /// and paged list).
        /// </summary>
        /// <param name="index">Index of the item we want to remove</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), IndexOutOfRange);
            }

            if (IsEditingItem || IsAddingNew)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, OperationNotAllowedDuringAddOrEdit, "RemoveAt"));
            }
            else if (!CanRemove)
            {
                throw new InvalidOperationException(RemoveNotSupported);
            }

            VerifyRefreshNotDeferred();

            // convert the index from "view-relative" to "list-relative"
            object item = GetItemAt(index);

            // before we remove the item, see if we are not on the last page
            // and will have to bring in a new item to replace it
            bool replaceItem = PageSize > 0 && !OnLastLocalPage;

            try
            {
                // temporarily disable the CollectionChanged event
                // handler so filtering, sorting, or grouping
                // doesn't get applied yet
                SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, false);

                SourceList?.Remove(item);
            }
            finally
            {
                SetFlag(CollectionViewFlags.ShouldProcessCollectionChanged, true);
            }

            // Modify our _trackingEnumerator so that it shows that our collection is "up to date"
            // and will not refresh for now.
            _trackingEnumerator = _sourceCollection.GetEnumerator();

            Debug.Assert(index == IndexOf(item), "IndexOf returned unexpected value");

            // remove the item from the internal list
            _internalList.Remove(item);

            if (IsGrouping)
            {
                if (PageSize > 0)
                {
                    _temporaryGroup.RemoveFromSubgroups(item);
                }

                _group.RemoveFromSubgroups(item);
            }

            object oldCurrentItem = CurrentItem;
            int oldCurrentPosition = CurrentPosition;
            bool oldIsCurrentAfterLast = IsCurrentAfterLast;
            bool oldIsCurrentBeforeFirst = IsCurrentBeforeFirst;

            AdjustCurrencyForRemove(index);

            // fire remove notification
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove,
                    item,
                    index));

            RaiseCurrencyChanges(false, oldCurrentItem, oldCurrentPosition, oldIsCurrentBeforeFirst, oldIsCurrentAfterLast);

            // if we removed all items from the current page,
            // move to the previous page. we do not need to
            // fire additional notifications, as moving the page will
            // trigger a reset.
            if (NeedToMoveToPreviousPage)
            {
                MoveToPreviousPage();
                return;
            }

            // if we are paging, we may have to fire another notification for the item
            // that needs to replace the one we removed on this page.
            if (replaceItem)
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

        #endregion Public Methods

        ////------------------------------------------------------
        ////
        ////  Static Methods
        ////
        ////------------------------------------------------------

        #region Static Methods

        /// <summary>
        /// Helper for SortList to handle nested properties (e.g. Address.Street)
        /// </summary>
        /// <param name="item">parent object</param>
        /// <param name="propertyPath">property names path</param>
        /// <param name="propertyType">property type that we want to check for</param>
        /// <returns>child object</returns>
        private static object InvokePath(object item, string propertyPath, Type propertyType)
        {
#pragma warning disable IDE0018 // 内联变量声明
            Exception exception;
#pragma warning restore IDE0018 // 内联变量声明
            object propertyValue = TypeHelper.GetNestedPropertyValue(item, propertyPath, propertyType, out exception);
            if (exception != null)
            {
                throw exception;
            }

            return propertyValue;
        }

        #endregion Static Methods

        ////------------------------------------------------------
        ////
        ////  Private Methods
        ////
        ////------------------------------------------------------
    }
}
