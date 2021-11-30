using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace A2.Views
{
    public class ErrorDialogue : Window
    {
        public ErrorDialogue()
        {
            InitializeComponent();
            #if DEBUG
            this.AttachDevTools();
            #endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void CloseClick(object? sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
