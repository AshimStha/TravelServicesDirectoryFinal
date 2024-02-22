using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelServicesDirectoryFinal.Models.ViewModels
{
    // This view model stores the information for the customers and packages model classes to use in the new bookings page
    public class DetailsBooking
    {
        public BookingDto SelectedBooking { get; set; }

        // the customerDto class
        public IEnumerable<CustomerDto> CustomersDetails { get; set; }

        // the packageDto class
        public IEnumerable<PackageDto> PackagesDetails { get; set; }
    }
}