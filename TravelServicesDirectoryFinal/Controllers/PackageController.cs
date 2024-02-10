using System;
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
    public class PackageController : Controller
    {
        // declaring a static readonly field named client of type HttpClient
        // readnoly indicates that the field can only be assigned a value once, either at the time of declaration or in the class's
        // constructor
        private static readonly HttpClient client;
        // declaring the JS serializer for the http content and json payload
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PackageController()
        {
            // instantiating a new client
            client = new HttpClient();
            // defining the base path for the URL
            client.BaseAddress = new Uri("https://localhost:44375/api/PackagesData/");
        }

        /// <summary>
        /// A function to list the total packages by communicating with tha PackagesDataController into a list view.
        /// 
        /// Similar to CURL request -> GET: curl https://localhost:44375/api/PackagesData/ListPackages
        /// 
        /// A client is anything that is accessing information from the server. Since, the client is using the data access API and
        /// anticipating some response from the server. Here, the client exists on the server since we have the database setup locally.
        /// 
        /// This uses the URL to get the response and both the client and server are located in the same local device.
        /// </summary>
        /// 
        /// <returns>
        /// The List view with the package object.
        /// </returns>
        /// 

        // GET: Customer/List
        public ActionResult List()
        {
            // defining a new http client object (Note: The client has been defined above for the entire application and can be reused.)
            // HttpClient client = new HttpClient();

            // defining the URL element to be added to the base address
            string url = "ListPackages";

            // the response for the client
            HttpResponseMessage response = client.GetAsync(url).Result;

            // to check the status code of the response
            // Debug.WriteLine("The response code is: ");
            // Debug.WriteLine(response.StatusCode);

            /*
             * Here, we are parsing the list of packages as an IEnumerable of type Package (model) from the response.
             * We are reading (as async) the content as type IEnumerable Package.
             * 
             * The Model was later changed to PackageDto because we can use the data transfer object from the model class that could
             * also have the attributes from the other related tables. Such columns can also be made accessible when using the DTO class.
             * 
             */
            IEnumerable<PackageDto> packages = response.Content.ReadAsAsync<IEnumerable<PackageDto>>().Result;
            // Debug.WriteLine("There are "+ packages.Count() +" customers in this system.");

            // returning the view with our package object
            return View(packages);
        }

        /// <summary>
        /// A function to display one package (details) by communicating with tha PackagesDataController into a display view.
        /// 
        /// Similar to CURL request -> GET: curl https://localhost:44375/api/PackagesData/FindPackage/{id}
        /// 
        /// This uses the URL to get the response and both the client and server are located in the same local device.
        /// </summary>
        /// 
        /// <param name="id">the package id</param>
        /// 
        /// <returns>
        /// The Details page for the selected package.
        /// </returns>
        /// 

        // GET: Package/Details/1
        public ActionResult Details(int id)
        {
            // defining a new http client object
            // HttpClient client = new HttpClient();

            // defining the URL element to be added to the base address
            string url = "FindPackage/" + id;

            // the response for the client
            HttpResponseMessage response = client.GetAsync(url).Result;

            // to check the status code of the response
            // Debug.WriteLine("The response code is: ");
            // Debug.WriteLine(response.StatusCode);

            /*
             * Here, we are parsing the selected package from the response.
             * We are reading (as async) the content as type IEnumerable Customer.
             * 
             * The Model was later changed to PackageDto because we can use the data transfer object from the model class that could
             * also have the attributes from the other related tables. Such columns can also be made accessible when using the DTO class.
             * 
             */
            PackageDto selectedPackage = response.Content.ReadAsAsync<PackageDto>().Result;
            // Debug.WriteLine("The received package was: ");
            // Debug.WriteLine(selectedPackage.Name);

            // returning the details view with the selected package
            return View(selectedPackage);
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
        /// A function to display the package creation form.
        /// 
        /// This function only does the task of displaying the form.
        /// </summary>
        /// <returns>
        /// The view with the package creation form
        /// </returns>
        /// 

        // GET: Package/New
        public ActionResult New()
        {
            // returns the view with the form
            return View();
        }

        /// <summary>
        /// A function that collects and saves the data of the new package from the form.
        /// </summary>
        /// <param name="package">The package object to be added</param>
        /// <returns>
        /// Creates a new package and redirects to the packages list view.
        /// </returns>
        /// 

        // POST: Package/Create
        [HttpPost]
        public ActionResult Create(Package package)
        {
            // Debug.WriteLine("the JSON payload is :");
            // Debug.WriteLine(booking.Name);

            // objective: To add a new package into the system using the API

            /*
             * The curl request would be something like;
             * curl -H "Content-Type:application/json" -d @packages.json https://localhost:44375/api/PackagesData/ 
             * (similar to the CURL request used for the API)
             * 
             * where the url is the base address for the http client and in case if the json data/file can not be accessed, use the 
             * relative path for the file.
             * */
            string url = "addpackage";

            /*
             * We are using JS serializer to convert the Package C# object into a JSON string.
             * 
             * The line below means the json payload for our js serializer is the booking object.
             */
            string jsonpayload = jss.Serialize(package);
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
                // redirect to the packages list page
                return RedirectToAction("List");
            }
            else
            {
                // redirect to the error page
                return RedirectToAction("Error");
            }
        }

        /// <summary>
        /// A function that grabs the details of the selected package and renders the edit form view
        /// </summary>
        /// <param name="id">The package to be edited</param>
        /// <returns>
        /// Returns the edit package form page with the data. 
        /// </returns>
        /// 

        // GET: Package/Edit/5
        public ActionResult Edit(int id)
        {
            //grab the package information

            //objective: communicate with our package data api to retrieve one package
            //curl https://localhost:44324/api/packagesdata/findpackage/{id}

            string url = "findpackage/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            //Debug.WriteLine("The response code is ");
            //Debug.WriteLine(response.StatusCode);

            PackageDto selectedPackage = response.Content.ReadAsAsync<PackageDto>().Result;
            //Debug.WriteLine("Package received : ");
            //Debug.WriteLine(selectedPackage.Name);

            return View(selectedPackage);
        }

        // POST: Package/Edit/5
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

        // GET: Package/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Package/Delete/5
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
