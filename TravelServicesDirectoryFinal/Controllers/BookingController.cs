﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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
            client.BaseAddress = new Uri("https://localhost:44375/api/BookingsData/");
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
            string url = "ListBookings";

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
            string url = "FindBooking/" + id;

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
        /// </summary>
        /// <returns>
        /// The view with the booking creation form
        /// </returns>
        /// 

        // GET: Booking/New
        public ActionResult New()
        {
            // returns the view with the form
            return View();
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
            string url = "addbooking";

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

        // GET: Booking/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Booking/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Booking/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Booking/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
