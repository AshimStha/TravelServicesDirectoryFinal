using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// reference library for the data annotations
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelServicesDirectoryFinal.Models
{
    public class Booking
    {
        // What describes a booking?

        // The primary key for the Booking table (Bridging table created using the explicit way)
        [Key]
        public int BookingId { get; set; }

        // The status of the booking made
        public string Status { get; set; }

        // The date of booking
        public DateTime BookingDate { get; set; }

        // The grand total - will be the total after Tax/VAT
        public float GrandTotal { get; set; }


        // Creating a one-to-many relationship between Booking and Package tables (The explicit way)(uses one-many relationship)
        // As a Booking has only one package for an individual

        /*
         * Note: After a new table has been created and the foreign key relationship has been made, to run the commands
         * to create-database, we first need to clear the manually added rows in the table. Then, try updating the database 
         * again.
         */

        // The PK from the Package table
        [ForeignKey("Package")]
        public int PkgId { get; set; }

        // To access the name of the package from the Package table
        // A virtual navigation property
        // The idea being when a booking is checked, the selected package is also accessible
        public virtual Package Package { get; set; }

        // Creating a one-to-many relationship between Booking and Customers tables
        // A customer has could have many bookings

        // The PK from the Customers table
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        // To access the name of the customer from the Customer table
        // A virtual navigation property
        // The idea being when a booking is checked, the selected customer is also accessible
        public virtual Customer Customer { get; set; }
    }

    /*
     * A simpler version of the booking class with simpler data.
     * Used to display the shared columns between two tables but should have an explicit (FKs) way of m-m relationship
     */
    public class BookingDto
    {
        [Key]
        // The PK of Booking table
        public int BookingId { get; set; }

        // The payment status
        public string Status { get; set; }

        // The date when the booking was done
        public DateTime BookingDate { get; set; }

        // The firstname for customers
        public string Firstname { get; set; }

        // The lastname for customers
        public string Lastname { get; set; }

        // The name of the travel package
        public string Name { get; set; }

        // The type of the travel package
        public string Type { get; set; }

        // The total after tax and VAT
        public float GrandTotal { get; set; }
    }
}