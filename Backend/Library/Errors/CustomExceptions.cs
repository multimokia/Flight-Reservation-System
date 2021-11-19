using System;

namespace Library.Errors
{
    public class FlightNotFoundException : Exception
    {
        private int _flightNumber;
        public override string Message
        {
            get { return $"A flight with flight number {_flightNumber} was not found."; }
        }

        public FlightNotFoundException(int flightNumber)
        {
            _flightNumber = flightNumber;
        }
    }

    public class DuplicateCustomerException : Exception
    {
        private Customer _customer;
        public override string Message
        {
            get { return $"A customer with name {_customer.FirstName} {_customer.LastName} and number {_customer.PhoneNumber} already exists."; }
        }

        public DuplicateCustomerException(Customer customer)
        {
            _customer = customer;
        }
    }

    public class DuplicateBookingException : Exception
    {
        private Booking _booking;
        public override string Message
        {
            get { return $"A booking with this specification already exists ({_booking.Id})."; }
        }

        public DuplicateBookingException(Booking booking)
        {
            _booking = booking;
        }
    }

    public class CustomerNotFoundException : Exception
    {
        private string _customerId;

        public override string Message
        {
            get { return $"A customer with id {_customerId} was not found."; }
        }

        public CustomerNotFoundException(string customerId)
        {
            _customerId = customerId;
        }
    }
}
