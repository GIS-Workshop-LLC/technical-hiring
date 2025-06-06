using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gWorks.Hiring.Services.Models
{
    public class StudentScheduleDto
    {
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public List<ClassScheduleDto>? Schedule { get; set; }
    }

}
