using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TravelServicesDirectoryFinal.Migrations;
using TravelServicesDirectoryFinal.Models;

namespace TravelServicesDirectoryFinal.Controllers
{
    /*
     * Using the HttpClient class to define the client for the requests in a global manner for code refactoring and SocketException
     * error handling.
     * Instantiating client class for every request will lead to exhaustion of available sockets under heavy loads.
     * 
     * This http client class will be available for the entire application.
     */
    public class BookingController : Controller
    {
        // declaring a static readonly field named client of type HttpClient
        // readnoly indicates that the field can only be assigned a value once, either at the time of declaration or in the class's
        // constructor
        private static readonly HttpClient client;
        // declaring the JS serializer for the http content and json payload
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static BookingController()
        {
            // instantiating a new client
            client = new HttpClient();
            // defining the base path for the URL
            // removed the /BookingsData from the end to assist the url for customer model queries
            client.BaseAddress = new Uri("https://localhost:44375/api/");
        }

        /// <summary>
        /// A function to list the total bookings by communicating with tha BookingsDataController into a list view.
        /// 
        /// Similar to CURL request -> GET: curl https://localhost:44375/api/BookingsData/ListBookings
        /// 
        /// A client is anything that is accessing information from the server. Since, the client is using the data access API and
        /// anticipating some response from the server. Here, the client exists on the server since we have the database setup locally.
        /// 
        /// This uses the URL to get the response and both the client and server are located in the same local device.
        /// </summary>
        /// 
        /// <returns>
        /// The List view with the booking object.
        /// </returns>
        ///

        // GET: Booking/List

        public ActionResult List()
        {
            // defining a new http client object (Note: The client has been defined above for the entire application and can be reused.)
            // HttpClient client = new HttpClient();

            // defining the URL element to be added to the base address
            string url = "BookingsData/ListBookings";

            // the response for the client
            HttpResponseMessage response = client.GetAsync(url).Result;

            // to check the status code of the response
            // Debug.WriteLine("The response code is: ");
            // Debug.WriteLine(response.StatusCode);

            /*
             * Here, we are parsing the list of bookings as an IEnumerable of type Booking (model) from the response.
             * We are reading (as async) the content as type IEnumerable Booking.
             * 
             * The Model was later changed to BookingDto because we can use the data transfer object from the model class that could
             * also have the attributes from the other related tables. Such columns can also be made accessible when using the DTO class.
             * 
             */
            IEnumerable<BookingDto> bookings = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;
            // Debug.WriteLine("There are "+ bookings.Count() +" bookings in this system.");

            // returning the view with our booking object
            return View(bookings);
        }

        /// <summary>
        /// A function to display one booking (details) by communicating with tha BookingsDataController into a display view.
        /// 
        /// Similar to CURL request -> GET: curl https://localhost:44375/api/BookingsData/FindBooking/{id}
        /// 
        /// This uses the URL to get the response and both the client and server are located in the same local device.
        /// </summary>
        /// 
        /// <param name="id">the booking id</param>
        /// 
        /// <returns>
        /// The Details page for the selected booking.
        /// </returns>
        /// 

        // GET: Booking/Details/4
        public ActionResult Details(int id)
        {
            // defining a new http client object
            // HttpClient client = new HttpClient();

            // defining the URL element to be added to the base address
            string url = "BookingsData/FindBooking/" + id;

            // the response for the client
            HttpResponseMessage response = client.GetAsync(url).Result;

            // to check the status code of the response
            // Debug.WriteLine("The response code is: ");
            // Debug.WriteLine(response.StatusCode);

            /*
             * Here, we are parsing the selected booking from the response.
             * We are reading (as async) the content as type IEnumerable Booking.
             * 
             * The Model was later changed to BookingDto because we can use the data transfer object from the model class that could
             * also have the attributes from the other related tables. Such columns can also be made accessible when using the DTO class.
             * 
             */
            BookingDto selectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
            // Debug.WriteLine("The received booking was: ");
            // Debug.WriteLine(selectedBooking.Name);

            // returning the details view with the selected booking
            return View(selectedBooking);
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
        /// A function to display the booking creation form.
        /// 
        /// This function only does the task of displaying the form.
        /// 
        /// Here, to list the available customers in the database while creating a new booking, we are using the CustomersDTO to fetch the data
        /// as an HTTP request.
        /// 
        /// </summary>
        /// <returns>
        /// The view with the booking creation form
        /// </returns>
        /// 

        // GET: Booking/New
        public ActionResult New()
        {
            //information about all customers in the system.
            //GET api/customersdata/listcustomers

            string url = "customersdata/listcustomers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<CustomerDto> CustomersOptions = response.Content.ReadAsAsync<IEnumerable<CustomerDto>>().Result;

            return View(CustomersOptions);
        }

        /// <summary>
        /// A function that collects and saves the data of the new booking from the form.
        /// 
        /// For this bridging table, there should already be an existing Customer id and Package id (FK relationship) before the new
        /// booking creation form can be used which is then used by this Create function.
        /// </summary>
        /// <param name="booking">The booking object to be added</param>
        /// <returns>
        /// Creates a new booking and redirects to the bookings list view.
        /// </returns>
        /// 

        // POST: Booking/Create
        [HttpPost]
        public ActionResult Create(Booking booking)
        {
            // Debug.WriteLine("the JSON payload is :");
            // Debug.WriteLine(booking.status);

            // objective: To add a new booking into the system using the API

            /*
             * The curl request would be something like;
             * curl -H "Content-Type:application/json" -d @bookings.json https://localhost:44375/api/BookingsData/ 
             * (similar to the CURL request used for the API)
             * 
             * where the url is the base address for the http client and in case if the json data/file can not be accessed, use the 
             * relative path for the file.
             * */
            string url = "BookingsData/addbooking";

            /*
             * We are using JS serializer to convert the Booking C# object into a JSON string.
             * 
             * The line below means the json payload for our js serializer is the booking object.
             */
            string jsonpayload = jss.Serialize(booking);
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
                // redirect to the bookings list page
                return RedirectToAction("List");
            }
            else
            {
                // redirect to the error page
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// A function that grabs the details of the selected booking and renders the edit form view
        /// </summary>
        /// <param name="id">The booking to be edited</param>
        /// <returns>
        /// Returns the edit booking form page with the data. 
        /// </returns>
        /// 

        // GET: Booking/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the booking information

            //objective: communicate with our booking data api to retrieve one booking
            //curl https://localhost:44324/api/bookingsdata/findbooking/{id}

            string url = "BookingsData/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            Booking selectedBooking = response.Content.ReadAsAsync<Booking>().Result;
            //Debug.WriteLine("Booking received : ");
            //Debug.WriteLine(selectedBooking.Status);

            return View(selectedBooking);
        }

        /// <summary>
        /// A function to update the details of the selected booking
        /// </summary>
        /// <param name="id">The booking id</param>
        /// <param name="booking">The booking object</param>
        /// <returns>
        /// The list view page after updating the details
        /// </returns>
        /// 

        // POST: Booking/Update/5
        [HttpPost]
        public ActionResult Update(int id, Booking booking)
        {
            try
            {
                // Debug.WriteLine("The new package info is:");
                // Debug.WriteLine(booking.Status);
                // Debug.WriteLine(booking.BookingDate);

                // Serialize into JSON
                // Send the request to the API

                string url = "BookingsData/UpdateBooking/" + id;


                string jsonpayload = jss.Serialize(booking);
                Debug.WriteLine(jsonpayload);

                HttpContent content = new StringContent(jsonpayload);
                content.Headers.ContentType.MediaType = "application/json";

                //POST: api/BookingsData/UpdateBooking/{id}
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
        /// <param name="id">The booking id to be deleted</param>
        /// <returns>
        /// The delete confirm dialog box.
        /// </returns>
        /// 

        // GET: Booking/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "BookingsData/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            BookingDto selectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
            return View(selectedBooking);
        }

        /// <summary>
        /// Function to delete the selected booking entry.
        /// </summary>
        /// <param name="id">The booking id</param>
        /// <returns>
        /// The list of bookings if successful else, the error view page.
        /// </returns>

        // POST: Booking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "BookingsData/deletebooking/" + id;
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
