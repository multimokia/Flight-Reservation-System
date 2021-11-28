using System;
using System.Threading.Tasks;
using Avalonia.Data;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using System.Windows.Input;

using A2.Models;
using A2.Services;

using Library.Errors;
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
        public ICommand AddFlight {get;}

        public ReactiveCommand<Flight, Unit> DeleteFlight {get;}

        //Command implementations
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

        private async Task DeleteFlightCommand(Flight flight)
        {
            bool isSuccess = Coordinator.DeleteFlight(flight.FlightNumber);

            //The only possible failure is if the flight has customers booked on board
            //We should inform the user of this
            if (!isSuccess)
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel(
                    "Please remove all customers booked on this flight",
                    "Cannot delete flight"
                );
                await ErrorDialogue.Handle(error);
                return;
            }

            //Update flightlist
            Flights = Coordinator.GetFlights();
            return;
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

        public ReactiveCommand<Customer, Unit> DeleteCustomer {get;}

        //Command implementations
        public async Task AddCustomerCommand()
        {
            //Sanity check to make sure the user has entered all the required data and that it is valid
            if (
                string.IsNullOrWhiteSpace(_firstName)
                || string.IsNullOrWhiteSpace(_lastName)
                || string.IsNullOrWhiteSpace(_phoneNumber)
            )
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel(
                    "Please fill in all fields",
                    "Invalid data"
                );
                await ErrorDialogue.Handle(error);
                return;
            }

            bool isSuccess = Coordinator.AddCustomer(_firstName, _lastName, _phoneNumber);

            if (!isSuccess)
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel(
                    "Customer already exists",
                    "Duplicate Customer"
                );
                await ErrorDialogue.Handle(error);
                return;
            }

            //Update the customers list
            Customers = Coordinator.GetCustomers();
        }

        public async Task DeleteCustomerCommand(Customer customer)
        {
            bool isSuccess = Coordinator.DeleteCustomer(customer.Id);

            //The only reason this can fail is if the customer has a flight associated with them
            //So we should alert the user if this happens
            if (!isSuccess)
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel(
                    "Please make sure this customer has no bookings.",
                    "Invalid Operation"
                );

                await ErrorDialogue.Handle(error);
            }

            //Update customer list
            Customers = Coordinator.GetCustomers();
            return;
        }
        #endregion

        #region Booking UI Properties

        //View properties
        private Booking[] _bookings;

        public Booking[] Bookings
        {
            get => _bookings;
            set => this.RaiseAndSetIfChanged(ref _bookings, value);
        }

        Flight SelectedFlight {get; set;}

        Customer SelectedCustomer {get; set;}

        DateTime SelectedDate {get; set;}

        //Interactions and commands
        public ICommand CreateBooking {get;}

        public ReactiveCommand<Booking, Unit> DeleteBooking {get;}

        //Command implementations
        public async Task CreateBookingCommand()
        {
            //Sanity check to make sure the user has entered all the required data and that it is valid
            if (
                SelectedFlight is null
                || SelectedCustomer is null
            )
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel(
                    "Please select a flight, customer, and date",
                    "Invalid data"
                );
                await ErrorDialogue.Handle(error);
                return;
            }

            //Now let's check if the flight has room for more bookings
            try
                { bool isSuccess = Coordinator.AddBooking(SelectedDate, SelectedFlight, SelectedCustomer); }

            catch (DuplicateBookingException)
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel("Booking already exists.", "Duplicate Booking");
                await ErrorDialogue.Handle(error);
                return;
            }

            catch (InvalidOperationException)
            {
                ErrorDialogueViewModel error = new ErrorDialogueViewModel("Flight is fully booked.", "Invalid Operation");
                await ErrorDialogue.Handle(error);
                return;
            }

            //Update the bookings list
            Bookings = Coordinator.GetBookings();
        }

        public async Task DeleteBookingCommand(Booking booking)
        {
            Coordinator.DeleteBooking(booking.Id);

            //Update customer list
            Bookings = Coordinator.GetBookings();
            return;
        }

        #endregion


        /// <summary>
        /// Interation root to create error dialogues
        /// </summary>
        public Interaction<ErrorDialogueViewModel, object?> ErrorDialogue {get;}

        public MainWindowViewModel()
        {
            //Init our storage
            Flights = Coordinator.GetFlights();
            Customers = Coordinator.GetCustomers();
            Bookings = Coordinator.GetBookings();

            ////Register handlers
            //Flights
            AddFlight = ReactiveCommand.CreateFromTask(AddFlightCommand);
            DeleteFlight = ReactiveCommand.CreateFromTask<Flight>(DeleteFlightCommand);

            //Customers
            AddCustomer = ReactiveCommand.CreateFromTask(AddCustomerCommand);
            DeleteCustomer = ReactiveCommand.CreateFromTask<Customer>(DeleteCustomerCommand);

            //Bookings
            CreateBooking = ReactiveCommand.CreateFromTask(CreateBookingCommand);
            DeleteBooking = ReactiveCommand.CreateFromTask<Booking>(DeleteBookingCommand);

            ////Predefine inital field values
            //Flights
            _flightNumber = null;
            _numberOfSeats = null;
            _originAirport = "";
            _destinationAirport = "";

            //Customers
            _firstName = "";
            _lastName = "";
            _phoneNumber = "";

            //Bookings
            SelectedFlight = null;
            SelectedCustomer = null;
            SelectedDate = DateTime.Now;

            ErrorDialogue = new Interaction<ErrorDialogueViewModel, object?>();
        }
    }
}
