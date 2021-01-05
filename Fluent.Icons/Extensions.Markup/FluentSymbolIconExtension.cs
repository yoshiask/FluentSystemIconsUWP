using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

namespace Fluent.Icons.Extensions.Markup
{
    /// <summary>
    /// Custom <see cref="MarkupExtension"/> which can provide symbol-based <see cref="FontIcon"/> values.
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(FontIcon))]
    public class SymbolIconExtension : TextIconExtension
    {
        /// <summary>
        /// Gets or sets the <see cref="FluentSymbol"/> value representing the icon to display.
        /// </summary>
        public FluentSymbol Symbol { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            var fontIcon = new FluentSymbolIcon(Symbol);

            //if (FontSize > 0)
            //{
            //    fontIcon.FontSize = FontSize;
            //}

            //if (Foreground != null)
            //{
            //    fontIcon.Foreground = Foreground;
            //}

            return fontIcon;
        }
    }
}