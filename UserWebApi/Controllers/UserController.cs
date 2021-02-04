using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using UserWebApi.Models;


namespace UserWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select userId,userName,countryName,phoneNumber 
                            from [User]
                            join Country 
                            On [User].CountryId = Country.countryId";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            SqlDataReader reader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    reader = sqlCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(User user)
        {
            string countryQuery = @"select countryId from Country where countryName ='"+user.CountryName+@"' ";
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            DataTable table1 = new DataTable();
            int countryId;

            SqlDataReader reader1;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                using (SqlCommand sqlCommand = new SqlCommand(countryQuery, myCon))
                {
                    reader1 = sqlCommand.ExecuteReader();
                    table1.Load(reader1);
                    countryId = Int32.Parse( table1.Rows[0][0].ToString());
                    reader1.Close();
                    myCon.Close();
                }
            }
            string query = @"insert into dbo.[User] values('" + user.UserName + @"'," + countryId + @",'" + user.PhoneNumber + @"')";

            DataTable table = new DataTable();
            
            SqlDataReader reader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    reader = sqlCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(User user)
        {
          

            DataTable table = new DataTable();
            
            string countryQuery = @"select countryId from Country where countryName ='" + user.CountryName + @"' ";
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            DataTable table1 = new DataTable();
            int countryId;

            SqlDataReader reader1;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                using (SqlCommand sqlCommand = new SqlCommand(countryQuery, myCon))
                {
                    reader1 = sqlCommand.ExecuteReader();
                    table1.Load(reader1);
                    countryId = Int32.Parse(table1.Rows[0][0].ToString());
                    reader1.Close();
                    myCon.Close();
                }
            }

            string query = @"
                            update dbo.[User] set 
                            UserName = '" + user.UserName + @"'
                            ,countryId = " + countryId + @"
                            ,phoneNumber = '" + user.PhoneNumber + @"'
                            where UserId = " + user.UserId + @"
                            ";
            SqlDataReader reader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    reader = sqlCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.[User] where UserId = " + id + @"
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            SqlDataReader reader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    reader = sqlCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Delete Successfully");
        }

        [Route("GetAllCountryNames")]
        public JsonResult GetAllCountryNames()
        {
            string query = @"select CountryName
                            from Country ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("UserAppCon");
            SqlDataReader reader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, myCon))
                {
                    reader = sqlCommand.ExecuteReader();
                    table.Load(reader);
                    reader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }
    }
}
