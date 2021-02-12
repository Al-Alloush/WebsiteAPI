using Core.Interfaces.Repository;
using Core.Specifications;
using Infrastructure.Data;
using Infrastructure.SpecEvaluators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<T> ModelDetailsAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationsEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        public async Task<bool> AddAsync(T model)
        {
            EntityEntry<T> result = await _context.Set<T>().AddAsync(model);
            if (result.State.ToString() == "Added")
                return true;
            else
                return false;

        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveAsync(T model)
        {
            EntityEntry<T> result =  _context.Remove(model) ;
            if (result.State.ToString() == "added")
                return true;
            else
                return false;
        }

        // return file Name and File Directory in server if add success, else return false
        public async Task<string>  UploadFileAsync(IFormFile file,string fileDirPath)
        {
            DateTimeOffset dto = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

            if (file != null && file.Length > 0)
            {

                // this fileDirPath is from webHostEnvironment.WebRootPath is under wwwroot folder
                string filePath = Path.Combine(fileDirPath);
                // check if this directory exists, if not exists, create it
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                // create new uniqu file name sing UNIX Timestemp, cast to int before convert to string to delete milliseconds from UMIX time
                string unixTimestamp = ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                string fileName = unixTimestamp.ToString() + "_" + file.FileName;
                //string fileName = (Guid.NewGuid().ToString().Substring(0, 8)) + "_" + file.FileName;
                filePath = Path.Combine(filePath, fileName);

                using (FileStream fileStream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }
                return fileName;
            }
            return null;
        }

        public async Task<bool> DeleteFilesFromServerAsync(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T model)
        {
            try
            {
                _context.Set<T>().Attach(model);
                _context.Entry(model).State = EntityState.Modified;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
