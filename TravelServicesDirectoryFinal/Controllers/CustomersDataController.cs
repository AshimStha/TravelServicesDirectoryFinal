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
using System.Diagnostics;

namespace TravelServicesDirectoryFinal.Controllers
{
    public class CustomersDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// A function to list the customers present in the database.
        /// It uses the Customer and CustomerDto model classes to access the data.
        /// </summary>
        /// 
        /// <example>
        /// GET: curl https://localhost:44375/api/CustomersData/ListCustomers
        /// </example>
        /// 
        /// <returns>
        /// The list of customers in the system db.
        /// 
        /// [{"CustomerId":1,"Firstname":"Christine","Lastname":"Bittle","DOB":"1999-12-28T00:00:00","Address":"29 uber Road",
        /// "Email":"Christine@gmail.com","Phone":"123-456-7890"},{"CustomerId":2,"Firstname":"Ashim","Lastname":"Test",
        /// "DOB":"1987-12-21T00:00:00","Address":"165 boley road","Email":"ashim@gmail.com","Phone":"121-234-2213"}]
        /// </returns>
        /// 

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

        /// <summary>
        /// A function to find a specific customer from the database using the id.
        /// It uses the Customer and CustomerDto model classes to access the data.
        /// </summary>
        /// 
        /// <example>
        /// GET: curl https://localhost:44375/api/CustomersData/FindCustomer/2
        /// </example>
        /// 
        /// <param name="id">The customer id</param>
        /// 
        /// <returns>
        /// The searched customer details.
        /// 
        /// {"CustomerId":2,"Firstname":"Ashim","Lastname":"Test","DOB":"1987-12-21T00:00:00","Address":"165 boley road",
        ///"Email":"ashim@gmail.com","Phone":"121-234-2213"}
        /// </returns>

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

        /// <summary>
        /// A function to update a specific customer from the database using the id.
        /// It uses the Customer model class to access the data and has the id and customer object as parameters.
        /// 
        /// To use this file as post data,
        ///     - Navigate to the root folder
        ///     - Copy the file path as text
        ///     - Provide the path in the curl request
        ///     - cd into that directory
        ///     
        /// To use the curl request method
        /// POST: curl -d @customers.json -H "Content-type:application/json" https://localhost:44375/api/CustomersData/UpdateCustomer/6
        /// 
        /// The file name only method was not working. Used the relative path to the file.
        /// curl -d @C:\Users\Asus\source\repos\TravelServicesDirectoryFinal\TravelServicesDirectoryFinal\jsondata\customers.json -H "Content-type:application/json" https://localhost:44375/api/CustomersData/UpdateCustomer/6
        /// 
        /// Here, @customers.json is the post data that is passed with the request while -H is the header type for the request.
        /// -> -d is the post data
        /// -> -H is the information for the type of content we are sending
        /// 
        /// JSON Data for the update (To add, just remove the id attribute)
        /// {
        ///     "CustomerId": 6,
        ///     "Firstname": "Daniel",
        ///     "Lastname": "Bryan",
        ///     "DOB": "11/12/1991",
        ///     "Address": "87 Chimichangas Street",
        ///     "Email": "Daniel@gmail.com",
        //      "Phone": "122-352-0000"
        /// }
        /// </summary>
        /// 
        /// <param name="id">The customer id</param>
        /// <param name="customer">The customer model object</param>
        /// 
        /// <returns>
        /// The customer data in the local database is updated.
        /// </returns>
        /// 

        // POST: api/CustomersData/UpdateCustomer/6
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCustomer(int id, Customer customer)
        {
            Debug.WriteLine("I am in the update customer method!");
            Debug.WriteLine("The received customer id is: " +  id);
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

        /// <summary>
        /// A function to add a new customer to the system.
        /// It uses the Customer model class to add a new entry.
        ///     
        /// To use the curl request method
        /// POST: curl -d @customers.json -H "Content-type:application/json" https://localhost:44375/api/CustomersData/AddCustomer
        /// 
        /// The file name only method was not working. Used the relative path to the file.
        /// curl -d @C:\Users\Asus\source\repos\TravelServicesDirectoryFinal\TravelServicesDirectoryFinal\jsondata\customers.json -H "Content-type:application/json" https://localhost:44375/api/CustomersData/AddCustomer
        /// 
        /// Here, @customers.json is the post data that is passed with the request while -H is the header type for the request.
        /// -> -d is the post data
        /// -> -H is the information for the type of content we are sending
        /// 
        /// After, adding one entry, we can just change the values from the json data below and keep adding more entries.
        /// This json data can serve as the post data when we try to add a new customer.
        /// 
        /// {
        ///     "Firstname": "Daniel",
        ///     "Lastname": "Bryan",
        ///     "DOB": "11/12/1991",
        ///     "Address": "87 Chimichangas Street",
        ///     "Email": "Daniel@gmail.com",
        //      "Phone": "122-352-0000"
        /// }
        /// </summary>
        /// 
        /// <param name="customer">The customer model object</param>
        /// 
        /// <returns>
        /// A new customer is added to the system's database.
        ///
        /// {"CustomerId":5,"Firstname":"Steve","Lastname":"Rogers","DOB":"1989-12-03T00:00:00","Address":"102 Borack Road",
        /// "Email":"Steve@gmail.com","Phone":"122-352-1223"}
        /// </returns>

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

        /// <summary>
        /// A function to delete a specific customer from the database using the id.
        /// It uses the Customer model class to delete the data.
        /// </summary>
        /// 
        /// <example>
        /// POST: curl -d "" https://localhost:44375/api/CustomersData/DeleteCustomer/3
        /// 
        /// Here, "" is the post data and it being empty means no post data was passed.
        /// </example>
        /// 
        /// <param name="id">The customer id</param>
        /// 
        /// <returns>
        /// The selected data entry with the id is deleted from the database.
        /// </returns>
        /// 

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