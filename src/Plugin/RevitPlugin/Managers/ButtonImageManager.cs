using System.Collections.Generic;
using System.IO;

namespace RevitPlugin.Managers
{
    public class ButtonImageManager
    {
        private static ButtonImageManager? _instance;
        public static ButtonImageManager Instance => _instance ??= new ButtonImageManager();

        private readonly Dictionary<string, string> _paths = new Dictionary<string, string>();
        private readonly string _configPath;

        private ButtonImageManager()
        {
            var dir = Path.GetDirectoryName(typeof(ButtonImageManager).Assembly.Location);
            _configPath = Path.Combine(dir ?? string.Empty, "button_images.txt");
            Load();
        }

        private void Load()
        {
            if (!File.Exists(_configPath))
                return;
            foreach (var line in File.ReadAllLines(_configPath))
            {
                var parts = line.Split('|');
                if (parts.Length == 2)
                    _paths[parts[0]] = parts[1];
            }
        }

        private void Save()
        {
            using var sw = new StreamWriter(_configPath, false);
            foreach (var kv in _paths)
                sw.WriteLine($"{kv.Key}|{kv.Value}");
        }

        public string GetPath(string key)
        {
            return _paths.TryGetValue(key, out var path) ? path : string.Empty;
        }

        public void SetPath(string key, string path)
        {
            _paths[key] = path;
            Save();
        }

        public void Remove(string key)
        {
            if (_paths.Remove(key))
                Save();
        }
    }
}
