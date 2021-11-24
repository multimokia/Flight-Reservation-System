using System;
using System.Collections.Generic;

using Library.Errors;
using static Library.Utilities.Persistence;

namespace Library
{
    class CustomerManager : IPersistable
    {
        const string CUSTOMER_PERSISTENCE_FILE = "./data/customers.json";
        private Dictionary<string, Customer> _customers;

        public CustomerManager()
        {
            this.Load();
        }

        public void Save()
        {
            OverwriteJson(_customers, CUSTOMER_PERSISTENCE_FILE);
        }

        public void Load()
        {
            CreateIfNotExists(CUSTOMER_PERSISTENCE_FILE);
            _customers = LoadFromJson<Dictionary<string, Customer>>(CUSTOMER_PERSISTENCE_FILE);
        }

        /// <summary>
        /// Registers a customer. Checks for duplicates
        /// </summary>
        /// <param name="firstName">Customer's first name</param>
        /// <param name="lastName">Customer's last name</param>
        /// <param name="phoneNumber">Customer's phone number</param>
        /// <exception cref="DuplicateCustomerException">Thrown when customer is already registered</exception>
        public void AddCustomer(string firstName, string lastName, string phoneNumber)
        {
            Customer customer = new Customer(firstName, lastName, phoneNumber);

            //If we already have a customer w/ this specification, we throw an exception
            if (HasCustomer(customer.Id))
                { throw new DuplicateCustomerException(customer); }

            //Otherwise add the customer to the dictionary
            _customers.Add(customer.Id, customer);
        }

        /// <summary>
        /// Removes a customer, runs all checks to see if it's possible for them to be removed
        /// </summary>
        /// <param name="customerId">Id of the customer to delete</param>
        /// <exception cref="CustomerNotFoundException">If the customer doesn't exist</exception>
        /// <exception cref="InvalidOperationException">If the customer has bookings</exception>
        public void RemoveCustomer(string customerId)
        {
            Customer customer = GetCustomer(customerId);

            if (customer == null)
                { throw new CustomerNotFoundException(customerId); }

            //A customer may only be deleted if and only if they have no bookings
            if (customer.Bookings.Count > 0)
                { throw new InvalidOperationException("A customer can only be removed if they have no bookings."); }

            _customers.Remove(customerId);
        }

        /// <summary>
        /// Checks if the given if is in the customers dictionary
        /// </summary>
        /// <param name="customerId">Id of the customer check for</param>
        /// <returns>true if a customer with the given id exists</returns>
        public bool HasCustomer(string customerId)
        {
            return _customers.ContainsKey(customerId);
        }

        /// <summary>
        /// Adds a booking to the customer's bookings
        /// </summary>
        /// <param name="customerId">Id of the customer to add a booking to</param>
        /// <param name="bookingId">Id of the booking to add</param>
        public void AddBookingReference(string customerId, string bookingId)
        {
            Customer customer = GetCustomer(customerId);

            if (customer == null)
                { throw new CustomerNotFoundException(customerId); }

            customer.AddBookingReference(bookingId);
        }

        /// <summary>
        /// Safe getter for customers
        /// </summary>
        /// <param name="customerId">id of the customer to get</param>
        /// <returns>Customer object if found or null if not</returns>
        public Customer GetCustomer(string customerId)
        {
            if (!_customers.ContainsKey(customerId))
                { return null; }

            return _customers[customerId];
        }

        /// <summary>
        /// Returns the customer dict
        /// </summary>
        /// <returns>Dict of customers</returns>
            public Dictionary<string, Customer> GetCustomers()
        {
            return _customers;
        }
    }
}
