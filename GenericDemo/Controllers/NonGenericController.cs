using GenericDemo.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace GenericDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NonGenericController : ControllerBase
    {


        [HttpPost]
        public async Task<IActionResult> Post([FromForm] GiftImportRequest request)
        {
            try
            {
                string response = string.Empty;

                if (request.File != null)
                {
                    string folder = "Uploads";
                    string fileName = request.File.FileName;
                    string newFileName = Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(fileName));
                    string filePath = Path.Combine(folder, newFileName);

                    using FileStream fs = new(filePath, FileMode.Create);
                    await request.File.OpenReadStream().CopyToAsync(fs);

                    response = "File uploaded successfully";
                }
                else
                {
                    return BadRequest("No file provided in the request");
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internet Server Error: {ex.Message}");
            }
        }
    }
}
