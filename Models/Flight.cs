using System.Collections.Generic;

namespace A2.Models
{
    public class Flight
    {
        /// <summary>
        /// The flight number
        /// </summary>
        //private int _flightNumber;
        public int FlightNumber {get; init;}


        /// <summary>
        /// The origin airport of the flight.
        /// </summary>
        //private string _originAirport;
        public string OriginAirport {get; init;}

        /// <summary>
        /// The destination airport of the flight.
        /// </summary>
        //private string _destinationAirport;
        public string DestinationAirport {get; init;}

        /// <summary>
        /// The maximum number of passengers that can be on the flight.
        /// </summary>
        //private int _maxPassengers;
        public int MaxSeats {get; init;}

        /// <summary>
        /// Map of passenger id -> passenger
        /// </summary>
        private Dictionary<string, Customer> _passengers;

        public string MenuPrompt => $"{this.FlightNumber.ToString()}: {this.OriginAirport} -> {this.DestinationAirport}";

        public Flight(int flightNumber, int maxSeats, string origin, string destination)
        {
            FlightNumber = flightNumber;
            MaxSeats = maxSeats;
            OriginAirport = origin;
            DestinationAirport = destination;
            _passengers = new Dictionary<string, Customer>();
        }

        /// <summary>
        /// Helper method to get the number of passengers on the flight.
        /// </summary>
        /// <returns>amount of passengers registered on this flight</returns>
        public int GetNumPassengers()
        {
            return _passengers.Count;
        }

        /// <summary>
        /// Adds a passenger to the flight if there is room.
        /// </summary>
        /// <param name="customer">Customer object to add</param>
        /// <returns>True if the passenger was added, False otherwise</returns>
        public bool AddPassenger(Customer customer)
        {
            //Reject if we're maxed out on seats
            if (GetNumPassengers() >= MaxSeats)
                { return false; }

            //Otherwise add
            _passengers.Add(customer.Id, customer);
            return true;
        }

        /// <summary>
        /// Tries to get a passenger by id. Returns null if not found.
        /// </summary>
        /// <param name="customerId">Id of the customer to get</param>
        /// <returns>Customer object with the given id if found, null if not.</returns>
        public Customer GetPassenger(string customerId)
        {
            if (!_passengers.ContainsKey(customerId))
                { return null; }

            return _passengers[customerId];
        }

        /// <summary>
        /// Removes a passenger from the flight. Returns false if nothing was removed.
        /// </summary>
        /// <param name="customerId">Id of the customer to remove</param>
        /// <returns>true if removed successfully, false otherwise</returns>
        public bool RemovePassenger(string customerId)
        {
            return _passengers.Remove(customerId);
        }

        /// <summary>
        /// Returns a human readable list of all passengers on the flight.
        /// </summary>
        /// <param name="additionalIndent">If the output should be indented, the indent may be supplied. (Default: "")</param>
        /// <returns>A string list of all passengers on the flight</returns>
        public string GetPassengerList(string additionalIndent="")
        {
            if (_passengers.Count == 0)
                { return "None"; }

            string rv = $"\n{additionalIndent}Passengers on flight " + FlightNumber + ":";
            foreach (Customer customer in _passengers.Values)
                { rv += $"\n\t{additionalIndent}{customer.FirstName} {customer.LastName}"; }

            return rv;
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>String representation of this flight</returns>
        public override string ToString()
        {
            return (
                $"Flight: {FlightNumber}:"
                + $"\n    From {OriginAirport} to {DestinationAirport}."
                + $"\n    Number of Passengers: {GetNumPassengers()} "
                + $"\n    Available seats: {(MaxSeats - GetNumPassengers())}"
                + $"\n    Passengers on board: {GetPassengerList("    ")}"
            );
        }
    }
}
