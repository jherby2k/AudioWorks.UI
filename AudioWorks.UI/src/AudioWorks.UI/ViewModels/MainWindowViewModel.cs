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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using AudioWorks.Api;
using AudioWorks.UI.Services;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AudioWorks.UI.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public sealed class MainWindowViewModel : BindableBase
    {
        readonly Configuration _configuration;
        readonly object _lock = new();
        readonly GroupDescription _directoryGroupDescription = new PropertyGroupDescription
            { Converter = new GroupByDirectoryConverter() };
        readonly GroupDescription _albumGroupDescription = new PropertyGroupDescription
            { Converter = new GroupByAlbumConverter() };
        bool _isBusy;
        string _statusText = "Showing 0 files";
        bool _groupingDisabled;
        bool _groupByDirectory;
        bool _groupByAlbum;
        bool _showModified;
        bool _showFileName;
        bool _showPath;
        bool _showAlbum;
        bool _showAlbumArtist;
        bool _showArtist;
        bool _showComment;
        bool _showComposer;
        bool _showCoverArt;
        bool _showDay;
        bool _showGenre;
        bool _showMonth;
        bool _showTitle;
        bool _showTrackNumber;
        bool _showTrackCount;
        bool _showYear;
        bool _showAlbumGain;
        bool _showAlbumPeak;
        bool _showTrackGain;
        bool _showTrackPeak;
        bool _showBitRate;
        bool _showBitsPerSample;
        bool _showChannels;
        bool _showFormat;
        bool _showFrameCount;
        bool _showPlayLength;
        bool _showSampleRate;
        bool _showMetadataSettings;
        bool _showAnalyzerSettings;
        bool _showEncoderSettings;
        List<AudioFileViewModel> _selectedAudioFiles = new(0);

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public string StatusText
        {
            get => _statusText;
            private set => SetProperty(ref _statusText, value);
        }

        public bool GroupingDisabled
        {
            get => _groupingDisabled;
            set
            {
                if (!value) return;

                SetGroupDescriptions(null);
                SetProperty(ref _groupingDisabled, true);
                SetProperty(ref _groupByDirectory, false, nameof(GroupByDirectory));
                SetProperty(ref _groupByAlbum, false, nameof(GroupByAlbum));
                SaveSetting("GroupBy", "None");
            }
        }

        public bool GroupByDirectory
        {
            get => _groupByDirectory;
            set
            {
                if (!value) return;

                SetGroupDescriptions(_directoryGroupDescription);
                SetProperty(ref _groupByDirectory, true);
                SetProperty(ref _groupingDisabled, false, nameof(GroupingDisabled));
                SetProperty(ref _groupByAlbum, false, nameof(GroupByAlbum));
                SaveSetting("GroupBy", "Directory");
            }
        }

        public bool GroupByAlbum
        {
            get => _groupByAlbum;
            set
            {
                if (!value) return;

                SetGroupDescriptions(_albumGroupDescription);
                SetProperty(ref _groupByAlbum, true);
                SetProperty(ref _groupingDisabled, false, nameof(GroupingDisabled));
                SetProperty(ref _groupByDirectory, false, nameof(GroupByDirectory));
                SaveSetting("GroupBy", "Album");
            }
        }

        public bool ShowModified
        {
            get => _showModified;
            set
            {
                SetProperty(ref _showModified, value);
                SaveSetting("ShowModified", value);
            }
        }

        public bool ShowFileName
        {
            get => _showFileName;
            set
            {
                SetProperty(ref _showFileName, value);
                SaveSetting("ShowFileName", value);
            }
        }

        public bool ShowPath
        {
            get => _showPath;
            set
            {
                SetProperty(ref _showPath, value);
                SaveSetting("ShowPath", value);
            }
        }

        public bool ShowAlbum
        {
            get => _showAlbum;
            set
            {
                SetProperty(ref _showAlbum, value);
                SaveSetting("ShowAlbum", value);
            }
        }

        public bool ShowAlbumArtist
        {
            get => _showAlbumArtist;
            set
            {
                SetProperty(ref _showAlbumArtist, value);
                SaveSetting("ShowAlbumArtist", value);
            }
        }

        public bool ShowArtist
        {
            get => _showArtist;
            set
            {
                SetProperty(ref _showArtist, value);
                SaveSetting("ShowArtist", value);
            }
        }

        public bool ShowComment
        {
            get => _showComment;
            set
            {
                SetProperty(ref _showComment, value);
                SaveSetting("ShowComment", value);
            }
        }

        public bool ShowComposer
        {
            get => _showComposer;
            set
            {
                SetProperty(ref _showComposer, value);
                SaveSetting("ShowComposer", value);
            }
        }

        public bool ShowCoverArt
        {
            get => _showCoverArt;
            set
            {
                SetProperty(ref _showCoverArt, value);
                SaveSetting("ShowCoverArt", value);
            }
        }

        public bool ShowDay
        {
            get => _showDay;
            set
            {
                SetProperty(ref _showDay, value);
                SaveSetting("ShowDay", value);
            }
        }

        public bool ShowGenre
        {
            get => _showGenre;
            set
            {
                SetProperty(ref _showGenre, value);
                SaveSetting("ShowGenre", value);
            }
        }

        public bool ShowMonth
        {
            get => _showMonth;
            set
            {
                SetProperty(ref _showMonth, value);
                SaveSetting("ShowMonth", value);
            }
        }

        public bool ShowTitle
        {
            get => _showTitle;
            set
            {
                SetProperty(ref _showTitle, value);
                SaveSetting("ShowTitle", value);
            }
        }

        public bool ShowTrackNumber
        {
            get => _showTrackNumber;
            set
            {
                SetProperty(ref _showTrackNumber, value);
                SaveSetting("ShowTrackNumber", value);
            }
        }

        public bool ShowTrackCount
        {
            get => _showTrackCount;
            set
            {
                SetProperty(ref _showTrackCount, value);
                SaveSetting("ShowTrackCount", value);
            }
        }

        public bool ShowYear
        {
            get => _showYear;
            set
            {
                SetProperty(ref _showYear, value);
                SaveSetting("ShowYear", value);
            }
        }

        public bool ShowAlbumGain
        {
            get => _showAlbumGain;
            set
            {
                SetProperty(ref _showAlbumGain, value);
                SaveSetting("ShowAlbumGain", value);
            }
        }

        public bool ShowAlbumPeak
        {
            get => _showAlbumPeak;
            set
            {
                SetProperty(ref _showAlbumPeak, value);
                SaveSetting("ShowAlbumPeak", value);
            }
        }

        public bool ShowTrackGain
        {
            get => _showTrackGain;
            set
            {
                SetProperty(ref _showTrackGain, value);
                SaveSetting("ShowTrackGain", value);
            }
        }

        public bool ShowTrackPeak
        {
            get => _showTrackPeak;
            set
            {
                SetProperty(ref _showTrackPeak, value);
                SaveSetting("ShowTrackPeak", value);
            }
        }

        public bool ShowBitRate
        {
            get => _showBitRate;
            set
            {
                SetProperty(ref _showBitRate, value);
                SaveSetting("ShowBitRate", value);
            }
        }

        public bool ShowBitsPerSample
        {
            get => _showBitsPerSample;
            set
            {
                SetProperty(ref _showBitsPerSample, value);
                SaveSetting("ShowBitsPerSample", value);
            }
        }

        public bool ShowChannels
        {
            get => _showChannels;
            set
            {
                SetProperty(ref _showChannels, value);
                SaveSetting("ShowChannels", value);
            }
        }

        public bool ShowFormat
        {
            get => _showFormat;
            set
            {
                SetProperty(ref _showFormat, value);
                SaveSetting("ShowFormat", value);
            }
        }

        public bool ShowFrameCount
        {
            get => _showFrameCount;
            set
            {
                SetProperty(ref _showFrameCount, value);
                SaveSetting("ShowFrameCount", value);
            }
        }

        public bool ShowPlayLength
        {
            get => _showPlayLength;
            set
            {
                SetProperty(ref _showPlayLength, value);
                SaveSetting("ShowPlayLength", value);
            }
        }

        public bool ShowSampleRate
        {
            get => _showSampleRate;
            set
            {
                SetProperty(ref _showSampleRate, value);
                SaveSetting("ShowSampleRate", value);
            }
        }

        public bool ShowMetadataSettings
        {
            get => _showMetadataSettings;
            set
            {
                SetProperty(ref _showMetadataSettings, value);
                if (!value) return;

                SetProperty(ref _showAnalyzerSettings, false, nameof(ShowAnalyzerSettings));
                SetProperty(ref _showEncoderSettings, false, nameof(ShowEncoderSettings));
            }
        }

        public bool ShowAnalyzerSettings
        {
            get => _showAnalyzerSettings;
            set
            {
                SetProperty(ref _showAnalyzerSettings, value);
                if (!value) return;

                SetProperty(ref _showMetadataSettings, false, nameof(ShowMetadataSettings));
                SetProperty(ref _showEncoderSettings, false, nameof(ShowEncoderSettings));
            }
        }

        public bool ShowEncoderSettings
        {
            get => _showEncoderSettings;
            set
            {
                SetProperty(ref _showEncoderSettings, value);
                if (!value) return;

                SetProperty(ref _showMetadataSettings, false, nameof(ShowMetadataSettings));
                SetProperty(ref _showAnalyzerSettings, false, nameof(ShowAnalyzerSettings));
            }
        }

        public IReadOnlyList<AudioAnalyzerInfo> Analyzers { get; } = AudioAnalyzerManager.GetAnalyzerInfo().ToArray();

        public ObservableCollection<AudioFileViewModel> AudioFiles { get; }

        public DelegateCommand<IList> SelectionChangedCommand { get; }

        public DelegateCommand OpenFilesCommand { get; }

        public DelegateCommand OpenDirectoryCommand { get; }

        public DelegateCommand<KeyEventArgs> KeyDownCommand { get; }

        public DelegateCommand<DragEventArgs> DropCommand { get; }

        public DelegateCommand EditSelectionCommand { get; }

        public DelegateCommand RevertSelectionCommand { get; }

        public DelegateCommand RevertModifiedCommand { get; }

        public DelegateCommand SaveSelectionCommand { get; }

        public DelegateCommand SaveModifiedCommand { get; }

        public DelegateCommand SaveAllCommand { get; }

        public DelegateCommand RemoveSelectionCommand { get; }

        public DelegateCommand<string> AnalyzeAllCommand { get; }

        public DelegateCommand EncodeAllCommand { get; }

        public DelegateCommand<CancelEventArgs> ExitCommand { get; }

        public MainWindowViewModel(
            IFileSelectionService fileSelectionService,
            IDialogService prismDialogService,
            IDialogCoordinator metroDialogCoordinator)
        {
            AudioFiles = new();
            BindingOperations.EnableCollectionSynchronization(AudioFiles, _lock);

            _configuration = InitializeConfiguration();
            var settings = _configuration.AppSettings.Settings;

            switch (settings["GroupBy"].Value.ToUpperInvariant())
            {
                case "DIRECTORY":
                    GroupByDirectory = true;
                    break;
                case "ALBUM":
                    GroupByAlbum = true;
                    break;
                default:
                    GroupingDisabled = true;
                    break;
            }

            if (bool.TryParse(settings["ShowModified"]?.Value, out var showModified) && showModified)
                ShowModified = true;
            if (bool.TryParse(settings["ShowFileName"]?.Value, out var showFileName) && showFileName)
                ShowFileName = true;
            if (bool.TryParse(settings["ShowPath"]?.Value, out var showPath) && showPath)
                ShowPath = true;
            if (bool.TryParse(settings["ShowAlbum"]?.Value, out var showAlbum) && showAlbum)
                ShowAlbum = true;
            if (bool.TryParse(settings["ShowAlbumArtist"]?.Value, out var showAlbumArtist) && showAlbumArtist)
                ShowAlbumArtist = true;
            if (bool.TryParse(settings["ShowArtist"]?.Value, out var showArtist) && showArtist)
                ShowArtist = true;
            if (bool.TryParse(settings["ShowComment"]?.Value, out var showComment) && showComment)
                ShowComment = true;
            if (bool.TryParse(settings["ShowComposer"]?.Value, out var showComposer) && showComposer)
                ShowComposer = true;
            if (bool.TryParse(settings["ShowCoverArt"]?.Value, out var showCoverArt) && showCoverArt)
                ShowCoverArt = true;
            if (bool.TryParse(settings["ShowDay"]?.Value, out var showDay) && showDay)
                ShowDay = true;
            if (bool.TryParse(settings["ShowGenre"]?.Value, out var showGenre) && showGenre)
                ShowGenre = true;
            if (bool.TryParse(settings["ShowMonth"]?.Value, out var showMonth) && showMonth)
                ShowMonth = true;
            if (bool.TryParse(settings["ShowTitle"]?.Value, out var showTitle) && showTitle)
                ShowTitle = true;
            if (bool.TryParse(settings["ShowTrackNumber"]?.Value, out var showTrackNumber) && showTrackNumber)
                ShowTrackNumber = true;
            if (bool.TryParse(settings["ShowTrackCount"]?.Value, out var showTrackCount) && showTrackCount)
                ShowTrackCount = true;
            if (bool.TryParse(settings["ShowYear"]?.Value, out var showYear) && showYear)
                ShowYear = true;
            if (bool.TryParse(settings["ShowAlbumGain"]?.Value, out var showAlbumGain) && showAlbumGain)
                ShowAlbumGain = true;
            if (bool.TryParse(settings["ShowAlbumPeak"]?.Value, out var showAlbumPeak) && showAlbumPeak)
                ShowAlbumPeak = true;
            if (bool.TryParse(settings["ShowTrackGain"]?.Value, out var showTrackGain) && showTrackGain)
                ShowTrackGain = true;
            if (bool.TryParse(settings["ShowTrackPeak"]?.Value, out var showTrackPeak) && showTrackPeak)
                ShowTrackPeak = true;
            if (bool.TryParse(settings["ShowBitRate"]?.Value, out var showBitRate) && showBitRate)
                ShowBitRate = true;
            if (bool.TryParse(settings["ShowBitsPerSample"]?.Value, out var showBitsPerSample) && showBitsPerSample)
                ShowBitsPerSample = true;
            if (bool.TryParse(settings["ShowChannels"]?.Value, out var showChannels) && showChannels)
                ShowChannels = true;
            if (bool.TryParse(settings["ShowFormat"]?.Value, out var showFormat) && showFormat)
                ShowFormat = true;
            if (bool.TryParse(settings["ShowFrameCount"]?.Value, out var showFrameCount) && showFrameCount)
                ShowFrameCount = true;
            if (bool.TryParse(settings["ShowPlayLength"]?.Value, out var showPlayLength) && showPlayLength)
                ShowPlayLength = true;
            if (bool.TryParse(settings["ShowSampleRate"]?.Value, out var showSampleRate) && showSampleRate)
                ShowSampleRate = true;

            AudioFiles.CollectionChanged += (_, e) =>
            {
                UpdateStatusText();

                if (e.NewItems != null)
                    foreach (var newItem in e.NewItems)
                    {
                        if (newItem == null) continue;

                        ((AudioFileViewModel) newItem).Metadata.PropertyChanged += Metadata_PropertyChanged;
                        SaveAllCommand?.RaiseCanExecuteChanged();
                    }

                if (e.OldItems != null)
                    foreach (var oldItem in e.OldItems)
                    {
                        if (oldItem == null) continue;

                        ((AudioFileViewModel) oldItem).Metadata.PropertyChanged -= Metadata_PropertyChanged;
                        RevertModifiedCommand!.RaiseCanExecuteChanged();
                        SaveModifiedCommand!.RaiseCanExecuteChanged();
                        SaveAllCommand!.RaiseCanExecuteChanged();
                        AnalyzeAllCommand!.RaiseCanExecuteChanged();
                        EncodeAllCommand!.RaiseCanExecuteChanged();
                    }
            };

            SelectionChangedCommand = new(selectedItems =>
            {
                _selectedAudioFiles = selectedItems.Cast<AudioFileViewModel>().ToList();
                EditSelectionCommand!.RaiseCanExecuteChanged();
                RevertSelectionCommand!.RaiseCanExecuteChanged();
                SaveSelectionCommand!.RaiseCanExecuteChanged();
                RemoveSelectionCommand!.RaiseCanExecuteChanged();
            });

            OpenFilesCommand = new DelegateCommand(async () =>
                {
                    IsBusy = true;
                    await AddFilesAsync(fileSelectionService.SelectFiles());
                    IsBusy = false;
                }, () => !IsBusy)
                .ObservesProperty(() => IsBusy);

            OpenDirectoryCommand = new DelegateCommand(async () =>
                {
                    IsBusy = true;
                    await AddFilesAsync(GetFilesRecursively(fileSelectionService.SelectDirectory()));
                    IsBusy = false;
                }, () => !IsBusy)
                .ObservesProperty(() => IsBusy);

            KeyDownCommand = new(e =>
            {
                if (e.Key != Key.Delete) return;

                if (RemoveSelectionCommand!.CanExecute())
                    RemoveSelectionCommand.Execute();
                e.Handled = true;
            });

            DropCommand = new DelegateCommand<DragEventArgs>(async e =>
                {
                    IsBusy = true;
                    await AddFilesAsync(((DataObject) e.Data).GetFileDropList().Cast<string>().SelectMany(path =>
                        Directory.Exists(path) ? GetFilesRecursively(path) : new[] { path }));
                    IsBusy = false;
                }, _ => !IsBusy)
                .ObservesProperty(() => IsBusy);

            EditSelectionCommand = new(() =>
                    prismDialogService.ShowDialog("EditControl",
                        new DialogParameters { { "AudioFiles", _selectedAudioFiles } }, null),
                () => _selectedAudioFiles.Count > 0);

            RevertSelectionCommand = new DelegateCommand(() =>
                {
                    IsBusy = true;
                    foreach (var audioFile in _selectedAudioFiles.Where(audioFile =>
                        audioFile.RevertCommand.CanExecute()))
                        audioFile.RevertCommand.Execute();
                    IsBusy = false;
                }, () => !IsBusy && _selectedAudioFiles.Any(audioFile => audioFile.RevertCommand.CanExecute()))
                .ObservesProperty(() => IsBusy);

            RevertModifiedCommand = new DelegateCommand(() =>
                {
                    IsBusy = true;
                    foreach (var audioFile in AudioFiles.Where(audioFile => audioFile.RevertCommand.CanExecute()))
                        audioFile.RevertCommand.Execute();
                    IsBusy = false;
                }, () => !IsBusy && AudioFiles.Any(audioFile => audioFile.RevertCommand.CanExecute()))
                .ObservesProperty(() => IsBusy);

            SaveSelectionCommand = new DelegateCommand(() =>
                {
                    IsBusy = true;
                    foreach (var audioFile in _selectedAudioFiles.Where(audioFile =>
                        audioFile.SaveCommand.CanExecute()))
                        audioFile.SaveCommand.Execute();
                    IsBusy = false;
                }, () => !IsBusy && _selectedAudioFiles.Count > 0)
                .ObservesProperty(() => IsBusy);

            SaveModifiedCommand = new DelegateCommand(() =>
                {
                    IsBusy = true;
                    foreach (var audioFile in AudioFiles.Where(audioFile =>
                        audioFile.Metadata.Modified && audioFile.SaveCommand.CanExecute()))
                        audioFile.SaveCommand.Execute();
                    IsBusy = false;
                }, () => !IsBusy && AudioFiles.Any(audioFile =>
                    audioFile.Metadata.Modified && audioFile.SaveCommand.CanExecute()))
                .ObservesProperty(() => IsBusy);

            SaveAllCommand = new DelegateCommand(async () =>
                {
                    IsBusy = true;
                    if (await metroDialogCoordinator.ShowMessageAsync(this, "Are You Sure?",
                            "All files will be re-written according to the current metadata encoder settings.",
                            MessageDialogStyle.AffirmativeAndNegative) == MessageDialogResult.Affirmative)
                        foreach (var audioFile in AudioFiles.Where(audioFile => audioFile.SaveCommand.CanExecute()))
                            audioFile.SaveCommand.Execute();
                    IsBusy = false;
                }, () => !IsBusy && AudioFiles.Count > 0)
                .ObservesProperty(() => IsBusy);

            RemoveSelectionCommand = new(async () =>
            {
                var modifications = _selectedAudioFiles.Count(audioFile => audioFile.Metadata.Modified);
                if (modifications > 0)
                    switch (await metroDialogCoordinator.ShowMessageAsync(this, "Unsaved Changes",
                        $"There are {modifications} unsaved change(s) in the files you're removing. Do you want to save them now?",
                        MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                        new()
                        {
                            AffirmativeButtonText = "Yes",
                            NegativeButtonText = "No",
                            FirstAuxiliaryButtonText = "Cancel",
                            DefaultButtonFocus = MessageDialogResult.FirstAuxiliary
                        }))
                    {
                        case MessageDialogResult.Affirmative:
                            SaveSelectionCommand.Execute();
                            break;
                        case MessageDialogResult.FirstAuxiliary:
                            return;
                    }

                foreach (var audioFile in _selectedAudioFiles)
                    AudioFiles.Remove(audioFile);
            }, () => _selectedAudioFiles.Count > 0);

            AnalyzeAllCommand = new DelegateCommand<string>(name =>
                        prismDialogService.ShowDialog("AnalysisControl",
                            new DialogParameters
                            {
                                { "Name", name },
                                { "AudioFiles", CollectionViewSource.GetDefaultView(AudioFiles) }
                            }, result =>
                            {
                                IsBusy = true;
                                if (result.Result == ButtonResult.OK)
                                {
                                    SystemSounds.Beep.Play();

                                    foreach (var audioFile in AudioFiles)
                                        audioFile.Metadata.Refresh();
                                }
                                else
                                {
                                    foreach (var audioFile in AudioFiles.Where(audioFile =>
                                        audioFile.RevertCommand.CanExecute()))
                                        audioFile.RevertCommand.Execute();
                                }
                                IsBusy = false;
                            }),
                    _ => !IsBusy && AudioFiles.Count > 0)
                .ObservesProperty(() => IsBusy);

            EncodeAllCommand = new DelegateCommand(() =>
                        prismDialogService.ShowDialog("EncoderControl",
                            new DialogParameters { { "AudioFiles", AudioFiles } }, null),
                    () => !IsBusy && AudioFiles.Count > 0)
                .ObservesProperty(() => IsBusy);

            ExitCommand = new(e =>
            {
                var modifications = AudioFiles.Count(audioFile => audioFile.Metadata.Modified);
                if (modifications == 0) return;

                switch (metroDialogCoordinator.ShowModalMessageExternal(this, "Unsaved Changes",
                    $"There are {modifications} unsaved change(s). Do you want to save them now?",
                    MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary,
                    new()
                    {
                        AffirmativeButtonText = "Yes",
                        NegativeButtonText = "No",
                        FirstAuxiliaryButtonText = "Cancel",
                        DefaultButtonFocus = MessageDialogResult.FirstAuxiliary
                    }))
                {
                    case MessageDialogResult.Affirmative:
                        SaveAllCommand.Execute();
                        break;
                    case MessageDialogResult.FirstAuxiliary:
                        e.Cancel = true;
                        break;
                }
            });
        }

        void UpdateStatusText()
        {
            var result = new StringBuilder($"Showing {AudioFiles.Count} file");
            if (AudioFiles.Count != 1)
                result.Append('s');

            var modifications = AudioFiles.Count(audioFile => audioFile.Metadata.Modified);
            if (modifications > 0)
                result.Append($" ({modifications} modified)");

            StatusText = result.ToString();
        }

        void SetGroupDescriptions(GroupDescription? groupDescription)
        {
            var view = CollectionViewSource.GetDefaultView(AudioFiles);
            view.GroupDescriptions.Clear();
            if (groupDescription != null)
                view.GroupDescriptions.Add(groupDescription);
        }

        void SaveSetting(string key, string value)
        {
            var settings = _configuration.AppSettings.Settings;
            if (settings[key] == null)
                settings.Add(key, value);
            else if (!settings[key].Value.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                settings[key].Value = value;
                _configuration.Save(ConfigurationSaveMode.Minimal);
                ConfigurationManager.RefreshSection(_configuration.AppSettings.SectionInformation.Name);
            }
        }

        void SaveSetting(string key, bool value) => SaveSetting(key, value.ToString());

        void Metadata_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            UpdateStatusText();

            RevertSelectionCommand.RaiseCanExecuteChanged();
            RevertModifiedCommand.RaiseCanExecuteChanged();
            SaveModifiedCommand.RaiseCanExecuteChanged();
        }

        async Task AddFilesAsync(IEnumerable<string> filePaths) =>
            await Task.Run(() =>
            {
                var validExtensions = AudioFileManager.GetFormatInfo().Select(info => info.Extension).ToList();
                var existingFiles = AudioFiles.Select(audioFile => audioFile.Path);

                foreach (var newFile in filePaths.Where(file =>
                        validExtensions.Contains(new FileInfo(file).Extension, StringComparer.OrdinalIgnoreCase) &&
                        !existingFiles.Contains(file, StringComparer.OrdinalIgnoreCase))
                    .Select(file => new AudioFileViewModel(new TaggedAudioFile(file))))
                    AudioFiles.Add(newFile);
            });

        static Configuration InitializeConfiguration()
        {
            var defaultConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var userConfigFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "AudioWorks", "UI", "Settings", "App.config");

            if (!File.Exists(userConfigFile))
                defaultConfiguration.SaveAs(userConfigFile);

            return ConfigurationManager.OpenMappedExeConfiguration(
                new() { ExeConfigFilename = userConfigFile }, ConfigurationUserLevel.None);
        }

        static IEnumerable<string> GetFilesRecursively(string directoryPath) =>
            string.IsNullOrEmpty(directoryPath)
                ? Enumerable.Empty<string>()
                : Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories);
    }
}
