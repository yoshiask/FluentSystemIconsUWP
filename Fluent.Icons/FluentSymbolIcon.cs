using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Fluent.Icons.Compact
{
    /// <summary>
    /// Represents an icon that uses a glyph from the Fluent System Icons font as its content.
    /// </summary>
    public partial class FluentSymbolIcon : FontIcon
    {
        /// <summary>
        /// The font family for displaying Fluent System Icons.
        /// </summary>
        public static FontFamily FSIRegularFontFamily = new FontFamily("ms-appx:///Fluent.Icons.Compact/Assets/FluentSystemIcons-Regular.ttf#FluentSystemIcons-Regular");

        /// <summary>
        /// The font family for displaying filled Fluent System Icons.
        /// </summary>
        public static FontFamily FSIFilledFontFamily = new FontFamily("ms-appx:///Fluent.Icons.Compact/Assets/FluentSystemIcons-Filled.ttf#FluentSystemIcons-Filled");

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentSymbolIcon"/> class.
        /// </summary>
        public FluentSymbolIcon()
        {
            FontFamily = FSIRegularFontFamily;
            FontSize = 32;
        }

        /// <summary>
        /// Constructs a <see cref="FluentSymbolIcon"/> with the specified symbol.
        /// </summary>
        public FluentSymbolIcon(FluentSymbol symbol)
        {
            FontFamily = GetFontFamilyForSymbol(symbol);
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
            nameof(Symbol), typeof(FluentSymbol), typeof(FluentSymbolIcon), new PropertyMetadata(null, OnSymbolChanged)
        );

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FluentSymbolIcon self && (e.NewValue is FluentSymbol || e.NewValue is int))
            {
                // Set internal glpyh and font family
                FluentSymbol symbol = (FluentSymbol)e.NewValue;
                self.Glyph = GetGlyph(symbol);
                self.FontFamily = GetFontFamilyForSymbol(symbol);
            }
        }

        /// <summary>
        /// Returns a <see cref="FontIcon"/> with the glyph associated with the provided <see cref="FluentSymbol"/>.
        /// </summary>
        public static FontIcon GetFontIcon(FluentSymbol symbol)
        {
            return new FontIcon {
                Glyph = GetGlyph(symbol),
                FontFamily = GetFontFamilyForSymbol(symbol)
            };
        }

        /// <summary>
        /// Returns the glyph associated with the provided <see cref="int"/>.
        /// The <paramref name="symbol"/> parameter is cast to <see cref="FluentSymbol"/>.
        /// </summary>
        public static string GetGlyph(int symbol)
        {
            var bytes = BitConverter.GetBytes(Math.Abs(symbol));
            return System.Text.Encoding.Unicode.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Returns the glyph associated with the provided <see cref="FluentSymbol"/>.
        /// </summary>
        public static string GetGlyph(FluentSymbol symbol)
        {
            return GetGlyph((int)symbol);
        }

        /// <summary>
        /// Returns the font family required to display the <paramref name="symbol"/>.
        /// </summary>
        public static FontFamily GetFontFamilyForSymbol(FluentSymbol symbol)
        {
            return (long)symbol < 0
                ? FSIFilledFontFamily
                : FSIRegularFontFamily;
        }
    }
}
