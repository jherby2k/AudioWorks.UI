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

namespace AudioWorks.UI.Modules.Apple.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AacEncoderSettingsControlViewModel : BindableBase
    {
        const int _defaultQuality = 9;
        const int _defaultBitRate = 128;
        const int _defaultPadding = 2048;

        readonly SettingDictionary _settings;

        public string Title { get; } = "AAC";

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

                    _settings.Remove("ControlMode");
                    RaisePropertyChanged(nameof(ControlModeIndex));
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

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public IReadOnlyList<string> ControlModes => new[] { "Constrained", "Average", "Constant" };

        public int ControlModeIndex
        {
            get
            {
                if (_settings.TryGetValue("ControlMode", out string? controlMode))
                    switch (controlMode)
                    {
                        case "Average":
                            return 1;
                        case "Constant":
                            return 2;
                    }

                return 0;
            }
            set
            {
                switch (value)
                {
                    case 1:
                        _settings["ControlMode"] = "Average";
                        break;
                    case 2:
                        _settings["ControlMode"] = "Constant";
                        break;
                    default:
                        _settings.Remove("ControlMode");
                        break;
                }
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

        public AacEncoderSettingsControlViewModel(IEncoderSettingService settingService) =>
            _settings = settingService["AppleAAC"];
    }
}
