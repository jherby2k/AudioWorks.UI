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

namespace AudioWorks.UI.Modules.Opus.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class OpusEncoderSettingsControlViewModel : BindableBase
    {
        const int _defaultBitRate = 128;

        readonly SettingDictionary _settings;

        public string Title { get; } = "Opus";

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
        public IReadOnlyList<string> ControlModes => new[] { "Variable", "Constrained", "Constant" };

        public int ControlModeIndex
        {
            get
            {
                if (_settings.TryGetValue("ControlMode", out string? controlMode))
                    switch (controlMode)
                    {
                        case "Variable":
                            return 0;
                        case "Constant":
                            return 2;
                    }

                return 1;
            }
            set
            {
                switch (value)
                {
                    case 0:
                        _settings["ControlMode"] = "Variable";
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
        public IReadOnlyList<string> SignalTypes => new[] { "Music", "Speech" };

        public int SignalTypeIndex
        {
            get => _settings.TryGetValue("SignalType", out string? signalType) &&
                   signalType!.Equals("Speech", StringComparison.Ordinal)
                ? 1
                : 0;
            set
            {
                if (value == 1)
                    _settings["SignalType"] = "Speech";
                else
                    _settings.Remove("SignalType");
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

        public OpusEncoderSettingsControlViewModel(IEncoderSettingService settingService) =>
            _settings = settingService["Opus"];
    }
}
