using GenericDemo.Dtos;
using GenericDemo.Services.NonGeneric;
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

                    using (FileStream fs = new(filePath, FileMode.Create))
                    {

                        await request.File.OpenReadStream().CopyToAsync(fs);
                        // 在同一個程序中, 若要接續讀出剛才的存檔檔案, 必須手動Flush以關閉串流
                        await fs.FlushAsync();
                    }

                    TextFileProccessor proccessor = new TextFileProccessor();
                    var output = await proccessor.LoadData(filePath);

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
