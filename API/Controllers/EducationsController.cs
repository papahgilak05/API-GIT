using API.Base;
using API.Models;
using API.Repository.Data;

namespace API.Controllers
{
    public class EducationsController : BaseController<Education, EducationRepository, int>
    {
        public EducationsController(EducationRepository educationRepository) : base(educationRepository)
        {

        }
    }
}
