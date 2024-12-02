using HubbaGolfAdmin.Database.Dtos;
using HubbaGolfAdmin.Database.Models;
using HubbaGolfAdmin.Shared;

namespace HubbaGolfAdmin.Services.Interfaces
{
    public interface IWebService
    {
        #region [Quản lý Category]
        Task<List<CategoryDto>>? GetListCategoryAsync();
        Task<List<CategoryDto>>? GetListCategoryAllAsync();
        Task<CategoryDto?> GetCategoryByIdAsync(int categoryId);
        Task<int> SaveCategory(int id, CategoryDto categoryDto);
        Task<int> DeleteCategoryById(int id);
        #endregion
        Task<List<CategoryDto>>? GetListLocationAsync();
        Task<List<ArticleDto>> GetListArticleByCategoryIdAdminAsync(int categoryId);
        Task<List<ArticleDto>> GetListArticleByCategoryIdAsync(int categoryId);
        Task<List<ArticleDto>> GetListArticleMenuTopTierAsync();
        Task<List<CategoryDto>> GetListAllCategoryAsync();

        Task<List<ArticleDto>> SearchArticle(int id, string value);
        Task<ArticleDto?> GetArticleByIdAsync(int id);
        Task<TypeArticle> TypeOfArticle(int cateId);
        Task<Result<int>> SaveArticleAsync(int id, ArticleDto articleDto);
        Task<Result<int>> DeleteArticleAsync(int id);
        Task<Result<int>> ChangeStatusArticleAsync(int id);
        Task<List<CountryDto>> GetAllLocationAsync();
        Task<bool> UpdateRank(int id, int rank);
        Task<List<MenuHeaderDto>> GetMenuHeaderAsync();
        Task<List<ArticleDto>> GetCourseByCountryIdAsync(int id);
        #region [<Manage Banner>]
        Task<List<ArticleDto>> GetArticleHomepage(int id);
        Task<ArticleDto?> GetArticleHomepageById(int id);
        Task<int> SaveArticleHomepage(int id, ArticleDto articleDto);
        Task<int> DeleteArticleHomepageById(int id);
        Task<List<ArticleDto>> GetBanner();
        #endregion
    }
}