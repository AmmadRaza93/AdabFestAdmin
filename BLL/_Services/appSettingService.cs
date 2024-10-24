﻿using BAL.Repositories;
using AdabFest_Admin._Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdabFest_Admin.BLL._Services
{
    public class appSettingService : baseService
    {
        appSettingDB _service;
        public appSettingService()
        {
            _service = new appSettingDB();
        }

        public List<AppSetingBLL> GetAll()
        {
            try
            {
                return _service.GetAll();
            }
            catch (Exception ex)
            {
                return new List<AppSetingBLL>();
            }
        }
        
        public AppSetingBLL Get(int id)
        {
            try
            {
                return _service.Get(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public int Insert(AppSetingBLL data)
        {
            try
            {
                //data.Createdon = _UTCDateTime_SA();
                var result = _service.Insert(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(AppSetingBLL data,string filePath, IWebHostEnvironment _env)
        {
            try
            {
                data.SplashScreen = UploadImage(data.SplashScreen, "Banner", _env);                
                var result = _service.Update(data, filePath);

                return result;
            }

            
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(AppSetingBLL data)
        {
            try
            {
               // data.Updatedon = _UTCDateTime_SA();
                var result = _service.Delete(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
