﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.Repositories;
using DAL.Models;
using AdabFest_Admin._Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdabFest_Admin.Controllers
{
    [Route("api/[controller]")]
  
    public class loginController : ControllerBase
    {
        loginDB _service;
        public loginController()
        {
            _service = new loginDB();
        }


        [HttpGet]
        [Route("authenticate/{username}/{password}")]
        public LoginBLL AutheticateUser(string username, string password)
        {
            return _service.GetAuthenticateUser(username, password);

        }
    }
}
