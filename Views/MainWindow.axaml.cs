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
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void _addFlightButton(object? sender, RoutedEventArgs e)
        {
            TextBox flightNum = this.FindControl<TextBox>("FlightNumber");
            TextBox numSeats = this.FindControl<TextBox>("NumSeats");
            TextBox originAirport = this.FindControl<TextBox>("OriginAirport");
            TextBox destinationAirport = this.FindControl<TextBox>("DestinationAirport");

            if (
                MainWindowViewModel.Coordinator.AddFlight(
                    Convert.ToInt32(flightNum.Text),
                    Convert.ToInt32(numSeats.Text),
                    originAirport.Text,
                    destinationAirport.Text
                )
            )
            {
                flightNum.Text = "";
                numSeats.Text = "";
                originAirport.Text = "";
                destinationAirport.Text = "";
            }
        }

        private async Task ShowDialogueAsync()
        {

        }
    }
}
