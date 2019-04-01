using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentExercise5_WebAPI.Models;

namespace StudentExercise5_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CohortController : ControllerBase
    {
        public SqlConnection Connection
        {
            get
            {
                string connectionString = "Server=HNEAL-PC\\SQLEXPRESS;Database=StudentExercise3;Integrated Security=true";
                return new SqlConnection(connectionString);
            }
        }


        // GET: api/Cohort
        [HttpGet]
        public IEnumerable<Cohort> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, CohortName FROM Cohort";
                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Cohort> cohorts = new List<Cohort>();
                    while (reader.Read())
                    {
                        Cohort cohort = new Cohort
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                        };
                        cohorts.Add(cohort);
                    };
                    reader.Close();
                    return cohorts;
                }
            }
        }

        // GET: api/Cohort/id
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Cohort
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Cohort/id
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/id
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
