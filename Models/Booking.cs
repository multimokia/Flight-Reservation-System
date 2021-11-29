using System;
using Newtonsoft.Json;

using static Library.Utilities.Utilities;

namespace A2.Models
{
    public class Booking
    {
        /// <summary>
        /// Id of the booking
        /// </summary>
        public string Id {get; init;}

        /// <summary>
        /// Id of the flight associated with the booking
        /// </summary>
        public int FlightId {get; init;}

        /// <summary>
        /// Id of the customer associated with the booking
        /// </summary>
        public string CustomerId {get; init;}

        /// <summary>
        /// Date of the booking as timestamp
        /// </summary>
        public long TimeStamp {get; init;}

        /// <summary>
        /// Prompt for menu options
        /// </summary>
        /// <returns>menu option prompt</returns>
        [JsonIgnore]
        public String MenuPrompt => $"At {this.GetBookingDateTime()} for flight {this.FlightId}.";

        public Booking(DateTime date, Flight flight, Customer customer)
        {
            TimeStamp = date.ToTimestamp();
            FlightId = flight.FlightNumber;
            CustomerId = customer.Id;

            // Generate a unique id
            Id = HashString($"{FlightId}{CustomerId}{TimeStamp}");
        }

        [JsonConstructor]
        public Booking(string id, int flightId, string customerId, long timestamp)
        {
            Id = id;
            FlightId = flightId;
            CustomerId = customerId;
            TimeStamp = timestamp;
        }

        /// <summary>
        /// Returns a DateTime value of the booking's internal timestamp
        /// </summary>
        /// <returns>DateTime of timestamp</returns>
        public DateTime GetBookingDateTime()
        {
            return FromTimestamp(TimeStamp);
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>String representing the booking</returns>
        public override string ToString()
        {
            return (
                "Booking:"
                + $"\n    On: {GetBookingDateTime()}"
                + $"\n    Flightnumber: {FlightId}"
                + $"\n    For: {CustomerId}");
        }
    }
}
