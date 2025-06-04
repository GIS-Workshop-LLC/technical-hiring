using System;
namespace gWorks.Hiring.Services.Models
{
	public class StudentScheduleDto
	{
		public string StudentName { get; set; }
		public List<CourseScheduleDto> Courses { get; set; }
	}
}

