using HomeBanking.Models;
using Microsoft.EntityFrameworkCore.Query;
//using static HomeBanking.Repositories.IRepositoryBase;
using System.Linq.Expressions;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace HomeBanking.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class 
        //:significan implementa de
    {
        protected HomeBankingContext RepositoryContext { get; set; }
        //accesibilidad--> protected, HomebankingContext-->tipo de dato, nombre de la variable: RepositoryContext 
        public RepositoryBase(HomeBankingContext repositoryContext) //constructor 
        {
            this.RepositoryContext = repositoryContext; 
        }
        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTrackingWithIdentityResolution();
        }
        public IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> queryable = this.RepositoryContext.Set<T>();
                                     //trae los datos de la base de datos
            if (includes != null)   //el include es un metodo 
            {
                queryable = includes(queryable); //el include nos dice que agregamos informacion
            }
            return queryable.AsNoTrackingWithIdentityResolution();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>
                ().Where(expression).AsNoTrackingWithIdentityResolution();
        }
        public void Create(T entity)
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }

        public void SaveChanges()
        {
            this.RepositoryContext.SaveChanges();
        }
    }
}
