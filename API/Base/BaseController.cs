using API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
    where Entity : class
    where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;
        public BaseController(Repository repository) 
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<Entity> GetData()
        {
            var result = repository.Get();
            if (repository.Get().Count() == 0)
            {
                return StatusCode(404, new { status = HttpStatusCode.NotFound, result, Message = "Data Tidak Ditemukan" });

            }
            else
            return StatusCode(200, new {status = HttpStatusCode.OK, result, Message = "Data Ditemukan" });
        }

        [HttpPost]
        public ActionResult<Entity> Post(Entity entity, Key key)
        {
            var result = repository.Insert(entity);
            if (repository.Get(key) != null)
            {
                return StatusCode(403, new { status = HttpStatusCode.Forbidden, Message = "Data Sudah Ada" });
            }
            else
            return StatusCode(200, new { Status = HttpStatusCode.OK, result, Message = "Data Berhasil Dibuat" });
        }

        [HttpGet("{Key}")]
        public ActionResult<Entity> GetByKey(Key key)
        {
            var result = repository.Get(key);
            if (repository.Get(key) == null)
            {
                return StatusCode(404, new { Status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan" });
            }
            else
                return StatusCode(200, new { Status = HttpStatusCode.OK, result, message = "Data Ditemukan" });
        }

        [HttpDelete("{Key}")]
        public ActionResult<Entity> DeleteByKey(Key key)
        {

            if (repository.Get(key) == null)
            {
                return StatusCode(404, new { Status = HttpStatusCode.NotFound, message = "Data Tidak Ditemukan" });
            }
            var result = repository.Delete(key);
            return StatusCode(200, new { Status = HttpStatusCode.OK, result, message = "Data Berhasil Dihapus" });
        }

        [HttpPut]
        public ActionResult<Entity> Put(Entity entity, Key key) 
        {
            var result = repository.Update(entity, key);
            return StatusCode(200, new { Status = HttpStatusCode.OK, result, message = "Data Berhasil diupdate" });
        }

        
    }
}
