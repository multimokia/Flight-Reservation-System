using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using Avalonia.Data;
using Avalonia.Interactivity;
using ReactiveUI;

using Library;

namespace A2.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static AirlineCoordinator Coordinator = new AirlineCoordinator();
        public Interaction<FullFlightInfoViewModel, AlbumViewModel?> ShowDialogue {get; }

        public Flight[] Flights { get => Coordinator.GetFlights(); }

        private object _flightNumber;
        public object FlightNumber
        {
            get => _flightNumber;
            set
            {
                if (value is not int)
                {
                    throw new DataValidationException("Please enter an integer value");
                }

                _flightNumber = value;
            }
        }

        public string OriginAirport
        {
            set
            {
                if (value.Length == 0)
                    { throw new DataValidationException("This value is required."); }
            }
        }

        public MainWindowViewModel()
        {
            ShowDialogue = new Interaction<FullFlightInfoViewModel, AlbumViewModel?>();
        }
    }
}
