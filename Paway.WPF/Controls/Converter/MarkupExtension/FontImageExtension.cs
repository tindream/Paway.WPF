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
        public FontImageType Value { get; set; }

        /// <summary>
        /// </summary>
        public FontImageExtension() { }
        /// <summary>
        /// </summary>
        public FontImageExtension(FontImageType type)
        {
            this.Value = type;
        }
        /// <summary>
        /// 转换器
        /// </summary>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (service != null && service.TargetObject is Control obj)
            {
                obj.FontFamily = TConfig.FontAwesome;
            }
            var charactor = typeof(FontImageType).GetField(Value.ToString()).GetCustomAttribute<CharAttribute>().Value;
            return charactor.ToString();
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
        [Char('\uf042')]
        Adjust,

        /// <summary>
        /// </summary>
        [Char('\uf170')]
        Adn,

        /// <summary>
        /// </summary>
        [Char('\uf037')]
        AlignCenter,

        /// <summary>
        /// </summary>
        [Char('\uf039')]
        AlignJustify,

        /// <summary>
        /// </summary>
        [Char('\uf036')]
        AlignLeft,

        /// <summary>
        /// </summary>
        [Char('\uf038')]
        AlignRight,

        /// <summary>
        /// </summary>
        [Char('\uf0f9')]
        Ambulance,

        /// <summary>
        /// </summary>
        [Char('\uf13d')]
        Anchor,

        /// <summary>
        /// </summary>
        [Char('\uf17b')]
        Android,

        /// <summary>
        /// </summary>
        [Char('\uf209')]
        Angellist,

        /// <summary>
        /// </summary>
        [Char('\uf103')]
        AngleDoubleDown,

        /// <summary>
        /// </summary>
        [Char('\uf100')]
        AngleDoubleLeft,

        /// <summary>
        /// </summary>
        [Char('\uf101')]
        AngleDoubleRight,

        /// <summary>
        /// </summary>
        [Char('\uf102')]
        AngleDoubleUp,

        /// <summary>
        /// </summary>
        [Char('\uf107')]
        AngleDown,

        /// <summary>
        /// </summary>
        [Char('\uf104')]
        AngleLeft,

        /// <summary>
        /// </summary>
        [Char('\uf105')]
        AngleRight,

        /// <summary>
        /// </summary>
        [Char('\uf106')]
        AngleUp,

        /// <summary>
        /// </summary>
        [Char('\uf179')]
        Apple,

        /// <summary>
        /// </summary>
        [Char('\uf187')]
        Archive,

        /// <summary>
        /// </summary>
        [Char('\uf1fe')]
        AreaChart,

        /// <summary>
        /// </summary>
        [Char('\uf0ab')]
        ArrowCircleDown,

        /// <summary>
        /// </summary>
        [Char('\uf0a8')]
        ArrowCircleLeft,

        /// <summary>
        /// </summary>
        [Char('\uf01a')]
        ArrowCircleODown,

        /// <summary>
        /// </summary>
        [Char('\uf190')]
        ArrowCircleOLeft,

        /// <summary>
        /// </summary>
        [Char('\uf18e')]
        ArrowCircleORight,

        /// <summary>
        /// </summary>
        [Char('\uf01b')]
        ArrowCircleOUp,

        /// <summary>
        /// </summary>
        [Char('\uf0a9')]
        ArrowCircleRight,

        /// <summary>
        /// </summary>
        [Char('\uf0aa')]
        ArrowCircleUp,

        /// <summary>
        /// </summary>
        [Char('\uf063')]
        ArrowDown,

        /// <summary>
        /// </summary>
        [Char('\uf060')]
        ArrowLeft,

        /// <summary>
        /// </summary>
        [Char('\uf061')]
        ArrowRight,

        /// <summary>
        /// </summary>
        [Char('\uf062')]
        ArrowUp,

        /// <summary>
        /// </summary>
        [Char('\uf047')]
        Arrows,

        /// <summary>
        /// </summary>
        [Char('\uf0b2')]
        ArrowsAlt,

        /// <summary>
        /// </summary>
        [Char('\uf07e')]
        ArrowsH,

        /// <summary>
        /// </summary>
        [Char('\uf07d')]
        ArrowsV,

        /// <summary>
        /// </summary>
        [Char('\uf069')]
        Asterisk,

        /// <summary>
        /// </summary>
        [Char('\uf1fa')]
        At,

        /// <summary>
        /// </summary>
        [Char('\uf1b9')]
        Automobile,

        /// <summary>
        /// </summary>
        [Char('\uf04a')]
        Backward,

        /// <summary>
        /// </summary>
        [Char('\uf05e')]
        Ban,

        /// <summary>
        /// </summary>
        [Char('\uf19c')]
        Bank,

        /// <summary>
        /// </summary>
        [Char('\uf080')]
        BarChart,

        /// <summary>
        /// </summary>
        [Char('\uf080')]
        BarChartO,

        /// <summary>
        /// </summary>
        [Char('\uf02a')]
        Barcode,

        /// <summary>
        /// </summary>
        [Char('\uf0c9')]
        Bars,

        /// <summary>
        /// </summary>
        [Char('\uf236')]
        Bed,

        /// <summary>
        /// </summary>
        [Char('\uf0fc')]
        Beer,

        /// <summary>
        /// </summary>
        [Char('\uf1b4')]
        Behance,

        /// <summary>
        /// </summary>
        [Char('\uf1b5')]
        BehanceSquare,

        /// <summary>
        /// </summary>
        [Char('\uf0f3')]
        Bell,

        /// <summary>
        /// </summary>
        [Char('\uf0a2')]
        BellO,

        /// <summary>
        /// </summary>
        [Char('\uf1f6')]
        BellSlash,

        /// <summary>
        /// </summary>
        [Char('\uf1f7')]
        BellSlashO,

        /// <summary>
        /// </summary>
        [Char('\uf206')]
        Bicycle,

        /// <summary>
        /// </summary>
        [Char('\uf1e5')]
        Binoculars,

        /// <summary>
        /// </summary>
        [Char('\uf1fd')]
        BirthdayCake,

        /// <summary>
        /// </summary>
        [Char('\uf171')]
        Bitbucket,

        /// <summary>
        /// </summary>
        [Char('\uf172')]
        BitbucketSquare,

        /// <summary>
        /// </summary>
        [Char('\uf15a')]
        Bitcoin,

        /// <summary>
        /// </summary>
        [Char('\uf032')]
        Bold,

        /// <summary>
        /// </summary>
        [Char('\uf0e7')]
        Bolt,

        /// <summary>
        /// </summary>
        [Char('\uf1e2')]
        Bomb,

        /// <summary>
        /// </summary>
        [Char('\uf02d')]
        Book,

        /// <summary>
        /// </summary>
        [Char('\uf02e')]
        Bookmark,

        /// <summary>
        /// </summary>
        [Char('\uf097')]
        BookmarkO,

        /// <summary>
        /// </summary>
        [Char('\uf0b1')]
        Briefcase,

        /// <summary>
        /// </summary>
        [Char('\uf15a')]
        Btc,

        /// <summary>
        /// </summary>
        [Char('\uf188')]
        Bug,

        /// <summary>
        /// </summary>
        [Char('\uf1ad')]
        Building,

        /// <summary>
        /// </summary>
        [Char('\uf0f7')]
        BuildingO,

        /// <summary>
        /// </summary>
        [Char('\uf0a1')]
        Bullhorn,

        /// <summary>
        /// </summary>
        [Char('\uf140')]
        Bullseye,

        /// <summary>
        /// </summary>
        [Char('\uf207')]
        Bus,

        /// <summary>
        /// </summary>
        [Char('\uf20d')]
        Buysellads,

        /// <summary>
        /// </summary>
        [Char('\uf1ba')]
        Cab,

        /// <summary>
        /// </summary>
        [Char('\uf1ec')]
        Calculator,

        /// <summary>
        /// </summary>
        [Char('\uf073')]
        Calendar,

        /// <summary>
        /// </summary>
        [Char('\uf133')]
        CalendarO,

        /// <summary>
        /// </summary>
        [Char('\uf030')]
        Camera,

        /// <summary>
        /// </summary>
        [Char('\uf083')]
        CameraRetro,

        /// <summary>
        /// </summary>
        [Char('\uf1b9')]
        Car,

        /// <summary>
        /// </summary>
        [Char('\uf0d7')]
        CaretDown,

        /// <summary>
        /// </summary>
        [Char('\uf0d9')]
        CaretLeft,

        /// <summary>
        /// </summary>
        [Char('\uf0da')]
        CaretRight,

        /// <summary>
        /// </summary>
        [Char('\uf150')]
        CaretSquareODown,

        /// <summary>
        /// </summary>
        [Char('\uf191')]
        CaretSquareOLeft,

        /// <summary>
        /// </summary>
        [Char('\uf152')]
        CaretSquareORight,

        /// <summary>
        /// </summary>
        [Char('\uf151')]
        CaretSquareOUp,

        /// <summary>
        /// </summary>
        [Char('\uf0d8')]
        CaretUp,

        /// <summary>
        /// </summary>
        [Char('\uf218')]
        CartArrowDown,

        /// <summary>
        /// </summary>
        [Char('\uf217')]
        CartPlus,

        /// <summary>
        /// </summary>
        [Char('\uf20a')]
        Cc,

        /// <summary>
        /// </summary>
        [Char('\uf1f3')]
        CcAmex,

        /// <summary>
        /// </summary>
        [Char('\uf1f2')]
        CcDiscover,

        /// <summary>
        /// </summary>
        [Char('\uf1f1')]
        CcMastercard,

        /// <summary>
        /// </summary>
        [Char('\uf1f4')]
        CcPaypal,

        /// <summary>
        /// </summary>
        [Char('\uf1f5')]
        CcStripe,

        /// <summary>
        /// </summary>
        [Char('\uf1f0')]
        CcVisa,

        /// <summary>
        /// </summary>
        [Char('\uf0a3')]
        Certificate,

        /// <summary>
        /// </summary>
        [Char('\uf0c1')]
        Chain,

        /// <summary>
        /// </summary>
        [Char('\uf127')]
        ChainBroken,

        /// <summary>
        /// </summary>
        [Char('\uf00c')]
        Check,

        /// <summary>
        /// </summary>
        [Char('\uf058')]
        CheckCircle,

        /// <summary>
        /// </summary>
        [Char('\uf05d')]
        CheckCircleO,

        /// <summary>
        /// </summary>
        [Char('\uf14a')]
        CheckSquare,

        /// <summary>
        /// </summary>
        [Char('\uf046')]
        CheckSquareO,

        /// <summary>
        /// </summary>
        [Char('\uf13a')]
        ChevronCircleDown,

        /// <summary>
        /// </summary>
        [Char('\uf137')]
        ChevronCircleLeft,

        /// <summary>
        /// </summary>
        [Char('\uf138')]
        ChevronCircleRight,

        /// <summary>
        /// </summary>
        [Char('\uf139')]
        ChevronCircleUp,

        /// <summary>
        /// </summary>
        [Char('\uf078')]
        ChevronDown,

        /// <summary>
        /// </summary>
        [Char('\uf053')]
        ChevronLeft,

        /// <summary>
        /// </summary>
        [Char('\uf054')]
        ChevronRight,

        /// <summary>
        /// </summary>
        [Char('\uf077')]
        ChevronUp,

        /// <summary>
        /// </summary>
        [Char('\uf1ae')]
        Child,

        /// <summary>
        /// </summary>
        [Char('\uf111')]
        Circle,

        /// <summary>
        /// </summary>
        [Char('\uf10c')]
        CircleO,

        /// <summary>
        /// </summary>
        [Char('\uf1ce')]
        CircleONotch,

        /// <summary>
        /// </summary>
        [Char('\uf1db')]
        CircleThin,

        /// <summary>
        /// </summary>
        [Char('\uf0ea')]
        Clipboard,

        /// <summary>
        /// </summary>
        [Char('\uf017')]
        ClockO,

        /// <summary>
        /// </summary>
        [Char('\uf00d')]
        Close,

        /// <summary>
        /// </summary>
        [Char('\uf0c2')]
        Cloud,

        /// <summary>
        /// </summary>
        [Char('\uf0ed')]
        CloudDownload,

        /// <summary>
        /// </summary>
        [Char('\uf0ee')]
        CloudUpload,

        /// <summary>
        /// </summary>
        [Char('\uf157')]
        Cny,

        /// <summary>
        /// </summary>
        [Char('\uf121')]
        Code,

        /// <summary>
        /// </summary>
        [Char('\uf126')]
        CodeFork,

        /// <summary>
        /// </summary>
        [Char('\uf1cb')]
        Codepen,

        /// <summary>
        /// </summary>
        [Char('\uf0f4')]
        Coffee,

        /// <summary>
        /// </summary>
        [Char('\uf013')]
        Cog,

        /// <summary>
        /// </summary>
        [Char('\uf085')]
        Cogs,

        /// <summary>
        /// </summary>
        [Char('\uf0db')]
        Columns,

        /// <summary>
        /// </summary>
        [Char('\uf075')]
        Comment,

        /// <summary>
        /// </summary>
        [Char('\uf0e5')]
        CommentO,

        /// <summary>
        /// </summary>
        [Char('\uf086')]
        Comments,

        /// <summary>
        /// </summary>
        [Char('\uf0e6')]
        CommentsO,

        /// <summary>
        /// </summary>
        [Char('\uf14e')]
        Compass,

        /// <summary>
        /// </summary>
        [Char('\uf066')]
        Compress,

        /// <summary>
        /// </summary>
        [Char('\uf20e')]
        Connectdevelop,

        /// <summary>
        /// </summary>
        [Char('\uf0c5')]
        Copy,

        /// <summary>
        /// </summary>
        [Char('\uf1f9')]
        Copyright,

        /// <summary>
        /// </summary>
        [Char('\uf09d')]
        CreditCard,

        /// <summary>
        /// </summary>
        [Char('\uf125')]
        Crop,

        /// <summary>
        /// </summary>
        [Char('\uf05b')]
        Crosshairs,

        /// <summary>
        /// </summary>
        [Char('\uf13c')]
        Css3,

        /// <summary>
        /// </summary>
        [Char('\uf1b2')]
        Cube,

        /// <summary>
        /// </summary>
        [Char('\uf1b3')]
        Cubes,

        /// <summary>
        /// </summary>
        [Char('\uf0c4')]
        Cut,

        /// <summary>
        /// </summary>
        [Char('\uf0f5')]
        Cutlery,

        /// <summary>
        /// </summary>
        [Char('\uf0e4')]
        Dashboard,

        /// <summary>
        /// </summary>
        [Char('\uf210')]
        Dashcube,

        /// <summary>
        /// </summary>
        [Char('\uf1c0')]
        Database,

        /// <summary>
        /// </summary>
        [Char('\uf03b')]
        Dedent,

        /// <summary>
        /// </summary>
        [Char('\uf1a5')]
        Delicious,

        /// <summary>
        /// </summary>
        [Char('\uf108')]
        Desktop,

        /// <summary>
        /// </summary>
        [Char('\uf1bd')]
        Deviantart,

        /// <summary>
        /// </summary>
        [Char('\uf219')]
        Diamond,

        /// <summary>
        /// </summary>
        [Char('\uf1a6')]
        Digg,

        /// <summary>
        /// </summary>
        [Char('\uf155')]
        Dollar,

        /// <summary>
        /// </summary>
        [Char('\uf192')]
        DotCircleO,

        /// <summary>
        /// </summary>
        [Char('\uf019')]
        Download,

        /// <summary>
        /// </summary>
        [Char('\uf17d')]
        Dribbble,

        /// <summary>
        /// </summary>
        [Char('\uf16b')]
        Dropbox,

        /// <summary>
        /// </summary>
        [Char('\uf1a9')]
        Drupal,

        /// <summary>
        /// </summary>
        [Char('\uf044')]
        Edit,

        /// <summary>
        /// </summary>
        [Char('\uf052')]
        Eject,

        /// <summary>
        /// </summary>
        [Char('\uf141')]
        EllipsisH,

        /// <summary>
        /// </summary>
        [Char('\uf142')]
        EllipsisV,

        /// <summary>
        /// </summary>
        [Char('\uf1d1')]
        Empire,

        /// <summary>
        /// </summary>
        [Char('\uf0e0')]
        Envelope,

        /// <summary>
        /// </summary>
        [Char('\uf003')]
        EnvelopeO,

        /// <summary>
        /// </summary>
        [Char('\uf199')]
        EnvelopeSquare,

        /// <summary>
        /// </summary>
        [Char('\uf12d')]
        Eraser,

        /// <summary>
        /// </summary>
        [Char('\uf153')]
        Eur,

        /// <summary>
        /// </summary>
        [Char('\uf153')]
        Euro,

        /// <summary>
        /// </summary>
        [Char('\uf0ec')]
        Exchange,

        /// <summary>
        /// </summary>
        [Char('\uf12a')]
        Exclamation,

        /// <summary>
        /// </summary>
        [Char('\uf06a')]
        ExclamationCircle,

        /// <summary>
        /// </summary>
        [Char('\uf071')]
        ExclamationTriangle,

        /// <summary>
        /// </summary>
        [Char('\uf065')]
        Expand,

        /// <summary>
        /// </summary>
        [Char('\uf08e')]
        ExternalLink,

        /// <summary>
        /// </summary>
        [Char('\uf14c')]
        ExternalLinkSquare,

        /// <summary>
        /// </summary>
        [Char('\uf06e')]
        Eye,

        /// <summary>
        /// </summary>
        [Char('\uf070')]
        EyeSlash,

        /// <summary>
        /// </summary>
        [Char('\uf1fb')]
        Eyedropper,

        /// <summary>
        /// </summary>
        [Char('\uf09a')]
        Facebook,

        /// <summary>
        /// </summary>
        [Char('\uf09a')]
        FacebookF,

        /// <summary>
        /// </summary>
        [Char('\uf230')]
        FacebookOfficial,

        /// <summary>
        /// </summary>
        [Char('\uf082')]
        FacebookSquare,

        /// <summary>
        /// </summary>
        [Char('\uf049')]
        FastBackward,

        /// <summary>
        /// </summary>
        [Char('\uf050')]
        FastForward,

        /// <summary>
        /// </summary>
        [Char('\uf1ac')]
        Fax,

        /// <summary>
        /// </summary>
        [Char('\uf182')]
        Female,

        /// <summary>
        /// </summary>
        [Char('\uf0fb')]
        FighterJet,

        /// <summary>
        /// </summary>
        [Char('\uf15b')]
        File,

        /// <summary>
        /// </summary>
        [Char('\uf1c6')]
        FileArchiveO,

        /// <summary>
        /// </summary>
        [Char('\uf1c7')]
        FileAudioO,

        /// <summary>
        /// </summary>
        [Char('\uf1c9')]
        FileCodeO,

        /// <summary>
        /// </summary>
        [Char('\uf1c3')]
        FileExcelO,

        /// <summary>
        /// </summary>
        [Char('\uf1c5')]
        FileImageO,

        /// <summary>
        /// </summary>
        [Char('\uf1c8')]
        FileMovieO,

        /// <summary>
        /// </summary>
        [Char('\uf016')]
        FileO,

        /// <summary>
        /// </summary>
        [Char('\uf1c1')]
        FilePdfO,

        /// <summary>
        /// </summary>
        [Char('\uf1c5')]
        FilePhotoO,

        /// <summary>
        /// </summary>
        [Char('\uf1c5')]
        FilePictureO,

        /// <summary>
        /// </summary>
        [Char('\uf1c4')]
        FilePowerpointO,

        /// <summary>
        /// </summary>
        [Char('\uf1c7')]
        FileSoundO,

        /// <summary>
        /// </summary>
        [Char('\uf15c')]
        FileText,

        /// <summary>
        /// </summary>
        [Char('\uf0f6')]
        FileTextO,

        /// <summary>
        /// </summary>
        [Char('\uf1c8')]
        FileVideoO,

        /// <summary>
        /// </summary>
        [Char('\uf1c2')]
        FileWordO,

        /// <summary>
        /// </summary>
        [Char('\uf1c6')]
        FileZipO,

        /// <summary>
        /// </summary>
        [Char('\uf0c5')]
        FilesO,

        /// <summary>
        /// </summary>
        [Char('\uf008')]
        Film,

        /// <summary>
        /// </summary>
        [Char('\uf0b0')]
        Filter,

        /// <summary>
        /// </summary>
        [Char('\uf06d')]
        Fire,

        /// <summary>
        /// </summary>
        [Char('\uf134')]
        FireExtinguisher,

        /// <summary>
        /// </summary>
        [Char('\uf024')]
        Flag,

        /// <summary>
        /// </summary>
        [Char('\uf11e')]
        FlagCheckered,

        /// <summary>
        /// </summary>
        [Char('\uf11d')]
        FlagO,

        /// <summary>
        /// </summary>
        [Char('\uf0e7')]
        Flash,

        /// <summary>
        /// </summary>
        [Char('\uf0c3')]
        Flask,

        /// <summary>
        /// </summary>
        [Char('\uf16e')]
        Flickr,

        /// <summary>
        /// </summary>
        [Char('\uf0c7')]
        FloppyO,

        /// <summary>
        /// </summary>
        [Char('\uf07b')]
        Folder,

        /// <summary>
        /// </summary>
        [Char('\uf114')]
        FolderO,

        /// <summary>
        /// </summary>
        [Char('\uf07c')]
        FolderOpen,

        /// <summary>
        /// </summary>
        [Char('\uf115')]
        FolderOpenO,

        /// <summary>
        /// </summary>
        [Char('\uf031')]
        Font,

        /// <summary>
        /// </summary>
        [Char('\uf211')]
        Forumbee,

        /// <summary>
        /// </summary>
        [Char('\uf04e')]
        Forward,

        /// <summary>
        /// </summary>
        [Char('\uf180')]
        Foursquare,

        /// <summary>
        /// </summary>
        [Char('\uf119')]
        FrownO,

        /// <summary>
        /// </summary>
        [Char('\uf1e3')]
        FutbolO,

        /// <summary>
        /// </summary>
        [Char('\uf11b')]
        Gamepad,

        /// <summary>
        /// </summary>
        [Char('\uf0e3')]
        Gavel,

        /// <summary>
        /// </summary>
        [Char('\uf154')]
        Gbp,

        /// <summary>
        /// </summary>
        [Char('\uf1d1')]
        Ge,

        /// <summary>
        /// </summary>
        [Char('\uf013')]
        Gear,

        /// <summary>
        /// </summary>
        [Char('\uf085')]
        Gears,

        /// <summary>
        /// </summary>
        [Char('\uf1db')]
        Genderless,

        /// <summary>
        /// </summary>
        [Char('\uf06b')]
        Gift,

        /// <summary>
        /// </summary>
        [Char('\uf1d3')]
        Git,

        /// <summary>
        /// </summary>
        [Char('\uf1d2')]
        GitSquare,

        /// <summary>
        /// </summary>
        [Char('\uf09b')]
        Github,

        /// <summary>
        /// </summary>
        [Char('\uf113')]
        GithubAlt,

        /// <summary>
        /// </summary>
        [Char('\uf092')]
        GithubSquare,

        /// <summary>
        /// </summary>
        [Char('\uf184')]
        Gittip,

        /// <summary>
        /// </summary>
        [Char('\uf000')]
        Glass,

        /// <summary>
        /// </summary>
        [Char('\uf0ac')]
        Globe,

        /// <summary>
        /// </summary>
        [Char('\uf1a0')]
        Google,

        /// <summary>
        /// </summary>
        [Char('\uf0d5')]
        GooglePlus,

        /// <summary>
        /// </summary>
        [Char('\uf0d4')]
        GooglePlusSquare,

        /// <summary>
        /// </summary>
        [Char('\uf1ee')]
        GoogleWallet,

        /// <summary>
        /// </summary>
        [Char('\uf19d')]
        GraduationCap,

        /// <summary>
        /// </summary>
        [Char('\uf184')]
        Gratipay,

        /// <summary>
        /// </summary>
        [Char('\uf0c0')]
        Group,

        /// <summary>
        /// </summary>
        [Char('\uf0fd')]
        HSquare,

        /// <summary>
        /// </summary>
        [Char('\uf1d4')]
        HackerNews,

        /// <summary>
        /// </summary>
        [Char('\uf0a7')]
        HandODown,

        /// <summary>
        /// </summary>
        [Char('\uf0a5')]
        HandOLeft,

        /// <summary>
        /// </summary>
        [Char('\uf0a4')]
        HandORight,

        /// <summary>
        /// </summary>
        [Char('\uf0a6')]
        HandOUp,

        /// <summary>
        /// </summary>
        [Char('\uf0a0')]
        HddO,

        /// <summary>
        /// </summary>
        [Char('\uf1dc')]
        Header,

        /// <summary>
        /// </summary>
        [Char('\uf025')]
        Headphones,

        /// <summary>
        /// </summary>
        [Char('\uf004')]
        Heart,

        /// <summary>
        /// </summary>
        [Char('\uf08a')]
        HeartO,

        /// <summary>
        /// </summary>
        [Char('\uf21e')]
        Heartbeat,

        /// <summary>
        /// </summary>
        [Char('\uf1da')]
        History,

        /// <summary>
        /// </summary>
        [Char('\uf015')]
        Home,

        /// <summary>
        /// </summary>
        [Char('\uf0f8')]
        HospitalO,

        /// <summary>
        /// </summary>
        [Char('\uf236')]
        Hotel,

        /// <summary>
        /// </summary>
        [Char('\uf13b')]
        Html5,

        /// <summary>
        /// </summary>
        [Char('\uf20b')]
        Ils,

        /// <summary>
        /// </summary>
        [Char('\uf03e')]
        Image,

        /// <summary>
        /// </summary>
        [Char('\uf01c')]
        Inbox,

        /// <summary>
        /// </summary>
        [Char('\uf03c')]
        Indent,

        /// <summary>
        /// </summary>
        [Char('\uf129')]
        Info,

        /// <summary>
        /// </summary>
        [Char('\uf05a')]
        InfoCircle,

        /// <summary>
        /// </summary>
        [Char('\uf156')]
        Inr,

        /// <summary>
        /// </summary>
        [Char('\uf16d')]
        Instagram,

        /// <summary>
        /// </summary>
        [Char('\uf19c')]
        Institution,

        /// <summary>
        /// </summary>
        [Char('\uf208')]
        Ioxhost,

        /// <summary>
        /// </summary>
        [Char('\uf033')]
        Italic,

        /// <summary>
        /// </summary>
        [Char('\uf1aa')]
        Joomla,

        /// <summary>
        /// </summary>
        [Char('\uf157')]
        Jpy,

        /// <summary>
        /// </summary>
        [Char('\uf1cc')]
        Jsfiddle,

        /// <summary>
        /// </summary>
        [Char('\uf084')]
        Key,

        /// <summary>
        /// </summary>
        [Char('\uf11c')]
        KeyboardO,

        /// <summary>
        /// </summary>
        [Char('\uf159')]
        Krw,

        /// <summary>
        /// </summary>
        [Char('\uf1ab')]
        Language,

        /// <summary>
        /// </summary>
        [Char('\uf109')]
        Laptop,

        /// <summary>
        /// </summary>
        [Char('\uf202')]
        Lastfm,

        /// <summary>
        /// </summary>
        [Char('\uf203')]
        LastfmSquare,

        /// <summary>
        /// </summary>
        [Char('\uf06c')]
        Leaf,

        /// <summary>
        /// </summary>
        [Char('\uf212')]
        Leanpub,

        /// <summary>
        /// </summary>
        [Char('\uf0e3')]
        Legal,

        /// <summary>
        /// </summary>
        [Char('\uf094')]
        LemonO,

        /// <summary>
        /// </summary>
        [Char('\uf149')]
        LevelDown,

        /// <summary>
        /// </summary>
        [Char('\uf148')]
        LevelUp,

        /// <summary>
        /// </summary>
        [Char('\uf1cd')]
        LifeBouy,

        /// <summary>
        /// </summary>
        [Char('\uf1cd')]
        LifeBuoy,

        /// <summary>
        /// </summary>
        [Char('\uf1cd')]
        LifeRing,

        /// <summary>
        /// </summary>
        [Char('\uf1cd')]
        LifeSaver,

        /// <summary>
        /// </summary>
        [Char('\uf0eb')]
        LightbulbO,

        /// <summary>
        /// </summary>
        [Char('\uf201')]
        LineChart,

        /// <summary>
        /// </summary>
        [Char('\uf0c1')]
        Link,

        /// <summary>
        /// </summary>
        [Char('\uf0e1')]
        Linkedin,

        /// <summary>
        /// </summary>
        [Char('\uf08c')]
        LinkedinSquare,

        /// <summary>
        /// </summary>
        [Char('\uf17c')]
        Linux,

        /// <summary>
        /// </summary>
        [Char('\uf03a')]
        List,

        /// <summary>
        /// </summary>
        [Char('\uf022')]
        ListAlt,

        /// <summary>
        /// </summary>
        [Char('\uf0cb')]
        ListOl,

        /// <summary>
        /// </summary>
        [Char('\uf0ca')]
        ListUl,

        /// <summary>
        /// </summary>
        [Char('\uf124')]
        LocationArrow,

        /// <summary>
        /// </summary>
        [Char('\uf023')]
        Lock,

        /// <summary>
        /// </summary>
        [Char('\uf175')]
        LongArrowDown,

        /// <summary>
        /// </summary>
        [Char('\uf177')]
        LongArrowLeft,

        /// <summary>
        /// </summary>
        [Char('\uf178')]
        LongArrowRight,

        /// <summary>
        /// </summary>
        [Char('\uf176')]
        LongArrowUp,

        /// <summary>
        /// </summary>
        [Char('\uf0d0')]
        Magic,

        /// <summary>
        /// </summary>
        [Char('\uf076')]
        Magnet,

        /// <summary>
        /// </summary>
        [Char('\uf064')]
        MailForward,

        /// <summary>
        /// </summary>
        [Char('\uf112')]
        MailReply,

        /// <summary>
        /// </summary>
        [Char('\uf122')]
        MailReplyAll,

        /// <summary>
        /// </summary>
        [Char('\uf183')]
        Male,

        /// <summary>
        /// </summary>
        [Char('\uf041')]
        MapMarker,

        /// <summary>
        /// </summary>
        [Char('\uf222')]
        Mars,

        /// <summary>
        /// </summary>
        [Char('\uf227')]
        MarsDouble,

        /// <summary>
        /// </summary>
        [Char('\uf229')]
        MarsStroke,

        /// <summary>
        /// </summary>
        [Char('\uf22b')]
        MarsStrokeH,

        /// <summary>
        /// </summary>
        [Char('\uf22a')]
        MarsStrokeV,

        /// <summary>
        /// </summary>
        [Char('\uf136')]
        Maxcdn,

        /// <summary>
        /// </summary>
        [Char('\uf20c')]
        Meanpath,

        /// <summary>
        /// </summary>
        [Char('\uf23a')]
        Medium,

        /// <summary>
        /// </summary>
        [Char('\uf0fa')]
        Medkit,

        /// <summary>
        /// </summary>
        [Char('\uf11a')]
        MehO,

        /// <summary>
        /// </summary>
        [Char('\uf223')]
        Mercury,

        /// <summary>
        /// </summary>
        [Char('\uf130')]
        Microphone,

        /// <summary>
        /// </summary>
        [Char('\uf131')]
        MicrophoneSlash,

        /// <summary>
        /// </summary>
        [Char('\uf068')]
        Minus,

        /// <summary>
        /// </summary>
        [Char('\uf056')]
        MinusCircle,

        /// <summary>
        /// </summary>
        [Char('\uf146')]
        MinusSquare,

        /// <summary>
        /// </summary>
        [Char('\uf147')]
        MinusSquareO,

        /// <summary>
        /// </summary>
        [Char('\uf10b')]
        Mobile,

        /// <summary>
        /// </summary>
        [Char('\uf10b')]
        MobilePhone,

        /// <summary>
        /// </summary>
        [Char('\uf0d6')]
        Money,

        /// <summary>
        /// </summary>
        [Char('\uf186')]
        MoonO,

        /// <summary>
        /// </summary>
        [Char('\uf19d')]
        MortarBoard,

        /// <summary>
        /// </summary>
        [Char('\uf21c')]
        Motorcycle,

        /// <summary>
        /// </summary>
        [Char('\uf001')]
        Music,

        /// <summary>
        /// </summary>
        [Char('\uf0c9')]
        Navicon,

        /// <summary>
        /// </summary>
        [Char('\uf22c')]
        Neuter,

        /// <summary>
        /// </summary>
        [Char('\uf1ea')]
        NewspaperO,

        /// <summary>
        /// </summary>
        [Char('\uf19b')]
        Openid,

        /// <summary>
        /// </summary>
        [Char('\uf03b')]
        Outdent,

        /// <summary>
        /// </summary>
        [Char('\uf18c')]
        Pagelines,

        /// <summary>
        /// </summary>
        [Char('\uf1fc')]
        PaintBrush,

        /// <summary>
        /// </summary>
        [Char('\uf1d8')]
        PaperPlane,

        /// <summary>
        /// </summary>
        [Char('\uf1d9')]
        PaperPlaneO,

        /// <summary>
        /// </summary>
        [Char('\uf0c6')]
        Paperclip,

        /// <summary>
        /// </summary>
        [Char('\uf1dd')]
        Paragraph,

        /// <summary>
        /// </summary>
        [Char('\uf0ea')]
        Paste,

        /// <summary>
        /// </summary>
        [Char('\uf04c')]
        Pause,

        /// <summary>
        /// </summary>
        [Char('\uf1b0')]
        Paw,

        /// <summary>
        /// </summary>
        [Char('\uf1ed')]
        Paypal,

        /// <summary>
        /// </summary>
        [Char('\uf040')]
        Pencil,

        /// <summary>
        /// </summary>
        [Char('\uf14b')]
        PencilSquare,

        /// <summary>
        /// </summary>
        [Char('\uf044')]
        PencilSquareO,

        /// <summary>
        /// </summary>
        [Char('\uf095')]
        Phone,

        /// <summary>
        /// </summary>
        [Char('\uf098')]
        PhoneSquare,

        /// <summary>
        /// </summary>
        [Char('\uf03e')]
        Photo,

        /// <summary>
        /// </summary>
        [Char('\uf03e')]
        PictureO,

        /// <summary>
        /// </summary>
        [Char('\uf200')]
        PieChart,

        /// <summary>
        /// </summary>
        [Char('\uf1a7')]
        PiedPiper,

        /// <summary>
        /// </summary>
        [Char('\uf1a8')]
        PiedPiperAlt,

        /// <summary>
        /// </summary>
        [Char('\uf0d2')]
        Pinterest,

        /// <summary>
        /// </summary>
        [Char('\uf231')]
        PinterestP,

        /// <summary>
        /// </summary>
        [Char('\uf0d3')]
        PinterestSquare,

        /// <summary>
        /// </summary>
        [Char('\uf072')]
        Plane,

        /// <summary>
        /// </summary>
        [Char('\uf04b')]
        Play,

        /// <summary>
        /// </summary>
        [Char('\uf144')]
        PlayCircle,

        /// <summary>
        /// </summary>
        [Char('\uf01d')]
        PlayCircleO,

        /// <summary>
        /// </summary>
        [Char('\uf1e6')]
        Plug,

        /// <summary>
        /// </summary>
        [Char('\uf067')]
        Plus,

        /// <summary>
        /// </summary>
        [Char('\uf055')]
        PlusCircle,

        /// <summary>
        /// </summary>
        [Char('\uf0fe')]
        PlusSquare,

        /// <summary>
        /// </summary>
        [Char('\uf196')]
        PlusSquareO,

        /// <summary>
        /// </summary>
        [Char('\uf011')]
        PowerOff,

        /// <summary>
        /// </summary>
        [Char('\uf02f')]
        Print,

        /// <summary>
        /// </summary>
        [Char('\uf12e')]
        PuzzlePiece,

        /// <summary>
        /// </summary>
        [Char('\uf1d6')]
        Qq,

        /// <summary>
        /// </summary>
        [Char('\uf029')]
        Qrcode,

        /// <summary>
        /// </summary>
        [Char('\uf128')]
        Question,

        /// <summary>
        /// </summary>
        [Char('\uf059')]
        QuestionCircle,

        /// <summary>
        /// </summary>
        [Char('\uf10d')]
        QuoteLeft,

        /// <summary>
        /// </summary>
        [Char('\uf10e')]
        QuoteRight,

        /// <summary>
        /// </summary>
        [Char('\uf1d0')]
        Ra,

        /// <summary>
        /// </summary>
        [Char('\uf074')]
        Random,

        /// <summary>
        /// </summary>
        [Char('\uf1d0')]
        Rebel,

        /// <summary>
        /// </summary>
        [Char('\uf1b8')]
        Recycle,

        /// <summary>
        /// </summary>
        [Char('\uf1a1')]
        Reddit,

        /// <summary>
        /// </summary>
        [Char('\uf1a2')]
        RedditSquare,

        /// <summary>
        /// </summary>
        [Char('\uf021')]
        Refresh,

        /// <summary>
        /// </summary>
        [Char('\uf00d')]
        Remove,

        /// <summary>
        /// </summary>
        [Char('\uf18b')]
        Renren,

        /// <summary>
        /// </summary>
        [Char('\uf0c9')]
        Reorder,

        /// <summary>
        /// </summary>
        [Char('\uf01e')]
        Repeat,

        /// <summary>
        /// </summary>
        [Char('\uf112')]
        Reply,

        /// <summary>
        /// </summary>
        [Char('\uf122')]
        ReplyAll,

        /// <summary>
        /// </summary>
        [Char('\uf079')]
        Retweet,

        /// <summary>
        /// </summary>
        [Char('\uf157')]
        Rmb,

        /// <summary>
        /// </summary>
        [Char('\uf018')]
        Road,

        /// <summary>
        /// </summary>
        [Char('\uf135')]
        Rocket,

        /// <summary>
        /// </summary>
        [Char('\uf0e2')]
        RotateLeft,

        /// <summary>
        /// </summary>
        [Char('\uf01e')]
        RotateRight,

        /// <summary>
        /// </summary>
        [Char('\uf158')]
        Rouble,

        /// <summary>
        /// </summary>
        [Char('\uf09e')]
        Rss,

        /// <summary>
        /// </summary>
        [Char('\uf143')]
        RssSquare,

        /// <summary>
        /// </summary>
        [Char('\uf158')]
        Rub,

        /// <summary>
        /// </summary>
        [Char('\uf158')]
        Ruble,

        /// <summary>
        /// </summary>
        [Char('\uf156')]
        Rupee,

        /// <summary>
        /// </summary>
        [Char('\uf0c7')]
        Save,

        /// <summary>
        /// </summary>
        [Char('\uf0c4')]
        Scissors,

        /// <summary>
        /// </summary>
        [Char('\uf002')]
        Search,

        /// <summary>
        /// </summary>
        [Char('\uf010')]
        SearchMinus,

        /// <summary>
        /// </summary>
        [Char('\uf00e')]
        SearchPlus,

        /// <summary>
        /// </summary>
        [Char('\uf213')]
        Sellsy,

        /// <summary>
        /// </summary>
        [Char('\uf1d8')]
        Send,

        /// <summary>
        /// </summary>
        [Char('\uf1d9')]
        SendO,

        /// <summary>
        /// </summary>
        [Char('\uf233')]
        Server,

        /// <summary>
        /// </summary>
        [Char('\uf064')]
        Share,

        /// <summary>
        /// </summary>
        [Char('\uf1e0')]
        ShareAlt,

        /// <summary>
        /// </summary>
        [Char('\uf1e1')]
        ShareAltSquare,

        /// <summary>
        /// </summary>
        [Char('\uf14d')]
        ShareSquare,

        /// <summary>
        /// </summary>
        [Char('\uf045')]
        ShareSquareO,

        /// <summary>
        /// </summary>
        [Char('\uf20b')]
        Shekel,

        /// <summary>
        /// </summary>
        [Char('\uf20b')]
        Sheqel,

        /// <summary>
        /// </summary>
        [Char('\uf132')]
        Shield,

        /// <summary>
        /// </summary>
        [Char('\uf21a')]
        Ship,

        /// <summary>
        /// </summary>
        [Char('\uf214')]
        Shirtsinbulk,

        /// <summary>
        /// </summary>
        [Char('\uf07a')]
        ShoppingCart,

        /// <summary>
        /// </summary>
        [Char('\uf090')]
        SignIn,

        /// <summary>
        /// </summary>
        [Char('\uf08b')]
        SignOut,

        /// <summary>
        /// </summary>
        [Char('\uf012')]
        Signal,

        /// <summary>
        /// </summary>
        [Char('\uf215')]
        Simplybuilt,

        /// <summary>
        /// </summary>
        [Char('\uf0e8')]
        Sitemap,

        /// <summary>
        /// </summary>
        [Char('\uf216')]
        Skyatlas,

        /// <summary>
        /// </summary>
        [Char('\uf17e')]
        Skype,

        /// <summary>
        /// </summary>
        [Char('\uf198')]
        Slack,

        /// <summary>
        /// </summary>
        [Char('\uf1de')]
        Sliders,

        /// <summary>
        /// </summary>
        [Char('\uf1e7')]
        Slideshare,

        /// <summary>
        /// </summary>
        [Char('\uf118')]
        SmileO,

        /// <summary>
        /// </summary>
        [Char('\uf1e3')]
        SoccerBallO,

        /// <summary>
        /// </summary>
        [Char('\uf0dc')]
        Sort,

        /// <summary>
        /// </summary>
        [Char('\uf15d')]
        SortAlphaAsc,

        /// <summary>
        /// </summary>
        [Char('\uf15e')]
        SortAlphaDesc,

        /// <summary>
        /// </summary>
        [Char('\uf160')]
        SortAmountAsc,

        /// <summary>
        /// </summary>
        [Char('\uf161')]
        SortAmountDesc,

        /// <summary>
        /// </summary>
        [Char('\uf0de')]
        SortAsc,

        /// <summary>
        /// </summary>
        [Char('\uf0dd')]
        SortDesc,

        /// <summary>
        /// </summary>
        [Char('\uf0dd')]
        SortDown,

        /// <summary>
        /// </summary>
        [Char('\uf162')]
        SortNumericAsc,

        /// <summary>
        /// </summary>
        [Char('\uf163')]
        SortNumericDesc,

        /// <summary>
        /// </summary>
        [Char('\uf0de')]
        SortUp,

        /// <summary>
        /// </summary>
        [Char('\uf1be')]
        Soundcloud,

        /// <summary>
        /// </summary>
        [Char('\uf197')]
        SpaceShuttle,

        /// <summary>
        /// </summary>
        [Char('\uf110')]
        Spinner,

        /// <summary>
        /// </summary>
        [Char('\uf1b1')]
        Spoon,

        /// <summary>
        /// </summary>
        [Char('\uf1bc')]
        Spotify,

        /// <summary>
        /// </summary>
        [Char('\uf0c8')]
        Square,

        /// <summary>
        /// </summary>
        [Char('\uf096')]
        SquareO,

        /// <summary>
        /// </summary>
        [Char('\uf18d')]
        StackExchange,

        /// <summary>
        /// </summary>
        [Char('\uf16c')]
        StackOverflow,

        /// <summary>
        /// </summary>
        [Char('\uf005')]
        Star,

        /// <summary>
        /// </summary>
        [Char('\uf089')]
        StarHalf,

        /// <summary>
        /// </summary>
        [Char('\uf123')]
        StarHalfEmpty,

        /// <summary>
        /// </summary>
        [Char('\uf123')]
        StarHalfFull,

        /// <summary>
        /// </summary>
        [Char('\uf123')]
        StarHalfO,

        /// <summary>
        /// </summary>
        [Char('\uf006')]
        StarO,

        /// <summary>
        /// </summary>
        [Char('\uf1b6')]
        Steam,

        /// <summary>
        /// </summary>
        [Char('\uf1b7')]
        SteamSquare,

        /// <summary>
        /// </summary>
        [Char('\uf048')]
        StepBackward,

        /// <summary>
        /// </summary>
        [Char('\uf051')]
        StepForward,

        /// <summary>
        /// </summary>
        [Char('\uf0f1')]
        Stethoscope,

        /// <summary>
        /// </summary>
        [Char('\uf04d')]
        Stop,

        /// <summary>
        /// </summary>
        [Char('\uf21d')]
        StreetView,

        /// <summary>
        /// </summary>
        [Char('\uf0cc')]
        Strikethrough,

        /// <summary>
        /// </summary>
        [Char('\uf1a4')]
        Stumbleupon,

        /// <summary>
        /// </summary>
        [Char('\uf1a3')]
        StumbleuponCircle,

        /// <summary>
        /// </summary>
        [Char('\uf12c')]
        Subscript,

        /// <summary>
        /// </summary>
        [Char('\uf239')]
        Subway,

        /// <summary>
        /// </summary>
        [Char('\uf0f2')]
        Suitcase,

        /// <summary>
        /// </summary>
        [Char('\uf185')]
        SunO,

        /// <summary>
        /// </summary>
        [Char('\uf12b')]
        Superscript,

        /// <summary>
        /// </summary>
        [Char('\uf1cd')]
        Support,

        /// <summary>
        /// </summary>
        [Char('\uf0ce')]
        Table,

        /// <summary>
        /// </summary>
        [Char('\uf10a')]
        Tablet,

        /// <summary>
        /// </summary>
        [Char('\uf0e4')]
        Tachometer,

        /// <summary>
        /// </summary>
        [Char('\uf02b')]
        Tag,

        /// <summary>
        /// </summary>
        [Char('\uf02c')]
        Tags,

        /// <summary>
        /// </summary>
        [Char('\uf0ae')]
        Tasks,

        /// <summary>
        /// </summary>
        [Char('\uf1ba')]
        Taxi,

        /// <summary>
        /// </summary>
        [Char('\uf1d5')]
        TencentWeibo,

        /// <summary>
        /// </summary>
        [Char('\uf120')]
        Terminal,

        /// <summary>
        /// </summary>
        [Char('\uf034')]
        TextHeight,

        /// <summary>
        /// </summary>
        [Char('\uf035')]
        TextWidth,

        /// <summary>
        /// </summary>
        [Char('\uf00a')]
        Th,

        /// <summary>
        /// </summary>
        [Char('\uf009')]
        ThLarge,

        /// <summary>
        /// </summary>
        [Char('\uf00b')]
        ThList,

        /// <summary>
        /// </summary>
        [Char('\uf08d')]
        ThumbTack,

        /// <summary>
        /// </summary>
        [Char('\uf165')]
        ThumbsDown,

        /// <summary>
        /// </summary>
        [Char('\uf088')]
        ThumbsODown,

        /// <summary>
        /// </summary>
        [Char('\uf087')]
        ThumbsOUp,

        /// <summary>
        /// </summary>
        [Char('\uf164')]
        ThumbsUp,

        /// <summary>
        /// </summary>
        [Char('\uf145')]
        Ticket,

        /// <summary>
        /// </summary>
        [Char('\uf00d')]
        Times,

        /// <summary>
        /// </summary>
        [Char('\uf057')]
        TimesCircle,

        /// <summary>
        /// </summary>
        [Char('\uf05c')]
        TimesCircleO,

        /// <summary>
        /// </summary>
        [Char('\uf043')]
        Tint,

        /// <summary>
        /// </summary>
        [Char('\uf150')]
        ToggleDown,

        /// <summary>
        /// </summary>
        [Char('\uf191')]
        ToggleLeft,

        /// <summary>
        /// </summary>
        [Char('\uf204')]
        ToggleOff,

        /// <summary>
        /// </summary>
        [Char('\uf205')]
        ToggleOn,

        /// <summary>
        /// </summary>
        [Char('\uf152')]
        ToggleRight,

        /// <summary>
        /// </summary>
        [Char('\uf151')]
        ToggleUp,

        /// <summary>
        /// </summary>
        [Char('\uf238')]
        Train,

        /// <summary>
        /// </summary>
        [Char('\uf224')]
        Transgender,

        /// <summary>
        /// </summary>
        [Char('\uf225')]
        TransgenderAlt,

        /// <summary>
        /// </summary>
        [Char('\uf1f8')]
        Trash,

        /// <summary>
        /// </summary>
        [Char('\uf014')]
        TrashO,

        /// <summary>
        /// </summary>
        [Char('\uf1bb')]
        Tree,

        /// <summary>
        /// </summary>
        [Char('\uf181')]
        Trello,

        /// <summary>
        /// </summary>
        [Char('\uf091')]
        Trophy,

        /// <summary>
        /// </summary>
        [Char('\uf0d1')]
        Truck,

        /// <summary>
        /// </summary>
        [Char('\uf195')]
        Try,

        /// <summary>
        /// </summary>
        [Char('\uf1e4')]
        Tty,

        /// <summary>
        /// </summary>
        [Char('\uf173')]
        Tumblr,

        /// <summary>
        /// </summary>
        [Char('\uf174')]
        TumblrSquare,

        /// <summary>
        /// </summary>
        [Char('\uf195')]
        TurkishLira,

        /// <summary>
        /// </summary>
        [Char('\uf1e8')]
        Twitch,

        /// <summary>
        /// </summary>
        [Char('\uf099')]
        Twitter,

        /// <summary>
        /// </summary>
        [Char('\uf081')]
        TwitterSquare,

        /// <summary>
        /// </summary>
        [Char('\uf0e9')]
        Umbrella,

        /// <summary>
        /// </summary>
        [Char('\uf0cd')]
        Underline,

        /// <summary>
        /// </summary>
        [Char('\uf0e2')]
        Undo,

        /// <summary>
        /// </summary>
        [Char('\uf19c')]
        University,

        /// <summary>
        /// </summary>
        [Char('\uf127')]
        Unlink,

        /// <summary>
        /// </summary>
        [Char('\uf09c')]
        Unlock,

        /// <summary>
        /// </summary>
        [Char('\uf13e')]
        UnlockAlt,

        /// <summary>
        /// </summary>
        [Char('\uf0dc')]
        Unsorted,

        /// <summary>
        /// </summary>
        [Char('\uf093')]
        Upload,

        /// <summary>
        /// </summary>
        [Char('\uf155')]
        Usd,

        /// <summary>
        /// </summary>
        [Char('\uf007')]
        User,

        /// <summary>
        /// </summary>
        [Char('\uf0f0')]
        UserMd,

        /// <summary>
        /// </summary>
        [Char('\uf234')]
        UserPlus,

        /// <summary>
        /// </summary>
        [Char('\uf21b')]
        UserSecret,

        /// <summary>
        /// </summary>
        [Char('\uf235')]
        UserTimes,

        /// <summary>
        /// </summary>
        [Char('\uf0c0')]
        Users,

        /// <summary>
        /// </summary>
        [Char('\uf221')]
        Venus,

        /// <summary>
        /// </summary>
        [Char('\uf226')]
        VenusDouble,

        /// <summary>
        /// </summary>
        [Char('\uf228')]
        VenusMars,

        /// <summary>
        /// </summary>
        [Char('\uf237')]
        Viacoin,

        /// <summary>
        /// </summary>
        [Char('\uf03d')]
        VideoCamera,

        /// <summary>
        /// </summary>
        [Char('\uf194')]
        VimeoSquare,

        /// <summary>
        /// </summary>
        [Char('\uf1ca')]
        Vine,

        /// <summary>
        /// </summary>
        [Char('\uf189')]
        Vk,

        /// <summary>
        /// </summary>
        [Char('\uf027')]
        VolumeDown,

        /// <summary>
        /// </summary>
        [Char('\uf026')]
        VolumeOff,

        /// <summary>
        /// </summary>
        [Char('\uf028')]
        VolumeUp,

        /// <summary>
        /// </summary>
        [Char('\uf071')]
        Warning,

        /// <summary>
        /// </summary>
        [Char('\uf1d7')]
        Wechat,

        /// <summary>
        /// </summary>
        [Char('\uf18a')]
        Weibo,

        /// <summary>
        /// </summary>
        [Char('\uf1d7')]
        Weixin,

        /// <summary>
        /// </summary>
        [Char('\uf232')]
        Whatsapp,

        /// <summary>
        /// </summary>
        [Char('\uf193')]
        Wheelchair,

        /// <summary>
        /// </summary>
        [Char('\uf1eb')]
        Wifi,

        /// <summary>
        /// </summary>
        [Char('\uf17a')]
        Windows,

        /// <summary>
        /// </summary>
        [Char('\uf159')]
        Won,

        /// <summary>
        /// </summary>
        [Char('\uf19a')]
        Wordpress,

        /// <summary>
        /// </summary>
        [Char('\uf0ad')]
        Wrench,

        /// <summary>
        /// </summary>
        [Char('\uf168')]
        Xing,

        /// <summary>
        /// </summary>
        [Char('\uf169')]
        XingSquare,

        /// <summary>
        /// </summary>
        [Char('\uf19e')]
        Yahoo,

        /// <summary>
        /// </summary>
        [Char('\uf1e9')]
        Yelp,

        /// <summary>
        /// </summary>
        [Char('\uf157')]
        Yen,

        /// <summary>
        /// </summary>
        [Char('\uf167')]
        Youtube,

        /// <summary>
        /// </summary>
        [Char('\uf16a')]
        YoutubePlay,

        /// <summary>
        /// </summary>
        [Char('\uf166')]
        YoutubeSquare,
    }
}
