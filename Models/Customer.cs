using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

using static Library.Utilities.Utilities;

namespace A2.Models
{
    public class Customer
    {
        /// <summary>
        /// Customer id
        /// </summary>
        public string Id {get; init;}

        /// <summary>
        /// Customer's first name
        /// </summary>
        public string FirstName {get; init;}

        /// <summary>
        /// Customer's last name
        /// </summary>
        public string LastName {get; init;}

        /// <summary>
        /// Customer's phone number
        /// </summary>
        public string PhoneNumber {get; init;}

        /// <summary>
        /// A reference to the bookings this customer has
        /// </summary>
        public List<string> Bookings {get; init;}

        /// <summary>
        /// Menu prompt for use in a menuoption
        /// </summary>
        /// <returns>MenuOption prompt</returns>
        [JsonIgnore]
        public string MenuPrompt => $"{this.FirstName} {this.LastName}";

        public Customer(string firstName, string lastName, string phoneNumber)
        {
            //For the sake of easy value comparison, we'll hash the id so we know if two customers
            //share the same info through the value of its id
            Id = HashString($"{firstName}{lastName}{phoneNumber}".Replace(" ", "").ToLower());
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Bookings = new List<string>();
        }

        /// <summary>
        /// Json constructor for initializing customers w/ all persistent data
        /// </summary>
        /// <param name="id">Customer Id</param>
        /// <param name="firstName">First name</param>
        /// <param name="lastName">Last name</param>
        /// <param name="phoneNumber">Phone number</param>
        /// <param name="bookings">Registered booking references</param>
        [JsonConstructor]
        public Customer(string id, string firstName, string lastName, string phoneNumber, List<string> bookings)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Bookings = bookings;
        }

        /// <summary>
        /// Adds a booking to the customer's list of bookings
        /// </summary>
        /// <param name="bookingId">id of the booking to add</param>
        public void AddBookingReference(string bookingId)
        {
            //Safety check to prevent duplicate data, this should never happen
            if (!Bookings.Contains(bookingId))
                { Bookings.Add(bookingId); }
        }

        /// <summary>
        /// Removes a booking from the customer's list of bookings
        /// </summary>
        /// <param name="bookingId">id of the booking to remove</param>
        public void RemoveBookingReference(string bookingId)
        {
            Bookings.Remove(bookingId);
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>A string representation of the Customer</returns>
        public override string ToString()
        {
            return (
                $"Customer:"
                + $"\n    Name: {FirstName} {LastName}"
                + $"\n    Phone Number: {PhoneNumber}"
                + $"\n    Has {Bookings.Count} bookings"
                + $"\n    Id: {Id}"
            );
        }
    }
}
