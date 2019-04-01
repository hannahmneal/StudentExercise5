using StudentExercise5_WebAPI.Models;
using System.Collections.Generic;

namespace StudentExercise5_WebAPI.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string StudentSlackHandle { get; set; }
        public int student_cohort_id { get; set; }
        public int CohortId { get; set; }
        public Cohort Cohort { get; set; }
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
    }
}