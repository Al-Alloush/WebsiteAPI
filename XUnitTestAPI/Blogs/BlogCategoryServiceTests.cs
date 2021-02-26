using API.ControllerServices.Blogs;
using Core.Interfaces.Repository.Blogs;
using Core.Models.Blogs;
using Moq;
using System.Collections.Generic;
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
        public async Task ReadBlogCategoriesAsync_ReturnIReadOnlyListBlogCategory_WhenBlogCategoriesExists()
        {
            // Arrange
            // add list of BlogCategories
            string[] BlogCategoryNamesEn = { "Food", "Travel", "Music", "Lifestyle", "Fitness", "Sports" };
            List<BlogCategory> catsList = new List<BlogCategory>();
            var langId = "en";
            for (int i = 0; i < BlogCategoryNamesEn.Length; i++)
            {
                var newCateg = new BlogCategory
                {
                    Id = i + 1,
                    Name = BlogCategoryNamesEn[i],
                    SourceCategoryId = i + 1,
                    LanguageId = langId
                };
                catsList.Add(newCateg);
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
        public async Task ReadBlogCategoriesAsync_BySourceCatId_ReturnIReadOnlyListBlogCategories_WhenBlogCategoriesExists()
        {

            // Arrange
            // add list of BlogCategories
            string[] BlogCategories = { "Travel", "السفر", "Reise"};
            string[] BlogCategoriesLan = { "eb", "ar", "de"};
            List<BlogCategory> catsList = new List<BlogCategory>();

            for (int i = 0; i < BlogCategories.Length; i++)
            {
                var newCateg = new BlogCategory
                {
                    Id = i + 1,
                    Name = BlogCategories[i],
                    SourceCategoryId = 2,
                    LanguageId = BlogCategoriesLan[i]
                };
                catsList.Add(newCateg);
            }

            // Act
            _blogCategoryRepoMock.Setup(x => x.ListAsync(2)).ReturnsAsync(catsList);
            IReadOnlyList<BlogCategory> categoiesExist = await _bcs.ReadBlogCategoriesAsync(2);
            IReadOnlyList<BlogCategory> categoiesNotExist = await _bcs.ReadBlogCategoriesAsync(3);

            // Assert
            Assert.Equal(categoiesExist.Count, catsList.Count);
            Assert.Null(categoiesNotExist); // if the Categaories not exitst in this language return null

        }


        [Fact]
        public async Task ReadBlogCategoriesAsync_ByLanguageId_ReturnIReadOnlyListBlogCategories_WhenBlogCategoriesExists()
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
            _blogCategoryRepoMock.Setup(x => x.ListAsync(langId)).ReturnsAsync(catsList);
            IReadOnlyList<BlogCategory> categoiesExist = await _bcs.ReadBlogCategoriesAsync(langId);
            IReadOnlyList<BlogCategory> categoiesNotExist = await _bcs.ReadBlogCategoriesAsync(langId_2);

            // Assert
            Assert.Equal(categoiesExist.Count, catsList.Count);
            Assert.Null(categoiesNotExist); // if the Categaories not exitst in this language return null

        }

        [Fact]
        public async Task ReadBlogCategoriesAsync_BySourceCatId_And_LanguageId_IReadOnlyListBlogCategories_WhenBlogCategoryExists()
        {
            // Arrange
            // add list of BlogCategories
            string[] BlogCategories = { "Travel", "Travel2", "Travel3" };
            string[] BlogCategoriesLan = { "en", "en", "en" };
            List<BlogCategory> catsList = new List<BlogCategory>();

            for (int i = 0; i < BlogCategories.Length; i++)
            {
                var newCateg = new BlogCategory
                {
                    Id = i + 1,
                    Name = BlogCategories[i],
                    SourceCategoryId = 2,
                    LanguageId = BlogCategoriesLan[i]
                };
                catsList.Add(newCateg);
            }

            // Act
            _blogCategoryRepoMock.Setup(x => x.ListAsync(2, "en")).ReturnsAsync(catsList);
            IReadOnlyList<BlogCategory> categoiesExist = await _bcs.ReadBlogCategoriesAsync(2, "en");
            IReadOnlyList<BlogCategory> categoiesNotExist = await _bcs.ReadBlogCategoriesAsync(3, "en");

            // Assert
            Assert.Equal(categoiesExist.Count, catsList.Count);
            Assert.Null(categoiesNotExist); // if the Categaories not exitst in this language return null
        }

        [Fact]
        public async Task ReadBlogCategoryAsync_ById_ReturnOneBlogCategory_WhenBlogCategoryExists()
        {
            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";
            var cate = new BlogCategory
            {
                Id = 1,
                Name = catName,
                SourceCategoryId = catSourcesCatId,
                LanguageId = langId
            };

            // Act
            _blogCategoryRepoMock.Setup(x => x.ModelAsync(1)).ReturnsAsync(cate);
            var categoyExist = await _bcs.ReadBlogCategoryByIdAsync(1);
            var categoyNotExist = await _bcs.ReadBlogCategoryByIdAsync(2);

            // Assert
            Assert.Equal(categoyExist.LanguageId, langId);
            Assert.Equal(categoyExist.Name, catName);
            Assert.Null(categoyNotExist); // if the Categaory not exitst in this language return null

        }

        [Fact]
        public async Task ReadBlogCategoryAsync_BySourceCatId_And_LanguageId_And_Name_ReturnOneBlogCategory_WhenBlogCategoryExists()
        {
            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";
            var langIdNotExist = "ar";
            var cate = new BlogCategory
            {
                Id = 1,
                Name = catName,
                SourceCategoryId = catSourcesCatId,
                LanguageId = langId
            };

            // Act
            _blogCategoryRepoMock.Setup(x => x.ModelAsync(catSourcesCatId, langId, catName)).ReturnsAsync(cate);
            var categoyExist = await _bcs.ReadBlogCategoriesAsync(catSourcesCatId, langId, catName);
            var categoyNotExist = await _bcs.ReadBlogCategoriesAsync(catSourcesCatId, langIdNotExist, "Sport");

            // Assert
            Assert.Equal(categoyExist.LanguageId, langId);
            Assert.Equal(categoyExist.Name, catName);
            Assert.Null(categoyNotExist); // if the Categaory not exitst in this language return null

        }

        [Fact]
        public async Task CreateBlogCategoryAsync_IfAddAsyncAndSaveChangesAsyncSuccess_ReturnTrue()
        {
            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";

            // Act
            _blogCategoryRepoMock.Setup(x => x.AddAsync(It.IsAny<BlogCategory>())).ReturnsAsync(true);
            _blogCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var resultAddNewCat = await _bcs.CreateBlogCategoryAsync(catSourcesCatId, langId, catName);

            // Assert
            Assert.True(resultAddNewCat);
        }

        [Fact]
        public async Task CreateBlogCategoryAsync_IfAddAsyncAndSaveChangesAsyncFailed_ReturnFalse()
        {
            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";

            // Act
            _blogCategoryRepoMock.Setup(x => x.AddAsync(It.IsAny<BlogCategory>())).ReturnsAsync(false);
            _blogCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(false);
            var resultAddNewCat = await _bcs.CreateBlogCategoryAsync(catSourcesCatId, langId, catName);

            // Assert
            Assert.False(resultAddNewCat);
        }

        [Fact]
        public async Task CreateBlogCategoryAsync_IfAddAsyncSucessAndSaveChangesAsyncFailed_ReturnFalse()
        {
            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";

            // Act
            _blogCategoryRepoMock.Setup(x => x.AddAsync(It.IsAny<BlogCategory>())).ReturnsAsync(true);
            _blogCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(false);
            var resultAddNewCat = await _bcs.CreateBlogCategoryAsync(catSourcesCatId, langId, catName);

            // Assert
            Assert.False(resultAddNewCat);
        }

        [Fact]
        public async Task CreateBlogCategoryAsync_IfAddAsyncFaildAndSaveChangesAsyncSuccess_ReturnFalse()
        {
            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";

            // Act
            _blogCategoryRepoMock.Setup(x => x.AddAsync(It.IsAny<BlogCategory>())).ReturnsAsync(false);
            _blogCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var resultAddNewCat = await _bcs.CreateBlogCategoryAsync(catSourcesCatId, langId, catName);

            // Assert
            Assert.False(resultAddNewCat);
        }


        [Fact]
        public async Task UpdateBlogCategoryAsync_IfSuccess_ReturnTrue()
        {

            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";
            var cate = new BlogCategory
            {
                Id = 1,
                Name = catName,
                SourceCategoryId = catSourcesCatId,
                LanguageId = langId
            };

            // Act
            _blogCategoryRepoMock.Setup(x => x.UpdateAsync(cate)).ReturnsAsync(true);
            _blogCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var resultUpdateCat = await _bcs.UpdateBlogCategoryAsync(cate, catSourcesCatId, langId, catName);

            // Assert
            Assert.True(resultUpdateCat);
        }

        [Fact]
        public async Task UpdateBlogCategoryAsync_IfNotSuccess_ReturnFalse()
        {

            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";
            var cate = new BlogCategory
            {
                Id = 1,
                Name = catName,
                SourceCategoryId = catSourcesCatId,
                LanguageId = langId
            };

            // Act
            _blogCategoryRepoMock.Setup(x => x.UpdateAsync(cate)).ReturnsAsync(false);
            _blogCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var resultUpdateCat = await _bcs.UpdateBlogCategoryAsync(cate, catSourcesCatId, langId, catName);

            // Assert
            Assert.False(resultUpdateCat);
        }

        [Fact]
        public async Task DeleteSourceCategoryNameAsync_IfSuccess_ReturnTrue()
        {

            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";
            var cate = new BlogCategory
            {
                Id = 1,
                Name = catName,
                SourceCategoryId = catSourcesCatId,
                LanguageId = langId
            };

            // Act
            _blogCategoryRepoMock.Setup(x => x.RemoveAsync(cate)).ReturnsAsync(true);
            _blogCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var resultRemoveCat = await _bcs.DeleteSourceCategoryNameAsync(cate);

            // Assert
            Assert.True(resultRemoveCat);
        }

        [Fact]
        public async Task DeleteSourceCategoryNameAsync_IfNotSuccess_ReturnFalse()
        {

            // Arrange
            var catName = "Travel";
            var catSourcesCatId = 5;
            var langId = "en";
            var cate = new BlogCategory
            {
                Id = 1,
                Name = catName,
                SourceCategoryId = catSourcesCatId,
                LanguageId = langId
            };

            // Act
            _blogCategoryRepoMock.Setup(x => x.RemoveAsync(cate)).ReturnsAsync(false);
            _blogCategoryRepoMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(true);
            var resultRemoveCat = await _bcs.DeleteSourceCategoryNameAsync(cate);

            // Assert
            Assert.False(resultRemoveCat);
        }
    }
}
