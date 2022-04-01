using API.Base;
using API.Models;
using API.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountRoleController : BaseController<AccountRole, AccountRoleRepository, string>
    {
        public AccountRoleController(AccountRoleRepository accountRoleRepository) : base(accountRoleRepository)
        {

        }
    }
}
