using API.Context;
using API.Models;
using API.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace API.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, Account, string>
    {
        private readonly MyContext myCon;
        public IConfiguration _configuration;
        public AccountRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            myCon = myContext;
            _configuration = configuration;
        }



        public int Insert(RegisterVM registerVM)
        {
            var emp = new Employee
            {
                NIK = registerVM.NIK,
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Phone = registerVM.Phone,
                BirthDate = registerVM.Birthdate,
                Salary = registerVM.Salary,
                Email = registerVM.Email,
                Gender = (Gender)registerVM.Gender
            };
            myCon.Employees.Add(emp);
            var acc = new Account
            {
                NIK = emp.NIK,
                password = HASHPASSWORD(registerVM.Password)
            };
            myCon.Accounts.Add(acc);
            var edu = new Education
            {
                Degree = registerVM.Degree,
                GPA = registerVM.GPA,
                UniversitiesId = registerVM.UniversitiesID
            };
            myCon.Educations.Add(edu);

            var ar = new AccountRole
            {
                NIK = emp.NIK,
                RoleId = 3
            };
            myCon.AccountRoles.Add(ar);

            myCon.SaveChanges();

            var pro = new Profiling
            {
                NIK = emp.NIK,
                EducationsId = edu.Id
            };
            myCon.Profilings.Add(pro);


            var result = myCon.SaveChanges();
            return result;
        }

        public int SignManager(RegisterVM registerVM)
        {
            var emp = myCon.Employees.Where(e=>e.Email==registerVM.Email).FirstOrDefault();
            var ar = new AccountRole
            {
                NIK = emp.NIK,
                RoleId = 2
            };
            myCon.AccountRoles.Add(ar);

            var result = myCon.SaveChanges();
            return result;

        }
        

        public int Logon(LoginVM loginVM)
        {
            var emp = myCon.Employees.Where(e => e.Email == loginVM.Email).SingleOrDefault();
            //var acc = myCon.Employees.Include("Accounts").Where(e => e.Email == loginVM.Email && e.Accounts.password == loginVM.Password ).SingleOrDefault();
            
            
            if(emp == null)
            {
                return 0;
            }else
            {
                var pass = myCon.Accounts.Where(e => e.NIK == emp.NIK).SingleOrDefault();
                if ((emp.Email == loginVM.Email) && !VALIDATEPASSWORD(loginVM.Password, pass.password))
                return 1;
            }
            return 2;


        }

        public IEnumerable MasterData()
        {
            var q = (from emp in myCon.Employees
                     join acc in myCon.Accounts on emp.NIK equals acc.NIK
                     join pro in myCon.Profilings on acc.NIK equals pro.NIK
                     join edu in myCon.Educations on pro.EducationsId equals edu.Id
                     join uni in myCon.Universities on edu.UniversitiesId equals uni.Id
                     join ar in myCon.AccountRoles on acc.NIK equals ar.NIK
                     join rol in myCon.Roles on ar.RoleId equals rol.RoleId
                     orderby emp.NIK
                     select new
                     {
                         NIK = emp.NIK,
                         FullName = emp.FirstName + emp.LastName,
                         Phone = emp.Phone,
                         Gender =((Gender)emp.Gender).ToString(),
                         Email = emp.Email,
                         Birthdate = emp.BirthDate,
                         Salary = emp.Salary,
                         Educations = pro.EducationsId,
                         Degree = edu.Degree,
                         GPA = edu.GPA,
                         UniversityName = uni.Name,
                         RoleName = rol.RoleName
                     }).ToList();
            return q;
        }

        public int ForgotPass(ForgotPassVM forgotpassVM)
        {
            var emp = myCon.Employees.Where(e => e.Email == forgotpassVM.Email).SingleOrDefault();
            var OTP = myCon.Accounts.Where(e => e.OTP == forgotpassVM.OTP).SingleOrDefault();
            //var acc = myCon.Accounts.Where(e => e.NIK == emp.NIK).SingleOrDefault();
            if (emp == null)
            {
                return 0;
            }
            else {
                var acc = myCon.Accounts.Where(e => e.NIK == emp.NIK).SingleOrDefault();
                if (OTP == null)
                {
                    return 1;
                }
                else if (OTP.OTP == forgotpassVM.OTP && acc.IsUsed == true)
                {
                    return 2;
                }
                else if (OTP.OTP == forgotpassVM.OTP && acc.IsUsed == false && acc.ExpiredToken < DateTime.Now)
                {
                    return 3;
                }
                else if (OTP.OTP == forgotpassVM.OTP && acc.IsUsed == false && acc.ExpiredToken > DateTime.Now
                        && forgotpassVM.Newpassword != forgotpassVM.Confirmpassword)
                {
                    return 4;
                }
            }
            return 5;
        }

        public int ChangePass(ForgotPassVM forgotpassVM)
        {
            var emp = myCon.Employees.Where(e => e.Email == forgotpassVM.Email).SingleOrDefault();
            var acc = myCon.Accounts.Where(e => e.NIK == emp.NIK).SingleOrDefault();
            acc.password = HASHPASSWORD(forgotpassVM.Newpassword);
            acc.IsUsed = true;
            myCon.Entry(acc).State = EntityState.Modified;
            var result = myCon.SaveChanges();
            return result;
        }

        public int SendOTP(SendingOTPVM sendingOTPVM)
        {
            //membuat Random OTP
            Random rnd = new Random();
            var RandomNumber = (rnd.Next(100000, 999999)).ToString("D6");

            //membaca isi table Account untuk Diupdate
            var emp = myCon.Employees.Where(e => e.Email == sendingOTPVM.Email).SingleOrDefault();
            var acc = myCon.Accounts.Where(e => e.NIK == emp.NIK).SingleOrDefault();
            acc.OTP = Convert.ToInt32(RandomNumber);
            acc.ExpiredToken = DateTime.Now.AddMinutes(5);
            acc.IsUsed = false;
            myCon.Entry(acc).State = EntityState.Modified;
            var result = myCon.SaveChanges();


            string to = emp.Email.ToString(); //membaca email di database untuk dikirimkan pesan OTP
            string from = "tutuatlantica@gmail.com"; //email pengirim
            MailMessage message = new MailMessage(from, to); //perintah untuk mengirimkan email dari pengirim ke penerima
            string mailbody = $" {acc.OTP} Ini OTP kamu yang dapat kamu gunakan untuk Reset Password OTP ini hanya Berlaku untuk 5 Menit";
            message.Subject = "OTP untuk Ganti Password";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //mengirimkan berdasarkan Port email penerima
            NetworkCredential basiccredential = new
            NetworkCredential("tutuatlantica@gmail.com", "kakikubau"); //username dan password pengirim
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basiccredential;
            client.Send(message); //mengirimkan pesan
            return result;
            

        }

        public string GenerateJWT(LoginVM loginVM)
        {
            var NIK = myCon.Employees.Where(e => e.Email == loginVM.Email).SingleOrDefault();
            var cekrole = myCon.Roles.Where(r => r.AccountRole.Any(ar=> ar.NIK == NIK.NIK)).ToList();
            var claims = new List<Claim>();
            claims.Add(new Claim("Email", NIK.Email));
            foreach (var item in cekrole)
            {
                claims.Add(new Claim("roles", item.RoleName));
            }

            //Generate JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn
                );
            var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
            claims.Add(new Claim("TokenSecurity", idtoken.ToString()));
            return idtoken;
        }

        

/*        public int RandOTP(SendingOTPVM sendingOTPVM)
        {
            Random rnd = new Random();
            var RandomNumber = (rnd.Next(100000, 999999)).ToString("D6");
            var emp = myCon.Employees.Where(e => e.Email == sendingOTPVM.Email).SingleOrDefault();
            var acc = myCon.Accounts.Where(e => e.NIK == emp.NIK).SingleOrDefault();
            acc.OTP = Convert.ToInt32(RandomNumber);
            acc.ExpiredToken = DateTime.Now.AddMinutes(5);
            acc.IsUsed = false;
            myCon.Entry(acc).State = EntityState.Modified;
            var result = myCon.SaveChanges();
            return result;
        }*/

        private static string GETRANDOMSALT()
        {
            return BCrypt.Net.BCrypt.GenerateSalt(12);
        }
        public static string HASHPASSWORD(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, GETRANDOMSALT());
        }
        
        public static bool VALIDATEPASSWORD(string password, string correcthash)
        {
            return BCrypt.Net.BCrypt.Verify(password, correcthash);
        }


    }
}
