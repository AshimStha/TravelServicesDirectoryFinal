using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TravelServicesDirectoryFinal.Models;

namespace TravelServicesDirectoryFinal.Controllers
{
    public class PackagesDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /*
         * A function to list the packages present in the database.
         * It uses the Package and PackageDto model classes to access the data.
         * 
         * GET: curl https://localhost:44375/api/PackagesData/ListPackages
         * 
         * Result:
         * 
         * [{"PkgId":1,"Name":"Trip to Italy","Type":"Vacation","AccommodationType":"Hotel","Destination":"Italy",
         * "Departure":"2023-12-28T00:00:00","Arrival":"2024-01-10T00:00:00","Cost":1450.0},{"PkgId":2,"Name":"Maldives Visit",
         * "Type":"Medical","AccommodationType":"Resort","Destination":"Maldives","Departure":"2023-12-21T00:00:00",
         * "Arrival":"2024-01-23T00:00:00","Cost":2300.0}]
         * 
         */

        // GET: api/PackagesData/ListPackages
        [HttpGet]
        public IEnumerable<PackageDto> ListPackages()
        {
            List<Package> Packages = db.Packages.ToList();
            List<PackageDto> PackageDtos = new List<PackageDto>();

            // Here, we are specifying that the attribute on the left (from the DTO class) is equal to the 
            // attribute on the right (from the model class)
            Packages.ForEach(p => PackageDtos.Add(new PackageDto()
            {
                PkgId = p.PkgId,
                Name = p.Name,
                Type = p.Type,
                AccommodationType = p.AccommodationType,
                Destination = p.Destination,
                Departure = p.Departure,
                Arrival = p.Arrival,
                Cost = p.Cost
            }));

            return PackageDtos;
        }

        /*
         * A function to find a specific package from the database using the id
         * It uses the Package and PackageDto model classes to access the data.
         * 
         * GET: curl https://localhost:44375/api/PackagesData/FindPackage/2
         * 
         * Result:
         * 
         * {"PkgId":2,"Name":"Maldives Visit","Type":"Medical","AccommodationType":"Resort","Destination":"Maldives",
         * "Departure":"2023-12-21T00:00:00","Arrival":"2024-01-23T00:00:00","Cost":2300.0}
         * 
         */

        // GET: api/PackagesData/FindPackage/2
        [ResponseType(typeof(Package))]
        [HttpGet]
        public IHttpActionResult FindPackage(int id)
        {
            Package Package = db.Packages.Find(id);
            PackageDto PackageDto = new PackageDto()
            {
                PkgId = Package.PkgId,
                Name = Package.Name,
                Type = Package.Type,
                AccommodationType = Package.AccommodationType,
                Destination = Package.Destination,
                Departure = Package.Departure,
                Arrival = Package.Arrival,
                Cost = Package.Cost
            };

            if (Package == null)
            {
                // returns the 404 page not found error
                return NotFound();
            }
            // returning the dto model class object
            return Ok(PackageDto);
        }

        /*
         * A function to update a specific package from the database using the id.
         * It uses the Package model class to access the data and has the id and package object as parameters.
         * 
         * To use this file as post data,
         *  - Navigate to the root folder
         *  - Copy the file path as text
         *  - Provide the path in the curl request
         *  - cd into that directory
         * 
         * To use the curl request method
         * 
         * POST: curl -d @packages.json -H "Content-type:application/json" https://localhost:44375/api/PackagesData/UpdatePackage/5
         * 
         * The file name only method was not working. Used the relative path to the file.
         * 
         * curl -d @C:\Users\Asus\source\repos\TravelServicesDirectoryFinal\TravelServicesDirectoryFinal\jsondata\packages.json -H "Content-type:application/json" https://localhost:44375/api/PackagesData/UpdatePackage/5
         * 
         * Here, @packages.json is the post data that is passed with the request while -H is the header type for the request.
         * 
         *  -> -d is the post data
         *  -> -H is the information for the type of content we are sending
         *  
         *  JSON Data for the update (To add, just remove the id attribute)
         *  {
                "PkgId": 5,
                "Name": "Malta Vacay",
                "Type": "Holiday",
                "AccommodationType": "Resort",
                "Destination": "Malta",
                "Departure": "12-29-2023",
                "Arrival": "01-10-2024",
                "Cost": 1480
            }
         * 
         * Result:
         * 
         * The package data in the local database is updated.
         * 
         */

        // POST: api/PackagesData/UpdatePackage/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePackage(int id, Package package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != package.PkgId)
            {
                return BadRequest();
            }

            db.Entry(package).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PackageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /*
         * A function to add a new package to the system.
         * It uses the Package model class to add a new entry.
         * 
         * To use this file as post data,
         *  - Navigate to the root folder
         *  - Copy the file path as text
         *  - Provide the path in the curl request
         *  - cd into that directory
         * 
         * To use the curl request method
         * 
         * POST: curl -d @packages.json -H "Content-type:application/json" https://localhost:44375/api/PackagesData/AddPackage
         * 
         * The file name only method was not working. Used the relative path to the file.
         * 
         * curl -d @C:\Users\Asus\source\repos\TravelServicesDirectoryFinal\TravelServicesDirectoryFinal\jsondata\packages.json -H 
         * "Content-type:application/json" https://localhost:44375/api/PackagesData/AddPackage
         * 
         * Here, @packages.json is the post data that is passed with the request while -H is the header type for the request.
         * 
         *  -> -d is the post data
         *  -> -H is the information for the type of content we are sending
         * 
         * After, adding one entry, we can just change the values from the json data below and keep adding more entries.
         * This json data can serve as the post data when we try to add a new package
         * 
         * Result: 
         * 
         * A new package is added to the system's database.
         * 
         * {"PkgId":5,"Name":"Malta Vacay","Type":"Medical","AccommodationType":"Hotel","Destination":"Malta","Departure":"2023-12-29T00:00:00",
         * "Arrival":"2024-01-10T00:00:00","Cost":1480.0}
         * 
         */

        // POST: api/PackagesData/AddPackage
        [ResponseType(typeof(Package))]
        [HttpPost]
        public IHttpActionResult AddPackage(Package package)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Packages.Add(package);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = package.PkgId }, package);
        }

        /*
         * A function to delete a specific package from the database using the id
         * It uses the Package model class to delete the data.
         * 
         * GET: curl -d "" https://localhost:44375/api/PackagesData/DeletePackage/3
         * 
         * Here, "" is the post data and it being empty means no post data was passed.
         * 
         * Result: 
         * 
         * The selected data entry with the id is deleted from the database.
         * 
         */

        // POST: api/PackagesData/DeletePackage/3
        [ResponseType(typeof(Package))]
        [HttpPost]
        public IHttpActionResult DeletePackage(int id)
        {
            Package package = db.Packages.Find(id);
            if (package == null)
            {
                return NotFound();
            }

            db.Packages.Remove(package);
            db.SaveChanges();

            // Not returning anything in the OK()
            // Passing the object showed "null" values
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PackageExists(int id)
        {
            return db.Packages.Count(e => e.PkgId == id) > 0;
        }
    }
}