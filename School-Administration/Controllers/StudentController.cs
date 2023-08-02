using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School_Administration.Data;
using School_Administration.Dtos;
using School_Administration.Models;
using School_Administration.Repositories.Interface;
using System.Linq;
using System.Threading.Tasks;

namespace School_Administration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;

        public StudentController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        [HttpPost("AddStudent")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> AddStudent(StudentDto dto)
        {
            var student = new Student();
            var grade = new Grade();

            student.FirstName = dto.FirstName;

            student.LastName = dto.LastName;

            grade = (await _repository.GradeRepository.GetAllEntity(w =>
          w.GradeName == dto.GradeName || w.GradeId == dto.GradeId)).SingleOrDefault();


            if (grade == null)
                return Ok("grade not exist");

            student.GradeId = grade.GradeId;

            _repository.StudentRepository.Add(student);

            await _repository.SaveAsync();

            return Ok("Student Added Successfully");
        }

        [HttpPost("GetStudentsWithSearch")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult> SearchStudent(StudentDto dto)
        {
            var students = (await _repository.StudentRepository.StudentsWithSearch(dto)).Select(w=>new StudentResultDto
            {
                FirstName=w.FirstName,
                LastName=w.LastName,
                GradeName=w.Grade.GradeName,
                StudentId=w.StudentId
            });

          return Ok(students);
        }

        [HttpPut("UpdateStudent{studentId}")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> UpdateStudent(int studentId,StudentDto dto)
        {
            var student = await _repository.StudentRepository.GetById(studentId);

           var grade = (await _repository.GradeRepository.GetAllEntity(w =>
         w.GradeName == dto.GradeName || w.GradeId == dto.GradeId)).SingleOrDefault();

            if (grade == null)
                return Ok("grade not exist");

            student.FirstName= dto.FirstName;
            student.LastName = dto.LastName;
            student.GradeId = grade.GradeId;

            _repository.StudentRepository.Update(student);

            await _repository.SaveAsync();

            return Ok("Student updated successfully");


        }

        [HttpDelete("DeleteStudent{studentId}")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> DeleteStudent(int studentId)
        {
            var student = await _repository.StudentRepository.GetById(studentId);

            if(student == null)
                return Ok("Student ID not exist");

            await _repository.StudentRepository.Delete(studentId);

            await _repository.SaveAsync();

            return Ok("Student Deleted Successfully");
        }

        [HttpPost("AddGrade{gradeName}")]
        [Authorize(Policy = Policies.Admin)]
        public async Task<ActionResult> AddGrade(string gradeName)
        {
            var grade = new Grade();

            grade.GradeName = gradeName;

            _repository.GradeRepository.Add(grade);

            await _repository.SaveAsync();

            return Ok("Grade Added Successfully");
        }
    }
}
