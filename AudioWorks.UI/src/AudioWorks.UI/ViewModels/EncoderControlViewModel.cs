/* Copyright © 2020 Jeremy Herbison

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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using AudioWorks.Api;
using AudioWorks.UI.Services;
using Microsoft.Xaml.Behaviors.Core;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace AudioWorks.UI.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EncoderControlViewModel : BindableBase, IDialogAware
    {
        readonly IRegionManager _regionManager;
        readonly IEncoderSettingService _encoderSettingService;

        public string Title { get; } = "Encoding";

        public IReadOnlyList<AudioEncoderInfo> Encoders { get; } = AudioEncoderManager.GetEncoderInfo().ToArray();

        public int EncoderIndex { get; set; }

        public string Destination { get; set; } = string.Empty;

        public DelegateCommand BackCommand { get; }

        public DelegateCommand NextCommand { get; }

        public DelegateCommand CancelCommand { get; }

        public EncoderControlViewModel(
            IRegionManager regionManager,
            IEncoderSettingService encoderSettingService)
        {
            _regionManager = regionManager;
            _encoderSettingService = encoderSettingService;

            BackCommand = new (() =>
                    _regionManager.Regions["EncoderWizardRegion"].NavigationService.Journal.GoBack(),
                () => _regionManager.Regions["EncoderWizardRegion"].NavigationService.Journal.CanGoBack);
            NextCommand = new(() => NavigateNext());
            CancelCommand = new(() => CloseDialog(ButtonResult.Cancel));
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var audioFiles = parameters.GetValue<Collection<AudioFileViewModel>>("AudioFiles");

            _regionManager.RequestNavigate("EncoderWizardRegion", "EncoderSelectionControl");
        }

        public void OnDialogClosed()
        {
        }

        public bool CanCloseDialog() => true;

        public event Action<IDialogResult>? RequestClose;

        void NavigateNext()
        {
            var region = _regionManager.Regions["EncoderWizardRegion"];
            region.RequestNavigate(region.NavigationService.Journal.CurrentEntry.Uri.OriginalString
                switch
                {
                    "EncoderSelectionControl" => "EncoderDestinationControl",
                    _ => throw new NotImplementedException("Wizard is incomplete"),
                });
            BackCommand.RaiseCanExecuteChanged();
        }

        void CloseDialog(ButtonResult result) => RequestClose?.Invoke(new DialogResult(result));
    }
}
