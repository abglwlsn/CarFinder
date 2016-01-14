using CarFinder.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CarFinder.Controllers
{
    public class CarController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public IHttpActionResult GetUniqueYears()
        {
            var returnValue = db.Database.SqlQuery<Car>(
                "EXEC GetUniqueYears").ToList();
            return Ok(returnValue);
        }

        public IHttpActionResult GetUniqueMakesByYear(string model_year)
        {
            var returnValue = db.Database.SqlQuery<Car>(
            "EXEC GetUniqueMakesByYear @model_year",
            new SqlParameter("@model_year", model_year)).ToList(); 
            //with no parameters, take out @body_style through the id).
            // mutiple parameters: create parameter ahead of the EXEC functions, then simply list @id, @body_style, var, var
            return Ok(returnValue);
        }

        public IHttpActionResult GetUniqueModelsByYearMake(string model_year, string make)
        {
            var _model_year = new SqlParameter("@model_year", model_year);
            var _make = new SqlParameter("@make", make);

            var returnValue = db.Database.SqlQuery<Car>(
                "EXEC GetUniqueModelsByYearMake @model_year, @make", _model_year, _make).ToList();

            return Ok(returnValue);
        }

        public IHttpActionResult GetUniqueTrimsByYearMakeModel(string model_year, string make, string model_name)
        {
            var _model_year = new SqlParameter("@model_year", model_year);
            var _make = new SqlParameter("@make", make);
            var _model_name = new SqlParameter("@model_name", model_name);

            var returnValue = db.Database.SqlQuery<Car>(
                "EXEC GetUniqueTrimsByYearMakeModel @model_year, @make, @model_name", _model_year, _make, _model_name).ToList();

            return Ok(returnValue);
        }

        public IHttpActionResult GetCars(string model_year, string make, string model_name, string model_trim)
        {
            var _model_year = new SqlParameter("@model_year", model_year);
            var _make = new SqlParameter("@make", make);
            var _model_name = new SqlParameter("@model_name", model_name);
            var _model_trim = new SqlParameter("@model_trim", model_trim);

            var returnValue = db.Database.SqlQuery<Car>(
                "EXEC GetCars @model_year, @make, @model_name, @model_trim", _model_year, _make, _model_name, _model_trim).ToList();

            return Ok(returnValue);
        }
    }
}
