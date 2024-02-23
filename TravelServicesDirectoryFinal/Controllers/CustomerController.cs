using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Diagnostics;
using TravelServicesDirectoryFinal.Models;
using System.Web.Script.Serialization;
using TravelServicesDirectoryFinal.Models.ViewModels;

namespace TravelServicesDirectoryFinal.Controllers
{
    /*
     * Using the HttpClient class to define the client for the requests in a global manner for code refactoring and SocketException
     * error handling.
     * Instantiating client class for every request will lead to exhaustion of available sockets under heavy loads.
     * 
     * This http client class will be available for the entire application.
     */
    public class CustomerController : Controller
    {
        // declaring a static readonly field named client of type HttpClient
        // readnoly indicates that the field can only be assigned a value once, either at the time of declaration or in the class's
        // constructor
        private static readonly HttpClient client;
        // declaring the JS serializer for the http content and json payload
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CustomerController()
        {
            // instantiating a new client
            client = new HttpClient();
            // defining the base path for the URL
            client.BaseAddress = new Uri("https://localhost:44375/api/");
        }

        /// <summary>
        /// A function to list the total customers by communicating with tha CustomersDataController into a list view.
        /// 
        /// Similar to CURL request -> GET: curl https://localhost:44375/api/CustomersData/ListCustomers
        /// 
        /// A client is anything that is accessing information from the server. Since, the client is using the data access API and
        /// anticipating some response from the server. Here, the client exists on the server since we have the database setup locally.
        /// 
        /// This uses the URL to get the response and both the client and server are located in the same local device.
        /// </summary>
        /// 
        /// <returns>
        /// The List view with the customer object.
        /// </returns>
        /// 

        // GET: Customer/List
        public ActionResult List()
        {
            // defining a new http client object (Note: The client has been defined above for the entire application and can be reused.)
            // HttpClient client = new HttpClient();

            // defining the URL element to be added to the base address
            string url = "CustomersData/ListCustomers";

            // the response for the client
            HttpResponseMessage response = client.GetAsync(url).Result;

            // to check the status code of the response
            // Debug.WriteLine("The response code is: ");
            // Debug.WriteLine(response.StatusCode);

            /*
             * Here, we are parsing the list of customers as an IEnumerable of type Customer (model) from the response.
             * We are reading (as async) the content as type IEnumerable Customer.
             * 
             * The Model was later changed to CustomerDto because we can use the data transfer object from the model class that could
             * also have the attributes from the other related tables. Such columns can also be made accessible when using the DTO class.
             * 
             */
            IEnumerable<CustomerDto> customers = response.Content.ReadAsAsync<IEnumerable<CustomerDto>>().Result;
            // Debug.WriteLine("There are "+ customers.Count() +" customers in this system.");

            // returning the view with our customer object
            return View(customers);
        }

        /// <summary>
        /// A function to display one customer (details) by communicating with tha CustomersDataController into a display view.
        /// 
        /// Similar to CURL request -> GET: curl https://localhost:44375/api/CustomersData/FindCustomer/{id}
        /// 
        /// This uses the URL to get the response and both the client and server are located in the same local device.
        /// </summary>
        /// 
        /// <param name="id">the customer id</param>
        /// 
        /// <returns>
        /// The Details page for the selected customer.
        /// </returns>
        /// 

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            DetailsCustomer ViewModel = new DetailsCustomer();

            //objective: communicate with our customer data api to retrieve one customer
            //curl https://localhost:44324/api/customersdata/findcustomer/{id}

            // defining the URL element to be added to the base address
            string url = "CustomersData/FindCustomer/" + id;
            // sending the content data result to the url as the response
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine("The response code is ");
            Debug.WriteLine(response.StatusCode);

            /*
             * Here, we are parsing the selected customer from the response.
             * We are reading (as async) the content as type IEnumerable Customer.
             * 
             * The Model was later changed to CustomerDto because we can use the data transfer object from the model class that could
             * also have the attributes from the other related tables. Such columns can also be made accessible when using the DTO class.
             * 
             */
            CustomerDto SelectedCustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
            Debug.WriteLine("Customer received : ");
            Debug.WriteLine(SelectedCustomer.Firstname);

            // defining the viewModel class property
            ViewModel.SelectedCustomer = SelectedCustomer;

            //show associated bookings with this customer
            url = "BookingsData/ListBookingsForCustomers/" + id;
            response = client.GetAsync(url).Result;
            IEnumerable<BookingDto> RelatedBookings = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;

            // defining the viewModel class property
            ViewModel.RelatedBookings = RelatedBookings;

            // showing the unassociated bookings for this customer
            // url = "BookingsData/ListBookingsNotByCustomers/" + id;
            // response = client.GetAsync(url).Result;
            // IEnumerable<BookingDto> AvailableBookings = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;

            // defining the viewModel class property
            // ViewModel.AvailableBookings = AvailableBookings;

            // returning the details view with the selected customer
            return View(ViewModel);
        }

        /// <summary>
        /// Function to return the error page.
        /// 
        /// It is used in the add function in case of any error with the request or response.
        /// </summary>
        /// <returns>
        /// Returns the error page.
        /// </returns>
        public ActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// A function to display the customer creation form.
        /// 
        /// This function only does the task of displaying the form.
        /// </summary>
        /// <returns>
        /// The view with the customer creation form
        /// </returns>
        /// 

        // GET: Customer/New
        public ActionResult New()
        {
            // returns the view with the form
            return View();
        }

        /// <summary>
        /// A function that collects and saves the data of the new customer from the form.
        /// </summary>
        /// <param name="customer">The customer object to be added</param>
        /// <returns>
        /// Creates a new customer and redirects to the customers list view.
        /// </returns>
        /// 

        // POST: Customer/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            // Debug.WriteLine("the JSON payload is :");
            // Debug.WriteLine(customer.Firstname);

            // objective: To add a new customer into the system using the API

            /*
             * The curl request would be something like;
             * curl -H "Content-Type:application/json" -d @customers.json https://localhost:44375/api/CustomersData/ 
             * (similar to the CURL request used for the API)
             * 
             * where the url is the base address for the http client and in case if the json data/file can not be accessed, use the 
             * relative path for the file.
             * */
            string url = "CustomersData/addcustomer";

            /*
             * We are using JS serializer to convert the Customer C# object into a JSON string.
             * 
             * The line below means the json payload for our js serializer is the customer object.
             */
            string jsonpayload = jss.Serialize(customer);
            // the payload data was displayed in the output
            //Debug.WriteLine(jsonpayload);

            // sending our JSON payload to the url with the use of http client
            HttpContent content = new StringContent(jsonpayload);
            // configuring the content type header for the client to pass JSON data
            content.Headers.ContentType.MediaType = "application/json";

            // sending the content data result to the url as the response
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // redirect to the customers list page
                return RedirectToAction("List");
            }
            else
            {
                // redirect to the error page
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// A function that grabs the details of the selected customer and renders the edit form view
        /// </summary>
        /// <param name="id">The customer to be edited</param>
        /// <returns>
        /// Returns the edit customer form page with the data. 
        /// </returns>
        /// 

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the customer information

            //objective: communicate with our customer data api to retrieve one customer
            //curl https://localhost:44324/api/customersdata/findcustomer/{id}

            string url = "CustomersData/findcustomer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            CustomerDto selectedCustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
            //Debug.WriteLine("Customer received : ");
            //Debug.WriteLine(selectedCustomer.Firstname);

            return View(selectedCustomer);
        }

        /// <summary>
        /// A function to update the details of the selected customer
        /// </summary>
        /// <param name="id">The customer id</param>
        /// <param name="customer">The customer object</param>
        /// <returns>
        /// The list view page after updating the details
        /// </returns>
        /// 

        // POST: Customer/Update/5
        [HttpPost]
        public ActionResult Update(int id, Customer customer)
        {
            try
            {
                // Debug.WriteLine("The new customer info is:");
                // Debug.WriteLine(customer.Firstname);
                // Debug.WriteLine(customer.Lastname);
                // Debug.WriteLine(customer.DOB);

                // Serialize into JSON
                // Send the request to the API

                string url = "CustomersData/UpdateCustomer/" + id;


                string jsonpayload = jss.Serialize(customer);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/CustomersData/UpdateCustomer/{id}
                //Header : Content-Type: application/json
                HttpResponseMessage response = client.PostAsync(url, content).Result;


                return RedirectToAction("Details/" + id);
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Function to show the delete confirm dialog box.
        /// </summary>
        /// <param name="id">The customer id to be deleted</param>
        /// <returns>
        /// The delete confirm dialog box.
        /// </returns>
        /// 

        // GET: Customer/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CustomersData/FindCustomer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CustomerDto selectedCustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
            return View(selectedCustomer);
        }

        /// <summary>
        /// Function to delete the selected customer entry.
        /// </summary>
        /// <param name="id">The customer id</param>
        /// <returns>
        /// The list of customers if successful else, the error view page.
        /// </returns>
        /// 

        // POST: Customer/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "CustomersData/DeleteCustomer/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
    }
}
