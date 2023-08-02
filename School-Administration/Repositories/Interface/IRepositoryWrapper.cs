using School_Administration.Models;
using System.Threading.Tasks;

namespace School_Administration.Repositories.Interface
{
    public interface IRepositoryWrapper
    {
        IStudentRepository StudentRepository { get; }
        IBaseRepository<Grade> GradeRepository { get; }
        IBaseRepository<User> UserRepository { get; }
        IBaseRepository<Role> RoleRepository { get; }
        void Save();
        Task SaveAsync();
    }
}
