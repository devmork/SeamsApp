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
    }
}
