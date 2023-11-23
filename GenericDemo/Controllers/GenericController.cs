using GenericDemo.Dtos;
using GenericDemo.Services.Generic;
using Microsoft.AspNetCore.Mvc;

namespace GenericDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenericController : ControllerBase
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
                    string randomName = Path.GetRandomFileName();
                    string newFileName = Path.ChangeExtension(randomName, Path.GetExtension(fileName));
                    string filePath = Path.Combine(folder, newFileName);

                    using (FileStream fs = new(filePath, FileMode.Create))
                    {

                        await request.File.OpenReadStream().CopyToAsync(fs);
                        // 在同一個程序中, 若要接續讀出剛才的存檔檔案, 必須手動Flush以關閉串流
                        await fs.FlushAsync();
                    }

                    string saveFolder = "Uploads\\done";
                    if (!Directory.Exists(saveFolder))
                    {
                        Directory.CreateDirectory(saveFolder);
                    }
                    string saveFileName = Path.ChangeExtension(randomName, ".txt");
                    string savePath = Path.Combine(saveFolder, saveFileName);

                    TextFileProcessor processor = new TextFileProcessor();
                    var output = await processor.LoadData<Gift>(filePath);
                    await processor.SaveData(output, savePath);

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
