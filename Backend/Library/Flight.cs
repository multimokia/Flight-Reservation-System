using System.Collections.Generic;

namespace Library
{
    class Flight
    {
        /// <summary>
        /// The flight number
        /// </summary>
        private int _flightNumber;
        public int FlightNumber {get { return _flightNumber; }}


        /// <summary>
        /// The origin airport of the flight.
        /// </summary>
        private string _originAirport;
        public string OriginAirport {get { return _originAirport; }}

        /// <summary>
        /// The destination airport of the flight.
        /// </summary>
        private string _destinationAirport;
        public string DestinationAirport {get { return _destinationAirport; }}

        /// <summary>
        /// The maximum number of passengers that can be on the flight.
        /// </summary>
        private int _maxPassengers;
        public int MaxSeats {get { return _maxPassengers; }}

        /// <summary>
        /// Map of passenger id -> passenger
        /// </summary>
        private Dictionary<string, Customer> _passengers;

        public Flight(int flightNumber, int maxSeats, string origin, string destination)
        {
            _flightNumber = flightNumber;
            _maxPassengers = maxSeats;
            _originAirport = origin;
            _destinationAirport = destination;
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
            if (GetNumPassengers() >= _maxPassengers)
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
        /// Removes a passenger from the flight. Returns false if nothin was removed.
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
            string rv = $"\n{additionalIndent}Passengers on flight " + _flightNumber + ":";
            foreach (Customer customer in _passengers.Values)
                { rv += $"\n\t{additionalIndent}{customer.FirstName} {customer.LastName}"; }

            return rv;
        }

        /// <summary>
        /// Menu prompt for use in Menu calls
        /// </summary>
        /// <returns>A MenuOption prompt for this type</returns>
        public string GetMenuPrompt()
        {
            return $"{this.FlightNumber.ToString()}: {this.OriginAirport} -> {this.DestinationAirport}";
        }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns>String representation of this flight</returns>
        public override string ToString()
        {
            return (
                $"Flight: {_flightNumber}"
                + $"\n\tOrigin: {_originAirport}"
                + $"\n\tDestination: {_destinationAirport}"
                + $"\n\tNumber of Passengers: {GetNumPassengers()}"
                + $"\n\tAvailable seats: {(_maxPassengers - GetNumPassengers())}"
                + $"{GetPassengerList("\t")}"
            );
        }
    }
}
