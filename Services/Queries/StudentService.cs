using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QRCoder;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Interfaces.Services.Queries;
using SeamsApp.Models;
using SeamsApp.Models.Base;
using SeamsApp.Utilities;
using System.Runtime.Versioning;

namespace SeamsApp.Services.Queries
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;

        public StudentService(
            IStudentRepository studentRepository,
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher<User> passwordHasher)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;

        }
        public async Task<int> DeleteStudentByIdAsync(int studentId)
        {
            var student = await _studentRepository.DeleteStudentByIdAsync(studentId);
            return student;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllPendingStudentAsync()
        {
            var students = await _studentRepository.GetAllPendingStudentAsync();
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }
        public async Task<IEnumerable<StudentDTO>> GetAllApprovedStudentAsync()
        {
            var students = await _studentRepository.GetAllApprovedStudentAsync();
            return _mapper.Map<IEnumerable<StudentDTO>>(students);
        }
        public async Task<StudentDTO> GetStudentByIdAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            return _mapper.Map<StudentDTO>(student);
        }

        public Task<StudentDTO> GetStudentQRCodeAsync(string schoolStudentId)
        {
            throw new NotImplementedException();
        }
        public Task<int> UpdateStudentByIdAsync(StudentUpdateDTO studentUpdateDTO, int studentId)
        {
            var product = _mapper.Map<Student>(studentUpdateDTO);
            return _studentRepository.UpdateStudentByIdAsync(product);
        }

        public async Task<CreateStudentDTO> CreateStudent(CreateStudentDTO createStudentDTO)
        {
            var student = _mapper.Map<Student>(createStudentDTO);
            var createdStudent = await _studentRepository.CreateStudentAsync(student);
            return _mapper.Map<CreateStudentDTO>(createdStudent);
        }

        [SupportedOSPlatform("windows")]
        public async Task<StudentDTO> ApprovedStudentAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new ArgumentException("Student not found");

            var firstName = student.FirstName ?? "";
            var middleName = student.MiddleName ?? "";
            var lastName = student.LastName ?? "";
            var suffix = student.Suffix ?? "";
            var schoolStudentId = student.SchoolStudentId ?? "";

            student.QRCode = QRCodeUtility.GenerateQRCode(firstName, middleName, lastName, suffix, schoolStudentId);

            await _studentRepository.UpdateStudentStatusToApprovedAsync(studentId, 2, student.QRCode);

            var user = new User
            {
                //Email = student.Email,
                Role = "Student"
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, lastName.ToUpper());
            await _userRepository.CreateUserAsync(user);

            return _mapper.Map<StudentDTO>(student);
        }

        public async Task<StudentDTO> RejectStudentAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new ArgumentException("Student not found");

            await _studentRepository.UpdateStudentStatusToRejectAsync(studentId, 3);

            return _mapper.Map<StudentDTO>(student);
        }
    }
}
