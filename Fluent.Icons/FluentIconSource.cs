using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Fluent.Icons.FluentSymbolIcon;

namespace Fluent.Icons
{
    public partial class FluentIconSource : PathIconSource
    {
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
            typeof(FluentSymbol), typeof(FluentIconSource),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSymbolChanged))
        );

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FluentIconSource self && (e.NewValue is FluentSymbol || e.NewValue is int))
            {
                // Set internal Image to the SvgImageSource from the look-up table
                self.Data = GetPathData((FluentSymbol)e.NewValue);
            }
        }
    }
}
