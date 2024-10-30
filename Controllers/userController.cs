
using System;
using System.Collections.Generic;
using AdabFest_Admin._Models;
using AdabFest_Admin.BLL._Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AdabFest_Admin.Controllers
{
    [Route("api/[controller]")]

    public class userController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        userService _service;
        public userController(IWebHostEnvironment env)
        {
            _service = new userService();
            _env = env;
        }
        [HttpGet("getall")]
        public List<UserBLL> GetAllUser()
        {
            return _service.GetAllUser();
        }
        [HttpGet("{id}")]
        public UserBLL GetUser(int id)
        {
            return _service.GetUser(id);
        }
        [HttpPost]
        [Route("insertuser")]
        public int Post([FromBody] UserBLL obj)
        {
            return _service.InsertUser(obj);
        }
        [HttpPost]
        [Route("updateuser")]
        public int PostUpdate([FromBody] UserBLL obj)
        {
            return _service.UpdateUser(obj, _env);
        }
        [HttpPost]
        [Route("deleteuser")]
        public int PostDelete([FromBody] UserBLL obj)
        {
            try
            {
                var result = _service.DeleteUser(obj);

                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}