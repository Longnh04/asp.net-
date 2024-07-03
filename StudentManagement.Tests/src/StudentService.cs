using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.Tests
{
    public class StudentService
    {
        private readonly SchoolContext _context;

        public StudentService(SchoolContext context)
        {
            _context = context;
        }

        public void AddStudent(Student student)
        {
            _context.Add(student);
            _context.SaveChanges();
        }

        public Student GetStudentById(int id)
        {
            return _context.Students.Find(id);
        }

        public void UpdateStudent(Student student)
        {
            var existingStudent = _context.Students.Find(student.Id);
            if (existingStudent != null)
            {
                existingStudent.Name = student.Name;
                _context.SaveChanges();
            }
        }

        public void DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student != null)
            {
                _context.Remove(student);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _context.Students.ToList();
        }

        public Student GetStudentByName(string name)
        {
            return _context.Students.FirstOrDefault(s => s.Name == name);
        }

        public IEnumerable<Student> GetStudentsByCourse(string course)
        {
            return _context.Students.Where(s => s.Course == course).ToList();
        }
    }
}
