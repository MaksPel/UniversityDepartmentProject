using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using UniversityDepartment.Data;
using UniversityDepartment.Models;

namespace UniversityDepartment
{
    public static class DbInitializer
    {
        public static void Initialize(UniversityDepartmentContext context)
        {
            context.Database.Migrate();

            var random = new Random();

            // Initialize Faculties
            if (!context.Faculties.Any())
            {
                var faculties = Enumerable.Range(1, 500).Select(i => new Faculty
                {
                    FacultyId = Guid.NewGuid(),
                    Name = $"Faculty_{i}"
                }).ToList();

                context.Faculties.AddRange(faculties);
                context.SaveChanges();
            }

            // Initialize Departments
            if (!context.Departments.Any())
            {
                var faculties = context.Faculties.ToList();
                var departments = Enumerable.Range(1, 500).Select(i => new Department
                {
                    DepartmentId = Guid.NewGuid(),
                    Name = $"Department_{i}",
                    IsGraduating = random.Next(0, 2) == 1,
                    FacultyId = faculties[random.Next(faculties.Count)].FacultyId
                }).ToList();

                context.Departments.AddRange(departments);
                context.SaveChanges();
            }

            // Initialize Specialties
            if (!context.Specialties.Any())
            {
                var departments = context.Departments.ToList();
                var specialties = Enumerable.Range(1, 500).Select(i => new Specialty
                {
                    SpecialtyId = Guid.NewGuid(),
                    Name = $"Specialty_{i}",
                    DepartmentId = departments[random.Next(departments.Count)].DepartmentId
                }).ToList();

                context.Specialties.AddRange(specialties);
                context.SaveChanges();
            }

            // Initialize Courses
            if (!context.Courses.Any())
            {
                var specialties = context.Specialties.ToList();
                var courses = Enumerable.Range(1, 2000).Select(i => new Course
                {
                    CourseId = Guid.NewGuid(),
                    CourseNumber = random.Next(1, 5),
                    SemesterNumber = random.Next(1, 9),
                    SpecialtyId = specialties[random.Next(specialties.Count)].SpecialtyId
                }).ToList();

                context.Courses.AddRange(courses);
                context.SaveChanges();
            }

            // Initialize Subjects
            if (!context.Subjects.Any())
            {
                var subjects = Enumerable.Range(1, 2000).Select(i => new Subject
                {
                    SubjectId = Guid.NewGuid(),
                    Name = $"Subject_{i}",
                    LectureHours = random.Next(10, 50),
                    PracticalHours = random.Next(10, 50),
                    LabHours = random.Next(10, 50),
                    ReportingType = random.Next(0, 2) == 0 ? "Exam" : "Test"
                }).ToList();

                context.Subjects.AddRange(subjects);
                context.SaveChanges();
            }

            // Assign Subjects to Courses
            if (!context.Courses.SelectMany(c => c.Subjects).Any())
            {
                var courses = context.Courses.ToList();
                var subjects = context.Subjects.ToList();

                foreach (var course in courses)
                {
                    var assignedSubjects = subjects.OrderBy(s => random.Next()).Take(random.Next(3, 7)).ToList();
                    course.Subjects.AddRange(assignedSubjects);
                }

                context.SaveChanges();
            }

            // Initialize Teachers
            if (!context.Teachers.Any())
            {
                var teachers = Enumerable.Range(1, 2000).Select(i => new Teacher
                {
                    TeacherId = Guid.NewGuid(),
                    Name = $"Name_{i}",
                    Surname = $"Surname_{i}",
                    Midname = $"Midname_{i}",
                    Position = $"Position_{i}",
                    Age = random.Next(25, 65)
                }).ToList();

                context.Teachers.AddRange(teachers);
                context.SaveChanges();
            }

            // Assign Teachers to Subjects
            if (!context.Subjects.SelectMany(s => s.Teachers).Any())
            {
                var teachers = context.Teachers.ToList();
                var subjects = context.Subjects.ToList();

                foreach (var subject in subjects)
                {
                    var assignedTeachers = teachers.OrderBy(t => random.Next()).Take(random.Next(1, 4)).ToList();
                    subject.Teachers.AddRange(assignedTeachers);
                }

                context.SaveChanges();
            }
        }
    }
}
