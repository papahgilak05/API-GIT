using System;

namespace API.Models.ViewModels
{
    public class ForgotPassVM
    {
        public string Email { get; set; }
        public string Confirmpassword { get; set; }
        public string Newpassword { get; set; }
        public int OTP { get; set; }
    }
}
