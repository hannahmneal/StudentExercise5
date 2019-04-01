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
    public class StudentController : ControllerBase
    {
        public SqlConnection Connection
        {
            get
            {
                string connectionString = "Server=HNEAL-PC\\SQLEXPRESS;Database=StudentExercise3;Integrated Security=true";
                return new SqlConnection(connectionString);
            }
        }

        // GET: api/Student
        [HttpGet]
        public IEnumerable<Student> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
                                            s.Id,
                                            s.StudentFirstName,
                                            s.StudentLastName,
                                            s.StudentSlackHandle,
                                            s.student_cohort_id,
                                            c.CohortName
                                        FROM Student s INNER JOIN Cohort c ON s.student_cohort_id = c.Id";
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Student> students = new List<Student>();
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            StudentFirstName = reader.GetString(reader.GetOrdinal("StudentFirstName")),
                            StudentLastName = reader.GetString(reader.GetOrdinal("StudentLastName")),
                            StudentSlackHandle = reader.GetString(reader.GetOrdinal("StudentSlackHandle")),
                            student_cohort_id = reader.GetInt32(reader.GetOrdinal("student_cohort_id")),
                            Cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("student_cohort_id")),
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };

                        students.Add(student);
                    }

                    reader.Close();
                    return students;
                }
            }
        }

        // GET: api/Student/id
        [HttpGet("{id}", Name = "GetStudent")]
        public Student Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT
                                            s.Id,
                                            s.StudentFirstName,
                                            s.StudentLastName,
                                            s.StudentSlackHandle,
                                            s.student_cohort_id,
                                            c.CohortName
                                        FROM Student s INNER JOIN Cohort c ON s.student_cohort_id = c.Id
                                        WHERE s.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Student student = null;
                    if (reader.Read())
                    {
                        student = new Student
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            StudentFirstName = reader.GetString(reader.GetOrdinal("StudentFirstName")),
                            StudentLastName = reader.GetString(reader.GetOrdinal("StudentLastName")),
                            StudentSlackHandle = reader.GetString(reader.GetOrdinal("StudentSlackHandle")),
                            student_cohort_id = reader.GetInt32(reader.GetOrdinal("Student_cohort_id")),
                            Cohort = new Cohort
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("student_cohort_id")),
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                            }
                        };
                    }

                    reader.Close();
                    return student;

                }
            }
        }

        // POST: api/Student
        [HttpPost]
        public ActionResult Post([FromBody] Student newStudent)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Student
                                        (StudentFirstName, StudentLastName, StudentSlackHandle, student_cohort_id)
                                        OUTPUT INSERTED.Id
                                        VALUES
                                        (@StudentFirstName, @StudentLastName, @StudentSlackHandle, @student_cohort_id)";

                    cmd.Parameters.Add(new SqlParameter("@StudentFirstName", newStudent.StudentFirstName));
                    cmd.Parameters.Add(new SqlParameter("@StudentLastName", newStudent.StudentLastName));
                    cmd.Parameters.Add(new SqlParameter("@StudentSlackHandle", newStudent.StudentSlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@student_cohort_id", newStudent.student_cohort_id));

                    int newId = (int)cmd.ExecuteScalar();
                    newStudent.Id = newId;
                    return CreatedAtRoute("GetStudent", new { id = newId }, newStudent);

                }
            }
        }

        // PUT: api/Student/id
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Student student)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Student
                                        SET StudentFirstName = @StudentFirstName,
                                            StudentLastName = @StudentLastName,
                                            StudentSlackHandle = @StudentSlackHandle,
                                            student_cohort_id = @student_cohort_id
                                        WHERE id = @id";

                    cmd.Parameters.Add(new SqlParameter("@StudentFirstName", student.StudentFirstName));
                    cmd.Parameters.Add(new SqlParameter("@StudentLastName", student.StudentLastName));
                    cmd.Parameters.Add(new SqlParameter("@StudentSlackHandle", student.StudentSlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@student_cohort_id", student.student_cohort_id));
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // DELETE: api/ApiWithActions/id
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Student WHERE Id = @id;";
                    cmd.Parameters.Add(new SqlParameter("@Id", id));

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}