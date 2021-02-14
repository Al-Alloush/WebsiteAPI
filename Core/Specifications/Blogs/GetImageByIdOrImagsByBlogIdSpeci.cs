using Core.Models.Uploads;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class GetImageByIdOrImagsByBlogIdSpeci : BaseSpecification<UploadBlogImagesList>
    {
        public GetImageByIdOrImagsByBlogIdSpeci(int imageId) : base(x=>x.Id == imageId)
        {

        }

        public GetImageByIdOrImagsByBlogIdSpeci(int blogId, bool blogImageslist) : base(x => x.BlogId == blogId)
        {

        }

        public GetImageByIdOrImagsByBlogIdSpeci(int blogId, bool blogImageslist, bool defaultImage) : base(x => x.BlogId == blogId && (!defaultImage || x.Default == true))
        {

        }
    }
}
