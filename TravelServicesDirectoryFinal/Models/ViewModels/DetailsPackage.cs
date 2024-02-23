using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelServicesDirectoryFinal.Models.ViewModels
{
    public class DetailsPackage
    {
        public PackageDto SelectedPackage { get; set; }
        public IEnumerable<BookingDto> RelatedCustomers { get; set; }
    }
}