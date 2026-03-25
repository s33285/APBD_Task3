using LinqConsoleLab.EN.Data;

namespace LinqConsoleLab.EN.Exercises;

public sealed class LinqExercises
{ 
    public IEnumerable<string> Task01_StudentsFromWarsaw()
    {
        return UniversityData.Students
            .Where(s => string.Equals(s.City, "Warsaw", StringComparison.OrdinalIgnoreCase))
            .Select(s => $"{s.IndexNumber} | {s.FirstName} {s.LastName} | {s.City}");
    }
    
    public IEnumerable<string> Task02_StudentEmailAddresses()
    {
        return UniversityData.Students
            .Select(s  => s.Email);
    }
    
    public IEnumerable<string> Task03_StudentsSortedAlphabetically()
    {
        return UniversityData.Students
            .OrderBy(s => s.LastName).ThenBy(s=>s.FirstName)
            .Select(s => $"{s.IndexNumber} | {s.FirstName} {s.LastName}");
    }
    
    public IEnumerable<string> Task04_FirstAnalyticsCourse()
    {
        var course = UniversityData.Courses
            .FirstOrDefault(c=> string.Equals(c.Category, "Analytics", StringComparison.OrdinalIgnoreCase));
        if (course == null)
        {
            return new[] { "No Analytics course found. " };
        }
        return new[] { $"{course.Title} | start {course.StartDate} "};
    }
    
    public IEnumerable<string> Task05_IsThereAnyInactiveEnrollment()
    {
        var anyInactive = UniversityData.Enrollments.Any(e => !e.IsActive);
        return new[]
        {
            anyInactive.ToString()
        };
    }
    
    public IEnumerable<string> Task06_DoAllLecturersHaveDepartment()
    {
        var assignedDep = UniversityData.Lecturers.All(l => !string.IsNullOrEmpty(l.Department));
        return new[]
        {
            assignedDep.ToString()
        };
    }

    /// <summary>
    /// Task:
    /// Count how many active enrollments exist in the system.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Enrollments
    /// WHERE IsActive = 1;
    /// </summary>
    public IEnumerable<string> Task07_CountActiveEnrollments()
    {
        var count = UniversityData.Enrollments.Count(e => e.IsActive);
        return new[]
        {
            count.ToString()
        };
    }
    
    public IEnumerable<string> Task08_DistinctStudentCities()
    {
        return UniversityData.Students
            .Select(s => s.City).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(c => c);
    }
    
    public IEnumerable<string> Task09_ThreeNewestEnrollments()
    {
        return UniversityData.Enrollments.OrderByDescending(e => e.EnrollmentDate).Take(3)
            .Select(e => $"{e.EnrollmentDate} | student: {e.StudentId} | course: {e.CourseId}");
    }
    
    public IEnumerable<string> Task10_SecondPageOfCourses()
    {
        int pageSize = 2;
        int pageIndex = 1;

        return UniversityData.Courses.OrderBy(c => c.Title).Skip(pageIndex * pageSize).Take(pageSize)
            .Select(c => $"{c.Title} | {c.Category}");
    }
    
    public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    {
        return UniversityData.Enrollments.Join(UniversityData.Students, e => e.StudentId, s => s.Id,
            (e, s) => new {s.FirstName, s.LastName, e.EnrollmentDate})
            .Select(e => $"{e.FirstName} | {e.LastName} | Enrollment date: {e.EnrollmentDate}");
    }
    
    public IEnumerable<string> Task12_StudentCoursePairs()
    {
        return UniversityData.Enrollments
            .Join(UniversityData.Students, e => e.StudentId, s => s.Id, (e, s) => new{e,s})
            .Join(UniversityData.Courses, es =>es.e.CourseId, c => c.Id, (es, c) => new {Student = es.s, Course = c })
            .Select(x => $"{x.Student.FirstName} {x.Student.LastName} | {x.Course.Title}");
    }
    
    public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    {
        return UniversityData.Enrollments.GroupBy(e => e.CourseId)
            .Select(g =>
            {
                var course = UniversityData.Courses.FirstOrDefault(c => c.Id == g.Key);
                var title = course?.Title ?? $"CourseId: {g.Key}";
                return $"{title} | {g.Count()}";
            });
    }

    public IEnumerable<string> Task14_AverageGradePerCourse()
    {
        return UniversityData.Enrollments
            .Where(e => e.FinalGrade.HasValue)
            .GroupBy(e => e.CourseId)
            .Select(g =>
            {
                var course = UniversityData.Courses.First(c => c.Id == g.Key);
                return $"{course.Title} | {g.Average(e => e.FinalGrade!.Value):F2}";
            });
    }
    
    public IEnumerable<string> Task15_LecturersAndCourseCounts()
    {
        return UniversityData.Lecturers.Select(les =>
        {
            var count = UniversityData.Courses.Count(c => c.LecturerId == les.Id);
            return $"{les.FirstName} {les.LastName} | {count}";
        });
    }

    public IEnumerable<string> Task16_HighestGradePerStudent()
    {
        return UniversityData.Enrollments.Where(e => e.FinalGrade.HasValue)
            .GroupBy(e => e.StudentId).Select(g =>
            {
                var student = UniversityData.Students.First(s => s.Id == g.Key);
                var maxGrade = g.Max(e => e.FinalGrade.Value);
                return $"{student.FirstName} {student.LastName} | {maxGrade}";
            });
    }

    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        return UniversityData.Enrollments.Where(e => e.IsActive)
            .GroupBy(e => e.StudentId)
            .Where(g => g.Count() > 1)
            .Select(g =>
            {
                var student = UniversityData.Students.First(s => s.Id == g.Key);
                var name = student is null ? $"Student: {g.Key}" : $"{student.FirstName} {student.LastName}";
                return $"{name} | {g.Count()}";
            });
    }


    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        return UniversityData.Courses
            .Where(c=>c.StartDate.Month == 4 && c.StartDate.Year ==2026)
            .Where(c =>
            {
                var enrollments = UniversityData.Enrollments
                    .Where(e => e.CourseId == c.Id)
                    .ToList();
                return enrollments.Any() && enrollments.All(e => !e.FinalGrade.HasValue);

            })
            .Select(c => c.Title);
    }
    
    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        return UniversityData.Lecturers.Select(les =>
        {
            var grade = UniversityData.Courses
                .Where(c => c.LecturerId == les.Id)
                .SelectMany(c => UniversityData.Enrollments.Where(e => e.CourseId == c.Id && e.FinalGrade.HasValue)
                    .Select(e => e.FinalGrade!.Value)).ToList();

            if (grade.Any())
            {
                return $"{les.FirstName} {les.LastName} | {grade.Average():F2}";
            }
            return $"{les.FirstName} {les.LastName}";
        });
    }

    /// <summary>
    /// Challenge:
    /// Show student cities and the number of active enrollments created by students from each city.
    /// Sort the result by the active enrollment count in descending order.
    ///
    /// SQL:
    /// SELECT s.City, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.City
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    {
        throw NotImplemented(nameof(Challenge04_CitiesAndActiveEnrollmentCounts));
    }

    private static NotImplementedException NotImplemented(string methodName)
    {
        return new NotImplementedException(
            $"Complete method {methodName} in Exercises/LinqExercises.cs and run the command again.");
    }
}