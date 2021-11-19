using System;
using System.Collections.Generic;

using Library.Errors;
using static Library.Utilities.Persistence;
namespace Library
{
    class FlightManager : IPersistable
    {
        const string FLIGHT_PERSISTENCE_FILE = "./data/flights.json";
        private Dictionary<int, Flight> _flights;

        public FlightManager()
        {
            this.Load();
        }

        public void Save()
        {
            OverwriteJson(_flights, FLIGHT_PERSISTENCE_FILE);
        }

        public void Load()
        {
            CreateIfNotExists(FLIGHT_PERSISTENCE_FILE);
            _flights = LoadFromJson<Dictionary<int, Flight>>(FLIGHT_PERSISTENCE_FILE);
        }

        /// <summary>
        /// Creates and adds a flight
        /// </summary>
        /// <param name="flightNumber">flight number</param>
        /// <param name="maxSeats">Maximum amount of seats on this flight</param>
        /// <param name="origin">Origin airport</param>
        /// <param name="destination">Destination airport</param>
        /// <returns></returns>
        public bool AddFlight(int flightNumber, int maxSeats, string origin, string destination)
        {
            //If we already have this flight, don't try to add again
            if (_flights.ContainsKey(flightNumber))
                { return false; }

            Flight flight = new Flight(flightNumber, maxSeats, origin, destination);
            _flights.Add(flightNumber, flight);
            return true;
        }

        /// <summary>
        /// Gets a flight by flight number
        /// </summary>
        /// <param name="flightNumber">flight number to retrieve with</param>
        /// <returns>Flight object if found, null otherwise</returns>
        public Flight GetFlight(int flightNumber)
        {
            if (!_flights.ContainsKey(flightNumber))
                { return null; }

            return _flights[flightNumber];
        }

        /// <summary>
        /// Deletes a flight
        /// </summary>
        /// <param name="flightNumber">flight number of the flight to delete</param>
        /// <exception cref="FlightNotFoundException">Thrown if the flight doesn't exist</exception>
        /// <exception cref="InvalidOperationException">Thrown if the flight has passengers</exception>
        public void DeleteFlight(int flightNumber)
        {
            //Flights can only be deleted if they have no passengers
            Flight flight = GetFlight(flightNumber);

            if (flight == null)
                { throw new FlightNotFoundException(flightNumber); }

            if (flight.GetNumPassengers() > 0)
                { throw new InvalidOperationException("Flights may only be deleted if they have no passengers booked."); }

            //If checks pass, remove the flight
            _flights.Remove(flightNumber);
        }

        /// <summary>
        /// Gets a human readable list of flights
        /// </summary>
        /// <returns>List of flights as string</returns>
        public Dictionary<int, Flight> GetAllFlights()
        {
            return _flights;
        }
    }
}
