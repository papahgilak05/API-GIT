using API.Base;
using API.Context;
using API.Models;
using API.Models.ViewModels;
using API.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    public class EmployeesController : BaseController<Employee, EmployeeRepository, string>
    {
        //private readonly EmployeeRepository empRepository;
        private readonly EmployeeRepository employeeRepository1;
        private readonly MyContext myCon;
        public IConfiguration _configuration;
        public EmployeesController(EmployeeRepository employeeRepository, IConfiguration configuration) : base(employeeRepository)
        {
            this._configuration = configuration;
            this.employeeRepository1 = employeeRepository;

        }

        [HttpGet("TestCors")]
        public ActionResult testCORS()
        {
            //return StatusCode(200, new { Status = HttpStatusCode.OK, Message = "Test Cors berhasil" });
            return Ok("Test Cors Berhasil");
        }
    }
}

