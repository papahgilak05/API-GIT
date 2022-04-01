using API.Context;
using API.Models;

namespace API.Repository.Data
{
    public class AccountRoleRepository : GeneralRepository<MyContext,AccountRole, string>
    {
        public AccountRoleRepository(MyContext myContext) : base(myContext)
        {

        }
    }
}
