﻿
using System.Collections.Generic;
using AdabFest_Admin._Models;
using AdabFest_Admin.BLL._Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AdabFest_Admin.Controllers
{
    [Route("api/[controller]")]
  
    public class OrganizerController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        organizerService _service;
        public OrganizerController(IWebHostEnvironment env)
        {
            _service = new organizerService();
            _env = env;
        }
        [HttpGet("all")]
        public List<OrganizerBLL> Get()
        {
            return _service.Get();
        }
        [HttpGet("DropdownAll")]
        public List<OrganizerBLL> GetDropdown()
        {
            return _service.GetDropdown();
        }
        [HttpGet("organizer/{id}")]
        public OrganizerBLL Get(int id)
        {
            return _service.Get(id);
        }
        [HttpPost]
        [Route("insert")]
        public int Post([FromBody] OrganizerBLL obj)
        {
            return _service.Insert(obj, _env);
        }

        [HttpPost]
        [Route("update")]
        public int PostUpdate([FromBody] OrganizerBLL obj)
        {
            return _service.Update(obj, _env);
        }


        [HttpPost]
        [Route("delete")]
        public int PostDelete([FromBody] OrganizerBLL obj)
        {
            return _service.Delete(obj);
        }
    }
}
