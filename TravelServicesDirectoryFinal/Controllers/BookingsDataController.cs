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
    public class BookingsDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /*
         * A function to list the bookings made in the database.
         * It uses the Booking, BookingDto, Customer and Package model classes to access the data.
         * 
         * The attributes like the firstname, lastname, Name and Type are the attributes accessed using the entity relationship. 
         * 
         * GET: curl https://localhost:44375/api/BookingsData/ListBookings
         * 
         * Result:
         * 
         * [{"BookingId":1,"Status":"Pending","BookingDate":"2023-12-10T00:00:00","Firstname":"Christine","Lastname":"Bittle",
         * "Name":"Trip to Italy","Type":"Vacation","GrandTotal":1500.0},{"BookingId":2,"Status":"Paid","BookingDate":"2023-12-02T00:00:00",
         * "Firstname":"Ashim","Lastname":"Test","Name":"Maldives Visit","Type":"Medical","GrandTotal":2500.0}]
         * 
         */

        // GET: api/BookingsData/ListBookings
        [HttpGet]
        public IEnumerable<BookingDto> ListBookings()
        {
            List<Booking> Bookings = db.Bookings.ToList();
            List<BookingDto> BookingDtos = new List<BookingDto>();

            Bookings.ForEach(b => BookingDtos.Add(new BookingDto()
            {
                BookingId = b.BookingId,
                // using the entity relationship for these Customer entity columns
                Firstname = b.Customer.Firstname,
                Lastname = b.Customer.Lastname,
                // using the entity relationship for these Package entity columns
                Name = b.Package.Name,
                Type = b.Package.Type,
                Status = b.Status,
                BookingDate = b.BookingDate,
                GrandTotal = b.GrandTotal
            }));

            return BookingDtos;
        }

        /*
         * A function to find a specific booking from the database using the id
         * It uses the Booking, BookingDto, Customer and Package model classes to access the data.
         * 
         * GET: curl https://localhost:44375/api/BookingsData/FindBooking/1
         * 
         * Result:
         * 
         * {"BookingId":1,"Status":"Pending","BookingDate":"2023-12-10T00:00:00","Firstname":"Christine","Lastname":"Bittle",
         * "Name":"Trip to Italy","Type":"Vacation","GrandTotal":1500.0}
         * 
         */

        // GET: api/BookingsData/FindBooking/1
        [ResponseType(typeof(Booking))]
        [HttpGet]
        public IHttpActionResult FindBooking(int id)
        {
            Booking Booking = db.Bookings.Find(id);
            BookingDto BookingDto = new BookingDto()
            {
                BookingId = Booking.BookingId,
                // using the entity relationship for these Customer entity columns
                Firstname = Booking.Customer.Firstname,
                Lastname = Booking.Customer.Lastname,
                // using the entity relationship for these Package entity columns
                Name = Booking.Package.Name,
                Type = Booking.Package.Type,
                Status = Booking.Status,
                BookingDate = Booking.BookingDate,
                GrandTotal = Booking.GrandTotal
            };

            if (Booking == null)
            {
                // returns the 404 page not found error
                return NotFound();
            }
            // returning the dto model class object
            return Ok(BookingDto);
        }

        /*
        * A function to update a specific booking from the database using the id.
        * It uses the Booking model class to access the data and has the id and booking object as parameters.
        * 
        * To use this file as post data,
        *  - Navigate to the root folder
        *  - Copy the file path as text
        *  - Provide the path in the curl request
        *  - cd into that directory
        * 
        * To use the curl request method
        * 
        * POST: curl -d @bookings.json -H "Content-type:application/json" https://localhost:44375/api/BookingsData/UpdateBooking/6
        * 
        * The file name only method was not working. Used the relative path to the file.
        * 
        * curl -d @C:\Users\Asus\source\repos\TravelServicesDirectoryFinal\TravelServicesDirectoryFinal\jsondata\bookings.json -H "Content-type:application/json" https://localhost:44375/api/BookingsData/UpdateBooking/6
        * 
        * Here, @bookings.json is the post data that is passed with the request while -H is the header type for the request.
        * 
        *  -> -d is the post data
        *  -> -H is the information for the type of content we are sending
        *  
        *  JSON Data for the update (To add, just remove the id attribute)
        *  {
               "BookingId": 6,
               "Status": "Paid",
               "BookingDate": "12/03/2023",
               "GrandTotal": "1250",
               "PkgId": "2",
               "CustomerId": "6"
           }
        * 
        * Result:
        * 
        * The booking data in the local database is updated.
        * 
        */

        // POST: api/BookingsData/UpdateBooking/6
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateBooking(int id, Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != booking.BookingId)
            {
                return BadRequest();
            }

            db.Entry(booking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
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
         * A function to add a new booking to the system.
         * It uses the Booking model class to add a new entry.
         * 
         * To use this file as post data,
         *  - Navigate to the root folder
         *  - Copy the file path as text
         *  - Provide the path in the curl request
         *  - cd into that directory
         * 
         * To use the curl request method
         * 
         * POST: curl -d @bookings.json -H "Content-type:application/json" https://localhost:44375/api/BookingsData/AddBooking
         * 
         * The file name only method was not working. Used the relative path to the file.
         * 
         * curl -d @C:\Users\Asus\source\repos\TravelServicesDirectoryFinal\TravelServicesDirectoryFinal\jsondata\bookings.json -H "Content-type:application/json" https://localhost:44375/api/BookingsData/AddBooking
         * 
         * Here, @bookings.json is the post data that is passed with the request while -H is the header type for the request.
         * 
         *  -> -d is the post data
         *  -> -H is the information for the type of content we are sending
         * 
         * After, adding one entry, we can just change the values from the json data below and keep adding more entries.
         * This json data can serve as the post data when we try to add a new booking
         * 
         * Note:
         * ------------
         * 
         * This json data can serve as the post data when we try to add a new booking but as this table shares some ids from other
         * 2 tables (Customer and Package), we should mind about the data validity for those columns.
         * 
         * Since, the Pk has been shared, there should already be an existing id for the package and customer before inserting into the 
         * bookings bridging table since we are using them to add into the bridging table.
         * 
         * They not existing will violate the FK relationship.
         * 
         * Result: 
         * 
         * A new booking is added to the system's database.
         * 
         * {"BookingId":4,"Status":"Pending","BookingDate":"2023-12-03T00:00:00","GrandTotal":1250.0,"PkgId":1,"Package":null,"CustomerId":1,
         * "Customer":null}
         * 
         */

        // POST: api/BookingsData/AddBooking
        [ResponseType(typeof(Booking))]
        [HttpPost]
        public IHttpActionResult AddBooking(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(booking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = booking.BookingId }, booking);
        }

        /*
         * A function to delete a specific booking from the database using the id
         * It uses the Booking model class to delete the data.
         * 
         * GET: curl -d "" https://localhost:44375/api/BookingsData/DeleteBooking/3
         * 
         * Here, "" is the post data and it being empty means no post data was passed.
         * 
         * Result: 
         * 
         * The selected data entry with the id is deleted from the database.
         * 
         */

        // POST: api/BookingsData/DeleteBooking/3
        [ResponseType(typeof(Booking))]
        [HttpPost]
        public IHttpActionResult DeleteBooking(int id)
        {
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }

            db.Bookings.Remove(booking);
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

        private bool BookingExists(int id)
        {
            return db.Bookings.Count(e => e.BookingId == id) > 0;
        }
    }
}