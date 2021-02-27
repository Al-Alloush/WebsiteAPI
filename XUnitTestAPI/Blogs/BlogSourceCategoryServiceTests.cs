using API.ControllerServices.Blogs;
using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestAPI.Blogs
{
    public class BlogSourceCategoryServiceTests
    {
        private readonly BlogSourceCategoryService _bscs;
        private readonly Mock<IBlogSourceCategoryRepository> _blogSourceCategoryRepoMock = new Mock<IBlogSourceCategoryRepository>();
        public BlogSourceCategoryServiceTests( )
        {
            _bscs = new BlogSourceCategoryService(_blogSourceCategoryRepoMock.Object);
        }

        [Fact]
        public async Task ReadListSourceBlogCategoryNamesAsync_ReturnIReadOnlyListBlogSourceBlogCategoryNames_WhenBlogSourceBlogCategoryNamesExists()
        {
            // Arrange
            // add list of BlogCategories
            string[] BlogCategoryNamesEn = { "Food", "Travel", "Music", "Lifestyle", "Fitness", "Sports" };
            List<BlogSourceCategoryName> catsList = new List<BlogSourceCategoryName>();
            for (int i = 0; i < BlogCategoryNamesEn.Length; i++)
            {
                var newCateg = new BlogSourceCategoryName
                {
                    Id = i + 1,
                    Name = BlogCategoryNamesEn[i]
                };
                catsList.Add(newCateg);
            }

            // Act
            _blogSourceCategoryRepoMock.Setup(x => x.ListAsync()).ReturnsAsync(catsList);
            IReadOnlyList<BlogSourceCategoryName> category = await _bscs.ReadListSourceBlogCategoryNamesAsync();

            // Asserts
            Assert.Equal(category.Count, catsList.Count);
            Assert.Equal("Lifestyle", category[3].Name);
            Assert.NotEqual("Lifestyle111", category[3].Name);

        }

        [Fact]
        public async Task ReadSourceBlogCategoryNameAsync_ById_ReturnBlogSourceBlogCategoryName_WhenBlogSourceBlogCategoryNameExists()
        {
            // Arrange
            var catName = "Travel";
            var cate = new BlogSourceCategoryName
            {
                Id = 1,
                Name = catName
            };

            // Act
            _blogSourceCategoryRepoMock.Setup(x => x.ModelAsync(1)).ReturnsAsync(cate);
            var SourceCategoyExist = await _bscs.ReadSourceBlogCategoryNameAsync(1);
            var SourceCategoyNotExist = await _bscs.ReadSourceBlogCategoryNameAsync(2);

            Assert.Equal(SourceCategoyExist.Name, catName);
            Assert.Equal(1, SourceCategoyExist.Id);
            Assert.Null(SourceCategoyNotExist); // if the BlogSourceCategoryName not exitst return null

        }

        [Fact]
        public async Task ReadSourceBlogCategoryNameAsync_NyName_ReturnBlogSourceBlogCategoryName_WhenBlogSourceBlogCategoryNameExists()
        {
            // Arrange
            var catName = "Travel";
            var cate = new BlogSourceCategoryName
            {
                Id = 1,
                Name = catName
            };

            // Act
            _blogSourceCategoryRepoMock.Setup(x => x.ModelAsync(catName)).ReturnsAsync(cate);
            var SourceCategoyExist = await _bscs.ReadSourceBlogCategoryNameAsync(catName);
            var SourceCategoyNotExist = await _bscs.ReadSourceBlogCategoryNameAsync("Fitness");

            Assert.Equal(SourceCategoyExist.Name, catName);
            Assert.Equal(1, SourceCategoyExist.Id);
            Assert.Null(SourceCategoyNotExist); // if the BlogSourceCategoryName not exitst return null

        }

        [Fact]
        public async Task CreateSourceCategoryNameAsync_IfAddAsyncAndSaveChangesAsyncSuccess_ReturnTrue()
        {
            // Arrange
            var catName = "Travel";

            // Act
            _blogSourceCategoryRepoMock.Setup(x => x.AddAsync(It.IsAny<BlogSourceCategoryName>())).ReturnsAsync(true);
            _blogSourceCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var resultAddNewCat = await _bscs.CreateSourceCategoryNameAsync(catName);

            // Assert
            Assert.True(resultAddNewCat);
        }

        [Fact]
        public async Task UpdateSourceCategoryNameAsync_IfSuccess_ReturnTrue()
        {

            // Arrange
            var catName = "Music";

            var cate = new BlogSourceCategoryName
            {
                Id = 1,
                Name = catName
            };

            // Act
            _blogSourceCategoryRepoMock.Setup(x => x.UpdateAsync(cate)).ReturnsAsync(true);
            _blogSourceCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var resultUpdateCat = await _bscs.UpdateSourceCategoryNameAsync(cate, catName);

            // Assert
            Assert.True(resultUpdateCat);
        }

        [Fact]
        public async Task DeleteSourceCategoryNameAsync_IfSuccess_ReturnTrue()
        {

            // Arrange
            var catName = "Travel";
            var cate = new BlogSourceCategoryName
            {
                Id = 1,
                Name = catName
            };

            // Act
            _blogSourceCategoryRepoMock.Setup(x => x.RemoveAsync(cate)).ReturnsAsync(true);
            _blogSourceCategoryRepoMock.Setup(x => x.DeleteAllBlogCategoryList(1)).ReturnsAsync(true);
            _blogSourceCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var resultRemoveCat = await _bscs.DeleteSourceCategoryNameAsync(cate);

            // Assert
            Assert.True(resultRemoveCat);
        }
    }
}
