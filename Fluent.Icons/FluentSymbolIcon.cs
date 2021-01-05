using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Fluent.Icons.Compact
{
    public partial class FluentSymbolIcon : Control
    {
        public static FontFamily FSIFontFamily = new FontFamily("/Assets/FluentUI-System-Icons.ttf#FluentUI-System-Icons");

        private FontIcon iconDisplay;

        public FluentSymbolIcon()
        {
            DefaultStyleKey = typeof(FluentSymbolIcon);
        }

        /// <summary>
        /// Constructs a <see cref="FluentSymbolIcon"/> with the specified symbol.
        /// </summary>
        public FluentSymbolIcon(FluentSymbol symbol)
        {
            DefaultStyleKey = typeof(FluentSymbolIcon);
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
        public static readonly DependencyProperty SymbolProperty = DependencyProperty.Register(
            "Symbol",
            typeof(FluentSymbol), typeof(FluentSymbolIcon),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSymbolChanged))
        );

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("IconDisplay") is FontIcon pi)
            {
                iconDisplay = pi;
                // Awkward workaround for a weird bug where iconDisplay is null
                // when OnSymbolChanged fires in a newly created FluentSymbolIcon
                Symbol = Symbol;
            }
        }

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FluentSymbolIcon self && (e.NewValue is FluentSymbol || e.NewValue is int) && self.iconDisplay != null)
            {
                // Set internal Image to the SvgImageSource from the look-up table
                self.iconDisplay.Glyph = GetGlyph((FluentSymbol)e.NewValue);
            }
        }

        /// <summary>
        /// Returns a new <see cref="PathIcon"/> using the path associated with the provided <see cref="FluentSymbol"/>.
        /// </summary>
        public static FontIcon GetFontIcon(FluentSymbol symbol)
        {
            return new FontIcon {
                Glyph = GetGlyph(symbol),
                FontFamily = FSIFontFamily
            };
        }

        /// <summary>
        /// Returns a new <see cref="Geometry"/> using the path associated with the provided <see cref="int"/>.
        /// The <paramref name="symbol"/> parameter is cast to <see cref="FluentSymbol"/>.
        /// </summary>
        public static string GetGlyph(int symbol)
        {
            return GetGlyph((FluentSymbol)symbol);
        }

        /// <summary>
        /// Returns a new <see cref="Geometry"/> using the path associated with the provided <see cref="int"/>.
        /// </summary>
        public static string GetGlyph(FluentSymbol symbol)
        {
            return unchecked((char)symbol).ToString();
        }
    }
}
