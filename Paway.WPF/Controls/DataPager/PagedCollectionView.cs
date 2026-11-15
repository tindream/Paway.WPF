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
    /// <summary>
    /// PagedCollectionView view over an IEnumerable.
    /// <para>按职责拆分为多个 partial 文件：属性/索引器、公开方法、私有方法、嵌套类型</para>
    /// </summary>
    public sealed partial class PagedCollectionView : ICollectionView, IPagedCollectionView, IEditableCollectionView, INotifyPropertyChanged, IDisposable
    {
        private const string OperationNotAllowedDuringAddOrEdit = "'{0}' is not allowed during an AddNew or EditItem transaction.";
        private const string InvalidPageSize = "PageSize cannot have a negative value.";
        private const string CannotFilter = "The Filter property cannot be set when the CanFilter property returns false.";
        private const string ChangingPageSizeNotAllowedDuringAddOrEdit = "Changing the PageSize is not allowed during an AddNew or EditItem transaction.";
        private const string OperationNotAllowedForView = "'{0}' is not allowed for this view.";
        private const string OperationNotAllowedDuringTransaction = "'{0}' is not allowed during a transaction started by '{1}'.";
        private const string CancelEditNotSupported = "CancelEdit is not supported for the current edit item.";
        private const string IndexOutOfRange = "Index was out of range. Must be non-negative and less than the size of the collection.";
        private const string RemoveNotSupported = "Remove/RemoveAt is not supported.";
        private const string NoCheckOrChangeWhenDeferred = "Cannot change or check the contents or current position of the PagedCollectionView while Refresh is being deferred.";


        ////------------------------------------------------------
        ////
        ////  Constants
        ////
        ////------------------------------------------------------

        #region Static Fields and Constants

        /// <summary>
        /// Since there's nothing in the un-cancelable event args that is mutable,
        /// just create one instance to be used universally.
        /// </summary>
        private static readonly CurrentChangingEventArgs uncancelableCurrentChangingEventArgs = new CurrentChangingEventArgs(false);

        #endregion Static Fields and Constants

        ////------------------------------------------------------
        ////
        ////  Private Fields
        ////
        ////------------------------------------------------------

        #region Private Fields

        /// <summary>
        /// Value that we cache for the PageIndex if we are in a DeferRefresh,
        /// and the user has attempted to move to a different page.
        /// </summary>
        private int _cachedPageIndex = -1;

        /// <summary>
        /// Value that we cache for the PageSize if we are in a DeferRefresh,
        /// and the user has attempted to change the PageSize.
        /// </summary>
        private int _cachedPageSize;

        /// <summary>
        /// CultureInfo used in this PagedCollectionView
        /// </summary>
        private CultureInfo _culture;

        /// <summary>
        /// Private accessor for the Monitor we use to prevent recursion
        /// </summary>
        private readonly SimpleMonitor _currentChangedMonitor = new SimpleMonitor();

        /// <summary>
        /// Private accessor for the CurrentItem
        /// </summary>
        private object _currentItem;

        /// <summary>
        /// Private accessor for the CurrentPosition
        /// </summary>
        private int _currentPosition;

        /// <summary>
        /// The number of requests to defer Refresh()
        /// </summary>
        private int _deferLevel;

        /// <summary>
        /// The item we are currently editing
        /// </summary>
        private object _editItem;

        /// <summary>
        /// Private accessor for the Filter
        /// </summary>
        private Predicate<object> _filter;

        /// <summary>
        /// Private accessor for the CollectionViewFlags
        /// </summary>
        private CollectionViewFlags _flags = CollectionViewFlags.ShouldProcessCollectionChanged;

        /// <summary>
        /// Private accessor for the Grouping data
        /// </summary>
        private readonly CollectionViewGroupRoot _group;

        /// <summary>
        /// Private accessor for the InternalList
        /// </summary>
        private IList _internalList;

        /// <summary>
        /// Keeps track of whether groups have been applied to the
        /// collection already or not. Note that this can still be set
        /// to false even though we specify a GroupDescription, as the
        /// collection may not have gone through the PrepareGroups function.
        /// </summary>
        private bool _isGrouping;

        /// <summary>
        /// Private accessor for indicating whether we want to point to the temporary grouping data for calculations
        /// </summary>
        private bool _isUsingTemporaryGroup;

        /// <summary>
        /// ConstructorInfo obtained from reflection for generating new items
        /// </summary>
        private ConstructorInfo _itemConstructor;

        /// <summary>
        /// Whether we have the correct ConstructorInfo information for the ItemConstructor
        /// </summary>
        private bool _itemConstructorIsValid;

        /// <summary>
        /// The new item we are getting ready to add to the collection
        /// </summary>
        private object _newItem;

        /// <summary>
        /// Private accessor for the PageIndex
        /// </summary>
        private int _pageIndex = -1;

        /// <summary>
        /// Private accessor for the PageSize
        /// </summary>
        private int _pageSize;

        /// <summary>
        /// Whether the source needs to poll for changes
        /// (if it did not implement INotifyCollectionChanged)
        /// </summary>
        private readonly bool _pollForChanges;

        /// <summary>
        /// Private accessor for the SortDescriptions
        /// </summary>
        private SortDescriptionCollection _sortDescriptions;

        /// <summary>
        /// Private accessor for the SourceCollection
        /// </summary>
        private readonly IEnumerable _sourceCollection;

        /// <summary>
        /// Private accessor for the Grouping data on the entire collection
        /// </summary>
        private CollectionViewGroupRoot _temporaryGroup;

        /// <summary>
        /// Timestamp used to see if there was a collection change while
        /// processing enumerator changes
        /// </summary>
        private int _timestamp;

        /// <summary>
        /// Private accessor for the TrackingEnumerator
        /// </summary>
        private IEnumerator _trackingEnumerator;

        #endregion Private Fields

        ////------------------------------------------------------
        ////
        ////  Constructors
        ////
        ////------------------------------------------------------

        #region Constructors

        /// <summary>
        /// Helper constructor that sets default values for isDataSorted and isDataInGroupOrder.
        /// </summary>
        /// <param name="source">The source for the collection</param>
        public PagedCollectionView(IEnumerable source)
            : this(source, false /*isDataSorted*/, false /*isDataInGroupOrder*/)
        {
        }

        /// <summary>
        /// Initializes a new instance of the PagedCollectionView class.
        /// </summary>
        /// <param name="source">The source for the collection</param>
        /// <param name="isDataSorted">Determines whether the source is already sorted</param>
        /// <param name="isDataInGroupOrder">Whether the source is already in the correct order for grouping</param>
        public PagedCollectionView(IEnumerable source, bool isDataSorted, bool isDataInGroupOrder)
        {
            _sourceCollection = source ?? throw new ArgumentNullException(nameof(source));

            SetFlag(CollectionViewFlags.IsDataSorted, isDataSorted);
            SetFlag(CollectionViewFlags.IsDataInGroupOrder, isDataInGroupOrder);

            _temporaryGroup = new CollectionViewGroupRoot(this, isDataInGroupOrder);
            _group = new CollectionViewGroupRoot(this, false);
            _group.GroupDescriptionChanged += new EventHandler(OnGroupDescriptionChanged);
            _group.GroupDescriptions.CollectionChanged += new NotifyCollectionChangedEventHandler(OnGroupByChanged);

            CopySourceToInternalList();
            _trackingEnumerator = source.GetEnumerator();

            // set currency
            if (_internalList.Count > 0)
            {
                SetCurrent(_internalList[0], 0, 1);
            }
            else
            {
                SetCurrent(null, -1, 0);
            }

            // Set flag for whether the collection is empty
            SetFlag(CollectionViewFlags.CachedIsEmpty, Count == 0);

            // If the source doesn't raise collection change events, try to
            // detect changes by polling the enumerator
            _pollForChanges = !(source is INotifyCollectionChanged);

            // If we implement INotifyCollectionChanged
            if (!_pollForChanges)
            {
                //用具名委托保存订阅，才能在 Dispose 时解绑：
                //源集合常是 App/Service 级长生命周期对象，匿名 lambda 会把它反向钉住本视图及其绑定目标
                _sourceCollectionChanged = (sender, args) => { ProcessCollectionChanged(args); };
                ((INotifyCollectionChanged)source).CollectionChanged += _sourceCollectionChanged;
            }
        }
        private bool _disposed;
        private readonly NotifyCollectionChangedEventHandler _sourceCollectionChanged;
        /// <summary>
        /// 解除对源集合的订阅
        /// <para>视图被替换/页面关闭后应调用，否则长生命周期的源集合会一直持有本视图</para>
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            if (_sourceCollectionChanged != null && _sourceCollection is INotifyCollectionChanged notify)
            {
                notify.CollectionChanged -= _sourceCollectionChanged;
            }
        }

        #endregion Constructors

        ////------------------------------------------------------
        ////
        ////  Private Delegates
        ////
        ////------------------------------------------------------

        #region Private Delegates

        /// <summary>
        /// Delegate used to asynchronously trigger a page move.
        /// </summary>
        /// <param name="pageIndex">Requested page index</param>
        private delegate void RequestPageMoveDelegate(int pageIndex);

        #endregion Private Delegates

        ////------------------------------------------------------
        ////
        ////  Events
        ////
        ////------------------------------------------------------

        #region Events

        /// <summary>
        /// Raise this event when the (filtered) view changes
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// CollectionChanged event (per INotifyCollectionChanged).
        /// </summary>
        event NotifyCollectionChangedEventHandler INotifyCollectionChanged.CollectionChanged
        {
            add { CollectionChanged += value; }
            remove { CollectionChanged -= value; }
        }

        /// <summary>
        /// Raised when the CurrentItem property changed
        /// </summary>
        public event EventHandler CurrentChanged;

        /// <summary>
        /// Raised when the CurrentItem property is changing
        /// </summary>
        public event CurrentChangingEventHandler CurrentChanging;

        /// <summary>
        /// Raised when a page index change completed
        /// </summary>
        public event EventHandler<EventArgs> PageChanged;

        /// <summary>
        /// Raised when a page index change is requested
        /// </summary>
        public event EventHandler<PageChangingEventArgs> PageChanging;

        /// <summary>
        /// PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// PropertyChanged event (per INotifyPropertyChanged)
        /// </summary>
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }

        #endregion Events

        ////------------------------------------------------------
        ////
        ////  Enumerators
        ////
        ////------------------------------------------------------

        #region Enumerators

        /// <summary>
        /// Enum for CollectionViewFlags
        /// </summary>
        [Flags]
        private enum CollectionViewFlags
        {
            /// <summary>
            /// Whether the list of items (after applying the sort and filters, if any)
            /// is already in the correct order for grouping.
            /// </summary>
            IsDataInGroupOrder = 0x01,

            /// <summary>
            /// Whether the source collection is already sorted according to the SortDescriptions collection
            /// </summary>
            IsDataSorted = 0x02,

            /// <summary>
            /// Whether we should process the collection changed event
            /// </summary>
            ShouldProcessCollectionChanged = 0x04,

            /// <summary>
            /// Whether the current item is before the first
            /// </summary>
            IsCurrentBeforeFirst = 0x08,

            /// <summary>
            /// Whether the current item is after the last
            /// </summary>
            IsCurrentAfterLast = 0x10,

            /// <summary>
            /// Whether we need to refresh
            /// </summary>
            NeedsRefresh = 0x20,

            /// <summary>
            /// Whether we cache the IsEmpty value
            /// </summary>
            CachedIsEmpty = 0x40,

            /// <summary>
            /// Indicates whether a page index change is in process or not
            /// </summary>
            IsPageChanging = 0x80,

            /// <summary>
            /// Whether we need to move to another page after EndDefer
            /// </summary>
            IsMoveToPageDeferred = 0x100,

            /// <summary>
            /// Whether we need to update the PageSize after EndDefer
            /// </summary>
            IsUpdatePageSizeDeferred = 0x200
        }

        #endregion Enumerators

        ////------------------------------------------------------
        ////
        ////  Public Properties
        ////
        ////------------------------------------------------------
    }
}
