using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    class AirlineCoordinator
    {
        FlightManager _flightManager;
        CustomerManager _customerManager;
        BookingManager _bookingManager;

        public AirlineCoordinator()
        {
            _flightManager = new FlightManager();
            _customerManager = new CustomerManager();
            _bookingManager = new BookingManager();
        }

        /// <summary>
        /// Adds a flight to the flight manager.
        /// </summary>
        /// <param name="flightNumber"></param>
        /// <param name="maxSeats"></param>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public bool AddFlight(int flightNumber, int maxSeats, string origin, string destination)
        {
            bool success = _flightManager.AddFlight(flightNumber, maxSeats, origin, destination);

            if (success)
                { _flightManager.Save(); }

            return success;
        }

        /// <summary>
        /// Deletes a flight from the flight manager.
        /// </summary>
        /// <param name="flightNumber">flight number of the flight to delete</param>
        /// <exception cref="FlightNotFoundException">Thrown if the flight is not found</exception
        /// <returns>true if flight was deleted. false otherwise</returns>
        public bool DeleteFlight(int flightNumber)
        {
            try
                { _flightManager.DeleteFlight(flightNumber); }
            catch (InvalidOperationException)
                { return false; }

            _flightManager.Save();
            return true;
        }

        /// <summary>
        /// Gets an array of all flights.
        /// </summary>
        /// <returns>Array of Flights</returns>
        public Flight[] GetFlights()
        {
            return _flightManager.GetAllFlights().Values.ToArray();
        }

        /// <summary>
        /// Passthrough method to the flight manager, gets flight by flight number.
        /// </summary>
        /// <param name="flightNumber">flightNumber to get</param>
        /// <returns>Flight if found, null if not</returns>
        public Flight GetFlight(int flightNumber)
        {
            return _flightManager.GetFlight(flightNumber);
        }

        /// <summary>
        /// Adds a customer to the customer manager.
        /// </summary>
        /// <param name="firstName">Customer's first name</param>
        /// <param name="lastName">Customer's last name</param>
        /// <param name="phoneNumber">Customer's phone number</param>
        /// <returns>true if customer was added, false otherwise</returns>
        public bool AddCustomer(string firstName, string lastName, string phoneNumber)
        {
            try
                { _customerManager.AddCustomer(firstName, lastName, phoneNumber); }
            catch (Errors.DuplicateCustomerException)
                { return false; }

            _customerManager.Save();
            return true;
        }

        /// <summary>
        /// Removes a customer from the customer manager.
        /// </summary>
        /// <param name="customerId">Id of the customer</param>
        /// <exception cref="CustomerNotFoundException">If the customer does not exist</exception>
        /// <returns>true if customer was deleted, false otherwise</returns>
        public bool DeleteCustomer(string customerId)
        {
            try
                { _customerManager.RemoveCustomer(customerId); }

            catch (InvalidOperationException)
                { return false; }

            _customerManager.Save();
            return true;
        }

        /// <summary>
        /// Gets an array of all registered customers.
        /// </summary>
        /// <returns>Array of customers</returns>
        public Customer[] GetCustomers()
        {
            return _customerManager.GetCustomers().Values.ToArray();
        }

        /// <summary>
        /// Passthrough method to the customer manager to get a customer by id
        /// </summary>
        /// <param name="customerId">id of the customer to get</param>
        /// <returns>Customer if found, null if not</returns>
        public Customer GetCustomer(string customerId)
        {
            return _customerManager.GetCustomer(customerId);
        }

        /// <summary>
        /// Adds a booking to the booking manager.
        /// </summary>
        /// <param name="date">Date and Time of the bookingt</param>
        /// <param name="flight">Associated Flight</param>
        /// <param name="customer">Associated Customer</param>
        /// <returns>true if the booking was added, false otherwise</returns>
        public bool AddBooking(DateTime date, Flight flight, Customer customer)
        {
            try
            {
                string bookingId = _bookingManager.AddBooking(date, flight.FlightNumber, customer.Id);
                customer.AddBookingReference(bookingId);
                flight.AddPassenger(customer);
            }

            catch (Errors.DuplicateBookingException)
                { return false; }

            //Save all changes
            _bookingManager.Save();
            _customerManager.Save();
            _flightManager.Save();
            return true;
        }

        /// <summary>
        /// Delete a booking from the booking manager.
        /// </summary>
        /// <param name="bookingId">Id of the booking to delete</param>
        public void DeleteBooking(string bookingId)
        {
            Booking booking = _bookingManager.GetBooking(bookingId);
            Customer customer = _customerManager.GetCustomer(booking.CustomerId);
            Flight flight = _flightManager.GetFlight(booking.FlightId);

            _bookingManager.RemoveBooking(bookingId);
            customer.RemoveBookingReference(bookingId);
            flight.RemovePassenger(booking.CustomerId);

            //Save all changes
            _bookingManager.Save();
            _customerManager.Save();
            _flightManager.Save();
        }

        public Booking[] GetBookings()
        {
            return _bookingManager.GetBookings().Values.ToArray();
        }
    }
}
