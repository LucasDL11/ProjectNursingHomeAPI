using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio.Repositorio
{
    public interface IRepositorio<T>
    {
        public bool Add(T obj);
        public bool Remove(int id);
        public bool Update(T obj);
        public IEnumerable<T> FindAll();
        public IEnumerable<T> FindById(int id);
    }
}
