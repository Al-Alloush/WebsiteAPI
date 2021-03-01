using Core.Models.Uploads;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repository.Blogs
{
    public interface IUploadBlogImagesListRepository : IBaseRepository<UploadBlogImagesList>
    {
        Task<IReadOnlyList<UploadBlogImagesList>> GetBlogImagesListByBlogId(int blogId);
    }
}
