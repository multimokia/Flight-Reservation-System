using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using A2.ViewModels;
using Avalonia.Interactivity;
using ReactiveUI;
using Avalonia.ReactiveUI;

using Library;
namespace A2.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            #if DEBUG
            this.AttachDevTools();
            #endif

            this.WhenActivated(
                d => d(ViewModel!.ErrorDialogue.RegisterHandler(DoShowDialogueAsync))
            );
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task DoShowDialogueAsync(InteractionContext<ErrorDialogueViewModel, object> interaction)
        {
            ErrorDialogue dlg = new ErrorDialogue();
            dlg.DataContext = interaction.Input;

            object result = await dlg.ShowDialog<object>(this);
            interaction.SetOutput(result);
        }

        private void BookingsSubmit(object sender, RoutedEventArgs e)
        {
        }

        private void BookingsDelete(object sender, RoutedEventArgs e)
        {

        }
    }
}
