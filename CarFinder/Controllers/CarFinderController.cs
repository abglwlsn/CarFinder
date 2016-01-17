using Bing;
using CarFinder.Models;
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
    /// All stored procedures for CarFinder app: creating menu dropdowns and sorting of results by user-entered parameters
    /// </summary>
    public class CarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// List of unique years in table Cars for use in the dropdown menu
        /// </summary>
        /// <returns>All unique years in the column model_year</returns>
        public IHttpActionResult GetUniqueYears()
        {
            var returnValue = db.Database.SqlQuery<string>(
                "EXEC GetUniqueYears").ToList();
            return Ok(returnValue);
        }

        /// <summary>
        /// List of unique makes, sorted by year, from table Cars for use in dropdown menu
        /// </summary>
        /// <param name="model_year">For defining contents of list for Cars:make</param>
        /// <returns>All unique makes for user-selected year (column make, column model_year)</returns>
        public IHttpActionResult GetUniqueMakesByYear(string model_year)
        {
            var returnValue = db.Database.SqlQuery<string>(
            "EXEC GetUniqueMakesByYear @model_year",
            new SqlParameter("@model_year", model_year)).ToList();
            //with no parameters, take out @body_style through the id).
            // mutiple parameters: create parameter ahead of the EXEC functions, then simply list @id, @body_style, var, var
            return Ok(returnValue);
        }

        /// <summary>
        /// All unique models, sorted by year and make, for use in the dropdown menu
        /// </summary>
        /// <param name="model_year">for defining list of Cars:make</param>
        /// <param name="make">for further defining list of Cars:model_name</param>
        /// <returns>All unique models for selected year and make</returns>
        public IHttpActionResult GetUniqueModelsByYearMake(string model_year, string make)
        {
            var _model_year = new SqlParameter("@model_year", model_year);
            var _make = new SqlParameter("@make", make);

            var returnValue = db.Database.SqlQuery<string>(
                "EXEC GetUniqueModelsByYearMake @model_year, @make", _model_year, _make).ToList();

            return Ok(returnValue);
        }

        /// <summary>
        /// All unique trims from table Cars, sorted by year, make, and model, for use in the dropdown menu
        /// </summary>
        /// <param name="model_year">for further defining list of Cars:make</param>
        /// <param name="make">for further defining list of Cars:make, model</param>
        /// <param name="model_name">for furher defining list of Cars:make, model, trim</param>
        /// <returns>all unique trims for user-selected year, make, model</returns>
        public IHttpActionResult GetUniqueTrimsByYearMakeModel(string model_year, string make, string model_name)
        {
            var _model_year = new SqlParameter("@model_year", model_year);
            var _make = new SqlParameter("@make", make);
            var _model_name = new SqlParameter("@model_name", model_name);

            var returnValue = db.Database.SqlQuery<Car>(
                "EXEC GetUniqueTrimsByYearMakeModel @model_year, @make, @model_name", _model_year, _make, _model_name).ToList();

            return Ok(returnValue);
        }

        /// <summary>
        /// Displays all cars following the given user-selected parameters
        /// </summary>
        /// <param name="model_year">for refining list of Cars:make</param>
        /// <param name="make">for refining list of Cars:make, model</param>
        /// <param name="model_name">for refining list of Cars:make, model, trim</param>
        /// <param name="model_trim">for refining list of Cars:make, model, trim, etc</param>
        /// <param name="transmission_type">optional to select transmission type</param>
        /// <param name="drive_type">optional to select drive type</param>
        /// <returns>Returns all cars fitting the user-selected parameters</returns>
        public IHttpActionResult GetCars(string model_year, string make, string model_name, string model_trim, string transmission_type, string drive_type)
        {
            var _model_year = new SqlParameter("@model_year", model_year??"");
            var _make = new SqlParameter("@make", make??"");
            var _model_name = new SqlParameter("@model_name", model_name??"");
            var _model_trim = new SqlParameter("@model_trim", model_trim??"");
            var _transmission_type = new SqlParameter("@transmission_type", transmission_type??"");
            var _drive_type = new SqlParameter("@drive_type", drive_type??"");


            var returnValue = db.Database.SqlQuery<Car>(
                "EXEC GetCars @model_year, @make, @model_name, @model_trim, @transmission_type, @drive_type", _model_year, _make, _model_name, _model_trim, _transmission_type, _drive_type).ToList();

            return Ok(returnValue);
        }


        /// <summary>
        /// Displays all cars following the given user-selected parameters, with results sorted by horsepower (column engine_power_ps)
        /// </summary>
        /// <param name="model_year">for refining list of Cars:make</param>
        /// <param name="make">for refining list of Cars:make, model</param>
        /// <param name="model_name">for refining list of Cars:make, model, trim</param>
        /// <param name="model_trim">for refining list of Cars:make, model, trim, etc</param>
        /// <param name="transmission_type">optional to select transmission type</param>
        /// <param name="drive_type">optional to select drive type</param>
        /// <returns>Returns all cars fitting the user-selected parameters, sorted by horsepower.</returns>
        public IHttpActionResult GetCarsSortByHorsepower(string model_year, string make, string model_name, string model_trim, string transmission_type, string drive_type)
        {
            var _model_year = new SqlParameter("@model_year", model_year);
            var _make = new SqlParameter("@make", make);
            var _model_name = new SqlParameter("@model_name", model_name);
            var _model_trim = new SqlParameter("@model_trim", model_trim);
            var _transmission_type = new SqlParameter("@tranmission_type", transmission_type);
            var _drive_type = new SqlParameter("@drive_type", drive_type);

            var returnValue = db.Database.SqlQuery<Car>(
                "EXEC SortByHorsepower @model_year, @make, @model_name, @model_trim, @transmission_type, @drive_type", _model_year, _make, _model_name, _model_trim, _transmission_type, _drive_type).ToList();

            return Ok(returnValue);
        }

        /// <summary>
        /// Displays all cars fitting user-selected parameters, sorted by number of seats.
        /// </summary>
        /// <param name="model_year">for refining list of Cars:make</param>
        /// <param name="make">for refining list of Cars:make, model</param>
        /// <param name="model_name">for refining list of Cars:make, model, trim</param>
        /// <param name="model_trim">for refining list of Cars:make, model, trim, etc</param>
        /// <param name="transmission_type">optional to select transmission type</param>
        /// <param name="drive_type">optional to select drive type</param>
        /// <returns>Returns all cars fitting user-selected parameters, sorted by number of seats</returns>
        public IHttpActionResult GetCarsSortBySeats(string model_year, string make, string model_name, string model_trim, string transmission_type, string drive_type)
        {
            var _model_year = new SqlParameter("@model_year", model_year);
            var _make = new SqlParameter("@make", make);
            var _model_name = new SqlParameter("@model_name", model_name);
            var _model_trim = new SqlParameter("@model_trim", model_trim);
            var _transmission_type = new SqlParameter("@tranmission_type", transmission_type);
            var _drive_type = new SqlParameter("@drive_type", drive_type);

            var returnValue = db.Database.SqlQuery<Car>(
                "EXEC SortBySeats @model_year, @make, @model_name, @model_trim, @transmission_type, @drive_type", _model_year, _make, _model_name, _model_trim, _transmission_type, _drive_type).ToList();

            return Ok(returnValue);
        }

        /// <summary>
        /// Displays all cars fitting user-selected parameters, sorted by number of doors.
        /// </summary>
        /// <param name="model_year">for refining list of Cars:make</param>
        /// <param name="make">for refining list of Cars:make, model</param>
        /// <param name="model_name">for refining list of Cars:make, model, trim</param>
        /// <param name="model_trim">for refining list of Cars:make, model, trim, etc</param>
        /// <param name="transmission_type">optional to select transmission type</param>
        /// <param name="drive_type">optional to select drive type</param>
        /// <returns>Returns all cars fitting user-selected parameters, sorted by number of doors.</returns>
        public IHttpActionResult GetCarsSortByDoors(string model_year, string make, string model_name, string model_trim, string transmission_type, string drive_type)
        {
            var _model_year = new SqlParameter("@model_year", model_year);
            var _make = new SqlParameter("@make", make);
            var _model_name = new SqlParameter("@model_name", model_name);
            var _model_trim = new SqlParameter("@model_trim", model_trim);
            var _transmission_type = new SqlParameter("@tranmission_type", transmission_type);
            var _drive_type = new SqlParameter("@drive_type", drive_type);

            var returnValue = db.Database.SqlQuery<Car>(
                "EXEC SortByDoors @model_year, @make, @model_name, @model_trim, @transmission_type, @drive_type", _model_year, _make, _model_name, _model_trim, _transmission_type, _drive_type).ToList();

            return Ok(returnValue);
        }

        /// <summary>
        /// Acquires recall information from the NHTSA website for the selected car
        /// </summary>
        /// <param name="Id">The Id for the selected car (returned by GetCars API)</param>
        /// <returns>JSON file with all recall information for car</returns>
        public async Task<IHttpActionResult> GetRecalls(int Id)
        {
            HttpResponseMessage response;
            string content = "";
            var Car = db.Cars.Find(Id);
            var Recalls = "";
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
            Recalls = content;
            return Ok(new { car = Car, recalls = Recalls });
        }

        /// <summary>
        /// Get images for car from Bing API
        /// </summary>
        /// <param name="Id">The Id for the selected car (returned from GetCars API)</param>
        /// <returns>Images for the selected car, supplied by Bing</returns>
        public async Task<IHttpActionResult> GetImages(int Id)
        {
            var Image = "";
            var Car = db.Cars.Find(Id);

            var image = new BingSearchContainer(new Uri("https://api.datamarket.azure.com/Bing/search/"));

            image.Credentials = new NetworkCredential("accountKey", "5u/0CzVmYrTKDOjlxPePfPkh/G8llMIfVJ7QC/oNEvQ");
            var marketData = image.Composite(
                "image",
                Car.model_year + " " + Car.make + " " + Car.model_name + " " + Car.model_trim,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
                ).Execute();

            Image = marketData.First().Image.First().MediaUrl;
            return Ok(new { car = Car, image = Image });
        }
    }
}
