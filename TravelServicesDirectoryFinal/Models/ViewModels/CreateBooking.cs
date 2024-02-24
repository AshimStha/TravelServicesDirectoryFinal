using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelServicesDirectoryFinal.Models.ViewModels
{
    public class CreateBooking
    {
        public IEnumerable<PackageDto> packages { get; set; }

        public IEnumerable<CustomerDto> customers { get; set; }
    }
}