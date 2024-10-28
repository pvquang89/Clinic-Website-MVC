using System.Linq.Expressions;

namespace WebPhongKham.Repositories
{
    public interface IRepository<T> where T : class
    {
        //Expression<Func<T, object>> : cho phép tham số truyền vào là 1 arr các biểu thức lamda 
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        T GetById(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();

    }
}
