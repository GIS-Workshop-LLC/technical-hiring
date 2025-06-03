using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gWorks.Hiring.Services.Models
{
    public class GroupedStudentScheduleDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public Dictionary<string, List<StudentScheduleDto>> ScheduleByDay { get; set; } = [];
    }
}
