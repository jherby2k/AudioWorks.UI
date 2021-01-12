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
using AudioWorks.Common;
using AudioWorks.UI.Services;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using IO = System.IO;

namespace AudioWorks.UI.ViewModels
{
    public sealed class AudioFileViewModel : BindableBase
    {
        internal ITaggedAudioFile AudioFile { get; }

        public string Path => AudioFile.Path;

        public string FileName => IO.Path.GetFileName(AudioFile.Path);

        public AudioInfo Info => AudioFile.Info;

        public AudioMetadataViewModel Metadata { get; }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand RevertCommand { get; }

        public AudioFileViewModel(ITaggedAudioFile audioFile)
        {
            AudioFile = audioFile;
            Metadata = new(audioFile.Metadata);
            Metadata.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName != null && e.PropertyName.Equals("Modified", StringComparison.Ordinal))
                    RevertCommand!.RaiseCanExecuteChanged();
            };

            SaveCommand = new(() =>
            {
                AudioFile.SaveMetadata(
                    ContainerLocator.Current.Resolve<IMetadataSettingService>()
                        [IO.Path.GetExtension(Path).TrimStart('.')]);
                Metadata.UpdateModel(AudioFile.Metadata);
            });

            RevertCommand = new(() =>
            {
                AudioFile.LoadMetadata();
                Metadata.UpdateModel(AudioFile.Metadata);
            }, () => Metadata.Modified);
        }
    }
}
