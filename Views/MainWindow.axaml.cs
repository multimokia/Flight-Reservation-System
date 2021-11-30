using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using A2.ViewModels;
using Avalonia.Interactivity;
using ReactiveUI;
using Avalonia.ReactiveUI;

using A2.Models;
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

        /// <summary>
        /// Function to show a dialogue
        /// </summary>
        /// <param name="interaction">The interaction used</param>
        /// <returns>Task.CompletedTask</returns>
        private async Task DoShowDialogueAsync(InteractionContext<ErrorDialogueViewModel, object> interaction)
        {
            ErrorDialogue dlg = new ErrorDialogue();
            dlg.DataContext = interaction.Input;

            object result = await dlg.ShowDialog<object>(this);
            interaction.SetOutput(result);
        }

        /// <summary>
        /// This is very much a hack to force an update without formally implementing INotifyCollectionChanged and INotifyPropertyChanged
        /// in a nested context and basically redoing my entire backend.
        /// </summary>
        /// <param name="sender">Button element as object which sent this clickEvent</param>
        /// <param name="e">Eventargs</param>
        private void BookingsSubmit(object sender, RoutedEventArgs e)
        {
            if (DataContext is null)
                { return; }

            MainWindowViewModel? mwvm = (DataContext as MainWindowViewModel);

            if (mwvm is null)
                { return; }

            mwvm.CreateBooking.Execute(null);

            object? datactx = DataContext;
            DataContext = null;
            DataContext = datactx;
        }

        /// <summary>
        /// Also a hack to force an update without formally implementing INotifyCollectionChanged and INotifyPropertyChanged
        /// </summary>
        /// <param name="sender">Button element as object which sent this clickEvent</param>
        /// <param name="e">Eventargs</param>
        private void BookingsDelete(object sender, RoutedEventArgs e)
        {
            if (sender is null)
                { return; }

            Button? btn = sender as Button;

            if (btn is null)
                { return; }

            Booking? booking = btn.CommandParameter as Booking;

            if (booking is null)
                { return; }

            //If data context isn't null, we know it is a MainWindowViewModel
            (DataContext as MainWindowViewModel)!.DeleteBooking.Execute(booking);

            object? datactx = DataContext;
            DataContext = null;
            DataContext = datactx;
        }
    }
}
