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

using AudioWorks.Api;
using AudioWorks.UI.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using Prism.Commands;
using Prism.Mvvm;

namespace AudioWorks.UI.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class AnalysisControlViewModel : BindableBase, IDialogAware
    {
        readonly CancellationTokenSource _cancellationSource = new();
        readonly IAnalysisSettingService _analysisSettingService;
        string _description = string.Empty;
        int _progress;

        public string Title { get; set; } = string.Empty;

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public int Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public DelegateCommand CancelCommand { get; }

        public AnalysisControlViewModel(
            IAnalysisSettingService analysisSettingService)
        {
            _analysisSettingService = analysisSettingService;

            CancelCommand = new(() => CloseDialog(ButtonResult.Cancel));
        }

        public async void OnDialogOpened(IDialogParameters parameters)
        {
            var name = parameters.GetValue<string>("Name");
            var audioFiles = parameters.GetValue<ICollectionView>("AudioFiles");
            var audioFilesCollection = (Collection<AudioFileViewModel>) audioFiles.SourceCollection;

            Title = $"Performing {name} Analysis";
            Description = $"Analyzing {audioFilesCollection.Count} files...";

            var analyzer = new AudioFileAnalyzer(name, _analysisSettingService[name]);
            var totalFrames = (double) audioFilesCollection.Sum(audioFile => audioFile.Info.FrameCount);
            var previousFramesCompleted = 0L;
            var lastPercentComplete = 0;

            var progress = new Progress<ProgressToken>(token =>
            {
                var percentComplete =
                    (int) Math.Round((token.FramesCompleted + previousFramesCompleted) / totalFrames * 100);

                // Avoid reporting progress when nothing has changed
                if (percentComplete <= lastPercentComplete) return;
                lastPercentComplete = percentComplete;
                Progress = Math.Min(percentComplete, 100);
            });

            if (audioFiles.Groups != null)
                foreach (var group in audioFiles.Groups.Cast<CollectionViewGroup>())
                {
                    var groupAudioFiles = group.Items.Cast<AudioFileViewModel>()
                        .Select(viewModel => viewModel.AudioFile).ToArray();
                    await analyzer.AnalyzeAsync(groupAudioFiles, _cancellationSource.Token, progress);
                    previousFramesCompleted += groupAudioFiles.Sum(audioFile => audioFile.Info.FrameCount);
                }
            else
                await analyzer.AnalyzeAsync(
                    audioFilesCollection.Select(viewModel => viewModel.AudioFile),
                    _cancellationSource.Token,
                    progress);

            CloseDialog(_cancellationSource.IsCancellationRequested ? ButtonResult.Cancel : ButtonResult.OK);
        }

        public void OnDialogClosed()
        {
        }

        public bool CanCloseDialog() => true;

        public event Action<IDialogResult>? RequestClose;

        void CloseDialog(ButtonResult result) => RequestClose?.Invoke(new DialogResult(result));
    }
}
