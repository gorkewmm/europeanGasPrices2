using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        protected readonly TContext _context;
        public void Add(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                // Entity context'e eklenir; EF Core ChangeTracker bunu otomatik "Added" olarak işaretler.
                context.Set<TEntity>().Add(entity);
                context.SaveChanges(); // PostgreContext içindeki ChangeTracker tetiklenir, CreatedDate doldurulur.
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                // Entity güncellenir; EF Core ChangeTracker bunu otomatik "Modified" olarak işaretler.
                context.Set<TEntity>().Update(entity);
                context.SaveChanges(); // PostgreContext içindeki ChangeTracker tetiklenir, UpdatedDate doldurulur.
            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                // Entity silinmek üzere işaretlenir; EF Core ChangeTracker bunu otomatik "Deleted" olarak işaretler.
                context.Set<TEntity>().Remove(entity);

                // SaveChanges çağrıldığı an PostgreContext'teki SetAuditProperties() devreye girer:
                // State'in "Deleted" olduğunu görür, bunu "Modified" olarak değiştirir, 
                // IsDeleted = true yapar ve DeletedDate alanını doldurarak Soft-Delete işlemini tamamlar.
                context.SaveChanges();
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                // Tek bir kaydı getirirken anlık context oluşturulur ve sorgu tamamlanınca bellek boşaltılır.
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                // Filtre varsa filtreye uyanları, yoksa tüm listeyi çeker.
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }
    }
}
