using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebPhongKham.Models;

namespace WebPhongKham.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbset;

        public Repository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            //Set<T> : sẽ trả về 1 dbset<T> tương ứng
            _dbset = context.Set<T>();
        }

        //public IEnumerable<T> GetAll() => _dbset.ToList();

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[]includes )
        {
            //query đại diện cho truy vấn có thể thực thi trên bảng _dbset = context.Set<T>() tương ứng
            IQueryable<T> query = _dbset;
            // Duyệt qua các biểu thức lambda để thêm Include cho bảng liên quan
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }
        public T GetById(int id) => _dbset.Find(id);
        public void Insert(T entity) => _dbset.Add(entity);
        public void Update(T entity) => _dbset.Update(entity);
        public void Delete(T entity) => _dbset.Remove(entity);
        public void Save() => _context.SaveChanges();


        //bất đồng bộ
        //T? : T có thể null
        public async Task<T?> GetByIdAsync(int id) => await _dbset.FindAsync(id);

        public async Task InsertAsync(T entity) => await _dbset.AddAsync(entity);

        public async Task UpdateAsync(T entity)
        {
            _dbset.Update(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbset.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
