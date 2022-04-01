using API.Context;
using API.Models;
using API.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace API.Repository.Data
{
    public class EmployeeRepository : GeneralRepository<MyContext, Employee, string>
    {
        private readonly MyContext myCon;
        public EmployeeRepository(MyContext myContext) : base(myContext)
        {
            myCon = myContext;
        }

        public int Logon(LoginVM loginVM)
        {
            var emp = myCon.Employees.Where(e => e.Email == loginVM.Email).SingleOrDefault();
            //var acc = myCon.Employees.Include("Accounts").Where(e => e.Email == loginVM.Email && e.Accounts.password == loginVM.Password ).SingleOrDefault();


            if (emp == null)
            {
                return 0;
            }
            else
            {
                var pass = myCon.Accounts.Where(e => e.NIK == emp.NIK).SingleOrDefault();
                if ((emp.Email == loginVM.Email) && !VALIDATEPASSWORD(loginVM.Password, pass.password))
                    return 1;
            }
            return 2;
        }

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
