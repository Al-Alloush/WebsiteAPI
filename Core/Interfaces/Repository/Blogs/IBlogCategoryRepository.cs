using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository.Blogs
{
    public interface IBlogCategoryRepository
    {
        Task<IReadOnlyList<BlogCategory>> ListAsync();

        Task<IReadOnlyList<BlogCategory>> ListByLanguageIdAsync(string langId);

        Task<BlogCategory> ModelBySourceCatIdAndLangIdAsync(int sourceCategoryNameId, string langId);

        Task<BlogCategory> ModelBySourceCatIdAndLangIdAsync(int sourceCategoryNameId, string langId, string name);

        Task<bool> AddAsync(BlogCategory category);

        Task<bool> SaveChangesAsync();
    }
}
