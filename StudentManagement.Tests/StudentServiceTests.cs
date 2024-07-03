using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement.Tests
{
    public class StudentServiceTests
    {
        [Fact]
        public void AddStudent_ShouldAddStudentToDatabase()
        {
            // Arrange
            var context = new Mock<SchoolContext>();
            var studentService = new StudentService(context.Object);
            var student = new Student { Id = 1, Name = "John Doe" };

            // Act
            studentService.AddStudent(student);

            // Assert
            context.Verify(c => c.Add(It.Is<Student>(s => s.Name == "John Doe")), Times.Once);
            context.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void GetStudentById_ShouldReturnCorrectStudent()
        {
            // Arrange
            var context = new Mock<SchoolContext>();
            var student = new Student { Id = 1, Name = "John Doe" };
            context.Setup(c => c.Students.Find(1)).Returns(student);
            var studentService = new StudentService(context.Object);

            // Act
            var result = studentService.GetStudentById(1);

            // Assert
            Assert.Equal("John Doe", result.Name);
        }

        [Fact]
        public void UpdateStudent_ShouldModifyStudentDetails()
        {
            // Arrange
            var context = new Mock<SchoolContext>();
            var student = new Student { Id = 1, Name = "John Doe" };
            context.Setup(c => c.Students.Find(1)).Returns(student);
            var studentService = new StudentService(context.Object);
            var updatedStudent = new Student { Id = 1, Name = "Jane Doe" };

            // Act
            studentService.UpdateStudent(updatedStudent);

            // Assert
            Assert.Equal("Jane Doe", student.Name);
            context.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteStudent_ShouldRemoveStudentFromDatabase()
        {
            // Arrange
            var context = new Mock<SchoolContext>();
            var student = new Student { Id = 1, Name = "John Doe" };
            context.Setup(c => c.Students.Find(1)).Returns(student);
            var studentService = new StudentService(context.Object);

            // Act
            studentService.DeleteStudent(1);

            // Assert
            context.Verify(c => c.Remove(It.Is<Student>(s => s.Id == 1)), Times.Once);
            context.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void GetAllStudents_ShouldReturnAllStudents()
        {
            // Arrange
            var context = new Mock<SchoolContext>();
            var students = new List<Student>
        {
            new Student { Id = 1, Name = "John Doe" },
            new Student { Id = 2, Name = "Jane Doe" }
        }.AsQueryable();
            context.Setup(c => c.Students).ReturnsDbSet(students);
            var studentService = new StudentService(context.Object);

            // Act
            var result = studentService.GetAllStudents();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void AddStudent_ShouldThrowExceptionForDuplicateId()
        {
            // Arrange
            var context = new Mock<SchoolContext>();
            var student = new Student { Id = 1, Name = "John Doe" };
            context.Setup(c => c.Students.Find(1)).Returns(student);
            var studentService = new StudentService(context.Object);
            var newStudent = new Student { Id = 1, Name = "Jane Doe" };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => studentService.AddStudent(newStudent));
        }

        [Fact]
        public void GetStudentByName_ShouldReturnCorrectStudent()
        {
            // Arrange
            var context = new Mock<SchoolContext>();
            var students = new List<Student>
        {
            new Student { Id = 1, Name = "John Doe" },
            new Student { Id = 2, Name = "Jane Doe" }
        }.AsQueryable();
            context.Setup(c => c.Students).ReturnsDbSet(students);
            var studentService = new StudentService(context.Object);

            // Act
            var result = studentService.GetStudentByName("Jane Doe");

            // Assert
            Assert.Equal(2, result.Id);
        }

        [Fact]
        public void GetStudentsByCourse_ShouldReturnCorrectStudents()
        {
            // Arrange
            var context = new Mock<SchoolContext>();
            var students = new List<Student>
        {
            new Student { Id = 1, Name = "John Doe", Course = "Math" },
            new Student { Id = 2, Name = "Jane Doe", Course = "Science" }
        }.AsQueryable();
            context.Setup(c => c.Students).ReturnsDbSet(students);
            var studentService = new StudentService(context.Object);

            // Act
            var result = studentService.GetStudentsByCourse("Math");

            // Assert
            Assert.Single(result);
            Assert.Equal("John Doe", result.First().Name);
        }

        [Fact]
        public void AddStudent_ShouldCallSaveChanges()
        {
            // Arrange
            var context = new Mock<SchoolContext>();
            var studentService = new StudentService(context.Object);
            var student = new Student { Id = 1, Name = "John Doe" };

            // Act
            studentService.AddStudent(student);

            // Assert
            context.Verify(c => c.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateStudent_ShouldCallSaveChanges()
        {
            // Arrange
            var context = new Mock<SchoolContext>();
            var student = new Student { Id = 1, Name = "John Doe" };
            context.Setup(c => c.Students.Find(1)).Returns(student);
            var studentService = new StudentService(context.Object);
            var updatedStudent = new Student { Id = 1, Name = "Jane Doe" };

            // Act
            studentService.UpdateStudent(updatedStudent);

            // Assert
            context.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}
