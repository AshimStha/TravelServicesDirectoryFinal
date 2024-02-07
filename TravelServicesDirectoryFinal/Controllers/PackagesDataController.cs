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

        // PUT: api/PackagesData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPackage(int id, Package package)
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

        // POST: api/PackagesData
        [ResponseType(typeof(Package))]
        public IHttpActionResult PostPackage(Package package)
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