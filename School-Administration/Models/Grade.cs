using System.Collections.Generic;

namespace School_Administration.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        public string GradeName { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
