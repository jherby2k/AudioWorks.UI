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

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AudioWorks.Common;
using AudioWorks.UI.Services;
using Prism.Mvvm;

namespace AudioWorks.UI.Modules.Id3.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Id3MetadataSettingsControlViewModel : BindableBase
    {
        const int _defaultPadding = 2048;

        readonly SettingDictionary _settings;

        public string Title { get; } = "ID3";

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public IReadOnlyList<string> Versions => new[] { "2.3", "2.4" };

        public int VersionIndex
        {
            get => _settings.TryGetValue("TagVersion", out string? tagVersion) &&
                   tagVersion!.Equals("2.4", StringComparison.Ordinal)
                ? 1
                : 0;
            set
            {
                if (value == 1)
                    _settings["TagVersion"] = "2.4";
                else
                {
                    _settings.Remove("TagVersion");

                    // UTF-8 isn't supported with 2.3
                    if (EncodingIndex == 2)
                        EncodingIndex = 1;
                }

                RaisePropertyChanged();
            }
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public IReadOnlyList<string> Encodings => new[] { "Latin-1", "UTF-16", "UTF-8" };

        public int EncodingIndex
        {
            get
            {
                if (_settings.TryGetValue("TagEncoding", out string? tagVersion))
                    switch (tagVersion)
                    {
                        case "UTF-16":
                            return 1;
                        case "UTF-8":
                            return 2;
                    }

                return 0;
            }
            set
            {
                switch (value)
                {
                    case 1:
                        _settings["TagEncoding"] = "UTF-16";
                        break;
                    case 2:
                        _settings["TagEncoding"] = "UTF-8";

                        // UTF-8 isn't supported with 2.3
                        VersionIndex = 1;
                        break;
                    default:
                        _settings.Remove("TagEncoding");
                        break;
                }

                RaisePropertyChanged();
            }
        }

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

        public Id3MetadataSettingsControlViewModel(IMetadataSettingService settingService) =>
            _settings = settingService["mp3"];
    }
}
