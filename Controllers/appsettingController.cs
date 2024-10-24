
using System.Collections.Generic;
using BAL.Repositories;
using AdabFest_Admin._Models;
using AdabFest_Admin.BLL._Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace AdabFest_Admin.Controllers
{
    [Route("api/[controller]")]

    public class appsettingController : ControllerBase
    {
        private const string FolderName = "pdfFiles";
        private readonly IWebHostEnvironment _env;
        appSettingService _service;
        public appsettingController(IWebHostEnvironment env)
        {
            _service = new appSettingService();
            _env = env;
        }

        [HttpGet("all")]
        public List<AppSetingBLL> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public AppSetingBLL Get(int id)
        {
            return _service.Get(id);
        }

        [HttpPost]
        [Route("insert")]
        public int Post([FromBody] AppSetingBLL obj)
        {
            return _service.Insert(obj);
        }

        [HttpPost]
        [Route("update")]
        public async Task<int> UpdateDate(AppSetingBLL obj)
        {
            var filePath = "";
            if (obj.File != null)
            {
                filePath = await CopyPdfToPath(obj.File, FolderName);
            }
            else
            {
                
            }
            return _service.Update(obj, filePath, _env);
        }
        internal async Task<string> CopyPdfToPath(IFormFile file, string FolderName)
        {
            var fileName = Path.GetFileName(file.FileName);
            string ext = Path.GetExtension(file.FileName);

            if (!ext.ToLower().Equals(".pdf"))
            {
                return "";
            }

            string contentRootPath = _env.ContentRootPath;
            string path = "/ClientApp/dist/assets/Upload/" + FolderName + "/" + Path.GetFileName(Guid.NewGuid() + ".pdf");
            string filePath = contentRootPath + path;

            try
            {
                using FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await file.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {

                throw;
            }
            return path;
        }

        [HttpPost]
        [Route("delete")]
        public int Delete(AppSetingBLL obj)
        {
            return _service.Delete(obj);
        }
    }
}
