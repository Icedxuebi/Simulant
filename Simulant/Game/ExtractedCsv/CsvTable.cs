using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Simulant.Game.ExtractedCsv
{
    /// <summary>
    /// 泛型 CSV 表类，用于加载并解析 CSV 文件，并将行映射为强类型 Entry。
    /// </summary>
    public class CsvTable
    {
        /// <summary> 存储这个表的管理器实例 </summary>
        public CsvManager Manager { get; }

        /// <summary>
        /// 表头（第二行）
        /// </summary>
        public IReadOnlyList<string> Headers { get; }

        /// <summary>
        /// 类型信息（第三行）
        /// </summary>
        public IReadOnlyList<string> Types { get; }

        /// <summary>
        /// 数据行（第四行开始）
        /// </summary>
        public IReadOnlyDictionary<RowIndexKey, CsvRow> Rows { get; }

        /// <summary>
        /// 列名 → 列索引（忽略大小写）
        /// </summary>
        internal Dictionary<string, int> HeaderIndex { get; }

        /// <summary>
        /// Core：传入已读取好的 CSV 行并解析。
        /// </summary>
        internal CsvTable(CsvManager manager, string sourceName, List<string[]> lines)
        {
            var tempTable = new Dictionary<RowIndexKey, CsvRow>();

            if (lines.Count < 3)
                throw new InvalidDataException($"CSV 文件 {sourceName} 行数 ({lines.Count}) 不足，无法解析数据。");
            if (lines[0].Length < 1)
                throw new InvalidDataException($"CSV 文件 {sourceName} 列数 ({lines[0].Length}) 不足，无法解析数据。");

            int headerLine = 0;
            if (lines[0].Length > 0 && lines[0][0].Equals("key", StringComparison.OrdinalIgnoreCase))
                headerLine = 1;

            var headers = lines[headerLine];
            for (int i = 0; i < headers.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(headers[i]))
                    headers[i] = "unk_" + i;
            }
            Headers = headers;

            Types = lines[headerLine + 1];

            HeaderIndex = new Dictionary<string, int>(Headers.Count, StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < Headers.Count; i++)
            {
                string name = Headers[i];
                if (string.IsNullOrEmpty(name))
                    continue;

                if (HeaderIndex.ContainsKey(name))
                    throw new InvalidDataException($"CSV 文件 {sourceName} 存在重复列名: {name}");

                HeaderIndex.Add(name, i);
            }

            for (int rowIdx = headerLine + 2; rowIdx < lines.Count; rowIdx++)
            {
                var row = lines[rowIdx];
                if (row.Length == 0)
                    throw new InvalidDataException($"CSV 文件 {sourceName} 第 {rowIdx + 1} 行为空，无法解析数据。");

                var key = RowIndexKey.Parse(row[0]);
                if (tempTable.ContainsKey(key))
                    throw new InvalidDataException($"CSV {sourceName} 存在重复 key: {key}");

                var rowInstance = new CsvRow(this, key, row);
                tempTable[key] = rowInstance;
            }

            Rows = tempTable;
            Manager = manager;
        }

        internal static CsvTable CreateFromFilePath(CsvManager manager, string filePath)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return CreateFromStream(manager, filePath, stream);
            }
        }

        internal static CsvTable CreateFromResourcePath(CsvManager manager, string resourcePath)
        {
            var asm = typeof(CsvTable).Assembly;

            using (var stream = asm.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                    throw new InvalidOperationException("Embedded CSV resource not found: " + resourcePath);

                return CreateFromStream(manager, resourcePath, stream);
            }
        }

        private static CsvTable CreateFromStream(CsvManager manager, string sourceName, Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8, true))
            {
                var lines = ReadCsv(reader);
                return new CsvTable(manager, sourceName, lines);
            }
        }

        private static List<string[]> ReadCsv(TextReader reader)
        {
            var result = new List<string[]>();
            var row = new List<string>();
            var field = new StringBuilder();
            bool inQuotes = false;

            while (true)
            {
                int raw = reader.Read();
                if (raw < 0)
                {
                    if (inQuotes)
                        throw new InvalidDataException("CSV ended inside a quoted field.");

                    if (field.Length > 0 || row.Count > 0)
                    {
                        row.Add(field.ToString());
                        result.Add(row.ToArray());
                    }

                    break;
                }

                char ch = (char)raw;

                if (inQuotes)
                {
                    if (ch == '"')
                    {
                        if (reader.Peek() == '"')
                        {
                            reader.Read();
                            field.Append('"');
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        field.Append(ch);
                    }

                    continue;
                }

                switch (ch)
                {
                    case '"':
                        if (field.Length == 0)
                            inQuotes = true;
                        else
                            field.Append(ch);
                        break;

                    case ',':
                        row.Add(field.ToString());
                        field.Clear();
                        break;

                    case '\r':
                        if (reader.Peek() == '\n')
                            reader.Read();

                        row.Add(field.ToString());
                        field.Clear();
                        result.Add(row.ToArray());
                        row.Clear();
                        break;

                    case '\n':
                        row.Add(field.ToString());
                        field.Clear();
                        result.Add(row.ToArray());
                        row.Clear();
                        break;

                    default:
                        field.Append(ch);
                        break;
                }
            }

            return result;
        }

        public CsvRow this[RowIndexKey index] => Rows[index];
        public CsvRow this[string index] => Rows[RowIndexKey.Parse(index)];
        public bool TryGetRow(RowIndexKey index, out CsvRow row) => Rows.TryGetValue(index, out row);
        public bool TryGetRow(string index, out CsvRow row) => Rows.TryGetValue(RowIndexKey.Parse(index), out row);

        /// <summary>
        /// 返回表头与类型信息的文本表示，每行一组 "Header: Type"。
        /// </summary>
        public string GetHeaderTypeInfo()
        {
            var sb = new StringBuilder();

            int count = System.Math.Min(Headers.Count, Types.Count);
            for (int i = 0; i < count; i++)
            {
                string header = Headers[i];
                string type = Types[i];
                sb.AppendLine($"{header}: {type}");
            }

            return sb.ToString();
        }
    }
}