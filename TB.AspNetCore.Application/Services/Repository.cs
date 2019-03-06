using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TB.AspNetCore.Domain.Repositorys;
using TB.AspNetCore.Infrastructrue.Extensions;

namespace TB.AspNetCore.Application.Services
{
    /// <summary>
    /// 封装泛型仓储 依赖接口 DbContext约束
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class Repository<TContext> : IDbRepository<TContext> where TContext : DbContext
    {
        private TContext _context;

        protected virtual TContext DataContext
        {
            get
            {
                if ((object)this._context == null)
                    this._context = ServiceCollectionExtension.New<TContext>();
                return this._context;
            }
        }

        /// <summary>
        /// get <see cref="!:TSource" /> from raw sql query
        /// the TSource must in database or <see cref="T:Microsoft.EntityFrameworkCore.DbContext" />
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IQueryable<TSource> FromSql<TSource>(string sql, params object[] parameters) where TSource : class
        {
            return this.DataContext.Set<TSource>().FromSql<TSource>((RawSqlString)sql, parameters);
        }
        /// <summary>get single or default TSource</summary>
        /// <typeparam name="TSource">entity</typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TSource Single<TSource>(Expression<Func<TSource, bool>> predicate = null) where TSource : class
        {
            if (predicate == null)
                return this.DataContext.Set<TSource>().SingleOrDefault<TSource>();
            return this.DataContext.Set<TSource>().SingleOrDefault<TSource>(predicate);
        }

        /// <summary>
        /// get single or default tsource async
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<TSource> SingleAsycn<TSource>(Expression<Func<TSource, bool>> predicate = null) where TSource : class
        {
            if (predicate == null)
            {
                return DataContext.Set<TSource>().SingleOrDefaultAsync();
            }
            return DataContext.Set<TSource>().SingleOrDefaultAsync(predicate);
        }

        /// <summary>get first or default TSource</summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TSource First<TSource>(Expression<Func<TSource, bool>> predicate = null) where TSource : class
        {
            if (predicate == null)
                return this.DataContext.Set<TSource>().FirstOrDefault<TSource>();
            return this.DataContext.Set<TSource>().FirstOrDefault<TSource>(predicate);
        }

        /// <summary>
        /// get first or default TSource async
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<TSource> FirstAsync<TSource>(Expression<Func<TSource, bool>> predicate = null) where TSource : class
        {
            if (predicate == null)
                return this.DataContext.Set<TSource>().FirstOrDefaultAsync<TSource>();
            return this.DataContext.Set<TSource>().FirstOrDefaultAsync<TSource>(predicate);
        }

        /// <summary>select entity by conditions</summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<TSource> Where<TSource>(Expression<Func<TSource, bool>> predicate = null) where TSource : class
        {
            if (predicate == null)
                return this.DataContext.Set<TSource>().AsQueryable<TSource>();
            return this.DataContext.Set<TSource>().Where<TSource>(predicate);
        }

        /// <summary>select entity by conditions async</summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<List<TSource>> WhereAsync<TSource>(Expression<Func<TSource, bool>> predicate = null) where TSource : class
        {
            if (predicate == null)
                return this.DataContext.Set<TSource>().AsQueryable<TSource>().ToListAsync();
            return this.DataContext.Set<TSource>().Where<TSource>(predicate).ToListAsync();
        }

        /// <summary>counting the entity's count under this condition</summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count<TSource>(Expression<Func<TSource, bool>> predicate = null) where TSource : class
        {
            if (predicate == null)
                return this.DataContext.Set<TSource>().Count<TSource>();
            return this.DataContext.Set<TSource>().Count<TSource>(predicate);
        }

        /// <summary>counting the entity's count under this condition async</summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<int> CountAsync<TSource>(Expression<Func<TSource, bool>> predicate = null) where TSource : class
        {
            if (predicate == null)
                return this.DataContext.Set<TSource>().CountAsync<TSource>();
            return this.DataContext.Set<TSource>().CountAsync<TSource>(predicate);
        }

        /// <summary>return the query</summary>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public IQueryable<TSource> Query<TSource>() where TSource : class
        {
            return (IQueryable<TSource>)this.DataContext.Set<TSource>();
        }

        /// <summary>check the condition</summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exists<TSource>(Expression<Func<TSource, bool>> predicate = null) where TSource : class
        {
            if (predicate == null)
                return this.DataContext.Set<TSource>().Any<TSource>();
            return this.DataContext.Set<TSource>().Any<TSource>(predicate);
        }

        /// <summary>check the condition</summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<bool> ExistsAsync<TSource>(Expression<Func<TSource, bool>> predicate = null) where TSource : class
        {
            if (predicate == null)
                return this.DataContext.Set<TSource>().AnyAsync<TSource>();
            return this.DataContext.Set<TSource>().AnyAsync<TSource>(predicate);
        }

        /// <summary>paging the query</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size </param>
        /// <param name="count">total row record count</param>
        /// <returns></returns>
        public IQueryable<T> Pages<T>(IQueryable<T> query, int pageIndex, int pageSize, out int count) where T : class
        {
            if (pageIndex < 1)
                pageIndex = 1;
            if (pageSize < 1)
                pageSize = 10;
            count = query.Count<T>();
            query = query.Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            return query;
        }

        /// <summary>paging the query</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size </param>
        /// <param name="count">total row record count</param>
        /// <returns></returns>
        public IQueryable<T> Pages<T>(int pageIndex, int pageSize, out int count) where T : class
        {
            if (pageIndex < 1)
                pageIndex = 1;
            if (pageSize < 1)
                pageSize = 10;
            var source = this.DataContext.Set<T>().AsQueryable<T>();
            count = source.Count<T>();
            return source.Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IQueryable<T> Pages<T>(IQueryable<T> query, int pageIndex, int pageSize, out int count, out int pageCount) where T : class
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 10;
            }
            if (pageSize > 100)
            {
                pageSize = 100;
            }
            count = query.Count();
            pageCount = count / pageSize;
            if ((decimal)pageCount < (decimal)count / (decimal)pageSize)
            {
                pageCount++;
            }
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return query;
        }


        /// <summary>save all the changes add, update, delete</summary>
        public void Save()
        {
            this.DataContext.SaveChanges();
        }

        /// <summary>
        /// 异步保存
        /// </summary>
        /// <returns></returns>
        public async Task SaveAsync()
        {
            await this.DataContext.SaveChangesAsync();
        }

        /// <summary>add entity to context or database</summary>
        /// <param name="entity"></param>
        /// <param name="save">save the add and all changes before this to database</param>
        public void Add(object entity, bool save = false)
        {
            this.DataContext.Add(entity);
            if (!save)
                return;
            this.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="save"></param>
        public async Task AddAsync(object entity, bool save = false)
        {
            await this.DataContext.AddAsync(entity);
            if (save)
            {
                await this.SaveAsync();
            }

        }

        /// <summary>update entity to context or database</summary>
        /// <param name="entity"></param>
        /// <param name="save">save the update and all changes before this to database</param>
        public void Update(object entity, bool save = false)
        {
            this.DataContext.Update(entity);
            if (!save)
                return;
            this.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="save"></param>
        public async Task UpdateAsync(object entity, bool save = false)
        {
            this.DataContext.Update(entity);
            if (save)
            {
                await this.SaveAsync();
            }
        }

        /// <summary>update entitys to context or database</summary>
        /// <param name="list"></param>
        /// <param name="save">save the updates and all changes before this to database</param>
        public void Update(IEnumerable<object> list, bool save = false)
        {
            this.DataContext.UpdateRange(list);
            if (save)
                return;
            this.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="save"></param>
        public async Task UpdateAsync(IEnumerable<object> list, bool save = false)
        {
            this.DataContext.UpdateRange(list);
            if (save)
            {
                await this.SaveAsync();
            }

        }

        /// <summary>delete entity from context or database</summary>
        /// <param name="entity"></param>
        /// <param name="save">save the delete and all changes before this to database</param>
        public void Delete(object entity, bool save = false)
        {
            this.DataContext.Remove(entity);
            if (!save)
                return;
            this.Save();
        }

        public async Task DeleteAsync(object entity, bool save = false)
        {
            this.DataContext.Remove(entity);
            if (save)
            {
                await this.SaveAsync();
            }
        }

        /// <summary>delete entitys from context or database</summary>
        /// <param name="list"></param>
        /// <param name="save">save the deletes and all changes before this to database</param>
        public void Delete(IEnumerable<object> list, bool save = false)
        {
            this.DataContext.RemoveRange(list);
            if (!save)
                return;
            this.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="save"></param>
        /// <returns></returns>
        public async Task DeleteAsync(IEnumerable<object> list, bool save = false)
        {
            this.DataContext.RemoveRange(list);
            if (save)
            {
                await this.SaveAsync();
            }

        }
    }
}
