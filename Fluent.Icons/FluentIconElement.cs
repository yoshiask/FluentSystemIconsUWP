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

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Fluent.Icons
{
    public sealed class FluentIconElement : IconSourceElement
    {
        public FluentIconElement()
        {
            IconSource = new FluentIconSource();
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
            typeof(FluentSymbol), typeof(FluentIconSource),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSymbolChanged))
        );

        private static void OnSymbolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FluentIconElement self && (e.NewValue is FluentSymbol || e.NewValue is int))
            {
                if (self.IconSource is FluentIconSource source)
                {
                    // Set internal source to the new symbol
                    source.Symbol = (FluentSymbol)e.NewValue;
                }
            }
        }
    }
}
