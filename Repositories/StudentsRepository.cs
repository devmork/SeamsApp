using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Models.Base;

namespace SeamsApp.Data.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        public List<Student> GetAllStudent()
        {
            using ()
            {
                connection.Open();
                string sql = "SELECT Id, FirstName, MiddleName, LastName, SchoolStudentId, Course, YearLevel, Email FROM Student;";
                var students = connection.Query<Student>(sql).ToList();
                return students!;
            }
        }
        public void AddStudent(Student student)
        {
            using ()
            {
                connection.Open();
                string sql = @"INSERT INTO Student (FirstName, MiddleName, LastName, SchoolStudentId, Course, YearLevel, Email, QRCode)
                             VALUES (@FirstName, @MiddleName, @LastName, @SchoolStudentId, @Course, @YearLevel, @Email, @QRCode)";

                var parameters = new DynamicParameters();
                parameters.Add("@FirstName", student.FirstName);
                parameters.Add("@MiddleName", student.MiddleName);
                parameters.Add("@LastName", student.LastName);
                parameters.Add("@SchoolStudentId", student.SchoolStudentId);
                parameters.Add("@Course", student.Course);
                parameters.Add("@YearLevel", student.YearLevel);
                parameters.Add("@Email", student.Email);
                parameters.Add("@QRCode", student.QRCode);
                connection.Execute(sql, parameters);
            }
        }
        public void UpdateStudent(Student student)
        {
            using ()
            {
                connection.Open();
                string sql = @"UPDATE Student 
                             SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName, 
                                 SchoolStudentId = @SchoolStudentId, Course = @Course, YearLevel = @YearLevel, 
                                 Email = @Email, QRCode = @QRCode
                             WHERE Id = @Id";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", student.Id);
                parameters.Add("@FirstName", student.FirstName);
                parameters.Add("@MiddleName", student.MiddleName);
                parameters.Add("@LastName", student.LastName);
                parameters.Add("@SchoolStudentId", student.SchoolStudentId);
                parameters.Add("@Course", student.Course);
                parameters.Add("@YearLevel", student.YearLevel);
                parameters.Add("@Email", student.Email);
                parameters.Add("@QRCode", student.QRCode);
                connection.Execute(sql, parameters);
            }
        } 
        public int GetTotalStudents()
        {
            using ()
            {
                connection.Open();
                string sql = @"SELECT COUNT(SchoolStudentId) FROM Student";
                return connection.ExecuteScalar<int>(sql);
            }
        }
        public Student GetStudentById(string schoolStudentId)
        {
            using ()
            {
                connection.Open();
                string sql = "SELECT SchoolStudentId, FirstName, MiddleName, LastName, QRCode, Course, YearLevel FROM Student WHERE SchoolStudentId = @SchoolStudentId";
                var parameters = new DynamicParameters();
                parameters.Add("SchoolStudentId", schoolStudentId);
                return connection.QueryFirstOrDefault<Student>(sql, parameters);
            }
        }
        public bool CheckDuplicateSchoolId(string schoolStudentId, int id)
        {
            using ()
            {
                connection.Open();
                var query = @"SELECT COUNT(1) 
                              FROM Student 
                              WHERE SchoolStudentId = @SchoolStudentId
                              AND Id != @Id";

                var parameters = new DynamicParameters();
                parameters.Add("SchoolStudentId", schoolStudentId);
                parameters.Add("Id", id);

                var count = connection.ExecuteScalar<int>(query, parameters);
                return count > 0;
            }
        }
        public List<Student> GetStudentQRCode(string schoolStudentId)
        {
            string sql = "SELECT QRCode FROM Student WHERE SchoolStudentId = @SchoolStudentId";
            
            var parameters = new DynamicParameters();
            parameters.Add("SchoolStudentId", schoolStudentId);

            using ()
            {
                connection.Open();
                return connection.Query<Student>(sql, parameters).ToList();
            }
        }
    }
}
