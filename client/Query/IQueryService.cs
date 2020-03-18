using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace client.Query
{
    public interface IQueryService<T>
    {
        Task<List<T>> GetAll();
        T GetById(Guid id);
    }
}
