using GenericDemo.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GenericDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NonGenericController : ControllerBase
    {


        [HttpPost]
        public ActionResult Post([FromForm] GiftImportRequest request)
        {
            if (request.File != null)
            {
                string folder = "Uploads";
                string fileName = Guid.NewGuid().ToString() + "_" + request.File.FileName;
                string filePath = Path.Combine(folder, fileName);
                request.File.CopyTo(new FileStream(filePath, FileMode.Create));
            }

            return Ok("假裝完成");
        }
    }
}
