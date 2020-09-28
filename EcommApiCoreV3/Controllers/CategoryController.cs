using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EcommApiCoreV3.BAL.Interface;
using EcommApiCoreV3.Controllers.Common;
using EcommApiCoreV3.Entities;

namespace EcommApiCoreV3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class CategoryController : BaseController<CategoryController>
    {
        ICategoryBAL _Category;


        public CategoryController(ICategoryBAL Category)
        {
            _Category = Category;

        }

        [HttpGet]
        [Route("GetCategoryJson")]
        [AllowAnonymous]
        public string GetCategoryJson()
        {
            try
            {
                var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"Json\\leftMenuItems.json"}");
                var JSON = System.IO.File.ReadAllText(folderDetails);

                return JSON;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController GetCategoryJson action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController GetCategoryJson action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("SaveMainCategoryforJson")]
        public  List<MainCategoryJson> SaveMainCategoryforJson()
        {
            try
            {
                List<MainCategoryJson> lstmain = this._Category.SelecteMainCategoryforJson().Result;

                for (int i = 0; i < lstmain.Count; i++)
                {
                    List<CategoryJson> lstCategoryJson = this._Category.SelecteCategoryforJson(lstmain[i].MaincategoryId).Result;
                    lstmain[i].children = lstCategoryJson;

                    for (int j = 0; j < lstCategoryJson.Count; j++)
                    {

                        List<SubCategoryJson> lstSubCategoryJson = this._Category.SelecteSubCategoryforJson(lstCategoryJson[j].CategoryId).Result;
                        lstCategoryJson[j].children = lstSubCategoryJson;
                    }
                }
                var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"Json\\leftMenuItems.json"}");
                string json = JsonConvert.SerializeObject(lstmain.ToArray());
                System.IO.File.WriteAllText(folderDetails, json);
                return  lstmain;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController GetMainCategory action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController GetMainCategory action: {ex.Message}");
                return null;
            }
        }
        [HttpPost]
        [Route("GetMainCategory")]
        public async Task<List<Category>> GetMainCategory([FromBody] Category obj)
        {
            try
            {
                return await this._Category.GetMainCategory(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController GetMainCategory action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController GetMainCategory action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetAllMainCategory")]
        public async Task<List<Category>> GetAllMainCategory([FromBody] Category obj)
        {
            try
            {
                return await this._Category.GetAllMainCategory(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController GetAllMainCategory action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController GetAllMainCategory action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("SaveMainCategory")]
        public async Task<int> SaveMainCategory([FromBody] Category obj)
        {
            try
            {
                SaveMainCategoryforJson();
                return await this._Category.SaveMainCategory(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController SaveMainCategory action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController SaveMainCategory action: {ex.Message}");
                return -1;
            }
        }
        [HttpPost]
        [Route("GetCategory")]
        public async Task<List<Category>> GetCategory([FromBody] Category obj)
        {
            try
            {
                return await this._Category.GetCategory(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController GetCategory action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController GetCategory action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetAllCategory")]
        public async Task<List<Category>> GetAllCategory([FromBody] Category obj)
        {
            try
            {
                return await this._Category.GetAllCategory(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController GetAllCategory action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController GetAllCategory action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("SaveCategory")]
        public async Task<int> SaveCategory([FromBody] Category obj)
        {
            try
            {
                SaveMainCategoryforJson();
                return await this._Category.SaveCategory(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController SaveCategory action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController SaveCategory action: {ex.Message}");
                return -1;
            }
        }

        [HttpPost]
        [Route("GetSubCategory")]
        public async Task<List<Category>> GetSubCategory([FromBody] Category obj)
        {
            try
            {
                return await this._Category.GetSubCategory(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController GetSubCategory action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController GetSubCategory action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("GetAllSubCategory")]
        public async Task<List<Category>> GetAllSubCategory([FromBody] Category obj)
        {
            try
            {
                return await this._Category.GetAllSubCategory(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController GetAllSubCategory action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController GetAllSubCategory action: {ex.Message}");
                return null;
            }
        }

        [HttpPost]
        [Route("SaveSubCategory")]
        public async Task<int> SaveSubCategory([FromBody] Category obj)
        {
            try
            {
                SaveMainCategoryforJson();
                return await this._Category.SaveSubCategory(obj);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log($"Something went wrong inside CategoryController SaveSubCategory action: {ex.Message}");
                ErrorLogger.Log(ex.StackTrace);
                Logger.LogError($"Something went wrong inside CategoryController SaveSubCategory action: {ex.Message}");
                return -1;
            }
        }

    }
}