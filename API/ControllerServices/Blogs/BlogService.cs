using AutoMapper;
using Core.Dtos.Blogs;
using Core.Helppers;
using Core.Interfaces.Repository;
using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using Core.Models.Identity;
using Core.Models.Uploads;
using Core.Specifications.Blogs;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ControllerServices.Blogs
{

    public class BlogService
    {
        private string BLOG_IMAGE_DIRECTORY;

        private readonly UserManager<AppUser> _userManager;
        private readonly IGenericRepository<Blog> _blogRepo;
        private readonly IGenericRepository<UploadBlogImagesList> _uploadImageRepo;
        private readonly IBlogCommentRepository _commentRepo;
        private readonly IBlogLikeRepository _likeRepo;
        private readonly IGenericRepository<BlogSourceCategoryName> _blogSourceCategoryRepo;
        private readonly IGenericRepository<BlogCategoryList> _blogCategoryListRepo;
        private readonly IGenericRepository<BlogCategory> _blogCategoryRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _config;

        public BlogService(UserManager<AppUser> userManager,
                                IGenericRepository<Blog> blogRepo,
                                IGenericRepository<UploadBlogImagesList> uploadImageRepo,
                                IBlogCommentRepository commentRepo,
                                IBlogLikeRepository likeRepo,
                                IGenericRepository<BlogSourceCategoryName> blogSourceCategoryRepo,
                                IGenericRepository<BlogCategoryList> blogCategoryListRepo,
                                IGenericRepository<BlogCategory> blogCategoryRepo,
                                IMapper mapper,
                                AppDbContext context,
                                IWebHostEnvironment webHostEnvironment,
                                IConfiguration config 
                                )
        {
            _userManager = userManager;
            _blogRepo = blogRepo;
            _uploadImageRepo = uploadImageRepo;
            _commentRepo = commentRepo;
            _likeRepo = likeRepo;
            _blogSourceCategoryRepo = blogSourceCategoryRepo;
            _blogCategoryListRepo = blogCategoryListRepo;
            _blogCategoryRepo = blogCategoryRepo;
            _mapper = mapper;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _config = config;

            BLOG_IMAGE_DIRECTORY = _config["UploadsDir:Blogs:Images"];

        }


        public async Task<Pagination<BlogCardDto>> GetAllBlogAsync(SpecificParameters par, AppUser currentUser)
        {

            var userLangs = await _context.UserSelectedLanguages.Where(l => l.UserId == currentUser.Id).Select(l => l.LanguageId).ToListAsync();

            // this to get blogs by CategoryId, one to many relationships between Blog and BlogCategoriesList Tables. blog has one or more categories. FOR TEST
            //var blogs = await _context.Blog.Include(u=>u.UploadBlogImagesList).Where(b => b.BlogCategoriesList.OrderByDescending(c => c.Id).First().BlogCategoryId == par.CategoryId &&
            //                                            userLangs.Contains(b.LanguageId)) .ToListAsync();
            //var blogCount = blogs.Count();
            //**********************************

            // query all blogs with pagination tools
            IReadOnlyList<Blog> blogs = await _blogRepo.ListAsync(new GetBlogsListPaginationOrBlogDetailsSpeci(par, userLangs));
            // to get the count, it's the same criteria if spec
            int blogsCount = await _blogRepo.CountAsync(new GetBlogsListPaginationOrBlogDetailsSpeci(par, userLangs, emptyConstructor: true));

            // mapping from Blog -> BlogCardDto
            IReadOnlyList<BlogCardDto> _blogs = _mapper.Map<IReadOnlyList<Blog>, IReadOnlyList<BlogCardDto>>(blogs);

            // looping to add some data
            foreach (var blog in _blogs)
            {
                // add default image in BlogCard
                UploadBlogImagesList imagesList = await _uploadImageRepo.ModelDetailsAsync(new GetImageByIdOrImagsByBlogIdSpeci(blog.Id, true, true));
                if (imagesList != null)
                    blog.DefaultBlogImage = imagesList.Path;

                // count Blog's comments
                blog.CommentsCount = await _commentRepo.CountAsync(blog.Id);
                //count Blog's Like
                blog.LikesCount = await _likeRepo.CountAsync(blog.Id, true);
                //count Blog's Dislike
                blog.DislikesCount = await _likeRepo.CountAsync(blog.Id, false);
            }

            // return using pagination tools
            return new Pagination<BlogCardDto>(par.PageIndex, par.PageSize, blogsCount, _blogs);
        }


        public async Task<BlogDto> GetBlogDetailsAsync(int id)
        {

            // get Blog Details
            var blog = await _blogRepo.ModelDetailsAsync(new GetBlogsListPaginationOrBlogDetailsSpeci(id));
            if (blog == null)
                return null;

            var _blog = _mapper.Map<Blog, BlogDto>(blog);

            // add all comments for this blog & count Blog's comments
            var comments = await _commentRepo.ListAsync(_blog.Id);
            _blog.BlogComments = _mapper.Map<IReadOnlyList<BlogComment>, IReadOnlyList<BlogCommentDto>>(comments);
            _blog.CommentsCount = _blog.BlogComments.Count();

            //count the like and Dislike
            _blog.LikesCount = await _likeRepo.CountAsync(blog.Id, true);
            _blog.DislikesCount = await _likeRepo.CountAsync(blog.Id, false);

            // Get all the categories for this blog
            IReadOnlyList<BlogCategoryList> categories = await _blogCategoryListRepo.ListAsync(new GetBlogCategoriesListSpeci(_blog.Id));
            _blog.BlogCategoriesList = _mapper.Map<IReadOnlyList<BlogCategoryList>, IReadOnlyList<BlogCategoryListDto>>(categories);
            //  and get their names in the blog language
            foreach (var cat in _blog.BlogCategoriesList)
            {
                // to check if this category exist in BlogCategories table
                var catObject = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoryNameSpeci(cat.BlogCategoryId, blog.LanguageId));
                if (catObject != null)
                    cat.Name = catObject.Name;

                // else the defalult value is null
            }
                

            // get Blog's images;
            IReadOnlyList<UploadBlogImagesList> imagesList = await _uploadImageRepo.ListAsync(new GetImageByIdOrImagsByBlogIdSpeci(_blog.Id, true));
            _blog.BlogImagesList = _mapper.Map<IReadOnlyList<UploadBlogImagesList>, IReadOnlyList<BlogImageDto>>(imagesList);


            return _blog;
        }


        public async Task<string> CreateBlogAsync(BlogCreateDto blog, AppUser currentUser)
        {

            //check if all categories are existing, 
            var _categoriesIds = await CheckExistingCategories(blog.Categories);
            if (_categoriesIds == null)
            {
                throw new Exception("some categories not exist");
            }

            // mapping form BlogCreateDto -> Blog to add it in databse
            Blog newBlog = _mapper.Map<BlogCreateDto, Blog>(blog);
            newBlog.UserId = currentUser.Id;
            newBlog.AddedDateTime = DateTime.Now;
            newBlog.UserModifiedId = currentUser.Id;
            newBlog.ModifiedDate = DateTime.Now;

            if (await _blogRepo.AddAsync(newBlog))
            {
                if (await _blogRepo.SaveChangesAsync())
                {
                    // Add categories for this Blog
                    foreach (var id in _categoriesIds)
                    {
                        // to check if this category exsit in BlogCategories table or not 
                        var blogCatId = await _blogCategoryRepo.ModelDetailsAsync(new GetBlogCategoryNameSpeci(id, newBlog.LanguageId));
                        if(blogCatId == null)
                            throw new Exception($"this category not exist in BlogCategories table");

                        // add new Category
                        var newCat = new BlogCategoryList { BlogId = newBlog.Id, BlogCategoryId = id };
                        if (!await _blogCategoryListRepo.AddAsync(newCat))
                        {
                            // if not success create of any Blog's Categories, delete new Blog
                            await _blogRepo.RemoveAsync(newBlog);
                            await _blogRepo.SaveChangesAsync();
                            throw new Exception("Something Wrong! with Creating Categories");

                        }
                    }
                    // ************************************* add blog images
                    // the first image is Default
                    var updated = await UploadFilesAndUpdateTableAsync(blog.Files, currentUser.Id, newBlog, BLOG_IMAGE_DIRECTORY);
                    if (!updated)
                        throw new Exception("Something Wrong! with Upload images");

                    return "Add Blog successfully";
                }
            }
            throw new Exception("Something Wrong! with add new Blog");

        }


        public async Task<string> UpdateBlogAsync(BlogUpdateDto blog, AppUser currentUser)
        {

            // check id Blog existing
            var _blog = await _blogRepo.ModelDetailsAsync(new GetBlogsListPaginationOrBlogDetailsSpeci(blog.Id));
            if (_blog == null) return null;

            var _categoriesIds = await CheckExistingCategories(blog.Categories);
            if (_categoriesIds == null)
                return null;

            // check if curent user has a Permission
            var permission = await PermissionsManagementAsync(currentUser, _blog);
            if (!permission)
                return null;

            try
            {
                // maping _blog(BlogUpdateDto) class to a Blog class, 
                _mapper.Map(blog, _blog);
                // after update the blog set new user id who updated this blog, and update date
                _blog.UserModifiedId = currentUser.Id;
                _blog.ModifiedDate = DateTime.Now;

                if (await _blogRepo.UpdateAsync(_blog))
                {
                    // get old categoties list
                    IReadOnlyList<BlogCategoryList> oldCategories = await _blogCategoryListRepo.ListAsync(new GetBlogCategoriesListSpeci(_blog.Id));

                    // delete old Categoties
                    foreach (var oldCat in oldCategories)
                        await _blogCategoryListRepo.RemoveAsync(oldCat);

                    // Add new Categories
                    foreach (var id in _categoriesIds)
                    {
                        var newCat = new BlogCategoryList { BlogId = _blog.Id, BlogCategoryId = id };
                        await _blogCategoryListRepo.AddAsync(newCat);
                    }
                    // update
                    await _blogRepo.SaveChangesAsync();
                    return $"Update Blog Successfully";
                }
                throw new Exception($"Somthing wrong!");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<string> AddNewBlogImages(BlogAddImageDto blogImage, AppUser currentUser)
        {

            // check id Blog existing
            // get Blog Details
            var blog = await _blogRepo.ModelDetailsAsync(new GetBlogsListPaginationOrBlogDetailsSpeci(blogImage.BlogId));
            if (blog == null)
                return null;

            // check if curent user has a Permission
            var permission = await PermissionsManagementAsync(currentUser, blog);
            if (!permission)
                throw new Exception($"current User doesn't has a permation to update this blog");

            var updated = await UploadFilesAndUpdateTableAsync(blogImage.Files, currentUser.Id, blog, BLOG_IMAGE_DIRECTORY);
            if (!updated)
                throw new Exception($"Something Wrong! with Upload images");

            return "Add images successfully";
        }


        public async Task<string> DeleteBlogImageAsync( int imageId, AppUser currentUser)
        {
            // check if image existing 
            var image = await _uploadImageRepo.ModelDetailsAsync(new GetImageByIdOrImagsByBlogIdSpeci(imageId));
            if (image == null)
                return null;

            // check id Blog existing to get its Id
            // get Blog Details
            var blog = await _blogRepo.ModelDetailsAsync(new GetBlogsListPaginationOrBlogDetailsSpeci(image.BlogId));
            if (blog == null)
                return null;

            // check if curent user has a Permission
            var permission = await PermissionsManagementAsync(currentUser, blog);
            if (!permission)
                throw new Exception($"current User doesn't has a permation to delete this blog");

            if (await _uploadImageRepo.RemoveAsync(image))
                if (await _uploadImageRepo.SaveChangesAsync())
                {
                    System.IO.File.Delete(_webHostEnvironment.WebRootPath + image.Path);
                    return "Successfully Delete Image"; // Successfully Delete Image
                }

            throw new Exception("somthing wrong!");

        }

        public async Task<ActionResult<string>> DeleteBlogAsync( int blogId, AppUser currentUser)
        {

            // check id Blog existing
            var blog = await _blogRepo.ModelDetailsAsync(new GetBlogsListPaginationOrBlogDetailsSpeci(blogId));
            if (blog == null) return null;

            // check if curent user has a Permission
            var permission = await PermissionsManagementAsync(currentUser, blog);
            if (!permission)
                throw new Exception($"current User doesn't has a permation to delete this blog");

            // get the Images list to delete them from server
            IReadOnlyList<UploadBlogImagesList> imagesList = await _uploadImageRepo.ListAsync(new GetImageByIdOrImagsByBlogIdSpeci(blog.Id, true));

            // Delete the Blog, because this blog is relation with other tables with Cascade, we don't need to deleten all references Models form other tables, but need just to delete images from server
            if (await _blogRepo.RemoveAsync(blog))
                if (await _blogRepo.SaveChangesAsync())
                    foreach (var image in imagesList)
                    {
                        System.IO.File.Delete(_webHostEnvironment.WebRootPath + image.Path);
                        return "Delete Blog successfully";
                    }

            throw new Exception("somthing wrong!");
        }


        //***************************


        /// <summary>
        /// upload files to server
        /// </summary>
        /// <returns>
        /// If ClaimsPrincipal httpContextUser = HttpContext.User; retrun an User Object 
        /// else retrun null</returns> 
        private async Task<bool> UploadFilesAndUpdateTableAsync(IReadOnlyList<IFormFile> files, string userId, Blog blog, string uploadsDir)
        {
            bool defaultImage = false;

            // Check if this blog contains default image
            IReadOnlyList<UploadBlogImagesList> imagesList = await _uploadImageRepo.ListAsync(new getBlogImagesListOrDefaultImageSpeci(blog.Id, true));
            if (imagesList.Count() <= 0)
                defaultImage = true;

            if (files != null)
            {
                if (files.Count > 0 && files.Count < 6)
                {
                    List<string> uploadedFilesPath = new List<string>();
                    foreach (var file in files)
                    {
                        if (file != null && file.Length > 0)
                        {
                            // upload the image and return the new file name
                            var fileName = await _uploadImageRepo.UploadFileAsync(file, _webHostEnvironment.WebRootPath + uploadsDir);
                            if (!string.IsNullOrEmpty(fileName))
                            {
                                var blogImage = new UploadBlogImagesList
                                {
                                    Name = fileName,
                                    Path = uploadsDir + fileName,
                                    BlogId = blog.Id,
                                    Default = defaultImage,
                                    UserId = userId,
                                    UploadTypeId = 3 // Blog type
                                };
                                // first image is default
                                defaultImage = false;

                                // to delete these files from the server if not successful to add the Blog
                                uploadedFilesPath.Add(uploadsDir + fileName);

                                // add BlogImagesList Model
                                if (!await _uploadImageRepo.AddAsync(blogImage))
                                {
                                    // if not success add of any Blog's images, delete the new Blog with all uploadded images in server
                                    await _blogRepo.RemoveAsync(blog);
                                    await _blogRepo.SaveChangesAsync();
                                    foreach (var filePath in uploadedFilesPath)
                                        await _uploadImageRepo.DeleteFilesFromServerAsync(_webHostEnvironment.WebRootPath + filePath);

                                    return false;
                                }
                            }
                            else
                                return false;
                        }
                    }
                }
                else
                    return false;
            }
            // update database after add all images and Categories
            await _blogCategoryListRepo.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Powers are based on priority of <paramref name="currentUser"/> , SuperAdmin can do everything, Admin can create update all Editors Actions
        /// </summary>
        /// <returns>
        /// if curent user is the same blog's Creater: Return true. /
        /// if current user is SuperAdmin : Return true. /
        /// if current user is SuperAdmin and Blog Creater is any of (Editor, Admin or SuperAdmin): Return true. /
        /// if current user is Admin and Blog Creater is any of (Admin or SuperAdmin): Return false. /
        /// if current user is Admin and Blog Creater is Editor: Return true.
        /// </returns>

        private async Task<bool> PermissionsManagementAsync(AppUser currentUser, Blog blog)
        {
            var superAdmin = await _userManager.GetUsersInRoleAsync("SuperAdmin");
            var superAdminId = superAdmin.Select(u => u.Id).FirstOrDefault();
            // if current user is SuperAdmin update this blog
            if (currentUser.Id != superAdminId)
            {
                // if curent user is the blog's creator or the last user has this blog modified
                if (blog.UserModifiedId != currentUser.Id)
                {
                    // get curent user Role
                    var currentUserRole = (await _userManager.GetRolesAsync(currentUser)).FirstOrDefault();

                    // get Blog creater's Role
                    var blogCreater = await _userManager.FindByIdAsync(blog.UserModifiedId);
                    var blogCreaterRole = (await _userManager.GetRolesAsync(blogCreater)).FirstOrDefault();

                    if ((currentUserRole == "Admin" && blogCreaterRole == "Admin") ||
                        (currentUserRole == "Admin" && blogCreaterRole == "SuperAdmin") ||
                        (currentUserRole == "Editor" && blogCreaterRole == "Editor") ||
                        (currentUserRole == "Editor" && blogCreaterRole == "Admin") ||
                        (currentUserRole == "Editor" && blogCreaterRole == "SuperAdmin") ||
                        currentUserRole == "Visitor"
                        )
                        return false;
                }
            }
            return true;
        }



        // to check if all categories are existing in database
        private async Task<List<int>> CheckExistingCategories(string categories)
        {
            //check id category existing, example: Convert string "[1, 2, 3]" to int list
            List<int> _categoriesIds = categories.Trim('[', ']').Split(',').Select(int.Parse).ToList();
            foreach (var id in _categoriesIds)
            {
                // check if this Category existing in Category Source
                var categorySource = await _blogSourceCategoryRepo.ModelDetailsAsync(new GetBlogSourceCategories(id));
                if (categorySource == null)
                    return null;
            }
            return _categoriesIds;
        }
    }
}
