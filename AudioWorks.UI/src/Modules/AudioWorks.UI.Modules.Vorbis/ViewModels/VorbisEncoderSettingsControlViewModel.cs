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
using System.Diagnostics.CodeAnalysis;
using AudioWorks.Common;
using AudioWorks.UI.Services;
using Prism.Mvvm;

namespace AudioWorks.UI.Modules.Vorbis.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class VorbisEncoderSettingsControlViewModel : BindableBase
    {
        const int _defaultQuality = 5;
        const int _defaultBitRate = 128;

        readonly SettingDictionary _settings;

        public string Title { get; } = "Vorbis";

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
                    _settings.Remove("Quality");
                    RaisePropertyChanged(nameof(Quality));

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

        public int Quality
        {
            get => _settings.TryGetValue("Quality", out int vbrQuality)
                ? vbrQuality
                : _defaultQuality;
            set
            {
                if (value != _defaultQuality)
                    _settings["Quality"] = value;
                else
                    _settings.Remove("Quality");
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

        public VorbisEncoderSettingsControlViewModel(IEncoderSettingService settingService) =>
            _settings = settingService["Vorbis"];
    }
}
