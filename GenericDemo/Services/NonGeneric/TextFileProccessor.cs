using GenericDemo.Dtos;

namespace GenericDemo.Services.NonGeneric
{
    public class TextFileProccessor
    {
        public async Task<List<Gift>> LoadData(string filePath)
        {
            List<Gift> output = new();

            var lines = await File.ReadAllLinesAsync(filePath);
            var data = lines.Skip(1); // 忽略第一列

            foreach (var line in data)
            {
                var vals = line.Split(',');
                Gift gift = new Gift()
                {
                    Name = vals[0],
                    Description = vals[1],
                    IsActive = Convert.ToBoolean(Convert.ToInt16(vals[2])),
                };
                output.Add(gift);
            }

            return output;
        }

        public async Task SaveData(List<Gift> gifts, string filePath)
        {
            List<string> lines = new();

            foreach (var gift in gifts)
            {
                lines.Add($"{gift.Name},{gift.Description},{gift.IsActive}");
            }

            await File.AppendAllLinesAsync(filePath, lines);
        }
    }
}
