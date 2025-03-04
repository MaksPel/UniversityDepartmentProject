﻿using UniversityDepartment.Models;

namespace UniversityDepartment.ViewModels.Courses
{
    public class CourseViewModel
    {
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();
        public string Specialty { get; set; } = null!;
        public int CourseNumber { get; set; }
        public int SemesterNumber { get; set; }
        public PageViewModel? PageViewModel { get; set; }
        public CoursesSortViewModel? SortViewModel { get; set; }
    }
}
