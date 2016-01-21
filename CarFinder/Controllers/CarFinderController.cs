using Bing;
using CarFinder.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CarFinder.Controllers
{
    /// <summary>
    /// All stored procedures for the CarFinder app. Provides content for drop-down menus and information to fulfill user-influenced queries.
    /// </summary>
    public class CarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Provides an object to collect parameters of user selection on front end. For use in populating drop-down menus and the results grid.
        /// </summary>
        public class Selected
        {
            public string make { get; set; }
            public string model { get; set; }
            public string year { get; set; }
            public string trim { get; set; }
        }

        /// <summary>
        /// Provides an object to capture the Car Id for use in the GetCarDetails function, returning recall information and an image.
        /// </summary>
        public class IdParam
        {
            public int id { get; set; }
        }

        /// <summary>
        /// List of unique years in table Cars for use in the dropdown menu, sorted by make
        /// </summary>
        /// <returns>All unique years in the column model_year</returns>
       [HttpPost]
        public IHttpActionResult GetUniqueYearsByMakeModel(Selected selected)
        {
            var _make = new SqlParameter("@make", selected.make ?? "");
            var _model_name = new SqlParameter("@model_name", selected.model ?? "");

            var returnValue = db.Database.SqlQuery<string>(
                "EXEC GetUniqueYearsByMakeModel @make, @model_name", _make, _model_name).ToList();
            return Ok(returnValue);
        }

        /// <summary>
        /// List of unique makes from table Cars for use in dropdown menu
        /// </summary>
        /// <param name="model_year">For defining contents of list for Cars:make</param>
        /// <returns>All unique makes for user-selected year (column make, column model_year)</returns>
        [HttpPost]
        public IHttpActionResult GetUniqueMakes()
        {
            var returnValue = db.Database.SqlQuery<string>("EXEC GetUniqueMakes").ToList();
            //with no parameters, take out @body_style through the id).;
            // mutiple parameters: create parameter ahead of the EXEC functions, then simply list @id, @body_style, var, var
            return Ok(returnValue);
        }

        /// <summary>
        /// All unique models from table Cars for use in the dropdown menu, sorted by year and make
        /// </summary>
        /// <param name="model_year">for defining list of Cars:make</param>
        /// <param name="make">for further defining list of Cars:model_name</param>
        /// <returns>All unique models for selected year and make</returns>
        [HttpPost]
        public IHttpActionResult GetUniqueModelsByMake(Selected selected)
        {
            var _make = new SqlParameter("@make", selected.make ?? "");

            var returnValue = db.Database.SqlQuery<string>(
                "EXEC GetUniqueModelsByMake @make", _make).ToList();

            return Ok(returnValue);
        }

        /// <summary>
        /// All unique trims from table Cars for use in the dropdown menu, sorted by make, model, and year
        /// </summary>
        /// <param name="model_year">for further defining list of Cars:make</param>
        /// <param name="make">for further defining list of Cars:make, model</param>
        /// <param name="model_name">for furher defining list of Cars:make, model, trim</param>
        /// <returns>all unique trims for user-selected year, make, model</returns>
        [HttpPost]
        public IHttpActionResult GetUniqueTrimsByMakeModelYear(Selected selected)
        {
            var _model_year = new SqlParameter("@model_year", selected.year ?? "");
            var _make = new SqlParameter("@make", selected.make ?? "");
            var _model_name = new SqlParameter("@model_name", selected.model ?? "");

            var returnValue = db.Database.SqlQuery<string>(
                "EXEC GetUniqueTrimsByMakeModelYear @make, @model_name,  @model_year", _make, _model_name, _model_year).ToList();

            return Ok(returnValue);
        }

        /// <summary>
        /// Displays all cars from table Cars following the given user-selected parameters, including all columns provided in the database.
        /// </summary>
        /// <param name="model_year">for refining list of Cars:make</param>
        /// <param name="make">for refining list of Cars:make, model</param>
        /// <param name="model_name">for refining list of Cars:make, model, trim</param>
        /// <param name="model_trim">for refining list of Cars:make, model, trim, etc</param>
        /// <returns>Returns all cars fitting the user-selected parameters</returns>
        [HttpPost]
        public IHttpActionResult GetCars(Selected selected)
        {
            var _make = new SqlParameter("@make", selected.make ?? "");
            var _model_name = new SqlParameter("@model_name", selected.model ?? "");
            var _model_year = new SqlParameter("@model_year", selected.year ?? "");
            var _model_trim = new SqlParameter("@model_trim", selected.trim ?? "");

            var returnValue = db.Database.SqlQuery<Car>(
                "EXEC GetCars @make, @model_name, @model_year, @model_trim", _make, _model_name, _model_year, _model_trim).ToList();

            return Ok(returnValue);
        }


        /// <summary>
        /// Uses the Id from object Car as selected by the user. Accesses recall information from the National Highway and Traffic Safety Administration API. Acquires an image through the Bing search API.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> GetCarDetails(IdParam Id)
        {
            // ---- get Recall Information -----

            HttpResponseMessage response;
            string content = "";
            var Car = db.Cars.Find(Id.id);
            dynamic Recalls = "";
            var Image = "";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://www.nhtsa.gov/");
                try
                {
                    response = await client.GetAsync("webapi/api/Recalls/vehicle/modelyear/" + Car.model_year +
                                                                                    "/make/" + Car.make +
                                                                                    "/model/" + Car.model_name + "?format=json");
                    content = await response.Content.ReadAsStringAsync();
                }
                catch (Exception e)
                {
                    return InternalServerError(e);
                }
            }
            Recalls = JsonConvert.DeserializeObject(content);


            // ---------  Get Image --------
           
            //    var image = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));

            //    image.Credentials = new NetworkCredential("accountKey", "ih3uuQpD5VI9IrIcovu/1dEIjaJXkB6eFMgSg9CCxGY");
            //var marketData = image.Composite(
            //     "image",
            //     Car.model_year + " " + Car.make + " " + Car.model_name + " " + Car.model_trim,
            //     null,
            //     null,
            //     null,
            //     null,
            //     null,
            //     null,
            //     null,
            //     null,
            //     null,
            //     null,
            //     null,
            //     null,
            //     null
            //     ).Execute();

           // Image = marketData?.First()?.Image?.First()?.MediaUrl;

            return Ok(new { car = Car, recalls = Recalls });
        }
    }
}
