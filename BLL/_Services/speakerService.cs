﻿

using BAL.Repositories;
using AdabFest_Admin._Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdabFest_Admin.BLL._Services
{
    public class speakerService : baseService
    {
        speakerDB _service;
        public speakerService()
        {
            _service = new speakerDB();
        }


        public List<SpeakerBLL> Get()
        {
            try
            {
                return _service.GetAll();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<SpeakerBLL> GetDropdown()
        {
            try
            {
                return _service.GetDropdown();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public SpeakerBLL Get(int id)
        {
            try
            {
                return _service.Get(id);
            }
            catch (Exception)
            {
                return null;
            }
        }
        public int Insert(SpeakerBLL data, IWebHostEnvironment _env)
        {
            try
            {
                data.Image = UploadImage(data.Image, "Speaker", _env);
                data.Createdon = DateTime.UtcNow.AddMinutes(300);
                var result = _service.Insert(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Update(SpeakerBLL data, IWebHostEnvironment _env)
        {
            try
            {
                data.Image = UploadImage(data.Image, "Speaker", _env);
                data.Updatedon = DateTime.UtcNow.AddMinutes(300);
                var result = _service.Update(data);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public int Delete(SpeakerBLL data)
        {
            try
            {
                data.Updatedon = DateTime.UtcNow.AddMinutes(300);
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
