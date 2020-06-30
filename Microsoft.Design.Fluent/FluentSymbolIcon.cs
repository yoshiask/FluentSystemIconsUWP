using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Microsoft.Design.Fluent
{
    public partial class FluentSymbolIcon : Control
    {
        private Image imageDisplay;

        public FluentSymbolIcon()
        {
            this.DefaultStyleKey = typeof(FluentSymbolIcon);
        }

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

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (this.GetTemplateChild("ImageDisplay") is Image id)
                this.imageDisplay = id;
        }

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FluentSymbolIcon self && (e.NewValue is FluentSymbol || e.NewValue is int) && self.imageDisplay != null)
            {
                FluentSymbol val = (FluentSymbol)e.NewValue;
                // Check to see if Value really changed, else this was invoked because of boxing
                if (val != (FluentSymbol)e.OldValue)
                {
                    // TODO: Set internal Image to the SvgImageSource from the look-up table
                    if (AllFluentIcons.TryGetValue(val, out SvgImageSource svgSource))
                    {
                        self.imageDisplay.Source = svgSource;
                    }
                }
            }
        }
    }
}
