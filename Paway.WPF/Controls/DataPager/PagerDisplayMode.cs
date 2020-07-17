
namespace Paway.WPF
{
    /// <summary>
    /// PagerDisplayMode Enum
    /// </summary>
    /// <QualityBand>Preview</QualityBand>
    public enum PagerDisplayMode
    {
        /// <summary>
        /// Shows the First and Last buttons + the numeric display
        /// </summary>
        FirstLastNumeric,

        /// <summary>
        /// Shows the First, Last, Previous, Next buttons
        /// </summary>
        FirstLastPreviousNext,

        /// <summary>
        /// Shows the First, Last, Previous, Next buttons + the numeric display
        /// </summary>
        FirstLastPreviousNextNumeric,

        /// <summary>
        /// Shows the numeric display
        /// </summary>
        Numeric,

        /// <summary>
        /// Shows the Previous and Next buttons
        /// </summary>
        PreviousNext,

        /// <summary>
        /// Shows the Previous and Next buttons + the numeric display
        /// </summary>
        PreviousNextNumeric
    }
}
