using API.Context;
using API.Models;


namespace API.Repository.Data
{
    public class ProfillingRepository : GeneralRepository<MyContext,Profiling,string>
    {
        public ProfillingRepository(MyContext myContext) : base(myContext) 
        {
            
        }
    }
}
