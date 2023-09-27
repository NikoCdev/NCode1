using MusicPortal.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPortal.DAL.Interfaces
{   
        public interface IRepository<T> where T : class
        {
            Task<IEnumerable<T>> GetAll();
            Task<T> Get(int id);
            Task<T> Get(string name);
            Task Create(T item);
            void Update(T item);
            Task Delete(int id);
        }
    
}
