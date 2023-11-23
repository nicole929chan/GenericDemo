using System.Text;

namespace GenericDemo.Services.Generic
{
    public class TextFileProcessor
    {
        public async Task<List<T>> LoadData<T>(string filePath) where T : class, new()
        {
            List<T> output = new List<T>();
            T entry = new T();
            var cols = entry.GetType().GetProperties();  // 取得欄位名稱(Reflection)

            var lines = await File.ReadAllLinesAsync(filePath);
            var headers = lines[0].Split(",");
            var data = lines.Skip(1);

            if (data.Count() < 2)
            {
                throw new IndexOutOfRangeException("The file was either empty or missing.");
            }

            foreach (var line in data)
            {
                var vals = line.Split(",");
                entry = new T();

                for (var i = 0; i < headers.Length; i++)
                {
                    foreach (var col in cols)
                    {
                        if (col.Name == headers[i])
                        {
                            if (col.PropertyType == typeof(bool))
                            {
                                col.SetValue(entry, Convert.ChangeType(Convert.ToInt16(vals[i]), col.PropertyType));
                            }
                            else
                            {
                                col.SetValue(entry, Convert.ChangeType(vals[i], col.PropertyType));
                            }
                        }
                    }
                }
                output.Add(entry);
            }


            return output;
        }
        public async Task SaveData<T>(List<T> data, string filePath) where T : class
        {
            List<string> lines = new();
            StringBuilder line = new StringBuilder();

            if (data == null || data.Count() == 0)
            {
                throw new ArgumentNullException("data", "You must populate the data parameter with at least one value.");
            }

            var cols = data[0].GetType().GetProperties();  // 以其中一筆物件取得所有欄位名稱

            foreach (var col in cols)
            {
                line.Append(col.Name);
                line.Append(",");
            }
            lines.Add(line.ToString().Substring(0, line.Length - 1));

            foreach (var row in data)
            {
                line = new StringBuilder();
                foreach (var col in cols)
                {
                    line.Append(col.GetValue(row));  // 以欄位名稱對物件取值(好神奇)
                    line.Append(",");
                }
                lines.Add(line.ToString().Substring(0, line.Length - 1));
            }

            File.WriteAllLinesAsync(filePath, lines);
        }
    }
}
