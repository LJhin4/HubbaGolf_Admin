using HubbaGolfAdmin.Database.Dtos;
using HubbaGolfAdmin.Database.Models;
using HubbaGolfAdmin.Shared;

namespace HubbaGolfAdmin.Services.Interfaces
{
    public interface IWebService
    {
        #region [Quản lý Category]
        Task<List<CategoryDto>>? GetListCategoryAsync();
        Task<List<CategoryDto>>? GetListCategoryByParentIdAsync(int id);
        Task<CategoryDto?> GetCategoryByIdAsync(int categoryId);
        Task<int> SaveCategory(int id, CategoryDto categoryDto);
        Task<int> DeleteCategoryById(int id);
        #endregion
        Task<List<CategoryDto>>? GetListLocationAsync();
        Task<List<ArticleDto>> GetListArticleByCategoryIdAdminAsync(int categoryId);
        Task<List<ArticleDto>> GetListArticleByCategoryIdAsync(int categoryId);
        Task<List<ArticleDto>> GetListArticleMenuTopTierAsync();
        Task<List<CategoryDto>> GetListAllCategoryAsync();

        Task<List<ArticleDto>> SearchArticle(int id, string value, int? type);
        Task<ArticleDto?> GetArticleByIdAsync(int id);
        Task<TypeArticle> TypeOfArticle(int cateId);
        Task<Result<int>> SaveArticleAsync(int id, ArticleDto articleDto);
        Task<Result<int>> DeleteArticleAsync(int id);
        Task<Result<int>> ChangeStatusArticleAsync(int id);
        Task<List<CountryDto>> GetAllLocationAsync();
        Task<bool> UpdateRank(int id, int rank);
        Task<List<MenuHeaderDto>> GetMenuHeaderAsync();
        Task<List<ArticleDto>> GetCourseByCountryIdAsync(int id);
        Task<List<ArticleFacilityDto>> GetCourseGroupFacilityByCountryIdAsync(int countryId);
        Task<List<ArticleGroupDto>> GetCourseByCountryIdAndTypeIDAsync(int typeId, int countryId);
        #region [<Manage Banner>]
        Task<List<ArticleDto>> GetArticleHomepage(int id);
        Task<ArticleDto?> GetArticleHomepageById(int id);
        Task<int> SaveArticleHomepage(int id, ArticleDto articleDto);
        Task<int> DeleteArticleHomepageById(int id);
        Task<List<ArticleDto>> GetBanner();
        #endregion
        #region [<Manage Banner>]
        Task<PricingView> GetListPrice();
        Task<PricingDto?> GetPriceById(int id);
        Task<int> SavePrice(int id, PricingDto priceDto);
        Task<int> DeletePriceById(int id);
        Task<PricingDto?> GetPriceByArticleId(int id);
        #endregion
    }
}