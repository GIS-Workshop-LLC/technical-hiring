using System;
namespace gWorks.Hiring.Services.Models
{
	public class StudentScheduleDto
	{
		public int StudentId { get; set; }
		public string StudentName { get; set; }
		public List<CourseScheduleDto> Courses { get; set; }
	}
}

