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
        public string StudentName { get; set; } = string.Empty;
        public List<ClassScheduleDto> Classes { get; set; } = [];
    }

    public class ClassScheduleDto
    {
        public string ClassName { get; set; } = string.Empty;
        public string ClassroomName { get; set; } = string.Empty;
        public string Teacher { get; set; } = string.Empty;
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
    }
}
