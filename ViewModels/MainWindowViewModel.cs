using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using Avalonia.Data;
using Avalonia.Interactivity;
using ReactiveUI;

using A2.Models;
using A2.Services;

namespace A2.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        public static AirlineCoordinator Coordinator = new AirlineCoordinator();

        //We need this to force an update on the view
        private Flight[] _flights;
        public Flight[] Flights
        {
            get => _flights;
            set => this.RaiseAndSetIfChanged(ref _flights, value);
        }

        public int FlightNumber {get; set;}

        public int NumberOfSeats {get; set;}

        public string OriginAirport
        {
            get;
            set;
        }

        public string DestinationAirport
        {
            get;
            set;
        }

        public ReactiveCommand<Flight, bool> DeleteFlight {get;}

        public ReactiveCommand<string, bool> AddFlight {get;}


        public MainWindowViewModel()
        {
            Flights = Coordinator.GetFlights();

            //Register handlers
            DeleteFlight = ReactiveCommand.Create<Flight, bool>(DeleteFlightButtonCommand);
            AddFlight = ReactiveCommand.Create<string, bool>(AddFlightButtonCommand);
        }

        private bool DeleteFlightButtonCommand(Flight flight)
        {
            bool isSuccess = Coordinator.DeleteFlight(flight.FlightNumber);

            //Update flightlist
            Flights = Coordinator.GetFlights();
            return isSuccess;
        }

        private bool AddFlightButtonCommand(string _)
        {
            bool isSuccess = Coordinator.AddFlight(FlightNumber, NumberOfSeats, OriginAirport, DestinationAirport);

            Console.WriteLine($"Flight added: {isSuccess}");
            //Update the flights list
            Flights = Coordinator.GetFlights();
            return isSuccess;
        }
    }
}
