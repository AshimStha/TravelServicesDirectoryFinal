using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
// reference library for the data annotations
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelServicesDirectoryFinal.Models
{
    public class Package
    {
        // What describes a travel package?

        // The primary key for this entity
        [Key]
        public int PkgId { get; set; }

        // The name of the travel package
        public string Name { get; set; }

        // The type of the travel package
        public string Type { get; set; }

        // The type of accommodation the user might select
        public string AccommodationType { get; set; }

        // The destination for the package
        public string Destination { get; set; }

        // The departure date
        public DateTime Departure { get; set; }

        // The arrival/return date
        public DateTime Arrival { get; set; }

        // The cost for the package
        public float Cost { get; set; }


        // Inverse relation - usually done for denoting a m-m relationship
        // This is the (implicit way) of describing a bridging table
        /* 
         * In this way of creating a m-m relationship, there is no way of describing or adding columns
         * to the bridging table and the table only has the PKs from the related tables
         * 
         * Used explicit way of defining a m-m relationship
         */

        // A package has many bookings

        // public ICollection<Customer> Customers { get; set; }
    }

    /*
     * A simpler version of the customer class with simpler data.
     * Used to display the shared columns between two tables but should have an explicit (FKs) way of m-m relationship
     */
    public class PackageDto
    {
        // The primary key for this entity
        [Key]
        public int PkgId { get; set; }

        // The name of the travel package
        public string Name { get; set; }

        // The type of the travel package
        public string Type { get; set; }

        // The type of accommodation the user might select
        public string AccommodationType { get; set; }

        // The destination for the package
        public string Destination { get; set; }

        // The departure date
        public DateTime Departure { get; set; }

        // The arrival/return date
        public DateTime Arrival { get; set; }

        // The cost for the package
        public float Cost { get; set; }
    }
}