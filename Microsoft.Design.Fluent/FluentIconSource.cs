using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using static Microsoft.Design.Fluent.FluentSymbolIcon;

namespace Microsoft.Design.Fluent
{
    public partial class FluentIconSource : PathIconSource
    {
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

        public static PathIcon GetPathIcon(FluentSymbol symbol)
        {
            return new PathIcon
            {
                Data = (Geometry)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Geometry), GetPathData(symbol)),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        public static Geometry GetPathData(int symbol)
        {
            return GetPathData((FluentSymbol)symbol);
        }
        public static Geometry GetPathData(FluentSymbol symbol)
        {
            if (AllFluentIcons.TryGetValue(symbol, out string pathData))
            {
                return (Geometry)Windows.UI.Xaml.Markup.XamlBindingHelper.ConvertValue(typeof(Geometry), pathData);
            }
            else
            {
                return null;
            }
        }

    }
}
