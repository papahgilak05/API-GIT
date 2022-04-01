using API.Base;
using API.Models;
using API.Repository.Data;

namespace API.Controllers
{
    public class UniversitiesController : BaseController<University, UniversityRepository, int>
    {
        public UniversitiesController(UniversityRepository universityRepository) : base(universityRepository)
        {

        }
    }
}
