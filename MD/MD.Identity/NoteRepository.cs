﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MD.Data;

namespace MD.Identity
{
    /// <summary>
    /// Implementation of <see cref="IRepository{TEntity}" />.
    /// </summary>
    public class NoteRepository : IDisposable, IRepository<Note>
    {
        private readonly DbContext _dbContext;
        private bool _disposed;

        public NoteRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbSet<Note> DbSet => _dbContext.Set<Note>();

        public IQueryable<Note> GetAll(string userId)
        {
            return DbSet.Where(x => x.UserId == userId).Include(x => x.Photos);
        }

        public async Task<Note> GetByIdAsync(int? id)
        {
            var note = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (note != null)
            {
                var photos = await _dbContext.Entry(note).Collection(x => x.Photos).Query().ToListAsync();
                note.Photos = photos;
            }
            return note;
        }

        public async Task CreateAsync(Note item)
        {
            await DbSet.AddAsync(item);
        }

        public async Task<Note> DeleteAsync(int? id)
        {
            var item = await DbSet.FirstOrDefaultAsync(x => x.Id == id);
            if (item != null)
            {
                DbSet.Remove(item);
            }
            return item;
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Note item)
        {
            _dbContext.Entry(item).State = EntityState.Modified;
        }
    }
}