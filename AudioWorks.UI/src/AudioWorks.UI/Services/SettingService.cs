/* Copyright © 2019 Jeremy Herbison

This file is part of AudioWorks.

AudioWorks is free software: you can redistribute it and/or modify it under the terms of the GNU Affero General Public
License as published by the Free Software Foundation, either version 3 of the License, or (at your option) any later
version.

AudioWorks is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU Affero General Public License for more
details.

You should have received a copy of the GNU Affero General Public License along with AudioWorks. If not, see
<https://www.gnu.org/licenses/>. */

using System.Collections.Generic;
using System.IO;
using AudioWorks.Common;
using Newtonsoft.Json;

namespace AudioWorks.UI.Services
{
    public abstract class SettingService : ISettingService
    {
        readonly string _path;
        readonly Dictionary<string, SettingDictionary> _settingDictionaries;

        internal SettingService(string path)
        {
            _path = path;
            _settingDictionaries = LoadFromDisk(path);
        }

        public SettingDictionary this[string key]
        {
            get
            {
                if (_settingDictionaries.TryGetValue(key, out var result))
                    return result;

                result = new();
                _settingDictionaries.Add(key, result);
                return result;
            }
        }

        public void Save()
        {
            Directory.CreateDirectory(_path);
            foreach (var (key, settings) in _settingDictionaries)
                using (var writer = new StreamWriter(Path.Combine(_path, $"{key}.json")))
                    writer.Write(JsonConvert.SerializeObject(settings));
        }

        static Dictionary<string, SettingDictionary> LoadFromDisk(string path)
        {
            var result = new Dictionary<string, SettingDictionary>();
            if (Directory.Exists(path))
                foreach (var file in Directory.EnumerateFiles(path, "*.json"))
                    using (var reader = new StreamReader(file))
                        result[Path.GetFileNameWithoutExtension(file)] =
                            JsonConvert.DeserializeObject<SettingDictionary>(reader.ReadToEnd(),
                                new SettingDictionaryConverter());
            return result;
        }
    }
}