using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gWorks.Hiring.Services.Models
{
    public class ClassScheduleDto
    {
        public string? ClassName { get; set; }
        public string? InstructorName { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

}
