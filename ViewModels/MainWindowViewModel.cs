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

        #region Flight UI Properties
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

        //Commands and interactions
        public ReactiveCommand<Flight, bool> DeleteFlight {get;}

        public ICommand AddFlight {get;}

        private async Task AddFlightCommand()
        {
            //Sanity check to make sure the user has entered all the required data and that it is valid
            if (
                _flightNumber is null
                || _numberOfSeats is null
                || string.IsNullOrWhiteSpace(_originAirport)
                || string.IsNullOrEmpty(_destinationAirport)
            )
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

            //The only possible failure is if the flight already exists.
            //We should inform the user of this
            if (!isSuccess)
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel("Flight already exists", "Duplicate flight");
                await ErrorDialogue.Handle(error);
                return;
            }

            //Update the flights list
            Flights = Coordinator.GetFlights();
        }

        private bool DeleteFlightCommand(Flight flight)
        {
            bool isSuccess = Coordinator.DeleteFlight(flight.FlightNumber);

            //The only possible failure is if the flight has customers booked on board
            //We should inform the user of this
            if (!isSuccess)
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel("Please remove all customers booked on this flight", "Cannot delete flight");
                ErrorDialogue.Handle(error);
                return false;
            }

            //Update flightlist
            Flights = Coordinator.GetFlights();
            return isSuccess;
        }

        #endregion

        #region Customer UI Properties
        //View properties
        private Customer[] _customers;

        public Customer[] Customers
        {
            get => _customers;
            set => this.RaiseAndSetIfChanged(ref _customers, value);
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set {
                if (value.Length == 0)
                {
                    _firstName = "";
                    throw new DataValidationException("First name cannot be empty");
                }

                this.RaiseAndSetIfChanged(ref _firstName, value);
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set {
                if (value.Length == 0)
                {
                    _lastName = "";
                    throw new DataValidationException("Last name cannot be empty");
                }

                this.RaiseAndSetIfChanged(ref _lastName, value);
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get => _phoneNumber;
            set {
                if (value.Length == 0)
                {
                    _phoneNumber = "";
                    throw new DataValidationException("Phone number cannot be empty");
                }

                this.RaiseAndSetIfChanged(ref _phoneNumber, value);
            }
        }

        //Interactions and commands
        public ICommand AddCustomer {get;}

        public ReactiveCommand<Customer, bool> DeleteCustomer {get;}

        public async Task AddCustomerCommand()
        {
            //Sanity check to make sure the user has entered all the required data and that it is valid
            if (
                string.IsNullOrWhiteSpace(_firstName)
                || string.IsNullOrWhiteSpace(_lastName)
                || string.IsNullOrWhiteSpace(_phoneNumber)
            )
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel("Please fill in all fields", "Invalid data");
                await ErrorDialogue.Handle(error);
                return;
            }

            bool isSuccess = Coordinator.AddCustomer(_firstName, _lastName, _phoneNumber);

            if (!isSuccess)
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel("Customer already exists", "Duplicate Customer");
                await ErrorDialogue.Handle(error);
                return;
            }

            //Update the customers list
            Customers = Coordinator.GetCustomers();
        }

        public bool DeleteCustomerCommand(Customer customer)
        {
            bool isSuccess = Coordinator.DeleteCustomer(customer.Id);

            //The only reason this can fail is if the customer has a flight associated with them
            //So we should alert the user if this happens
            if (!isSuccess)
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel(
                    "Please make sure this customer is not booked on any flights.",
                    "Invalid Operation"
                );
            }

            //Update customer list
            Customers = Coordinator.GetCustomers();
            return isSuccess;
        }
        #endregion



        public Interaction<ErrorDialogueViewModel, object?> ErrorDialogue {get;}

        public MainWindowViewModel()
        {
            //Init our storage
            Flights = Coordinator.GetFlights();
            Customers = Coordinator.GetCustomers();

            ////Register handlers
            //Flights
            AddFlight = ReactiveCommand.CreateFromTask(AddFlightCommand);
            DeleteFlight = ReactiveCommand.Create<Flight, bool>(DeleteFlightCommand);

            //Customers
            AddCustomer = ReactiveCommand.CreateFromTask(AddCustomerCommand);
            DeleteCustomer = ReactiveCommand.Create<Customer, bool>(DeleteCustomerCommand);

            _flightNumber = null;
            _numberOfSeats = null;
            _originAirport = "";
            _destinationAirport = "";

            ErrorDialogue = new Interaction<ErrorDialogueViewModel, object?>();
        }
    }
}
