﻿/* Copyright © 2019 Jeremy Herbison

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

namespace AudioWorks.UI.Modules.Mp4.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Mp4MetadataSettingsControlViewModel : BindableBase
    {
        const int _defaultPadding = 2048;

        readonly SettingDictionary _settings;

        public string Title { get; } = "MP4";

        public bool ConfigurePadding
        {
            get => _settings.ContainsKey("Padding");
            set
            {
                if (value)
                    _settings["Padding"] = _defaultPadding;
                else
                    _settings.Remove("Padding");

                RaisePropertyChanged(nameof(Padding));
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

        public Mp4MetadataSettingsControlViewModel(IMetadataSettingService settingService) =>
            _settings = settingService["m4a"];
    }
}
