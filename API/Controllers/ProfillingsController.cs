using API.Base;
using API.Models;
using API.Repository.Data;

namespace API.Controllers
{
    public class ProfillingsController : BaseController<Profiling, ProfillingRepository, string>
    {
        public ProfillingsController(ProfillingRepository profillingRepository) : base(profillingRepository)
        {

        }
    }
}
