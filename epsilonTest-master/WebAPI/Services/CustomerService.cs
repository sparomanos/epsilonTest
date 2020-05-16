using Common;
using Interfaces;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Services
{
    public class CustomerService
    {
        private readonly IDbContext<Customer> _customerContext;
        public CustomerService(IDbContext<Customer> context)
        {
            _customerContext = context;
        }
        public  PaginationList<Customer> GetItems(int? pageNumber)
        {
            try
            {
                int pageIndex = pageNumber ?? 1;
                int pageSize = 5;
                var source = _customerContext.AsQueryable();
                int count = source.Count();
                var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return new PaginationList<Customer>(items, count, pageIndex, pageSize);
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
        public async Task<Customer> Get(string id)
        {
            return await _customerContext.FindByIdAsync(id);
        }
        public IQueryable<Customer> Get()
        {
            return  _customerContext.AsQueryable();
        }
        public async Task Create(Customer customer)
        {
            await _customerContext.InsertOneAsync(customer);
        }
        public async Task Update(Customer customer)
        {
             await _customerContext.ReplaceOneAsync(customer);
        }
        public async Task Delete(string id)
        {
            await _customerContext.DeleteByIdAsync(id);
        }
    }
}
