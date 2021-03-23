using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SiwesApp.Data;
using SiwesApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiwesApp.Repositories
{
    public class GlobalRepository : IGlobalRepository
    {
        private readonly ApplicationDataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public GlobalRepository(ApplicationDataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Add<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _dataContext.Add(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _dataContext.Entry(entity).State = EntityState.Modified;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update<TEntity>(List<TEntity> entities) where TEntity : class
        {
            try
            {
                _dataContext.UpdateRange(entities.AsEnumerable());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Add<TEntity>(List<TEntity> entities) where TEntity : class
        {
            try
            {
                await _dataContext.AddRangeAsync(entities.AsEnumerable());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete<TEntity>(TEntity entity) where TEntity : class
        {
            try
            {
                _dataContext.Remove(entity);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete<TEntity>(List<TEntity> entities) where TEntity : class
        {
            try
            {
                _dataContext.RemoveRange(entities.AsEnumerable());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool?> SaveAll()
        {
            try
            {
                int saveStatus = await _dataContext.SaveChangesAsync();
                if (saveStatus > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<TEntity> Get<TEntity>(int id) where TEntity : class
        {
            var entity = await _dataContext.FindAsync<TEntity>(id);
            return entity;
        }

        public async Task<List<TEntity>> Get<TEntity>() where TEntity : class
        {
            var entity = await _dataContext.Set<TEntity>().ToListAsync();
            return entity;
        }
    }
}
