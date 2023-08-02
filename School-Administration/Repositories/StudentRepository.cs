using Microsoft.EntityFrameworkCore;
using School_Administration.Data;
using School_Administration.Dtos;
using School_Administration.Models;
using School_Administration.Repositories.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace School_Administration.Repositories
{
    public class StudentRepository: BaseRepository<Student> , IStudentRepository
    {
        private readonly SchoolDbContext _context;

        public StudentRepository(SchoolDbContext context) : base(context)
        {
            _context = context;
        }

        
        public async Task<IEnumerable<Student>> StudentsWithSearch(StudentDto dto)
        {
            var query = _context.Students.Include(W => W.Grade).AsQueryable();

            if (dto.FirstName != null)
                query = query.Where(w => w.FirstName == dto.FirstName);

            if (dto.LastName != null)
                query = query.Where(w => w.LastName == dto.LastName);

            if (dto.GradeName != null)
                query = query.Where(w => w.Grade.GradeName == dto.GradeName);

            if (dto.GradeId != 0)
                query = query.Where(w => w.Grade.GradeId == dto.GradeId);

            return  await query.ToListAsync();
        }
    }
}
