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

    /// <summary>
    /// Task:
    /// Implement simple pagination for the course list.
    /// Assume a page size of 2 and return the second page of data.
    ///
    /// SQL:
    /// SELECT Title, Category
    /// FROM Courses
    /// ORDER BY Title
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Task10_SecondPageOfCourses()
    {
        throw NotImplemented(nameof(Task10_SecondPageOfCourses));
    }

    /// <summary>
    /// Task:
    /// Join students with enrollments by StudentId.
    /// Return the full student name and the enrollment date.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, e.EnrollmentDate
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId;
    /// </summary>
    public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    {
        throw NotImplemented(nameof(Task11_JoinStudentsWithEnrollments));
    }

    /// <summary>
    /// Task:
    /// Prepare all student-course pairs based on enrollments.
    /// Use an approach that flattens the data into a single result sequence.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, c.Title
    /// FROM Enrollments e
    /// JOIN Students s ON s.Id = e.StudentId
    /// JOIN Courses c ON c.Id = e.CourseId;
    /// </summary>
    public IEnumerable<string> Task12_StudentCoursePairs()
    {
        throw NotImplemented(nameof(Task12_StudentCoursePairs));
    }

    /// <summary>
    /// Task:
    /// Group enrollments by course and return the course title together with the number of enrollments.
    ///
    /// SQL:
    /// SELECT c.Title, COUNT(*)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    {
        throw NotImplemented(nameof(Task13_GroupEnrollmentsByCourse));
    }

    /// <summary>
    /// Task:
    /// Calculate the average final grade for each course.
    /// Ignore records where the final grade is null.
    ///
    /// SQL:
    /// SELECT c.Title, AVG(e.FinalGrade)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task14_AverageGradePerCourse()
    {
        throw NotImplemented(nameof(Task14_AverageGradePerCourse));
    }

    /// <summary>
    /// Task:
    /// For each lecturer, count how many courses are assigned to that lecturer.
    /// Return the full lecturer name and the course count.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, COUNT(c.Id)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Task15_LecturersAndCourseCounts()
    {
        throw NotImplemented(nameof(Task15_LecturersAndCourseCounts));
    }

    /// <summary>
    /// Task:
    /// For each student, find the highest final grade.
    /// Skip students who do not have any graded enrollment yet.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, MAX(e.FinalGrade)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY s.FirstName, s.LastName;
    /// </summary>
    public IEnumerable<string> Task16_HighestGradePerStudent()
    {
        throw NotImplemented(nameof(Task16_HighestGradePerStudent));
    }

    /// <summary>
    /// Challenge:
    /// Find students who have more than one active enrollment.
    /// Return the full name and the number of active courses.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.FirstName, s.LastName
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        throw NotImplemented(nameof(Challenge01_StudentsWithMoreThanOneActiveCourse));
    }

    /// <summary>
    /// Challenge:
    /// List the courses that start in April 2026 and do not have any final grades assigned yet.
    ///
    /// SQL:
    /// SELECT c.Title
    /// FROM Courses c
    /// JOIN Enrollments e ON c.Id = e.CourseId
    /// WHERE MONTH(c.StartDate) = 4 AND YEAR(c.StartDate) = 2026
    /// GROUP BY c.Title
    /// HAVING SUM(CASE WHEN e.FinalGrade IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        throw NotImplemented(nameof(Challenge02_AprilCoursesWithoutFinalGrades));
    }

    /// <summary>
    /// Challenge:
    /// Calculate the average final grade for every lecturer across all of their courses.
    /// Ignore missing grades but still keep the lecturers in mind as the reporting dimension.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, AVG(e.FinalGrade)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// LEFT JOIN Enrollments e ON e.CourseId = c.Id
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        throw NotImplemented(nameof(Challenge03_LecturersAndAverageGradeAcrossTheirCourses));
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