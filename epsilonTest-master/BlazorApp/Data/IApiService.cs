using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp.Data
{
    public interface IApiService
    {
        Task<PaginationList<T>> Get<T>(int? pageNumber, string route);
        Task<T> Post<T>(T obj, string route);
        Task<T> Put<T>(T obj, string route);
        Task Delete(string id, string route);
        Task<T> Get<T>(string id, string route);
    }

}
