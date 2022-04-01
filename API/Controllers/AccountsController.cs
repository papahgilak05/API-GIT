using API.Base;
using API.Models;
using API.Models.ViewModels;
using API.Repository.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace API.Controllers
{
    public class AccountsController : BaseController<Account, AccountRepository, string>
    {
        private readonly AccountRepository accRepository;
        public IConfiguration _configuration;
        public AccountsController(AccountRepository accountRepository, IConfiguration configuration) : base(accountRepository) 
        {
            this._configuration = configuration;
            this.accRepository = accountRepository;
            
        }

        [HttpPost("Register")]
        public ActionResult Register(RegisterVM registerVM) 
        {
            accRepository.Insert(registerVM);
            return StatusCode(200, new { status = HttpStatusCode.OK, Message = "Data berhasil ditambahkan" });
        }
        [HttpPost("Login")]
        public ActionResult Login(LoginVM loginVM)
        {
            var result = accRepository.Logon(loginVM);
            if (result == 0)
            {
                return StatusCode(404, new { Status = HttpStatusCode.NotFound, Message = "Email Salah" });
            }
            else if (result == 1)
            {
                return StatusCode(404, new { Status = HttpStatusCode.NotFound, Message = "Password Salah" });
            }
            else result = 2;
            return StatusCode(200, new { Status = HttpStatusCode.OK, Message = "Login berhasil" });

        }
        [HttpPost("LoginJWT")]
        public ActionResult LoginJWT(LoginVM loginVM)
        {
            var ceklogin = accRepository.Logon(loginVM);
            if(ceklogin > 0)
            {
                var result = accRepository.GenerateJWT(loginVM);
                return StatusCode(200, new { status = HttpStatusCode.OK,result, Message = "Token berhasil Dibuat" });
            }
            return BadRequest();
        }

        [Authorize()]
        [HttpGet("TestJWT")]
        public ActionResult TestJWT()
        {
            return StatusCode(200, new { Stauts = HttpStatusCode.OK, Message = "Test Jwt Berhasil" });
        }

        [HttpGet("MasterData")]
        public ActionResult MasterData()
        {
            var result = accRepository.MasterData();
            return Ok(result);
        }
        [Authorize(Roles ="Director,Manager")]
        [HttpGet("MasterDataJWT")]
        public ActionResult MasterDataJWT()
        {
            var result = accRepository.MasterData();
            return Ok(result);
        }

        [Authorize(Roles ="Director")]
        [HttpPost("SignManager")]
        public ActionResult SignManager(RegisterVM registerVM)
        {
            var result = accRepository.SignManager(registerVM);
            return StatusCode(200, new {Status = HttpStatusCode.OK, result, Message = "Berhasil Mengangkat Manager"});
        }

        [HttpPut("SendOTP")]
        public ActionResult OTP(SendingOTPVM sendingOTPVM)
        {
            if (sendingOTPVM.Email==null)
            {
                StatusCode(404, new { Message = "Email Tidak ditemukan" });
            }
            var result = accRepository.SendOTP(sendingOTPVM);
            return StatusCode(200, new { result, Message = "OTP Dikirimkan ke emailmu" });


        }

        [HttpPut("ForgotPassword")]
        public ActionResult ForgotPassword(ForgotPassVM forgotpassVM)
        {
            var result = accRepository.ForgotPass(forgotpassVM);
            if (result == 0)
            {
                return StatusCode(404, new { Status = HttpStatusCode.NotFound, Message = "Email Salah" });
            }
            else if (result == 1)
            {
                return StatusCode(404, new { Status = HttpStatusCode.NotFound, Message = "OTP Salah" });
            }
            else if (result == 2)
            {
                return StatusCode(404, new { Status = HttpStatusCode.NotFound, Message = "OTP Sudah Digunakan" });
            }
            else if (result == 3)
            {
                return StatusCode(404, new { Status = HttpStatusCode.NotFound, Message = "OTP Sudah Expired" });
            }
            else if (result == 4)
            {
                return StatusCode(404, new { Status = HttpStatusCode.NotFound, Message = "Password Tidak Sesuai" });
            }
            else result = 5;
            var hasil = accRepository.ChangePass(forgotpassVM);
            return StatusCode(200, new { Status = HttpStatusCode.OK, Message = "Password Berhasil dirubah" });

        }
    }

}
