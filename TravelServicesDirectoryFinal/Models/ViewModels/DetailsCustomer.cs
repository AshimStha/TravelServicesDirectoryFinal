using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelServicesDirectoryFinal.Models.ViewModels
{
    public class DetailsCustomer
    {
        public CustomerDto SelectedCustomer { get; set; }
        public IEnumerable<BookingDto> RelatedBookings { get; set; }
    }
}