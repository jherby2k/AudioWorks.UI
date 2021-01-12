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

using AudioWorks.Common;
using AudioWorks.UI.Services;
using Prism.Mvvm;

namespace AudioWorks.UI.Modules.Flac.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class FlacEncoderSettingsControlViewModel : BindableBase
    {
        const int _defaultCompressionLevel = 5;
        const int _defaultSeekPointInterval = 10;
        const int _defaultPadding = 8192;

        readonly SettingDictionary _settings;

        public string Title { get; } = "FLAC";

        public int CompressionLevel
        {
            get => _settings.TryGetValue("CompressionLevel", out int seekPointInterval)
                ? seekPointInterval
                : _defaultCompressionLevel;
            set
            {
                if (value != _defaultCompressionLevel)
                    _settings["CompressionLevel"] = value;
                else
                    _settings.Remove("CompressionLevel");
                RaisePropertyChanged();
            }
        }

        public int SeekPointInterval
        {
            get => _settings.TryGetValue("SeekPointInterval", out int seekPointInterval)
                ? seekPointInterval
                : _defaultSeekPointInterval;
            set
            {
                if (value != _defaultSeekPointInterval)
                    _settings["SeekPointInterval"] = value;
                else
                    _settings.Remove("SeekPointInterval");
                RaisePropertyChanged();
            }
        }

        public int Padding
        {
            get => _settings.TryGetValue("Padding", out int padding)
                ? padding
                : _defaultPadding;
            set
            {
                if (value != _defaultPadding)
                    _settings["Padding"] = value;
                else
                    _settings.Remove("Padding");
                RaisePropertyChanged();
            }
        }

        public FlacEncoderSettingsControlViewModel(IEncoderSettingService settingService) =>
            _settings = settingService["FLAC"];
    }
}
