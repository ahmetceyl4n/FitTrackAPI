using FitnessApp.Domain.Common;
using System.Linq.Expressions;

namespace FitnessApp.Application.Common.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // ARTIK LIST DEĞİL, IQUERYABLE DÖNÜYORUZ
        // Bu sayede Service katmanında .Where(), .OrderBy(), .Skip() diyebileceğiz.
        IQueryable<T> GetAll(bool trackChanges = false);

        Task<T> GetByIdAsync(Guid id, bool trackChanges = false);

        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // RETURN TİPLERİ ARTIK BOOLEAN DEĞİL, TASK VEYA VOID.
        // Çünkü kaydetme işlemi burada yapılmadığı için true/false dönemeyiz.
        Task AddAsync(T entity);

        // Silme işlemi için önce veriyi bulacağımız için nesne istiyoruz
        void Remove(T entity);

        void Update(T entity);
    }
}