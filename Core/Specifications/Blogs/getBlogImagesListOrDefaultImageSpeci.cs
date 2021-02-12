using Core.Models.Uploads;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications.Blogs
{
    public class getBlogImagesListOrDefaultImageSpeci : BaseSpecification<UploadBlogImagesList>
    {

        /// <summary>
        /// to get all Blog's Images
        /// </summary>
        public getBlogImagesListOrDefaultImageSpeci(int blogId) : base(x => x.BlogId == blogId)
        {

        }


        /// <summary>
        /// to get all Blog's default Image
        /// </summary>
        /// <para>blogId:  the Blog Id that we need to get all its images</para>
        /// <para>defaultImg:  if was true return default image</para>
        public getBlogImagesListOrDefaultImageSpeci(int blogId, bool defaultImg) : base(x => x.BlogId == blogId && (!defaultImg || x.Default == defaultImg))
        {

        }
    }
}
