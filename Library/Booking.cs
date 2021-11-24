using System;

using static Library.Utilities.Utilities;
namespace Library
{
    public class Booking
    {
        /// <summary>
        /// Id of the booking
        /// </summary>
        private string _id;
        public string Id {get { return _id; }}

        /// <summary>
        /// Id of the flight associated with the booking
        /// </summary>
        private int _flightId;
        public int FlightId {get { return _flightId; }}

        /// <summary>
        /// Id of the customer associated with the booking
        /// </summary>
        private string _customerId;
        public string CustomerId {get { return _customerId; }}

        /// <summary>
        /// Date of the booking as timestamp
        /// </summary>
        private long _date;
        public long Date {get { return _date; }}

        public Booking(DateTime date, int flightId, string customerId)
        {
            _date = date.ToTimestamp();
            _flightId = flightId;
            _customerId = customerId;

            // Generate a unique id
            _id = HashString($"{flightId}{customerId}{_date}");
        }

        /// <summary>
        /// Returns a DateTime value of the booking's internal timestamp
        /// </summary>
        /// <returns>DateTime of timestamp</returns>
        public DateTime GetBookingDateTime()
        {
            return FromTimestamp(_date);
        }

        /// <summary>
        /// Prompt for menu options
        /// </summary>
        /// <returns>menu option prompt</returns>
        public string GetMenuPrompt()
        {
            return $"Customer {this.CustomerId} for flight {this.FlightId}. {this.GetBookingDateTime()}";
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>String representing the booking</returns>
        public override string ToString()
        {
            return $"{Id} {GetBookingDateTime()} {FlightId} {CustomerId}";
        }
    }
}
