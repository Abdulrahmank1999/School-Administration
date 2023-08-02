using School_Administration.Dtos;
using School_Administration.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace School_Administration.Repositories.Interface
{
    public interface IStudentRepository: IBaseRepository<Student>
    {
        Task<IEnumerable<Student>> StudentsWithSearch(StudentDto dto);
    }
}
