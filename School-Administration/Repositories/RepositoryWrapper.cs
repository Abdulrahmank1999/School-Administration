using School_Administration.Data;
using School_Administration.Models;
using School_Administration.Repositories.Interface;
using System.Threading.Tasks;

namespace School_Administration.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        public IStudentRepository StudentRepository { get; }

        public IBaseRepository<Grade> GradeRepository { get; }

        public IBaseRepository<User> UserRepository { get; }

        public IBaseRepository<Role> RoleRepository { get; }

        private SchoolDbContext _context;

        public RepositoryWrapper(SchoolDbContext context)
        {
            _context = context;

            StudentRepository = new StudentRepository(_context);

            GradeRepository = new BaseRepository<Grade>(_context);

            UserRepository = new BaseRepository<User>(_context);

            RoleRepository = new BaseRepository<Role>(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
