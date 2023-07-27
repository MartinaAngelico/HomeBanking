using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace HomeBanking.Repositories
{
    public interface IRepositoryBase<T> 
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null); //me permite agregar mas informacion/datos
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression); 
        void Create(T entity); //guardar
        void Update(T entity); //actualizar
        void Delete(T entity); //borrar
        void SaveChanges();                  

    }
}

