using StockProject.DataAccess.Contexts;
using StockProject.DataAccess.Interfaces;
using StockProject.DataAccess.Repositories;
using StockProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProject.DataAccess.UnitOfWork
{
    public class Uow : IUow
    {
        private readonly StockContext _context;

        public Uow(StockContext context)
        {
            _context = context;
        }

        public IRepository<T> GetRepository<T>() where T : BaseEntity
        {
            return new Repository<T>(_context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
