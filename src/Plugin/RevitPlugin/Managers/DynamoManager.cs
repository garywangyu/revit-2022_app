using System;
using System.Collections.Generic;
using System.IO;

namespace RevitPlugin.Managers
{
    public class DynamoItem
    {
        public string DynamoPath { get; set; } = "";
        public string ImagePath { get; set; } = "";
        public string Instruction { get; set; } = "";
    }

    public class DynamoManager
    {
        private static DynamoManager _instance;
        public static DynamoManager Instance => _instance ?? (_instance = new DynamoManager());

        private readonly Dictionary<string, DynamoItem> _items = new Dictionary<string, DynamoItem>();
        private readonly string _configPath;

        private DynamoManager()
        {
            var dir = Path.GetDirectoryName(typeof(DynamoManager).Assembly.Location);
            _configPath = Path.Combine(dir ?? string.Empty, "dynamo_config.txt");
            Load();
        }

        public DynamoItem GetItem(string key)
        {
            if (!_items.TryGetValue(key, out var item))
            {
                item = new DynamoItem();
                _items[key] = item;
            }
            return item;
        }

        public void SetDynamoPath(string key, string path)
        {
            GetItem(key).DynamoPath = path;
            Save();
        }

        public void SetImagePath(string key, string path)
        {
            GetItem(key).ImagePath = path;
            Save();
        }

        public void SetInstruction(string key, string instruction)
        {
            GetItem(key).Instruction = instruction ?? string.Empty;
            Save();
        }

        public void Remove(string key)
        {
            if (_items.ContainsKey(key))
            {
                _items.Remove(key);
                Save();
            }
        }

        public void Export(string key, string targetPath)
        {
            var item = GetItem(key);
            if (File.Exists(item.DynamoPath))
                File.Copy(item.DynamoPath, targetPath, true);
        }

        private void Load()
        {
            if (!File.Exists(_configPath))
                return;
            foreach (var line in File.ReadAllLines(_configPath))
            {
                var parts = line.Split('|');
                if (parts.Length >= 3)
                {
                    string instruction = parts.Length >= 4 ? Decode(parts[3]) : string.Empty;
                    _items[parts[0]] = new DynamoItem
                    {
                        DynamoPath = parts[1],
                        ImagePath = parts[2],
                        Instruction = instruction
                    };
                }
            }
        }

        private void Save()
        {
            using (var sw = new StreamWriter(_configPath, false))
            {
                foreach (var kv in _items)
                {
                    string instruction = Encode(kv.Value.Instruction);
                    sw.WriteLine($"{kv.Key}|{kv.Value.DynamoPath}|{kv.Value.ImagePath}|{instruction}");
                }
            }
        }

        private static string Encode(string text)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text ?? string.Empty));
        }

        private static string Decode(string text)
        {
            try
            {
                return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(text));
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
