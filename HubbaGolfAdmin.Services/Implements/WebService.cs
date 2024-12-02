using AutoMapper;
using Microsoft.EntityFrameworkCore;
using HubbaGolfAdmin.Database;
using HubbaGolfAdmin.Database.Dtos;
using HubbaGolfAdmin.Database.Models;
using HubbaGolfAdmin.Services.Interfaces;
using HubbaGolfAdmin.Shared;

namespace HubbaGolfAdmin.Services.Implements
{
    public class WebService : IWebService
    {
        private readonly EComDbContextExtCustom _DbContext;
        private readonly IMapper _Mapper;

        public WebService(EComDbContextExtCustom dbContext, IMapper mapper)
        {
            _DbContext = dbContext;
            _Mapper = mapper;
        }

        #region [Manage Category]
        public async Task<List<CategoryDto>> GetListCategoryAsync()
        {
            var zCategory = await _DbContext.Categories.Where(r => r.RecordStatus != 99 && r.Type == TypeMenu.News).OrderBy(r => r.Sort).ToListAsync();
            return _Mapper.Map<List<CategoryDto>>(zCategory);
        }
        public async Task<List<CategoryDto>> GetListLocationAsync()
        {
            var zCategory = await _DbContext.Categories.Where(r => r.RecordStatus != 99 && r.Type == TypeMenu.Location).OrderBy(r => r.Sort).ToListAsync();
            return _Mapper.Map<List<CategoryDto>>(zCategory);
        }
        public async Task<List<CategoryDto>> GetListAllCategoryAsync()
        {
            var zCategory = await _DbContext.Categories.Where(r => r.RecordStatus != 99).OrderBy(r => r.Sort).ToListAsync();
            return _Mapper.Map<List<CategoryDto>>(zCategory);
        }
        public async Task<List<CategoryDto>> GetListCategoryAllAsync()
        {
            var zCategory = await _DbContext.Categories.Where(r => r.RecordStatus != 99).OrderBy(r => r.Sort).ToListAsync();
            return _Mapper.Map<List<CategoryDto>>(zCategory);
        }
        public async Task<CategoryDto?> GetCategoryByIdAsync(int id)
        {
            var zCategory = await _DbContext.Categories.FindAsync(id);
            return _Mapper.Map(zCategory, new CategoryDto());
        }
        public async Task<int> SaveCategory(int id, CategoryDto categoryDto)
        {
            var zCategory = await _DbContext.Categories.FindAsync(id);
            if (zCategory != null)
            {
                zCategory = _Mapper.Map(categoryDto, zCategory);
                _DbContext.Update(zCategory);
            }
            else
            {
                zCategory = _Mapper.Map(categoryDto, new Category());
                _DbContext.Add(zCategory);
            }
            return await _DbContext.SaveChangesAsync();
        }
        public async Task<int> DeleteCategoryById(int id)
        {
            var zCategory = await _DbContext.Categories.FindAsync(id);
            if (zCategory != null)
            {
                _DbContext.Remove(zCategory);
                return await _DbContext.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<ArticleDto>> GetListArticleByCategoryIdAdminAsync(int categoryId)
        {
            var zListArticle = await _DbContext.Articles
                                        .Where(r => r.RecordStatus != 99 && r.CategoryId == categoryId)
                                        .OrderBy(r => r.Rank)
                                        .ThenByDescending(r => r.CreatedOn)
                                        .ToListAsync();
            return _Mapper.Map<List<ArticleDto>>(zListArticle);
        }
        public async Task<List<ArticleDto>> GetListArticleByCategoryIdAsync(int categoryId)
        {
            var zListArticle = await _DbContext.Articles
                                        .Where(r => r.RecordStatus != 99 && r.CategoryId == categoryId && r.IsParent != true)
                                        .OrderBy(r => r.Rank)
                                        .ThenByDescending(r => r.CreatedOn)
                                        .ToListAsync();
            return _Mapper.Map<List<ArticleDto>>(zListArticle);
        }
        public async Task<List<ArticleDto>> GetListArticleMenuTopTierAsync()
        {
            var zList = await (from a in _DbContext.Articles
                               join c in _DbContext.Categories on a.CategoryId equals c.Id
                               join c0 in _DbContext.Categories on c.Parent equals c0.Id
                               where a.RecordStatus != 99 && c.RecordStatus != 99 && c0.RecordStatus != 99 && c0.Id == 24 && a.IsParent == true
                               select a)
                              .OrderByDescending(a => a.CreatedOn)
                              .ToListAsync();
            return _Mapper.Map<List<ArticleDto>>(zList);
        }
        #endregion

        public async Task<List<ArticleDto>> SearchArticle(int id, string value)
        {
            var zArticle = new List<Article>();
            if (!string.IsNullOrEmpty(value))
            {
                zArticle = await _DbContext.Articles.Where(s => s.CategoryId == id &&
                                                                s.RecordStatus != 99 &&
                                                                (s.Title.ToUpper().Contains(value.ToUpper()) ||
                                                                s.Summary.ToUpper().Contains(value.ToUpper())))
                                                    .OrderBy(r => r.Rank)
                                                    .ThenByDescending(s => s.CreatedOn)
                                                    .ToListAsync();
            }
            else
            {
                zArticle = await _DbContext.Articles.Where(s => s.CategoryId == id && s.RecordStatus != 99)
                                                    .OrderBy(r => r.Rank)
                                                    .ThenByDescending(s => s.CreatedOn)
                                                    .ToListAsync();
            }

            var zList = _Mapper.Map<List<ArticleDto>>(zArticle);
            return zList;
        }
        public async Task<ArticleDto?> GetArticleByIdAsync(int id)
        {
            if (id != 0)
            {
                var zArticle = await _DbContext.Articles.FindAsync(id);
                var zArticleDto = _Mapper.Map<ArticleDto>(zArticle);
                //var zArticleDocument = await _DbContext.Documents.Where(r => r.ArticleId == id && r.RecordStatus != 99).ToListAsync();
                //var zDocDto = new List<FileDto>();
                //foreach (var item in zArticleDocument)
                //{
                //    zDocDto.Add(new FileDto()
                //    {
                //        Id = item.Id,
                //        ArticleId = item.ArticleId,
                //        DisplayName = item.NameVn,
                //        Type = item.Type,
                //        Description = item.Description,
                //        Link = item.Link,
                //        CreatedOn = item.CreatedOn,
                //    });
                //}
                //zArticleDto.Documents = zDocDto;
                return zArticleDto;
            }

            return null;
        }
        public async Task<TypeArticle> TypeOfArticle(int cateId)
        {
            var zHasCate = await _DbContext.Categories
                .Where(s => s.Parent == 2 && s.RecordStatus != 99 && s.Id == cateId)
                .SingleOrDefaultAsync();
            if (zHasCate != null)
                return TypeArticle.IsCarrier;
            zHasCate = await _DbContext.Categories
                .Where(s => s.Parent == 3 && s.RecordStatus != 99 && s.Id == cateId)
                .SingleOrDefaultAsync();
            if (zHasCate != null)
                return TypeArticle.IsProduct;

            return TypeArticle.IsArticle;
        }
        public async Task<Result<int>> SaveArticleAsync(int id, ArticleDto articleDto)
        {
            using var transaction = await _DbContext.Database.BeginTransactionAsync();
            try
            {
                Article? zArticle;

                if (id != 0)
                {
                    zArticle = await _DbContext.Articles.FindAsync(id);
                    if (zArticle != null)
                    {
                        articleDto.NullToEmpty();
                        zArticle = _Mapper.Map(articleDto, zArticle);
                        _DbContext.Update(zArticle);
                    }
                    else
                    {
                        // Handle the case where the article with the given ID is not found
                        throw new Exception("Article with ID " + id + " not found.");
                    }
                }
                else
                {
                    zArticle = _Mapper.Map(articleDto, new Article());
                    _DbContext.Add(zArticle);
                }


                //await _DbContext.UsersSysLogs.AddAsync(new UsersSysLog { InputInfo = articleDto.ToJson(), Product = "ARTICLE", RequestId = zArticle.Id.ToString() });

                await _DbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                // Return the ID of the inserted or updated article
                return new Result<int>
                {
                    Success = true,
                    Data = zArticle.Id
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new Result<int>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<Result<int>> DeleteArticleAsync(int id)
        {
            var zArticle = await _DbContext.Articles.FindAsync(id);
            if (zArticle != null)
            {
                _DbContext.Remove(zArticle);
                await _DbContext.SaveChangesAsync();
                return new Result<int>
                {
                    Success = true,
                    Message = "Deleted successfully!"
                };
            }
            else
            {
                return new Result<int>
                {
                    Success = false,
                    Message = "An error occurred!"
                };
            }
        }
        public async Task<Result<int>> ChangeStatusArticleAsync(int id)
        {
            var zArticle = await _DbContext.Articles.FindAsync(id);

            if (zArticle != null)
            {
                var status = zArticle.Status;
                if (status == 1)
                {
                    zArticle.Status = 0;
                }
                else
                {
                    zArticle.Status = 1;
                }
                _DbContext.Update(zArticle);
                await _DbContext.SaveChangesAsync();
                return new Result<int>
                {
                    Success = true,
                    Message = "Changed successfully!"
                };
            }
            else
            {
                return new Result<int>
                {
                    Success = false,
                    Message = "An error occurred while converting Status!"
                };
            }
        }

        public async Task<List<CountryDto>> GetAllLocationAsync()
        {
            var zResult = new List<CountryDto>();
            var zCountry = await _DbContext.Categories.Where(r => r.RecordStatus != 99 && r.Type == TypeMenu.Location && r.Parent == 0).OrderBy(r => r.Sort).ToListAsync();
            foreach(var p in zCountry)
            {
                var zItem = new CountryDto();
                zItem.CountryId = p.Id;
                zItem.CountryName = p.Name;
                
                var zProvince = await _DbContext.Categories.Where(r => r.RecordStatus != 99 && r.Type == TypeMenu.Location && r.Parent == p.Id).OrderBy(r => r.Sort).ToListAsync();
                zItem.Provinces = _Mapper.Map<List<CategoryDto>>(zProvince);

                zResult.Add(zItem);
            }    
            return zResult;
        }
        public async Task<bool> UpdateRank(int id, int rank)
        {
            var zArticle = await _DbContext.Articles.FindAsync(id);
            if (zArticle != null)
            {
                zArticle.Rank = rank;
                _DbContext.Update(zArticle);
                await _DbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                // Handle the case where the article with the given ID is not found
                throw new Exception("Article with ID " + id + " not found.");
            }
        }
        public async Task<List<MenuHeaderDto>> GetMenuHeaderAsync()
        {
            var result = new List<MenuHeaderDto>();

            var itemHome = new MenuHeaderDto();
            itemHome.title = "Home";
            itemHome.link = "/";
            result.Add(itemHome);

            var itemLocation = new MenuHeaderDto();
            itemLocation.title = "Locations";
            itemLocation.link = "#";
            var subLocation = new List<ItemCountry>();
            var zCountry = await _DbContext.Categories.Where(r => r.RecordStatus != 99 && r.Type == TypeMenu.Location && r.Parent == 0).OrderBy(r => r.Sort).ToListAsync();
            foreach (var p in zCountry)
            {
                var zItem = new ItemCountry();
                zItem.id = p.Id;
                zItem.title = p.Name;
                var lstProvince = new List<ItemProcince>();

                var zProvince = await _DbContext.Categories.Where(r => r.RecordStatus != 99 && r.Type == TypeMenu.Location && r.Parent == p.Id).OrderBy(r => r.Sort).ToListAsync();
                foreach(var v in zProvince)
                {
                    var itemProvince = new ItemProcince();
                    itemProvince.id = v.Id;
                    itemProvince.name = v.Name;
                    lstProvince.Add(itemProvince);
                }
                zItem.items = lstProvince;
                subLocation.Add(zItem);
            }
            itemLocation.subMenu = subLocation;

            result.Add(itemLocation);

            var itemCourses = new MenuHeaderDto();
            itemCourses.title = "Courses";
            itemCourses.link = "#";

            var subCourse = new List<ItemCountry>();
            var item1 = new ItemCountry();
            item1.title = "Outdoors";
            var lstitem1 = new List<ItemProcince>();
            var iit1 = new ItemProcince();
            iit1.id = 0;
            iit1.name = "Singapore";
            lstitem1.Add(iit1);
            var iit2 = new ItemProcince();
            iit2.id = 0;
            iit2.name = "Malaysia";
            lstitem1.Add(iit2);
            var iit3 = new ItemProcince();
            iit3.id = 0;
            iit3.name = "Indonesia";
            lstitem1.Add(iit3);
            item1.items = lstitem1;
            subCourse.Add(item1);

            var item2 = new ItemCountry();
            item2.title = "Simulator";
            var lstitem2 = new List<ItemProcince>();
            var iit21 = new ItemProcince();
            iit21.id = 0;
            iit21.name = "Johor";
            lstitem2.Add(iit21);
            var iit22 = new ItemProcince();
            iit22.id = 0;
            iit22.name = "Kota Kinabalu";
            lstitem2.Add(iit22);
            var iit23 = new ItemProcince();
            iit23.id = 0;
            iit23.name = "Kuala Lumpur";
            lstitem2.Add(iit23);
            var iit24 = new ItemProcince();
            iit24.id = 0;
            iit24.name = "Kuching";
            lstitem2.Add(iit24);

            item2.items = lstitem2;
            subCourse.Add(item2);

            result.Add(itemCourses);

            var itemShop = new MenuHeaderDto();
            itemShop.title = "Shop";
            itemShop.link = "#shop";
            result.Add(itemShop);

            var itemBook = new MenuHeaderDto();
            itemBook.title = "Book";
            itemBook.link = "/#form";
            result.Add(itemBook);

            var itemContract = new MenuHeaderDto();
            itemContract.title = "Contact";
            itemContract.link = "/contact";
            result.Add(itemContract);

            var itemBlog = new MenuHeaderDto();
            itemBlog.title = "Blog";
            itemBlog.link = "/news";
            result.Add(itemBlog);

            return result;
        }
        public async Task<List<ArticleDto>> GetCourseByCountryIdAsync(int id)
        {
            var zList = await (from a in _DbContext.Articles
                               join c in _DbContext.Categories on a.CategoryId equals c.Id
                               join c0 in _DbContext.Categories on c.Parent equals c0.Id
                               where a.RecordStatus != 99 && c.RecordStatus != 99 && c0.RecordStatus != 99 && c0.Id == id
                               select a)
                  .OrderByDescending(a => a.CreatedOn)
                  .ToListAsync();
            return _Mapper.Map<List<ArticleDto>>(zList);
        }

        #region [Manage Banner]
        public async Task<List<ArticleDto>> GetArticleHomepage(int id)
        {
            var zArticles = await _DbContext.Articles
                                        .Where(r => r.RecordStatus != 99 && r.MenuId == id)
                                        .ToListAsync();
            return _Mapper.Map(zArticles, new List<ArticleDto>());
        }
        public async Task<ArticleDto?> GetArticleHomepageById(int id)
        {
            var zArticle = await _DbContext.Articles
                                        .Where(r => r.RecordStatus != 99 && r.Id == id)
                                        .FirstOrDefaultAsync();
            return _Mapper.Map(zArticle, new ArticleDto());
        }
        public async Task<int> SaveArticleHomepage(int id, ArticleDto articleDto)
        {
            Article? zArticle;

            if (id != 0)
            {
                zArticle = await _DbContext.Articles.FindAsync(id);
                if (zArticle != null)
                {
                    zArticle = _Mapper.Map(articleDto, zArticle);
                    _DbContext.Update(zArticle);
                }
                else
                {
                    // Handle the case where the article with the given ID is not found
                    throw new Exception("Article with ID " + id + " not found.");
                }
            }
            else
            {
                zArticle = _Mapper.Map(articleDto, new Article());
                _DbContext.Add(zArticle);
            }

            await _DbContext.SaveChangesAsync();

            // Return the ID of the inserted or updated article
            return zArticle.Id;
        }
        public async Task<int> DeleteArticleHomepageById(int id)
        {
            var zArticle = await _DbContext.Articles.FindAsync(id);
            if (zArticle != null)
            {
                _DbContext.Remove(zArticle);
                return await _DbContext.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }
        public async Task<List<ArticleDto>> GetBanner()
        {
            var zArticles = await _DbContext.Articles
                                        .Where(r => r.RecordStatus != 99 && r.MenuId == 5)
                                        .ToListAsync();
            return _Mapper.Map(zArticles, new List<ArticleDto>());
        }
        #endregion
    }
}