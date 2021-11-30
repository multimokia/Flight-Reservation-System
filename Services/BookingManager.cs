using System;
using System.Collections.Generic;

using Library.Errors;
using static Library.Utilities.Persistence;

using A2.Models;

namespace A2.Services
{
    class BookingManager : IPersistable
    {
        const string BOOKING_PERSISTENCE_FILE = "./data/bookings.json";

        private Dictionary<string, Booking> _bookings;

        public BookingManager()
        {
            this.Load();
        }

        public void Save()
        {
            OverwriteJson(_bookings, BOOKING_PERSISTENCE_FILE);
        }

        public void Load()
        {
            CreateIfNotExists(BOOKING_PERSISTENCE_FILE);
            _bookings = LoadFromJson<Dictionary<string, Booking>>(BOOKING_PERSISTENCE_FILE);
        }

        /// <summary>
        /// Creates a new booking.
        /// </summary>
        /// <param name="date">DateTime of the booking</param>
        /// <param name="flight">associated flight</param>
        /// <param name="customer">associated customer</param>
        /// <exception cref="DuplicateBookingException">Thrown when a booking already exists for the given flight and customer</exception>
        /// <exception cref="InvalidOperationException">Thrown when the flight is already filled</exception>
        /// <returns>Id of the created booking</returns>
        public string AddBooking(DateTime date, Flight flight, Customer customer)
        {
            Booking booking = new Booking(date, flight, customer);

            if (_bookings.ContainsKey(booking.Id))
                { throw new DuplicateBookingException(booking); }

            //Check if we can add the passenger to the flight
            if (!flight.AddPassenger(customer))
                { throw new InvalidOperationException("Flight is already filled"); }

            //Checks pass, formally add the reference
            customer.AddBookingReference(booking.Id);

            //Add the booking to the dict and register its reference in the customer
            _bookings.Add(booking.Id, booking);
            return booking.Id;
        }

        /// <summary>
        /// Deletes a booking
        /// </summary>
        /// <param name="bookingId">Id of the booking to remove</param>
        /// <returns>True if booking was removed, False otherwise</returns>
        public bool RemoveBooking(string bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                { return false; }

            return _bookings.Remove(bookingId);
        }

        /// <summary>
        /// Gets a specific booking by id
        /// </summary>
        /// <param name="bookingId">id of the booking to get</param>
        /// <returns>Booking object if found, null if not</returns>
        public Booking GetBooking(string bookingId)
        {
            if (!_bookings.ContainsKey(bookingId))
                { return null; }

            return _bookings[bookingId];
        }

        /// <summary>
        /// Getter for bookings
        /// </summary>
        /// <returns>Dict of bookings registered</returns>
        public Dictionary<string, Booking> GetBookings()
        {
            return _bookings;
        }
    }
}
