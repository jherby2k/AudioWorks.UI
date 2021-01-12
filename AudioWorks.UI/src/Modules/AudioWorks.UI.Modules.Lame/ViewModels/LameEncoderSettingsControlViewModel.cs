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

namespace AudioWorks.UI.Modules.Lame.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LameEncoderSettingsControlViewModel : BindableBase
    {
        const int _defaultQuality = 3;
        const int _defaultBitRate = 128;
        const int _defaultPadding = 2048;

        readonly SettingDictionary _settings;

        public string Title { get; } = "MP3";

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public IReadOnlyList<string> Modes => new[] { "Quality", "File Size" };

        public int ModeIndex
        {
            get => _settings.ContainsKey("BitRate") ? 1 : 0;
            set
            {
                if (value == 0)
                {
                    _settings.Remove("BitRate");

                    _settings.Remove("ForceCBR");
                    RaisePropertyChanged(nameof(ForceCbr));
                }
                else
                {
                    _settings.Remove("VBRQuality");
                    RaisePropertyChanged(nameof(VbrQuality));

                    _settings["BitRate"] = _defaultBitRate;
                }

                RaisePropertyChanged(nameof(BitRate));
                RaisePropertyChanged(nameof(QualityEnabled));
                RaisePropertyChanged(nameof(BitRateEnabled));
                RaisePropertyChanged();
            }
        }

        public bool QualityEnabled => ModeIndex == 0;

        public bool BitRateEnabled => ModeIndex == 1;

        public int VbrQuality
        {
            get => _settings.TryGetValue("VBRQuality", out int vbrQuality)
                ? vbrQuality
                : _defaultQuality;
            set
            {
                if (value != _defaultQuality)
                    _settings["VBRQuality"] = value;
                else
                    _settings.Remove("VBRQuality");
                RaisePropertyChanged();
            }
        }

        public int BitRate
        {
            get => _settings.TryGetValue("BitRate", out int bitRate)
                ? bitRate
                : _defaultBitRate;
            set
            {
                _settings["BitRate"] = value;
                RaisePropertyChanged();
            }
        }

        public bool ForceCbr
        {
            get => _settings.TryGetValue("ForceCBR", out bool forceCbr) && forceCbr;
            set
            {
                if (value)
                    _settings["ForceCBR"] = true;
                else
                    _settings.Remove("ForceCBR");
                RaisePropertyChanged();
            }
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public IReadOnlyList<string> ApplyGainValues => new[] { "None", "Track", "Album" };

        public int ApplyGainIndex
        {
            get
            {
                if (_settings.TryGetValue("ApplyGain", out string? applyGain))
                    switch (applyGain)
                    {
                        case "Track":
                            return 1;
                        case "Album":
                            return 2;
                    }

                return 0;
            }
            set
            {
                switch (value)
                {
                    case 1:
                        _settings["ApplyGain"] = "Track";
                        break;
                    case 2:
                        _settings["ApplyGain"] = "Album";
                        break;
                    default:
                        _settings.Remove("ApplyGain");
                        break;
                }
                RaisePropertyChanged();
            }
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public IReadOnlyList<string> TagVersions => new[] { "2.3", "2.4" };

        public int TagVersionIndex
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
                    if (TagEncodingIndex == 2)
                        TagEncodingIndex = 1;
                }

                RaisePropertyChanged();
            }
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public IReadOnlyList<string> TagEncodings => new[] { "Latin-1", "UTF-16", "UTF-8" };

        public int TagEncodingIndex
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
                        TagVersionIndex = 1;
                        break;
                    default:
                        _settings.Remove("TagEncoding");
                        break;
                }

                RaisePropertyChanged();
            }
        }

        public int TagPadding
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

        public LameEncoderSettingsControlViewModel(IEncoderSettingService settingService) =>
            _settings = settingService["LameMP3"];
    }
}
