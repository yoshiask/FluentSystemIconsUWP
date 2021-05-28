using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Fluent.Icons.Compact
{
    /// <summary>
    /// Represents an icon source that uses a Fluent System Icon as its content.
    /// </summary>
    public class FluentIconSource : FontIconSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FluentIconSource"/> class.
        /// </summary>
        public FluentIconSource()
        {
            FontFamily = FluentSymbolIcon.FSIRegularFontFamily;
            FontSize = 32;
        }

        /// <summary>
        /// Constructs an <see cref="IconSource"/> that uses a Fluent System Icon as its content.
        /// </summary>
        public FluentIconSource(FluentSymbol symbol)
        {
            FontFamily = FluentSymbolIcon.GetFontFamilyForSymbol(symbol);
            FontSize = 32;
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

        /// <summary>
        /// Identifies the <see cref="Symbol"/> property.
        /// </summary>
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(
            nameof(Symbol), typeof(FluentSymbol), typeof(FluentIconSource),
            new PropertyMetadata(null, OnSymbolChanged)
        );

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FluentIconSource self && (e.NewValue is FluentSymbol || e.NewValue is int))
            {
                // Set glpyh and font family
                FluentSymbol symbol = (FluentSymbol)e.NewValue;
                self.Glyph = FluentSymbolIcon.GetGlyph(symbol);
                self.FontFamily = FluentSymbolIcon.GetFontFamilyForSymbol(symbol);
            }
        }
    }
}
