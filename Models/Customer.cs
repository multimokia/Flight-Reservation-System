using System.Collections.Generic;

using static Library.Utilities.Utilities;

namespace A2.Models
{
    public class Customer
    {
        /// <summary>
        /// Customer id
        /// </summary>
        private string _id;
        public string Id {get { return _id; }}

        /// <summary>
        /// Customer's first name
        /// </summary>
        private string _firstName;
        public string FirstName {get { return _firstName; }}

        /// <summary>
        /// Customer's last name
        /// </summary>
        private string _lastName;
        public string LastName {get { return _lastName; }}

        /// <summary>
        /// Customer's phone number
        /// </summary>
        private string _phoneNumber;
        public string PhoneNumber {get { return _phoneNumber; }}

        /// <summary>
        /// A reference to the bookings this customer has
        /// </summary>
        private List<string> _bookings;
        public List<string> Bookings {get { return _bookings; }}

        /// <summary>
        /// Menu prompt for use in a menuoption
        /// </summary>
        /// <returns>MenuOption prompt</returns>
        public string MenuPrompt => $"{this.FirstName} {this.LastName}";

        public Customer(string firstName, string lastName, string phoneNumber)
        {
            //For the sake of easy value comparison, we'll hash the id so we know if two customers
            //share the same info through the value of its id
            _id = HashString($"{firstName}{lastName}{phoneNumber}".Replace(" ", "").ToLower());
            _firstName = firstName;
            _lastName = lastName;
            _phoneNumber = phoneNumber;
            _bookings = new List<string>();
        }

        /// <summary>
        /// Adds a booking to the customer's list of bookings
        /// </summary>
        /// <param name="bookingId">id of the booking to add</param>
        public void AddBookingReference(string bookingId)
        {
            //Safety check to prevent duplicate data, this should never happen
            if (!_bookings.Contains(bookingId))
                { _bookings.Add(bookingId); }
        }

        /// <summary>
        /// Removes a booking from the customer's list of bookings
        /// </summary>
        /// <param name="bookingId">id of the booking to remove</param>
        public void RemoveBookingReference(string bookingId)
        {
            _bookings.Remove(bookingId);
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>A string representation of the Customer</returns>
        public override string ToString()
        {
            return (
                $"Customer:"
                + $"\n    Name: {_firstName} {_lastName}"
                + $"\n    Phone Number: {_phoneNumber}"
                + $"\n    Has {_bookings.Count} bookings"
            );
        }
    }
}
