using AutoMapper;
using SeamsApp.DTOs.Student;
using SeamsApp.Interfaces.Repositories;
using SeamsApp.Interfaces.Services;
using SeamsApp.Models.Base;

namespace SeamsApp.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;
        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }
        public async Task<int> RegisterStudentAsync(StudentCreationDTO studentCreationDTO)
        {
            var student = _mapper.Map<Student>(studentCreationDTO);
            var registeredStudent = await _studentRepository.RegisterStudentAsync(student);
            return registeredStudent.StudentId;
        }
        public async Task<int> DeleteStudentByIdAsync(int studentId)
        {
            var student = await _studentRepository.DeleteStudentByIdAsync(studentId);
            return student;
        }

        public async Task<IEnumerable<StudentDTO>> GetAllStudentAsync()
        {
            var students = await _studentRepository.GetAllStudentAsync();
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
    }
}
