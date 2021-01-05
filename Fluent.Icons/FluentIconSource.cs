using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Fluent.Icons.Compact
{
    /// <summary>
    /// Represents an icon source that uses a Fluent System Icon as its content.
    /// </summary>
    public class FluentIconSource : FontIconSource
    {
        public FluentIconSource()
        {
            FontFamily = FluentSymbolIcon.FSIFontFamily;
        }

        /// <summary>
        /// Constructs an <see cref="IconSource"/> that uses a Fluent System Icon as its content.
        /// </summary>
        public FluentIconSource(FluentSymbol symbol)
        {
            FontFamily = FluentSymbolIcon.FSIFontFamily;
            Symbol = symbol;
        }

        /// <summary>
        /// Gets or sets the Fluent System Icons glyph used as the icon content.
        /// </summary>
        public FluentSymbol Symbol
        {
            get { return (FluentSymbol)GetValue(SymbolProperty); }
            set { SetValue(SymbolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Symbol.  This enables animation, styling, binding, etc...
        /// <summary>
        /// Identifies the <see cref="Symbol"/> property.
        /// </summary>
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(
            "Symbol",
            typeof(FluentSymbol), typeof(FluentIconSource),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSymbolChanged))
        );

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FluentIconSource self && (e.NewValue is FluentSymbol || e.NewValue is int))
            {
                // Set internal Image to the SvgImageSource from the look-up table
                self.Glyph = FluentSymbolIcon.GetGlyph((FluentSymbol)e.NewValue);
            }
        }
    }
}
