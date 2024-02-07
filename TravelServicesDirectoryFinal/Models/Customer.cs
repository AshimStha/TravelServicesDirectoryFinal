using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// reference library for the data annotations
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelServicesDirectoryFinal.Models
{
    public class Customer
    {
        // What describes a customer?

        [Key]
        // The PK of Customer table
        public int CustomerId { get; set; }

        // The firstname for customers
        public string Firstname { get; set; }

        // The lastname for customers
        public string Lastname { get; set; }

        // The date of birth
        public DateTime DOB { get; set; }

        // The customer's address
        public string Address { get; set; }

        // The email address for the customer
        public string Email { get; set; }

        // The phone number of the customer
        public string Phone { get; set; }


        // inverse relationship for describing the m-m relationship with the Package table
        // This is the (implicit way) of describing a bridging table
        /* 
         * In this way of creating a m-m relationship, there is no way of describing or adding columns
         * to the bridging table and the table only has the PKs from the related tables
         * 
         * For this project, the explicit way has been used
         */
        // public ICollection<Package> Packages { get; set; }


        /*
         * Note: For the previous version of the project, I got the InitialCreate migration when I registered a new 
         * user but it did not happen for this application. But the registration works fine.
         */
    }

    /*
     * A simpler version of the customer class with simpler data.
     * Used to display the shared columns between two tables but should have an explicit (FKs) way of m-m relationship
     * 
     * Do not need to add the migration for the DTO classes
     */
    public class CustomerDto
    {
        [Key]
        // The PK of Customer table
        public int CustomerId { get; set; }

        // The firstname for customers
        public string Firstname { get; set; }

        // The lastname for customers
        public string Lastname { get; set; }

        // The date of birth
        public DateTime DOB { get; set; }

        // The customer's address
        public string Address { get; set; }

        // The email address for the customer
        public string Email { get; set; }

        // The phone number of the customer
        public string Phone { get; set; }
    }
}