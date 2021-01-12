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

namespace AudioWorks.UI.Modules.ReplayGain.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ReplayGainAnalysisSettingsControlViewModel : BindableBase
    {
        readonly SettingDictionary _settings;

        public string Title { get; } = "ReplayGain";

        [SuppressMessage("Performance", "CA1822:Mark members as static")]
        public IReadOnlyList<string> PeakAnalysisTypes => new[] { "Simple", "Interpolated" };

        public int PeakAnalysisTypeIndex
        {
            get => _settings.TryGetValue("PeakAnalysis", out string? peakAnalysis) &&
                   peakAnalysis!.Equals("Interpolated", StringComparison.Ordinal)
                ? 1
                : 0;
            set
            {
                if (value == 1)
                    _settings["PeakAnalysis"] = "Interpolated";
                else
                    _settings.Remove("PeakAnalysis");
                RaisePropertyChanged();
            }
        }

        public ReplayGainAnalysisSettingsControlViewModel(IAnalysisSettingService settingService) =>
            _settings = settingService["ReplayGain"];
    }
}
