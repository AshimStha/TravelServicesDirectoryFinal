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
    public class CustomersDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /*
         * A function to list the customers present in the database.
         * It uses the Customer and CustomerDto model classes to access the data.
         * 
         * GET: curl https://localhost:44375/api/CustomersData/ListCustomers
         * 
         * Result:
         * 
         * [{"CustomerId":1,"Firstname":"Christine","Lastname":"Bittle","DOB":"1999-12-28T00:00:00","Address":"29 uber Road",
         * "Email":"Christine@gmail.com","Phone":"123-456-7890"},{"CustomerId":2,"Firstname":"Ashim","Lastname":"Test",
         * "DOB":"1987-12-21T00:00:00","Address":"165 boley road","Email":"ashim@gmail.com","Phone":"121-234-2213"}]
         */

        // GET: api/CustomersData/ListCustomers
        [HttpGet]
        public IEnumerable<CustomerDto> ListCustomers()
        {
            List<Customer> Customers = db.Customers.ToList();
            List<CustomerDto> CustomerDtos = new List<CustomerDto>();

            Customers.ForEach(c => CustomerDtos.Add(new CustomerDto()
            {
                CustomerId = c.CustomerId,
                Firstname = c.Firstname,
                Lastname = c.Lastname,
                DOB = c.DOB,
                Address = c.Address,
                Email = c.Email,
                Phone = c.Phone
            }));

            return CustomerDtos;
        }

        /*
         * A function to find a specific customer from the database using the id.
         * It uses the Customer and CustomerDto model classes to access the data.
         * 
         * GET: curl https://localhost:44375/api/CustomersData/FindCustomer/2
         * 
         * Result:
         * 
         * {"CustomerId":2,"Firstname":"Ashim","Lastname":"Test","DOB":"1987-12-21T00:00:00","Address":"165 boley road",
         * "Email":"ashim@gmail.com","Phone":"121-234-2213"}
         * 
         */

        // GET: api/CustomersData/FindCustomer/2
        [ResponseType(typeof(Customer))]
        [HttpGet]
        public IHttpActionResult FindCustomer(int id)
        {
            Customer Customer = db.Customers.Find(id);
            CustomerDto CustomerDto = new CustomerDto()
            {
                CustomerId = Customer.CustomerId,
                Firstname = Customer.Firstname,
                Lastname = Customer.Lastname,
                DOB = Customer.DOB,
                Address = Customer.Address,
                Email = Customer.Email,
                Phone = Customer.Phone
            };

            if (Customer == null)
            {
                // returns 404 not found
                return NotFound();
            }
            // returning the data transfer object
            return Ok(CustomerDto);
        }

        // PUT: api/CustomersData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerId)
            {
                return BadRequest();
            }

            db.Entry(customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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
         * A function to add a new customer to the system.
         * It uses the Customer model class to add a new entry.
         * 
         * POST: curl -d @customers.json -H "Content-type:application/json" https://localhost:44375/api/CustomersData/AddCustomer
         * 
         * Here, @customers.json is the post data that is passed with the request while -H is the header type for the request.
         * 
         * Result: 
         * 
         * A new customer is added to the system's database.
         * 
         */

        // POST: api/CustomersData/AddCustomer
        [ResponseType(typeof(Customer))]
        [HttpPost]
        public IHttpActionResult AddCustomer(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = customer.CustomerId }, customer);
        }

        /*
         * A function to delete a specific customer from the database using the id.
         * It uses the Customer model class to delete the data.
         * 
         * POST: curl -d "" https://localhost:44375/api/CustomersData/DeleteCustomer/3
         * 
         * Here, "" is the post data and it being empty means no post data was passed.
         * 
         * Result: 
         * 
         * The selected data entry with the id is deleted from the database.
         * 
         */

        // POST: api/CustomersData/DeleteCustomer/3
        [ResponseType(typeof(Customer))]
        [HttpPost]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
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

        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerId == id) > 0;
        }
    }
}