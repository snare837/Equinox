// Models/DataLayer/Repositories/Repository.cs
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Equinox.Models.DataLayer;   

namespace Equinox.Models.DataLayer.Repositories
{
    public class Repository<T> where T : class
    {
        protected EquinoxContext context { get; set; }
        private DbSet<T> dbset { get; set; }

        public Repository(EquinoxContext ctx)
        {
            context = ctx;
            dbset = context.Set<T>();
        }

        public int Count => dbset.Count();

        // retrieve a list of entities
        public virtual IEnumerable<T> List(QueryOptions<T> options) =>
            BuildQuery(options).ToList();

        // retrieve a single entity (3 overloads)
        public virtual T? Get(int id) => dbset.Find(id);
        public virtual T? Get(string id) => dbset.Find(id);
        public virtual T? Get(QueryOptions<T> options) =>
            BuildQuery(options).FirstOrDefault();

        // insert, update, delete, save
        public virtual void Insert(T entity) => dbset.Add(entity);
        public virtual void Update(T entity) => dbset.Update(entity);
        public virtual void Delete(T entity) => dbset.Remove(entity);
        public virtual void Save() => context.SaveChanges();

        // build query
        private IQueryable<T> BuildQuery(QueryOptions<T> options)
        {
            IQueryable<T> query = dbset;

            foreach (string include in options.GetIncludes())
                query = query.Include(include);

            if (options.HasWhere)
                query = query.Where(options.Where);

            if (options.HasOrderBy)
            {
                query = options.OrderByDirection == "asc"
                    ? query.OrderBy(options.OrderBy)
                    : query.OrderByDescending(options.OrderBy);
            }

            if (options.HasPaging)
                query = query.PageBy(options.PageNumber, options.PageSize);

            return query;
        }
    }
}
