using HubbaGolfAdmin.Database;
using HubbaGolfAdmin.Database.Dtos;
using HubbaGolfAdmin.Services.Interfaces;
using HubbaGolfAdmin.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using HubbaGolfAdmin.Helpers;

namespace HubbaGolf_Admin.Controllers
{
    public class WebsiteController : Controller
    {
        private readonly SessionStore _SessionStore;
        private readonly ISystemService _SystemService;
        private readonly IWebService _WebService;
        private readonly string _ImagePath;
        private readonly string _FilePath;

        public WebsiteController(
            SessionStore sessionStore,
            ISystemService systemService,
            IWebService webService,
            IConfiguration config)
        {
            _ImagePath = config.GetSection("UploadFiles:Img").Value;
            _FilePath = config.GetSection("UploadFiles:Doc").Value;
            _SystemService = systemService;
            _WebService = webService;
            _SessionStore = sessionStore;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetCategory()
        {
            _SessionStore.StoreCurrentUrl();
            var zCategories = await _WebService.GetListCategoryAsync();

            return View("~/Views/Website/CategoryList.cshtml", zCategories);
        }
        [HttpPost]
        public async Task<IActionResult> SaveCategory(int id, CategoryDto categoryDto)
        {
            if(categoryDto.Type.IsNullOrEmpty())
            {
                categoryDto.Type = TypeMenu.News;
            }    
            await _WebService.SaveCategory(id, categoryDto);
            return RedirectToAction(nameof(GetCategory));
        }
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var zCategories = await _WebService.GetCategoryByIdAsync(id);
            if (zCategories != null)
                return Ok(zCategories);
            else
                return BadRequest("not found");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategoryById(int id)
        {
            try
            {
                var zRecord = await _WebService.DeleteCategoryById(id);
                if (zRecord != 0)
                    return Ok();
                else
                    return BadRequest("nothing deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        public async Task<IActionResult> GetLocation()
        {
            _SessionStore.StoreCurrentUrl();
            var zCategories = await _WebService.GetListLocationAsync();

            return View("~/Views/Website/LocationList.cshtml", zCategories);
        }
        [HttpPost]
        public async Task<IActionResult> SaveLocation(int id, CategoryDto categoryDto)
        {
            if (categoryDto.Type.IsNullOrEmpty())
            {
                categoryDto.Type = TypeMenu.Location;
            }
            await _WebService.SaveCategory(id, categoryDto);
            return RedirectToAction(nameof(GetLocation));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLocation()
        {
            try
            {
                var zLocation = await _WebService.GetAllLocationAsync();
                return Ok(zLocation);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> GetListArticleByCategoryIdAdmin(int id)
        {
            _SessionStore.StoreCurrentUrl();

            var zList = await _WebService.GetListArticleByCategoryIdAdminAsync(id);
            ViewBag.CategoryId = id;
            ViewBag.SelectType = await _WebService.GetListCategoryByParentIdAsync(24);
            ViewBag.IsUseFac = false;
            var zFacility = await _WebService.GetListLocationAsync();
            if(zFacility != null)
            {
                ViewBag.IsUseFac = zFacility.Any(item => item.Id == id);
            }    

            HttpContext.Session.SetInt32("CategoryId", id);
            return View("~/Views/Website/ArticleList.cshtml", zList);
        }
        [HttpGet]
        public async Task<IActionResult> GetListArticleByCategoryId(int id)
        {
            try
            {
                var zList = await _WebService.GetListArticleByCategoryIdAsync(id);
                return Ok(zList);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public async Task<IActionResult> SearchArticle(int id, string value, int? type)
        {
            var zList = await _WebService.SearchArticle(id, value, type);
            var zCategoryId = HttpContext.Session.GetInt32("CategoryId");
            if (zCategoryId != null)
            {
                ViewBag.CategoryId = id;
            }
            if(type != null)
            {
                ViewBag.SelectedTypeId = type;
            }
            ViewBag.StrSearch = value;
            ViewBag.SelectType = await _WebService.GetListCategoryByParentIdAsync(24);
            ViewBag.IsUseFac = false;
            var zFacility = await _WebService.GetListLocationAsync();
            if (zFacility != null)
            {
                ViewBag.IsUseFac = zFacility.Any(item => item.Id == id);
            }

            return View("~/Views/Website/ArticleList.cshtml", zList);
        }
        public async Task<IActionResult> GetArticleByIdAdmin(int id)
        {
            var zArticle = await _WebService.GetArticleByIdAsync(id);
            ViewBag.SelectCategory = await _WebService.GetListAllCategoryAsync();
            ViewBag.SelectType = await _WebService.GetListCategoryByParentIdAsync(24);

            var zCategoryId = HttpContext.Session.GetInt32("CategoryId");
            if (zCategoryId != null)
            {
                ViewBag.CategoryId = zCategoryId;
            }

            if (zArticle != null && zArticle.CategoryId != null)
                ViewBag.TypeOfArticle = await _WebService.TypeOfArticle(zArticle.CategoryId.Value);
            else
                ViewBag.TypeOfArticle = await _WebService.TypeOfArticle(zCategoryId.Value);

            //if (id != 0)
            //    ViewBag.Log = await _WebService.GetArticleLogByIdAsync(id);
            return View("~/Views/Website/ArticleEdit.cshtml", zArticle);
        }
        [HttpGet]
        public async Task<IActionResult> GetArticleById(int id)
        {
            try
            {
                var zArticle = await _WebService.GetArticleByIdAsync(id);
                return Ok(zArticle);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }    
        }

        [HttpPost]
        public async Task<IActionResult> SaveArticle(int id, ArticleDto articleDto)
        {
            try
            {
                if (articleDto.FileImage != null &&
                    articleDto.FileImage.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + articleDto.FileImage.FileName;
                    articleDto.UrlImage = ViewHelper.UploadFile(articleDto.FileImage, _ImagePath);
                }

                if (articleDto.FileIcon != null &&
                    articleDto.FileIcon.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + "_" + articleDto.FileIcon.FileName;
                    articleDto.Icon = ViewHelper.UploadFile(articleDto.FileIcon, _ImagePath);
                }
                //HiddenStatus must be used to store the value of Publish
                //Because the check status button is not stable, and if it is checked it is 1, if it is not checked it is null
                articleDto.Status = articleDto.HiddenStatus == "1" ? 1 : 0;
                articleDto.IsParent = articleDto.HiddenParent == "1" ? true : false;

                var zResult = await _WebService.SaveArticleAsync(id, articleDto);
                if (zResult.Success && zResult.Data != 0)
                {
                    var zArticleId = zResult.Data;

                    if (articleDto.FileDocument != null &&
                        articleDto.FileDocument.Count > 0)
                    {
                        foreach (var file in articleDto.FileDocument)
                        {
                            var fileName = file.FileName;
                            if (fileName.Length > 0)
                            {
                                
                            }
                        }
                    }
                    if (articleDto.FileDocument != null &&
                        articleDto.FileDocument.Count > 0)
                    {
                        foreach (var file in articleDto.FileDocument)
                        {
                            var fileName = file.FileName;
                            if (fileName.Length > 0)
                            {
                                
                            }
                        }
                    }
                    if (articleDto.FileDocument != null &&
                        articleDto.FileDocument.Count > 0)
                    {
                        foreach (var file in articleDto.FileDocument)
                        {
                            var fileName = file.FileName;
                            if (fileName.Length > 0)
                            {
                                
                            }
                        }
                    }
                }

                if (zResult.Success)
                {
                    var redirectUrl = Url.Action(nameof(GetListArticleByCategoryIdAdmin), "Website", new { id = articleDto.CategoryId });
                    //return Json(new { success = true, redirectUrl = redirectUrl });
                    return Ok(redirectUrl);
                }
                else
                {
                    //return Json(new { success = false, message = zResult.Message });
                    return BadRequest(zResult.Message);
                }
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteArticleById(int id)
        {
            try
            {
                var zResult = await _WebService.DeleteArticleAsync(id);
                if (zResult.Success)
                {
                    //var zDocs = await _WebService.GetFileList(id);
                    //if (zDocs.Count > 0)
                    //{
                    //    foreach (var doc in zDocs)
                    //    {
                    //        ViewHelper.DeleteFile(doc.Link);
                    //    }
                    //}

                    return Ok(zResult);
                }

                return BadRequest("not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPost]

        public async Task<IActionResult> ChangeStatusArticle(int id)
        {
            var result = await _WebService.ChangeStatusArticleAsync(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest("not found");
        }
        public async Task<IActionResult> UpdateRank(int id, int rank)
        {
            if (id > 0)
            {
                var zResult = await _WebService.UpdateRank(id, rank);
                if (zResult)
                {
                    return Ok();
                }
            }

            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetMenuHeader()
        {
            try
            {
                var zMenu = await _WebService.GetMenuHeaderAsync();
                return Ok(zMenu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> getCourseByCountryId(int id)
        {
            try
            {
                var zMenu = await _WebService.GetCourseByCountryIdAsync(id);
                return Ok(zMenu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCourseGroupFacilityByCountryId(int id)
        {
            try
            {
                var zMenu = await _WebService.GetCourseGroupFacilityByCountryIdAsync(id);
                return Ok(zMenu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetCourseByCountryIdAndTypeID(int typeId, int countryId)
        {
            try
            {
                var zMenu = await _WebService.GetCourseByCountryIdAndTypeIDAsync(typeId, countryId);
                return Ok(zMenu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetListArticleMenuTopTier()
        {
            try
            {
                var zList = await _WebService.GetListArticleMenuTopTierAsync();
                return Ok(zList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #region [Manage Banner]
        public async Task<IActionResult> GetArticleHomepage(int id)
        {
            _SessionStore.StoreCurrentUrl();
            var zArticle = await _WebService.GetArticleHomepage(id);
            return View("~/Views/Website/ArticleHomepage.cshtml", zArticle);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticleHomepageById(int id)
        {
            var zArticle = await _WebService.GetArticleHomepageById(id);
            if (zArticle != null)
                return Ok(zArticle);
            else
                return BadRequest("not found");
        }

        [HttpPost]
        public async Task<IActionResult> SaveArticleHomepage(int id, ArticleDto articleDto)
        {
            articleDto.Status ??= 1;

            if (articleDto.FileImage != null &&
                articleDto.FileImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + "_" + articleDto.FileImage.FileName;
                articleDto.UrlImage = ViewHelper.UploadFile(articleDto.FileImage, _ImagePath); //Helper.UploadFile(articleDto.FileImage, fileName, _ImagePath);
            }

            var zRecord = await _WebService.SaveArticleHomepage(id, articleDto);

            if (zRecord != 0)
                return RedirectToAction(nameof(GetArticleHomepage), new { id = 5 });
            else
                return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteArticleHomepageById(int id)
        {
            try
            {
                var zRecord = await _WebService.DeleteArticleHomepageById(id);
                if (zRecord != 0)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetBanner()
        {
            var zArticle = await _WebService.GetBanner();
            if (zArticle != null)
                return Ok(zArticle);
            else
                return BadRequest("not found");
        }
        #endregion

        #region [Manage Price]
        public async Task<IActionResult> GetListPrice()
        {
            _SessionStore.StoreCurrentUrl();
            var zPrice = await _WebService.GetListPrice();
            return View("~/Views/Website/PriceList.cshtml", zPrice);
        }

        [HttpGet]
        public async Task<IActionResult> GetPriceById(int id)
        {
            var zPrice = await _WebService.GetPriceById(id);
            if (zPrice != null)
                return Ok(zPrice);
            else
                return BadRequest("not found");
        }

        [HttpPost]
        public async Task<IActionResult> SavePrice(int id, PricingDto pricingDto)
        {
            var zRecord = await _WebService.SavePrice(id, pricingDto);

            if (zRecord != 0)
                return RedirectToAction(nameof(GetListPrice));
            else
                return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> DeletePriceById(int id)
        {
            try
            {
                var zRecord = await _WebService.DeletePriceById(id);
                if (zRecord != 0)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPriceByArticleId(int id)
        {
            var zPrice = await _WebService.GetPriceByArticleId(id);
            if (zPrice != null)
                return Ok(zPrice);
            else
                return BadRequest("not found");
        }
        #endregion
    }
}
