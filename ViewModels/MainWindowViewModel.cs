using System;
using System.Threading.Tasks;
using Avalonia.Data;
using System.Reactive.Linq;
using ReactiveUI;
using System.Windows.Input;

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

        private int? _flightNumber;

        //FlightNumber property serves as validation for the internal _flightNumber field
        public string FlightNumber
        {
            get => _flightNumber.ToString() ?? "";
            set {
                if (int.TryParse(value, out int result))
                {
                    _flightNumber = result;
                    this.RaiseAndSetIfChanged(ref _flightNumber, result);
                }
                else
                {
                    _flightNumber = null;
                    throw new DataValidationException("Flight number must be an integer");
                }
            }
        }

        private int? _numberOfSeats;
        public string NumberOfSeats
        {
            get =>  _numberOfSeats.ToString() ?? "";
            set {
                if (int.TryParse(value, out int result))
                {
                    _numberOfSeats = result;
                    this.RaiseAndSetIfChanged(ref _numberOfSeats, result);
                }
                else
                {
                    _numberOfSeats = null;
                    throw new DataValidationException("Number of seats must be an integer");
                }
            }
        }

        private string _originAirport;
        public string OriginAirport
        {
            get => _originAirport;
            set {
                if (value.Length == 0)
                {
                    _originAirport = "";
                    throw new DataValidationException("Origin airport cannot be empty");
                }

                this.RaiseAndSetIfChanged(ref _originAirport, value);
            }
        }

        private string _destinationAirport;
        public string DestinationAirport
        {
            get => _destinationAirport;
            set {
                if (value.Length == 0)
                {
                    _destinationAirport = "";
                    throw new DataValidationException("Destination airport cannot be empty");
                }

                this.RaiseAndSetIfChanged(ref _destinationAirport, value);
            }
        }

        public bool ShouldSubmitBeEnabled =>
            _flightNumber.HasValue &&
            _numberOfSeats.HasValue &&
            !string.IsNullOrWhiteSpace(_originAirport) &&
            !string.IsNullOrEmpty(_destinationAirport);

        public ReactiveCommand<Flight, bool> DeleteFlight {get;}

        public ICommand AddFlight {get;}

        public Interaction<ErrorDialogueViewModel, object?> ErrorDialogue {get;}

        public MainWindowViewModel()
        {
            Flights = Coordinator.GetFlights();

            //Register handlers
            DeleteFlight = ReactiveCommand.Create<Flight, bool>(DeleteFlightButtonCommand);

            AddFlight = ReactiveCommand.CreateFromTask(AddFlightCommand);

            _flightNumber = null;
            _numberOfSeats = null;
            _originAirport = "";
            _destinationAirport = "";

            ErrorDialogue = new Interaction<ErrorDialogueViewModel, object?>();
        }

        private bool DeleteFlightButtonCommand(Flight flight)
        {
            bool isSuccess = Coordinator.DeleteFlight(flight.FlightNumber);

            //Update flightlist
            Flights = Coordinator.GetFlights();
            return isSuccess;
        }

        private async Task AddFlightCommand()
        {
            if (!ShouldSubmitBeEnabled)
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel("Please fill in all fields", "Invalid data");
                await ErrorDialogue.Handle(error);
                return;
            }

            bool isSuccess = Coordinator.AddFlight(
                (int)_flightNumber,
                (int)_numberOfSeats,
                _originAirport,
                _destinationAirport
            );

            Console.WriteLine($"Flight added: {isSuccess}");
            //Update the flights list
            Flights = Coordinator.GetFlights();
        }
    }
}
