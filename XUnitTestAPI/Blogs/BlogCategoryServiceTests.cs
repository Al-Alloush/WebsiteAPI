using API.ControllerServices.Blogs;
using AutoMapper;
using Core.Dtos.Blogs;
using Core.Interfaces.Repository;
using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using Core.Specifications;
using Core.Specifications.Blogs;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestAPI.Blogs
{
    public class BlogCategoryServiceTests
    {
        private readonly BlogCategoryService _bcs;
        private readonly Mock<IBlogCategoryRepository> _blogCategoryRepoMock = new Mock<IBlogCategoryRepository>();

        public BlogCategoryServiceTests()
        {
            _bcs = new BlogCategoryService(_blogCategoryRepoMock.Object);
        }


        [Fact]
        public async Task ReadBlogCategoriesAsync__ShouldReturnIReadOnlyListBlogCategory_WhenBlogCategoriesExists()
        {
            // Arrange
            // add list of BlogCategories
            string[] BlogCategoryNamesEn = { "Food", "Travel", "Music", "Lifestyle", "Fitness", "Sports" };
            List<BlogCategory> catsList = new List<BlogCategory>();
            var langId = "en";
            for (int i = 0; i < BlogCategoryNamesEn.Length; i++)
            {
                var lEn = new BlogCategory
                {
                    Id = i + 1,
                    Name = BlogCategoryNamesEn[i],
                    SourceCategoryId = i + 1,
                    LanguageId = langId
                };
                catsList.Add(lEn);
            }
            var name_3 = "Lifestyle";
            var id_2 = 3;
            var SourceCategoryId_5 = 6;

            // Act
            _blogCategoryRepoMock.Setup(x => x.ListAsync()).ReturnsAsync(catsList);
            IReadOnlyList<BlogCategory> category = await _bcs.ReadBlogCategoriesAsync();

            // Asserts
            Assert.Equal(category.Count, catsList.Count);
            Assert.Equal(category[3].Name, name_3);
            Assert.Equal(category[0].LanguageId, langId);
            Assert.Equal(category[2].Id, id_2);
            Assert.Equal(category[5].SourceCategoryId, SourceCategoryId_5);
            Assert.NotEqual(15, category[5].SourceCategoryId);
            Assert.NotEqual("Music", category[5].Name);
        }

        [Fact]
        public async Task ReadBlogCategoriesAsync__ShouldReturnIReadOnlyListBlogCategories_WhenBlogCategoriesExists_ByLanguageId()
        {

            // Arrange
            // add list of BlogCategories
            string[] BlogCategoryNamesEn = { "Food", "Travel", "Music", "Lifestyle", "Fitness", "Sports" };
            List<BlogCategory> catsList = new List<BlogCategory>();
            var langId_2 = "de";
            var langId = "en";

            for (int i = 0; i < BlogCategoryNamesEn.Length; i++)
            {
                var lEn = new BlogCategory
                {
                    Id = i + 1,
                    Name = BlogCategoryNamesEn[i],
                    SourceCategoryId = i + 1,
                    LanguageId = langId
                };
                catsList.Add(lEn);
            }

            // Act
            _blogCategoryRepoMock.Setup(x => x.ListByLanguageIdAsync(langId)).ReturnsAsync(catsList);
            IReadOnlyList<BlogCategory> categoiesExist = await _bcs.ReadBlogCategoriesAsync(langId);
            IReadOnlyList<BlogCategory> categoiesNotExist = await _bcs.ReadBlogCategoriesAsync(langId_2);

            // Assert
            Assert.Equal(categoiesExist.Count, catsList.Count);
            Assert.Null(categoiesNotExist); // if the Categaories not exitst in this language return null

        }

        public async Task ReadBlogCategoriesAsync__ShouldReturnOneBlogCategoryWhenBlogCategoryExists_BySourceCategoryIdAndByLanguageId()
        {
            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";
            var langIdNotExist = "ar";
            var cate = new BlogCategory
            {
                Id =1,
                Name = catName,
                SourceCategoryId = catSourcesCatId,
                LanguageId = langId
            };

            // Act
            _blogCategoryRepoMock.Setup(x => x.ModelBySourceCatIdAndLangIdAsync(catSourcesCatId, langId)).ReturnsAsync(cate);
            var categoyExist = await _bcs.ReadBlogCategoriesAsync(catSourcesCatId, langId);
            var categoyNotExist = await _bcs.ReadBlogCategoriesAsync(catSourcesCatId,langIdNotExist);

            // Assert
            Assert.Equal(categoyExist.LanguageId, langId);
            Assert.Equal(categoyExist.Name, catName);
            Assert.Null(categoyNotExist); // if the Categaory not exitst in this language return null

        }
    }
}
