using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Paway.Helper;

namespace Paway.WPF
{
    /// <summary>
    /// 文本图标转换器
    /// </summary>
    public class FontImageExtension : MarkupExtension
    {
        /// <summary>
        /// 文本图标
        /// </summary>
        public FontImageType Type { get; set; }

        /// <summary>
        /// </summary>
        public FontImageExtension() { }
        /// <summary>
        /// </summary>
        public FontImageExtension(FontImageType type)
        {
            this.Type = type;
        }
        /// <summary>
        /// 转换器
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (service != null)
            {
                if (service.TargetObject is Control obj) obj.FontFamily = PConfig.FontAwesome;
                else if (service.TargetObject is TextBlock textBlock) textBlock.FontFamily = PConfig.FontAwesome;
            }
            return Type.Description();
        }
    }
    /// <summary>
    /// 文本图标值
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class CharAttribute : Attribute
    {
        /// <summary>
        /// 值
        /// </summary>
        public char Value { get; private set; }
        /// <summary>
        /// </summary>
        public CharAttribute(char value)
        {
            this.Value = value;
        }
    }
    /// <summary>
    /// 文本图标
    /// </summary>
    public enum FontImageType
    {
        /// <summary>
        /// </summary>
        [Description("\uf042")]
        Adjust,

        /// <summary>
        /// </summary>
        [Description("\uf170")]
        Adn,

        /// <summary>
        /// </summary>
        [Description("\uf037")]
        AlignCenter,

        /// <summary>
        /// </summary>
        [Description("\uf039")]
        AlignJustify,

        /// <summary>
        /// </summary>
        [Description("\uf036")]
        AlignLeft,

        /// <summary>
        /// </summary>
        [Description("\uf038")]
        AlignRight,

        /// <summary>
        /// </summary>
        [Description("\uf0f9")]
        Ambulance,

        /// <summary>
        /// </summary>
        [Description("\uf13d")]
        Anchor,

        /// <summary>
        /// </summary>
        [Description("\uf17b")]
        Android,

        /// <summary>
        /// </summary>
        [Description("\uf209")]
        Angellist,

        /// <summary>
        /// </summary>
        [Description("\uf103")]
        AngleDoubleDown,

        /// <summary>
        /// </summary>
        [Description("\uf100")]
        AngleDoubleLeft,

        /// <summary>
        /// </summary>
        [Description("\uf101")]
        AngleDoubleRight,

        /// <summary>
        /// </summary>
        [Description("\uf102")]
        AngleDoubleUp,

        /// <summary>
        /// </summary>
        [Description("\uf107")]
        AngleDown,

        /// <summary>
        /// </summary>
        [Description("\uf104")]
        AngleLeft,

        /// <summary>
        /// </summary>
        [Description("\uf105")]
        AngleRight,

        /// <summary>
        /// </summary>
        [Description("\uf106")]
        AngleUp,

        /// <summary>
        /// </summary>
        [Description("\uf179")]
        Apple,

        /// <summary>
        /// </summary>
        [Description("\uf187")]
        Archive,

        /// <summary>
        /// </summary>
        [Description("\uf1fe")]
        AreaChart,

        /// <summary>
        /// </summary>
        [Description("\uf0ab")]
        ArrowCircleDown,

        /// <summary>
        /// </summary>
        [Description("\uf0a8")]
        ArrowCircleLeft,

        /// <summary>
        /// </summary>
        [Description("\uf01a")]
        ArrowCircleODown,

        /// <summary>
        /// </summary>
        [Description("\uf190")]
        ArrowCircleOLeft,

        /// <summary>
        /// </summary>
        [Description("\uf18e")]
        ArrowCircleORight,

        /// <summary>
        /// </summary>
        [Description("\uf01b")]
        ArrowCircleOUp,

        /// <summary>
        /// </summary>
        [Description("\uf0a9")]
        ArrowCircleRight,

        /// <summary>
        /// </summary>
        [Description("\uf0aa")]
        ArrowCircleUp,

        /// <summary>
        /// </summary>
        [Description("\uf063")]
        ArrowDown,

        /// <summary>
        /// </summary>
        [Description("\uf060")]
        ArrowLeft,

        /// <summary>
        /// </summary>
        [Description("\uf061")]
        ArrowRight,

        /// <summary>
        /// </summary>
        [Description("\uf062")]
        ArrowUp,

        /// <summary>
        /// </summary>
        [Description("\uf047")]
        Arrows,

        /// <summary>
        /// </summary>
        [Description("\uf0b2")]
        ArrowsAlt,

        /// <summary>
        /// </summary>
        [Description("\uf07e")]
        ArrowsH,

        /// <summary>
        /// </summary>
        [Description("\uf07d")]
        ArrowsV,

        /// <summary>
        /// </summary>
        [Description("\uf069")]
        Asterisk,

        /// <summary>
        /// </summary>
        [Description("\uf1fa")]
        At,

        /// <summary>
        /// </summary>
        [Description("\uf1b9")]
        Automobile,

        /// <summary>
        /// </summary>
        [Description("\uf04a")]
        Backward,

        /// <summary>
        /// </summary>
        [Description("\uf05e")]
        Ban,

        /// <summary>
        /// </summary>
        [Description("\uf19c")]
        Bank,

        /// <summary>
        /// </summary>
        [Description("\uf080")]
        BarChart,

        /// <summary>
        /// </summary>
        [Description("\uf080")]
        BarChartO,

        /// <summary>
        /// </summary>
        [Description("\uf02a")]
        Barcode,

        /// <summary>
        /// </summary>
        [Description("\uf0c9")]
        Bars,

        /// <summary>
        /// </summary>
        [Description("\uf236")]
        Bed,

        /// <summary>
        /// </summary>
        [Description("\uf0fc")]
        Beer,

        /// <summary>
        /// </summary>
        [Description("\uf1b4")]
        Behance,

        /// <summary>
        /// </summary>
        [Description("\uf1b5")]
        BehanceSquare,

        /// <summary>
        /// </summary>
        [Description("\uf0f3")]
        Bell,

        /// <summary>
        /// </summary>
        [Description("\uf0a2")]
        BellO,

        /// <summary>
        /// </summary>
        [Description("\uf1f6")]
        BellSlash,

        /// <summary>
        /// </summary>
        [Description("\uf1f7")]
        BellSlashO,

        /// <summary>
        /// </summary>
        [Description("\uf206")]
        Bicycle,

        /// <summary>
        /// </summary>
        [Description("\uf1e5")]
        Binoculars,

        /// <summary>
        /// </summary>
        [Description("\uf1fd")]
        BirthdayCake,

        /// <summary>
        /// </summary>
        [Description("\uf171")]
        Bitbucket,

        /// <summary>
        /// </summary>
        [Description("\uf172")]
        BitbucketSquare,

        /// <summary>
        /// </summary>
        [Description("\uf15a")]
        Bitcoin,

        /// <summary>
        /// </summary>
        [Description("\uf032")]
        Bold,

        /// <summary>
        /// </summary>
        [Description("\uf0e7")]
        Bolt,

        /// <summary>
        /// </summary>
        [Description("\uf1e2")]
        Bomb,

        /// <summary>
        /// </summary>
        [Description("\uf02d")]
        Book,

        /// <summary>
        /// </summary>
        [Description("\uf02e")]
        Bookmark,

        /// <summary>
        /// </summary>
        [Description("\uf097")]
        BookmarkO,

        /// <summary>
        /// </summary>
        [Description("\uf0b1")]
        Briefcase,

        /// <summary>
        /// </summary>
        [Description("\uf15a")]
        Btc,

        /// <summary>
        /// </summary>
        [Description("\uf188")]
        Bug,

        /// <summary>
        /// </summary>
        [Description("\uf1ad")]
        Building,

        /// <summary>
        /// </summary>
        [Description("\uf0f7")]
        BuildingO,

        /// <summary>
        /// </summary>
        [Description("\uf0a1")]
        Bullhorn,

        /// <summary>
        /// </summary>
        [Description("\uf140")]
        Bullseye,

        /// <summary>
        /// </summary>
        [Description("\uf207")]
        Bus,

        /// <summary>
        /// </summary>
        [Description("\uf20d")]
        Buysellads,

        /// <summary>
        /// </summary>
        [Description("\uf1ba")]
        Cab,

        /// <summary>
        /// </summary>
        [Description("\uf1ec")]
        Calculator,

        /// <summary>
        /// </summary>
        [Description("\uf073")]
        Calendar,

        /// <summary>
        /// </summary>
        [Description("\uf133")]
        CalendarO,

        /// <summary>
        /// </summary>
        [Description("\uf030")]
        Camera,

        /// <summary>
        /// </summary>
        [Description("\uf083")]
        CameraRetro,

        /// <summary>
        /// </summary>
        [Description("\uf1b9")]
        Car,

        /// <summary>
        /// </summary>
        [Description("\uf0d7")]
        CaretDown,

        /// <summary>
        /// </summary>
        [Description("\uf0d9")]
        CaretLeft,

        /// <summary>
        /// </summary>
        [Description("\uf0da")]
        CaretRight,

        /// <summary>
        /// </summary>
        [Description("\uf150")]
        CaretSquareODown,

        /// <summary>
        /// </summary>
        [Description("\uf191")]
        CaretSquareOLeft,

        /// <summary>
        /// </summary>
        [Description("\uf152")]
        CaretSquareORight,

        /// <summary>
        /// </summary>
        [Description("\uf151")]
        CaretSquareOUp,

        /// <summary>
        /// </summary>
        [Description("\uf0d8")]
        CaretUp,

        /// <summary>
        /// </summary>
        [Description("\uf218")]
        CartArrowDown,

        /// <summary>
        /// </summary>
        [Description("\uf217")]
        CartPlus,

        /// <summary>
        /// </summary>
        [Description("\uf20a")]
        Cc,

        /// <summary>
        /// </summary>
        [Description("\uf1f3")]
        CcAmex,

        /// <summary>
        /// </summary>
        [Description("\uf1f2")]
        CcDiscover,

        /// <summary>
        /// </summary>
        [Description("\uf1f1")]
        CcMastercard,

        /// <summary>
        /// </summary>
        [Description("\uf1f4")]
        CcPaypal,

        /// <summary>
        /// </summary>
        [Description("\uf1f5")]
        CcStripe,

        /// <summary>
        /// </summary>
        [Description("\uf1f0")]
        CcVisa,

        /// <summary>
        /// </summary>
        [Description("\uf0a3")]
        Certificate,

        /// <summary>
        /// </summary>
        [Description("\uf0c1")]
        Chain,

        /// <summary>
        /// </summary>
        [Description("\uf127")]
        ChainBroken,

        /// <summary>
        /// </summary>
        [Description("\uf00c")]
        Check,

        /// <summary>
        /// </summary>
        [Description("\uf058")]
        CheckCircle,

        /// <summary>
        /// </summary>
        [Description("\uf05d")]
        CheckCircleO,

        /// <summary>
        /// </summary>
        [Description("\uf14a")]
        CheckSquare,

        /// <summary>
        /// </summary>
        [Description("\uf046")]
        CheckSquareO,

        /// <summary>
        /// </summary>
        [Description("\uf13a")]
        ChevronCircleDown,

        /// <summary>
        /// </summary>
        [Description("\uf137")]
        ChevronCircleLeft,

        /// <summary>
        /// </summary>
        [Description("\uf138")]
        ChevronCircleRight,

        /// <summary>
        /// </summary>
        [Description("\uf139")]
        ChevronCircleUp,

        /// <summary>
        /// </summary>
        [Description("\uf078")]
        ChevronDown,

        /// <summary>
        /// </summary>
        [Description("\uf053")]
        ChevronLeft,

        /// <summary>
        /// </summary>
        [Description("\uf054")]
        ChevronRight,

        /// <summary>
        /// </summary>
        [Description("\uf077")]
        ChevronUp,

        /// <summary>
        /// </summary>
        [Description("\uf1ae")]
        Child,

        /// <summary>
        /// </summary>
        [Description("\uf111")]
        Circle,

        /// <summary>
        /// </summary>
        [Description("\uf10c")]
        CircleO,

        /// <summary>
        /// </summary>
        [Description("\uf1ce")]
        CircleONotch,

        /// <summary>
        /// </summary>
        [Description("\uf1db")]
        CircleThin,

        /// <summary>
        /// </summary>
        [Description("\uf0ea")]
        Clipboard,

        /// <summary>
        /// </summary>
        [Description("\uf017")]
        ClockO,

        /// <summary>
        /// </summary>
        [Description("\uf00d")]
        Close,

        /// <summary>
        /// </summary>
        [Description("\uf0c2")]
        Cloud,

        /// <summary>
        /// </summary>
        [Description("\uf0ed")]
        CloudDownload,

        /// <summary>
        /// </summary>
        [Description("\uf0ee")]
        CloudUpload,

        /// <summary>
        /// </summary>
        [Description("\uf157")]
        Cny,

        /// <summary>
        /// </summary>
        [Description("\uf121")]
        Code,

        /// <summary>
        /// </summary>
        [Description("\uf126")]
        CodeFork,

        /// <summary>
        /// </summary>
        [Description("\uf1cb")]
        Codepen,

        /// <summary>
        /// </summary>
        [Description("\uf0f4")]
        Coffee,

        /// <summary>
        /// </summary>
        [Description("\uf013")]
        Cog,

        /// <summary>
        /// </summary>
        [Description("\uf085")]
        Cogs,

        /// <summary>
        /// </summary>
        [Description("\uf0db")]
        Columns,

        /// <summary>
        /// </summary>
        [Description("\uf075")]
        Comment,

        /// <summary>
        /// </summary>
        [Description("\uf0e5")]
        CommentO,

        /// <summary>
        /// </summary>
        [Description("\uf086")]
        Comments,

        /// <summary>
        /// </summary>
        [Description("\uf0e6")]
        CommentsO,

        /// <summary>
        /// </summary>
        [Description("\uf14e")]
        Compass,

        /// <summary>
        /// </summary>
        [Description("\uf066")]
        Compress,

        /// <summary>
        /// </summary>
        [Description("\uf20e")]
        Connectdevelop,

        /// <summary>
        /// </summary>
        [Description("\uf0c5")]
        Copy,

        /// <summary>
        /// </summary>
        [Description("\uf1f9")]
        Copyright,

        /// <summary>
        /// </summary>
        [Description("\uf09d")]
        CreditCard,

        /// <summary>
        /// </summary>
        [Description("\uf125")]
        Crop,

        /// <summary>
        /// </summary>
        [Description("\uf05b")]
        Crosshairs,

        /// <summary>
        /// </summary>
        [Description("\uf13c")]
        Css3,

        /// <summary>
        /// </summary>
        [Description("\uf1b2")]
        Cube,

        /// <summary>
        /// </summary>
        [Description("\uf1b3")]
        Cubes,

        /// <summary>
        /// </summary>
        [Description("\uf0c4")]
        Cut,

        /// <summary>
        /// </summary>
        [Description("\uf0f5")]
        Cutlery,

        /// <summary>
        /// </summary>
        [Description("\uf0e4")]
        Dashboard,

        /// <summary>
        /// </summary>
        [Description("\uf210")]
        Dashcube,

        /// <summary>
        /// </summary>
        [Description("\uf1c0")]
        Database,

        /// <summary>
        /// </summary>
        [Description("\uf03b")]
        Dedent,

        /// <summary>
        /// </summary>
        [Description("\uf1a5")]
        Delicious,

        /// <summary>
        /// </summary>
        [Description("\uf108")]
        Desktop,

        /// <summary>
        /// </summary>
        [Description("\uf1bd")]
        Deviantart,

        /// <summary>
        /// </summary>
        [Description("\uf219")]
        Diamond,

        /// <summary>
        /// </summary>
        [Description("\uf1a6")]
        Digg,

        /// <summary>
        /// </summary>
        [Description("\uf155")]
        Dollar,

        /// <summary>
        /// </summary>
        [Description("\uf192")]
        DotCircleO,

        /// <summary>
        /// </summary>
        [Description("\uf019")]
        Download,

        /// <summary>
        /// </summary>
        [Description("\uf17d")]
        Dribbble,

        /// <summary>
        /// </summary>
        [Description("\uf16b")]
        Dropbox,

        /// <summary>
        /// </summary>
        [Description("\uf1a9")]
        Drupal,

        /// <summary>
        /// </summary>
        [Description("\uf044")]
        Edit,

        /// <summary>
        /// </summary>
        [Description("\uf052")]
        Eject,

        /// <summary>
        /// </summary>
        [Description("\uf141")]
        EllipsisH,

        /// <summary>
        /// </summary>
        [Description("\uf142")]
        EllipsisV,

        /// <summary>
        /// </summary>
        [Description("\uf1d1")]
        Empire,

        /// <summary>
        /// </summary>
        [Description("\uf0e0")]
        Envelope,

        /// <summary>
        /// </summary>
        [Description("\uf003")]
        EnvelopeO,

        /// <summary>
        /// </summary>
        [Description("\uf199")]
        EnvelopeSquare,

        /// <summary>
        /// </summary>
        [Description("\uf12d")]
        Eraser,

        /// <summary>
        /// </summary>
        [Description("\uf153")]
        Eur,

        /// <summary>
        /// </summary>
        [Description("\uf153")]
        Euro,

        /// <summary>
        /// </summary>
        [Description("\uf0ec")]
        Exchange,

        /// <summary>
        /// </summary>
        [Description("\uf12a")]
        Exclamation,

        /// <summary>
        /// </summary>
        [Description("\uf06a")]
        ExclamationCircle,

        /// <summary>
        /// </summary>
        [Description("\uf071")]
        ExclamationTriangle,

        /// <summary>
        /// </summary>
        [Description("\uf065")]
        Expand,

        /// <summary>
        /// </summary>
        [Description("\uf08e")]
        ExternalLink,

        /// <summary>
        /// </summary>
        [Description("\uf14c")]
        ExternalLinkSquare,

        /// <summary>
        /// </summary>
        [Description("\uf06e")]
        Eye,

        /// <summary>
        /// </summary>
        [Description("\uf070")]
        EyeSlash,

        /// <summary>
        /// </summary>
        [Description("\uf1fb")]
        Eyedropper,

        /// <summary>
        /// </summary>
        [Description("\uf09a")]
        Facebook,

        /// <summary>
        /// </summary>
        [Description("\uf09a")]
        FacebookF,

        /// <summary>
        /// </summary>
        [Description("\uf230")]
        FacebookOfficial,

        /// <summary>
        /// </summary>
        [Description("\uf082")]
        FacebookSquare,

        /// <summary>
        /// </summary>
        [Description("\uf049")]
        FastBackward,

        /// <summary>
        /// </summary>
        [Description("\uf050")]
        FastForward,

        /// <summary>
        /// </summary>
        [Description("\uf1ac")]
        Fax,

        /// <summary>
        /// </summary>
        [Description("\uf182")]
        Female,

        /// <summary>
        /// </summary>
        [Description("\uf0fb")]
        FighterJet,

        /// <summary>
        /// </summary>
        [Description("\uf15b")]
        File,

        /// <summary>
        /// </summary>
        [Description("\uf1c6")]
        FileArchiveO,

        /// <summary>
        /// </summary>
        [Description("\uf1c7")]
        FileAudioO,

        /// <summary>
        /// </summary>
        [Description("\uf1c9")]
        FileCodeO,

        /// <summary>
        /// </summary>
        [Description("\uf1c3")]
        FileExcelO,

        /// <summary>
        /// </summary>
        [Description("\uf1c5")]
        FileImageO,

        /// <summary>
        /// </summary>
        [Description("\uf1c8")]
        FileMovieO,

        /// <summary>
        /// </summary>
        [Description("\uf016")]
        FileO,

        /// <summary>
        /// </summary>
        [Description("\uf1c1")]
        FilePdfO,

        /// <summary>
        /// </summary>
        [Description("\uf1c5")]
        FilePhotoO,

        /// <summary>
        /// </summary>
        [Description("\uf1c5")]
        FilePictureO,

        /// <summary>
        /// </summary>
        [Description("\uf1c4")]
        FilePowerpointO,

        /// <summary>
        /// </summary>
        [Description("\uf1c7")]
        FileSoundO,

        /// <summary>
        /// </summary>
        [Description("\uf15c")]
        FileText,

        /// <summary>
        /// </summary>
        [Description("\uf0f6")]
        FileTextO,

        /// <summary>
        /// </summary>
        [Description("\uf1c8")]
        FileVideoO,

        /// <summary>
        /// </summary>
        [Description("\uf1c2")]
        FileWordO,

        /// <summary>
        /// </summary>
        [Description("\uf1c6")]
        FileZipO,

        /// <summary>
        /// </summary>
        [Description("\uf0c5")]
        FilesO,

        /// <summary>
        /// </summary>
        [Description("\uf008")]
        Film,

        /// <summary>
        /// </summary>
        [Description("\uf0b0")]
        Filter,

        /// <summary>
        /// </summary>
        [Description("\uf06d")]
        Fire,

        /// <summary>
        /// </summary>
        [Description("\uf134")]
        FireExtinguisher,

        /// <summary>
        /// </summary>
        [Description("\uf024")]
        Flag,

        /// <summary>
        /// </summary>
        [Description("\uf11e")]
        FlagCheckered,

        /// <summary>
        /// </summary>
        [Description("\uf11d")]
        FlagO,

        /// <summary>
        /// </summary>
        [Description("\uf0e7")]
        Flash,

        /// <summary>
        /// </summary>
        [Description("\uf0c3")]
        Flask,

        /// <summary>
        /// </summary>
        [Description("\uf16e")]
        Flickr,

        /// <summary>
        /// </summary>
        [Description("\uf0c7")]
        FloppyO,

        /// <summary>
        /// </summary>
        [Description("\uf07b")]
        Folder,

        /// <summary>
        /// </summary>
        [Description("\uf114")]
        FolderO,

        /// <summary>
        /// </summary>
        [Description("\uf07c")]
        FolderOpen,

        /// <summary>
        /// </summary>
        [Description("\uf115")]
        FolderOpenO,

        /// <summary>
        /// </summary>
        [Description("\uf031")]
        Font,

        /// <summary>
        /// </summary>
        [Description("\uf211")]
        Forumbee,

        /// <summary>
        /// </summary>
        [Description("\uf04e")]
        Forward,

        /// <summary>
        /// </summary>
        [Description("\uf180")]
        Foursquare,

        /// <summary>
        /// </summary>
        [Description("\uf119")]
        FrownO,

        /// <summary>
        /// </summary>
        [Description("\uf1e3")]
        FutbolO,

        /// <summary>
        /// </summary>
        [Description("\uf11b")]
        Gamepad,

        /// <summary>
        /// </summary>
        [Description("\uf0e3")]
        Gavel,

        /// <summary>
        /// </summary>
        [Description("\uf154")]
        Gbp,

        /// <summary>
        /// </summary>
        [Description("\uf1d1")]
        Ge,

        /// <summary>
        /// </summary>
        [Description("\uf013")]
        Gear,

        /// <summary>
        /// </summary>
        [Description("\uf085")]
        Gears,

        /// <summary>
        /// </summary>
        [Description("\uf1db")]
        Genderless,

        /// <summary>
        /// </summary>
        [Description("\uf06b")]
        Gift,

        /// <summary>
        /// </summary>
        [Description("\uf1d3")]
        Git,

        /// <summary>
        /// </summary>
        [Description("\uf1d2")]
        GitSquare,

        /// <summary>
        /// </summary>
        [Description("\uf09b")]
        Github,

        /// <summary>
        /// </summary>
        [Description("\uf113")]
        GithubAlt,

        /// <summary>
        /// </summary>
        [Description("\uf092")]
        GithubSquare,

        /// <summary>
        /// </summary>
        [Description("\uf184")]
        Gittip,

        /// <summary>
        /// </summary>
        [Description("\uf000")]
        Glass,

        /// <summary>
        /// </summary>
        [Description("\uf0ac")]
        Globe,

        /// <summary>
        /// </summary>
        [Description("\uf1a0")]
        Google,

        /// <summary>
        /// </summary>
        [Description("\uf0d5")]
        GooglePlus,

        /// <summary>
        /// </summary>
        [Description("\uf0d4")]
        GooglePlusSquare,

        /// <summary>
        /// </summary>
        [Description("\uf1ee")]
        GoogleWallet,

        /// <summary>
        /// </summary>
        [Description("\uf19d")]
        GraduationCap,

        /// <summary>
        /// </summary>
        [Description("\uf184")]
        Gratipay,

        /// <summary>
        /// </summary>
        [Description("\uf0c0")]
        Group,

        /// <summary>
        /// </summary>
        [Description("\uf0fd")]
        HSquare,

        /// <summary>
        /// </summary>
        [Description("\uf1d4")]
        HackerNews,

        /// <summary>
        /// </summary>
        [Description("\uf0a7")]
        HandODown,

        /// <summary>
        /// </summary>
        [Description("\uf0a5")]
        HandOLeft,

        /// <summary>
        /// </summary>
        [Description("\uf0a4")]
        HandORight,

        /// <summary>
        /// </summary>
        [Description("\uf0a6")]
        HandOUp,

        /// <summary>
        /// </summary>
        [Description("\uf0a0")]
        HddO,

        /// <summary>
        /// </summary>
        [Description("\uf1dc")]
        Header,

        /// <summary>
        /// </summary>
        [Description("\uf025")]
        Headphones,

        /// <summary>
        /// </summary>
        [Description("\uf004")]
        Heart,

        /// <summary>
        /// </summary>
        [Description("\uf08a")]
        HeartO,

        /// <summary>
        /// </summary>
        [Description("\uf21e")]
        Heartbeat,

        /// <summary>
        /// </summary>
        [Description("\uf1da")]
        History,

        /// <summary>
        /// </summary>
        [Description("\uf015")]
        Home,

        /// <summary>
        /// </summary>
        [Description("\uf0f8")]
        HospitalO,

        /// <summary>
        /// </summary>
        [Description("\uf236")]
        Hotel,

        /// <summary>
        /// </summary>
        [Description("\uf13b")]
        Html5,

        /// <summary>
        /// </summary>
        [Description("\uf20b")]
        Ils,

        /// <summary>
        /// </summary>
        [Description("\uf03e")]
        Image,

        /// <summary>
        /// </summary>
        [Description("\uf01c")]
        Inbox,

        /// <summary>
        /// </summary>
        [Description("\uf03c")]
        Indent,

        /// <summary>
        /// </summary>
        [Description("\uf129")]
        Info,

        /// <summary>
        /// </summary>
        [Description("\uf05a")]
        InfoCircle,

        /// <summary>
        /// </summary>
        [Description("\uf156")]
        Inr,

        /// <summary>
        /// </summary>
        [Description("\uf16d")]
        Instagram,

        /// <summary>
        /// </summary>
        [Description("\uf19c")]
        Institution,

        /// <summary>
        /// </summary>
        [Description("\uf208")]
        Ioxhost,

        /// <summary>
        /// </summary>
        [Description("\uf033")]
        Italic,

        /// <summary>
        /// </summary>
        [Description("\uf1aa")]
        Joomla,

        /// <summary>
        /// </summary>
        [Description("\uf157")]
        Jpy,

        /// <summary>
        /// </summary>
        [Description("\uf1cc")]
        Jsfiddle,

        /// <summary>
        /// </summary>
        [Description("\uf084")]
        Key,

        /// <summary>
        /// </summary>
        [Description("\uf11c")]
        KeyboardO,

        /// <summary>
        /// </summary>
        [Description("\uf159")]
        Krw,

        /// <summary>
        /// </summary>
        [Description("\uf1ab")]
        Language,

        /// <summary>
        /// </summary>
        [Description("\uf109")]
        Laptop,

        /// <summary>
        /// </summary>
        [Description("\uf202")]
        Lastfm,

        /// <summary>
        /// </summary>
        [Description("\uf203")]
        LastfmSquare,

        /// <summary>
        /// </summary>
        [Description("\uf06c")]
        Leaf,

        /// <summary>
        /// </summary>
        [Description("\uf212")]
        Leanpub,

        /// <summary>
        /// </summary>
        [Description("\uf0e3")]
        Legal,

        /// <summary>
        /// </summary>
        [Description("\uf094")]
        LemonO,

        /// <summary>
        /// </summary>
        [Description("\uf149")]
        LevelDown,

        /// <summary>
        /// </summary>
        [Description("\uf148")]
        LevelUp,

        /// <summary>
        /// </summary>
        [Description("\uf1cd")]
        LifeBouy,

        /// <summary>
        /// </summary>
        [Description("\uf1cd")]
        LifeBuoy,

        /// <summary>
        /// </summary>
        [Description("\uf1cd")]
        LifeRing,

        /// <summary>
        /// </summary>
        [Description("\uf1cd")]
        LifeSaver,

        /// <summary>
        /// </summary>
        [Description("\uf0eb")]
        LightbulbO,

        /// <summary>
        /// </summary>
        [Description("\uf201")]
        LineChart,

        /// <summary>
        /// </summary>
        [Description("\uf0c1")]
        Link,

        /// <summary>
        /// </summary>
        [Description("\uf0e1")]
        Linkedin,

        /// <summary>
        /// </summary>
        [Description("\uf08c")]
        LinkedinSquare,

        /// <summary>
        /// </summary>
        [Description("\uf17c")]
        Linux,

        /// <summary>
        /// </summary>
        [Description("\uf03a")]
        List,

        /// <summary>
        /// </summary>
        [Description("\uf022")]
        ListAlt,

        /// <summary>
        /// </summary>
        [Description("\uf0cb")]
        ListOl,

        /// <summary>
        /// </summary>
        [Description("\uf0ca")]
        ListUl,

        /// <summary>
        /// </summary>
        [Description("\uf124")]
        LocationArrow,

        /// <summary>
        /// </summary>
        [Description("\uf023")]
        Lock,

        /// <summary>
        /// </summary>
        [Description("\uf175")]
        LongArrowDown,

        /// <summary>
        /// </summary>
        [Description("\uf177")]
        LongArrowLeft,

        /// <summary>
        /// </summary>
        [Description("\uf178")]
        LongArrowRight,

        /// <summary>
        /// </summary>
        [Description("\uf176")]
        LongArrowUp,

        /// <summary>
        /// </summary>
        [Description("\uf0d0")]
        Magic,

        /// <summary>
        /// </summary>
        [Description("\uf076")]
        Magnet,

        /// <summary>
        /// </summary>
        [Description("\uf064")]
        MailForward,

        /// <summary>
        /// </summary>
        [Description("\uf112")]
        MailReply,

        /// <summary>
        /// </summary>
        [Description("\uf122")]
        MailReplyAll,

        /// <summary>
        /// </summary>
        [Description("\uf183")]
        Male,

        /// <summary>
        /// </summary>
        [Description("\uf041")]
        MapMarker,

        /// <summary>
        /// </summary>
        [Description("\uf222")]
        Mars,

        /// <summary>
        /// </summary>
        [Description("\uf227")]
        MarsDouble,

        /// <summary>
        /// </summary>
        [Description("\uf229")]
        MarsStroke,

        /// <summary>
        /// </summary>
        [Description("\uf22b")]
        MarsStrokeH,

        /// <summary>
        /// </summary>
        [Description("\uf22a")]
        MarsStrokeV,

        /// <summary>
        /// </summary>
        [Description("\uf136")]
        Maxcdn,

        /// <summary>
        /// </summary>
        [Description("\uf20c")]
        Meanpath,

        /// <summary>
        /// </summary>
        [Description("\uf23a")]
        Medium,

        /// <summary>
        /// </summary>
        [Description("\uf0fa")]
        Medkit,

        /// <summary>
        /// </summary>
        [Description("\uf11a")]
        MehO,

        /// <summary>
        /// </summary>
        [Description("\uf223")]
        Mercury,

        /// <summary>
        /// </summary>
        [Description("\uf130")]
        Microphone,

        /// <summary>
        /// </summary>
        [Description("\uf131")]
        MicrophoneSlash,

        /// <summary>
        /// </summary>
        [Description("\uf068")]
        Minus,

        /// <summary>
        /// </summary>
        [Description("\uf056")]
        MinusCircle,

        /// <summary>
        /// </summary>
        [Description("\uf146")]
        MinusSquare,

        /// <summary>
        /// </summary>
        [Description("\uf147")]
        MinusSquareO,

        /// <summary>
        /// </summary>
        [Description("\uf10b")]
        Mobile,

        /// <summary>
        /// </summary>
        [Description("\uf10b")]
        MobilePhone,

        /// <summary>
        /// </summary>
        [Description("\uf0d6")]
        Money,

        /// <summary>
        /// </summary>
        [Description("\uf186")]
        MoonO,

        /// <summary>
        /// </summary>
        [Description("\uf19d")]
        MortarBoard,

        /// <summary>
        /// </summary>
        [Description("\uf21c")]
        Motorcycle,

        /// <summary>
        /// </summary>
        [Description("\uf001")]
        Music,

        /// <summary>
        /// </summary>
        [Description("\uf0c9")]
        Navicon,

        /// <summary>
        /// </summary>
        [Description("\uf22c")]
        Neuter,

        /// <summary>
        /// </summary>
        [Description("\uf1ea")]
        NewspaperO,

        /// <summary>
        /// </summary>
        [Description("\uf19b")]
        Openid,

        /// <summary>
        /// </summary>
        [Description("\uf03b")]
        Outdent,

        /// <summary>
        /// </summary>
        [Description("\uf18c")]
        Pagelines,

        /// <summary>
        /// </summary>
        [Description("\uf1fc")]
        PaintBrush,

        /// <summary>
        /// </summary>
        [Description("\uf1d8")]
        PaperPlane,

        /// <summary>
        /// </summary>
        [Description("\uf1d9")]
        PaperPlaneO,

        /// <summary>
        /// </summary>
        [Description("\uf0c6")]
        Paperclip,

        /// <summary>
        /// </summary>
        [Description("\uf1dd")]
        Paragraph,

        /// <summary>
        /// </summary>
        [Description("\uf0ea")]
        Paste,

        /// <summary>
        /// </summary>
        [Description("\uf04c")]
        Pause,

        /// <summary>
        /// </summary>
        [Description("\uf1b0")]
        Paw,

        /// <summary>
        /// </summary>
        [Description("\uf1ed")]
        Paypal,

        /// <summary>
        /// </summary>
        [Description("\uf040")]
        Pencil,

        /// <summary>
        /// </summary>
        [Description("\uf14b")]
        PencilSquare,

        /// <summary>
        /// </summary>
        [Description("\uf044")]
        PencilSquareO,

        /// <summary>
        /// </summary>
        [Description("\uf095")]
        Phone,

        /// <summary>
        /// </summary>
        [Description("\uf098")]
        PhoneSquare,

        /// <summary>
        /// </summary>
        [Description("\uf03e")]
        Photo,

        /// <summary>
        /// </summary>
        [Description("\uf03e")]
        PictureO,

        /// <summary>
        /// </summary>
        [Description("\uf200")]
        PieChart,

        /// <summary>
        /// </summary>
        [Description("\uf1a7")]
        PiedPiper,

        /// <summary>
        /// </summary>
        [Description("\uf1a8")]
        PiedPiperAlt,

        /// <summary>
        /// </summary>
        [Description("\uf0d2")]
        Pinterest,

        /// <summary>
        /// </summary>
        [Description("\uf231")]
        PinterestP,

        /// <summary>
        /// </summary>
        [Description("\uf0d3")]
        PinterestSquare,

        /// <summary>
        /// </summary>
        [Description("\uf072")]
        Plane,

        /// <summary>
        /// </summary>
        [Description("\uf04b")]
        Play,

        /// <summary>
        /// </summary>
        [Description("\uf144")]
        PlayCircle,

        /// <summary>
        /// </summary>
        [Description("\uf01d")]
        PlayCircleO,

        /// <summary>
        /// </summary>
        [Description("\uf1e6")]
        Plug,

        /// <summary>
        /// </summary>
        [Description("\uf067")]
        Plus,

        /// <summary>
        /// </summary>
        [Description("\uf055")]
        PlusCircle,

        /// <summary>
        /// </summary>
        [Description("\uf0fe")]
        PlusSquare,

        /// <summary>
        /// </summary>
        [Description("\uf196")]
        PlusSquareO,

        /// <summary>
        /// </summary>
        [Description("\uf011")]
        PowerOff,

        /// <summary>
        /// </summary>
        [Description("\uf02f")]
        Print,

        /// <summary>
        /// </summary>
        [Description("\uf12e")]
        PuzzlePiece,

        /// <summary>
        /// </summary>
        [Description("\uf1d6")]
        Qq,

        /// <summary>
        /// </summary>
        [Description("\uf029")]
        Qrcode,

        /// <summary>
        /// </summary>
        [Description("\uf128")]
        Question,

        /// <summary>
        /// </summary>
        [Description("\uf059")]
        QuestionCircle,

        /// <summary>
        /// </summary>
        [Description("\uf10d")]
        QuoteLeft,

        /// <summary>
        /// </summary>
        [Description("\uf10e")]
        QuoteRight,

        /// <summary>
        /// </summary>
        [Description("\uf1d0")]
        Ra,

        /// <summary>
        /// </summary>
        [Description("\uf074")]
        Random,

        /// <summary>
        /// </summary>
        [Description("\uf1d0")]
        Rebel,

        /// <summary>
        /// </summary>
        [Description("\uf1b8")]
        Recycle,

        /// <summary>
        /// </summary>
        [Description("\uf1a1")]
        Reddit,

        /// <summary>
        /// </summary>
        [Description("\uf1a2")]
        RedditSquare,

        /// <summary>
        /// </summary>
        [Description("\uf021")]
        Refresh,

        /// <summary>
        /// </summary>
        [Description("\uf00d")]
        Remove,

        /// <summary>
        /// </summary>
        [Description("\uf18b")]
        Renren,

        /// <summary>
        /// </summary>
        [Description("\uf0c9")]
        Reorder,

        /// <summary>
        /// </summary>
        [Description("\uf01e")]
        Repeat,

        /// <summary>
        /// </summary>
        [Description("\uf112")]
        Reply,

        /// <summary>
        /// </summary>
        [Description("\uf122")]
        ReplyAll,

        /// <summary>
        /// </summary>
        [Description("\uf079")]
        Retweet,

        /// <summary>
        /// </summary>
        [Description("\uf157")]
        Rmb,

        /// <summary>
        /// </summary>
        [Description("\uf018")]
        Road,

        /// <summary>
        /// </summary>
        [Description("\uf135")]
        Rocket,

        /// <summary>
        /// </summary>
        [Description("\uf0e2")]
        RotateLeft,

        /// <summary>
        /// </summary>
        [Description("\uf01e")]
        RotateRight,

        /// <summary>
        /// </summary>
        [Description("\uf158")]
        Rouble,

        /// <summary>
        /// </summary>
        [Description("\uf09e")]
        Rss,

        /// <summary>
        /// </summary>
        [Description("\uf143")]
        RssSquare,

        /// <summary>
        /// </summary>
        [Description("\uf158")]
        Rub,

        /// <summary>
        /// </summary>
        [Description("\uf158")]
        Ruble,

        /// <summary>
        /// </summary>
        [Description("\uf156")]
        Rupee,

        /// <summary>
        /// </summary>
        [Description("\uf0c7")]
        Save,

        /// <summary>
        /// </summary>
        [Description("\uf0c4")]
        Scissors,

        /// <summary>
        /// </summary>
        [Description("\uf002")]
        Search,

        /// <summary>
        /// </summary>
        [Description("\uf010")]
        SearchMinus,

        /// <summary>
        /// </summary>
        [Description("\uf00e")]
        SearchPlus,

        /// <summary>
        /// </summary>
        [Description("\uf213")]
        Sellsy,

        /// <summary>
        /// </summary>
        [Description("\uf1d8")]
        Send,

        /// <summary>
        /// </summary>
        [Description("\uf1d9")]
        SendO,

        /// <summary>
        /// </summary>
        [Description("\uf233")]
        Server,

        /// <summary>
        /// </summary>
        [Description("\uf064")]
        Share,

        /// <summary>
        /// </summary>
        [Description("\uf1e0")]
        ShareAlt,

        /// <summary>
        /// </summary>
        [Description("\uf1e1")]
        ShareAltSquare,

        /// <summary>
        /// </summary>
        [Description("\uf14d")]
        ShareSquare,

        /// <summary>
        /// </summary>
        [Description("\uf045")]
        ShareSquareO,

        /// <summary>
        /// </summary>
        [Description("\uf20b")]
        Shekel,

        /// <summary>
        /// </summary>
        [Description("\uf20b")]
        Sheqel,

        /// <summary>
        /// </summary>
        [Description("\uf132")]
        Shield,

        /// <summary>
        /// </summary>
        [Description("\uf21a")]
        Ship,

        /// <summary>
        /// </summary>
        [Description("\uf214")]
        Shirtsinbulk,

        /// <summary>
        /// </summary>
        [Description("\uf07a")]
        ShoppingCart,

        /// <summary>
        /// </summary>
        [Description("\uf090")]
        SignIn,

        /// <summary>
        /// </summary>
        [Description("\uf08b")]
        SignOut,

        /// <summary>
        /// </summary>
        [Description("\uf012")]
        Signal,

        /// <summary>
        /// </summary>
        [Description("\uf215")]
        Simplybuilt,

        /// <summary>
        /// </summary>
        [Description("\uf0e8")]
        Sitemap,

        /// <summary>
        /// </summary>
        [Description("\uf216")]
        Skyatlas,

        /// <summary>
        /// </summary>
        [Description("\uf17e")]
        Skype,

        /// <summary>
        /// </summary>
        [Description("\uf198")]
        Slack,

        /// <summary>
        /// </summary>
        [Description("\uf1de")]
        Sliders,

        /// <summary>
        /// </summary>
        [Description("\uf1e7")]
        Slideshare,

        /// <summary>
        /// </summary>
        [Description("\uf118")]
        SmileO,

        /// <summary>
        /// </summary>
        [Description("\uf1e3")]
        SoccerBallO,

        /// <summary>
        /// </summary>
        [Description("\uf0dc")]
        Sort,

        /// <summary>
        /// </summary>
        [Description("\uf15d")]
        SortAlphaAsc,

        /// <summary>
        /// </summary>
        [Description("\uf15e")]
        SortAlphaDesc,

        /// <summary>
        /// </summary>
        [Description("\uf160")]
        SortAmountAsc,

        /// <summary>
        /// </summary>
        [Description("\uf161")]
        SortAmountDesc,

        /// <summary>
        /// </summary>
        [Description("\uf0de")]
        SortAsc,

        /// <summary>
        /// </summary>
        [Description("\uf0dd")]
        SortDesc,

        /// <summary>
        /// </summary>
        [Description("\uf0dd")]
        SortDown,

        /// <summary>
        /// </summary>
        [Description("\uf162")]
        SortNumericAsc,

        /// <summary>
        /// </summary>
        [Description("\uf163")]
        SortNumericDesc,

        /// <summary>
        /// </summary>
        [Description("\uf0de")]
        SortUp,

        /// <summary>
        /// </summary>
        [Description("\uf1be")]
        Soundcloud,

        /// <summary>
        /// </summary>
        [Description("\uf197")]
        SpaceShuttle,

        /// <summary>
        /// </summary>
        [Description("\uf110")]
        Spinner,

        /// <summary>
        /// </summary>
        [Description("\uf1b1")]
        Spoon,

        /// <summary>
        /// </summary>
        [Description("\uf1bc")]
        Spotify,

        /// <summary>
        /// </summary>
        [Description("\uf0c8")]
        Square,

        /// <summary>
        /// </summary>
        [Description("\uf096")]
        SquareO,

        /// <summary>
        /// </summary>
        [Description("\uf18d")]
        StackExchange,

        /// <summary>
        /// </summary>
        [Description("\uf16c")]
        StackOverflow,

        /// <summary>
        /// </summary>
        [Description("\uf005")]
        Star,

        /// <summary>
        /// </summary>
        [Description("\uf089")]
        StarHalf,

        /// <summary>
        /// </summary>
        [Description("\uf123")]
        StarHalfEmpty,

        /// <summary>
        /// </summary>
        [Description("\uf123")]
        StarHalfFull,

        /// <summary>
        /// </summary>
        [Description("\uf123")]
        StarHalfO,

        /// <summary>
        /// </summary>
        [Description("\uf006")]
        StarO,

        /// <summary>
        /// </summary>
        [Description("\uf1b6")]
        Steam,

        /// <summary>
        /// </summary>
        [Description("\uf1b7")]
        SteamSquare,

        /// <summary>
        /// </summary>
        [Description("\uf048")]
        StepBackward,

        /// <summary>
        /// </summary>
        [Description("\uf051")]
        StepForward,

        /// <summary>
        /// </summary>
        [Description("\uf0f1")]
        Stethoscope,

        /// <summary>
        /// </summary>
        [Description("\uf04d")]
        Stop,

        /// <summary>
        /// </summary>
        [Description("\uf21d")]
        StreetView,

        /// <summary>
        /// </summary>
        [Description("\uf0cc")]
        Strikethrough,

        /// <summary>
        /// </summary>
        [Description("\uf1a4")]
        Stumbleupon,

        /// <summary>
        /// </summary>
        [Description("\uf1a3")]
        StumbleuponCircle,

        /// <summary>
        /// </summary>
        [Description("\uf12c")]
        Subscript,

        /// <summary>
        /// </summary>
        [Description("\uf239")]
        Subway,

        /// <summary>
        /// </summary>
        [Description("\uf0f2")]
        Suitcase,

        /// <summary>
        /// </summary>
        [Description("\uf185")]
        SunO,

        /// <summary>
        /// </summary>
        [Description("\uf12b")]
        Superscript,

        /// <summary>
        /// </summary>
        [Description("\uf1cd")]
        Support,

        /// <summary>
        /// </summary>
        [Description("\uf0ce")]
        Table,

        /// <summary>
        /// </summary>
        [Description("\uf10a")]
        Tablet,

        /// <summary>
        /// </summary>
        [Description("\uf0e4")]
        Tachometer,

        /// <summary>
        /// </summary>
        [Description("\uf02b")]
        Tag,

        /// <summary>
        /// </summary>
        [Description("\uf02c")]
        Tags,

        /// <summary>
        /// </summary>
        [Description("\uf0ae")]
        Tasks,

        /// <summary>
        /// </summary>
        [Description("\uf1ba")]
        Taxi,

        /// <summary>
        /// </summary>
        [Description("\uf1d5")]
        TencentWeibo,

        /// <summary>
        /// </summary>
        [Description("\uf120")]
        Terminal,

        /// <summary>
        /// </summary>
        [Description("\uf034")]
        TextHeight,

        /// <summary>
        /// </summary>
        [Description("\uf035")]
        TextWidth,

        /// <summary>
        /// </summary>
        [Description("\uf00a")]
        Th,

        /// <summary>
        /// </summary>
        [Description("\uf009")]
        ThLarge,

        /// <summary>
        /// </summary>
        [Description("\uf00b")]
        ThList,

        /// <summary>
        /// </summary>
        [Description("\uf08d")]
        ThumbTack,

        /// <summary>
        /// </summary>
        [Description("\uf165")]
        ThumbsDown,

        /// <summary>
        /// </summary>
        [Description("\uf088")]
        ThumbsODown,

        /// <summary>
        /// </summary>
        [Description("\uf087")]
        ThumbsOUp,

        /// <summary>
        /// </summary>
        [Description("\uf164")]
        ThumbsUp,

        /// <summary>
        /// </summary>
        [Description("\uf145")]
        Ticket,

        /// <summary>
        /// </summary>
        [Description("\uf00d")]
        Times,

        /// <summary>
        /// </summary>
        [Description("\uf057")]
        TimesCircle,

        /// <summary>
        /// </summary>
        [Description("\uf05c")]
        TimesCircleO,

        /// <summary>
        /// </summary>
        [Description("\uf043")]
        Tint,

        /// <summary>
        /// </summary>
        [Description("\uf150")]
        ToggleDown,

        /// <summary>
        /// </summary>
        [Description("\uf191")]
        ToggleLeft,

        /// <summary>
        /// </summary>
        [Description("\uf204")]
        ToggleOff,

        /// <summary>
        /// </summary>
        [Description("\uf205")]
        ToggleOn,

        /// <summary>
        /// </summary>
        [Description("\uf152")]
        ToggleRight,

        /// <summary>
        /// </summary>
        [Description("\uf151")]
        ToggleUp,

        /// <summary>
        /// </summary>
        [Description("\uf238")]
        Train,

        /// <summary>
        /// </summary>
        [Description("\uf224")]
        Transgender,

        /// <summary>
        /// </summary>
        [Description("\uf225")]
        TransgenderAlt,

        /// <summary>
        /// </summary>
        [Description("\uf1f8")]
        Trash,

        /// <summary>
        /// </summary>
        [Description("\uf014")]
        TrashO,

        /// <summary>
        /// </summary>
        [Description("\uf1bb")]
        Tree,

        /// <summary>
        /// </summary>
        [Description("\uf181")]
        Trello,

        /// <summary>
        /// </summary>
        [Description("\uf091")]
        Trophy,

        /// <summary>
        /// </summary>
        [Description("\uf0d1")]
        Truck,

        /// <summary>
        /// </summary>
        [Description("\uf195")]
        Try,

        /// <summary>
        /// </summary>
        [Description("\uf1e4")]
        Tty,

        /// <summary>
        /// </summary>
        [Description("\uf173")]
        Tumblr,

        /// <summary>
        /// </summary>
        [Description("\uf174")]
        TumblrSquare,

        /// <summary>
        /// </summary>
        [Description("\uf195")]
        TurkishLira,

        /// <summary>
        /// </summary>
        [Description("\uf1e8")]
        Twitch,

        /// <summary>
        /// </summary>
        [Description("\uf099")]
        Twitter,

        /// <summary>
        /// </summary>
        [Description("\uf081")]
        TwitterSquare,

        /// <summary>
        /// </summary>
        [Description("\uf0e9")]
        Umbrella,

        /// <summary>
        /// </summary>
        [Description("\uf0cd")]
        Underline,

        /// <summary>
        /// </summary>
        [Description("\uf0e2")]
        Undo,

        /// <summary>
        /// </summary>
        [Description("\uf19c")]
        University,

        /// <summary>
        /// </summary>
        [Description("\uf127")]
        Unlink,

        /// <summary>
        /// </summary>
        [Description("\uf09c")]
        Unlock,

        /// <summary>
        /// </summary>
        [Description("\uf13e")]
        UnlockAlt,

        /// <summary>
        /// </summary>
        [Description("\uf0dc")]
        Unsorted,

        /// <summary>
        /// </summary>
        [Description("\uf093")]
        Upload,

        /// <summary>
        /// </summary>
        [Description("\uf155")]
        Usd,

        /// <summary>
        /// </summary>
        [Description("\uf007")]
        User,

        /// <summary>
        /// </summary>
        [Description("\uf0f0")]
        UserMd,

        /// <summary>
        /// </summary>
        [Description("\uf234")]
        UserPlus,

        /// <summary>
        /// </summary>
        [Description("\uf21b")]
        UserSecret,

        /// <summary>
        /// </summary>
        [Description("\uf235")]
        UserTimes,

        /// <summary>
        /// </summary>
        [Description("\uf0c0")]
        Users,

        /// <summary>
        /// </summary>
        [Description("\uf221")]
        Venus,

        /// <summary>
        /// </summary>
        [Description("\uf226")]
        VenusDouble,

        /// <summary>
        /// </summary>
        [Description("\uf228")]
        VenusMars,

        /// <summary>
        /// </summary>
        [Description("\uf237")]
        Viacoin,

        /// <summary>
        /// </summary>
        [Description("\uf03d")]
        VideoCamera,

        /// <summary>
        /// </summary>
        [Description("\uf194")]
        VimeoSquare,

        /// <summary>
        /// </summary>
        [Description("\uf1ca")]
        Vine,

        /// <summary>
        /// </summary>
        [Description("\uf189")]
        Vk,

        /// <summary>
        /// </summary>
        [Description("\uf027")]
        VolumeDown,

        /// <summary>
        /// </summary>
        [Description("\uf026")]
        VolumeOff,

        /// <summary>
        /// </summary>
        [Description("\uf028")]
        VolumeUp,

        /// <summary>
        /// </summary>
        [Description("\uf071")]
        Warning,

        /// <summary>
        /// </summary>
        [Description("\uf1d7")]
        Wechat,

        /// <summary>
        /// </summary>
        [Description("\uf18a")]
        Weibo,

        /// <summary>
        /// </summary>
        [Description("\uf1d7")]
        Weixin,

        /// <summary>
        /// </summary>
        [Description("\uf232")]
        Whatsapp,

        /// <summary>
        /// </summary>
        [Description("\uf193")]
        Wheelchair,

        /// <summary>
        /// </summary>
        [Description("\uf1eb")]
        Wifi,

        /// <summary>
        /// </summary>
        [Description("\uf17a")]
        Windows,

        /// <summary>
        /// </summary>
        [Description("\uf159")]
        Won,

        /// <summary>
        /// </summary>
        [Description("\uf19a")]
        Wordpress,

        /// <summary>
        /// </summary>
        [Description("\uf0ad")]
        Wrench,

        /// <summary>
        /// </summary>
        [Description("\uf168")]
        Xing,

        /// <summary>
        /// </summary>
        [Description("\uf169")]
        XingSquare,

        /// <summary>
        /// </summary>
        [Description("\uf19e")]
        Yahoo,

        /// <summary>
        /// </summary>
        [Description("\uf1e9")]
        Yelp,

        /// <summary>
        /// </summary>
        [Description("\uf157")]
        Yen,

        /// <summary>
        /// </summary>
        [Description("\uf167")]
        Youtube,

        /// <summary>
        /// </summary>
        [Description("\uf16a")]
        YoutubePlay,

        /// <summary>
        /// </summary>
        [Description("\uf166")]
        YoutubeSquare,
    }
}
