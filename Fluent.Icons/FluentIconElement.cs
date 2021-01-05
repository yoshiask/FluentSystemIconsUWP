using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace Fluent.Icons.Compact
{
    /// <summary>
    /// Represents an icon that uses a <see cref="FluentIconSource"/> as its content.
    /// </summary>
    public sealed class FluentIconElement : IconSourceElement
    {
        /// <summary>
        /// Constructs an empty <see cref="FluentIconElement"/>.
        /// </summary>
        public FluentIconElement()
        {
            IconSource = new FluentIconSource();
        }

        /// <summary>
        /// Constructs a <see cref="FluentIconElement"/> displaying the specified symbol.
        /// </summary>
        /// <param name="symbol"></param>
        public FluentIconElement(FluentSymbol symbol)
        {
            IconSource = new FluentIconSource(symbol);
        }

        /// <summary>
        /// Constructs a <see cref="FluentIconElement"/> with the specified source.
        /// </summary>
        public FluentIconElement(FluentIconSource source)
        {
            IconSource = source;
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
