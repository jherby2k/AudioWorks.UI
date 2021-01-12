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
using System.Linq;
using System.Reflection;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace AudioWorks.UI.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class EditControlViewModel : BindableBase, IDialogAware
    {
        List<AudioFileViewModel>? _audioFiles;
        bool _isMultiple;
        bool _songTitleIsCommon;
        string _songTitle = string.Empty;
        bool _artistIsCommon;
        string _artist = string.Empty;
        bool _albumIsCommon;
        string _album = string.Empty;
        bool _albumArtistIsCommon;
        string _albumArtist = string.Empty;
        bool _composerIsCommon;
        string _composer = string.Empty;
        bool _genreIsCommon;
        string _genre = string.Empty;
        bool _commentIsCommon;
        string _comment = string.Empty;
        bool _dayIsCommon;
        string _day = string.Empty;
        bool _monthIsCommon;
        string _month = string.Empty;
        bool _yearIsCommon;
        string _year = string.Empty;
        bool _trackNumberIsCommon;
        string _trackNumber = string.Empty;
        bool _trackCountIsCommon;
        string _trackCount = string.Empty;

        public string Title { get; set; } = string.Empty;

        public bool IsMultiple
        {
            get => _isMultiple;
            set => SetProperty(ref _isMultiple, value);
        }

        public bool SongTitleIsCommon
        {
            get => _songTitleIsCommon;
            set => SetProperty(ref _songTitleIsCommon, value);
        }

        public string SongTitle
        {
            get => _songTitle;
            set => SetProperty(ref _songTitle, value);
        }

        public bool ArtistIsCommon
        {
            get => _artistIsCommon;
            set => SetProperty(ref _artistIsCommon, value);
        }

        public string Artist
        {
            get => _artist;
            set => SetProperty(ref _artist, value);
        }

        public bool AlbumIsCommon
        {
            get => _albumIsCommon;
            set => SetProperty(ref _albumIsCommon, value);
        }

        public string Album
        {
            get => _album;
            set => SetProperty(ref _album, value);
        }

        public bool AlbumArtistIsCommon
        {
            get => _albumArtistIsCommon;
            set => SetProperty(ref _albumArtistIsCommon, value);
        }

        public string AlbumArtist
        {
            get => _albumArtist;
            set => SetProperty(ref _albumArtist, value);
        }

        public bool ComposerIsCommon
        {
            get => _composerIsCommon;
            set => SetProperty(ref _composerIsCommon, value);
        }

        public string Composer
        {
            get => _composer;
            set => SetProperty(ref _composer, value);
        }

        public bool GenreIsCommon
        {
            get => _genreIsCommon;
            set => SetProperty(ref _genreIsCommon, value);
        }

        public string Genre
        {
            get => _genre;
            set => SetProperty(ref _genre, value);
        }

        public bool CommentIsCommon
        {
            get => _commentIsCommon;
            set => SetProperty(ref _commentIsCommon, value);
        }

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public bool DayIsCommon
        {
            get => _dayIsCommon;
            set => SetProperty(ref _dayIsCommon, value);
        }

        public string Day
        {
            get => _day;
            set => SetProperty(ref _day, value);
        }

        public bool MonthIsCommon
        {
            get => _monthIsCommon;
            set => SetProperty(ref _monthIsCommon, value);
        }

        public string Month
        {
            get => _month;
            set => SetProperty(ref _month, value);
        }

        public bool YearIsCommon
        {
            get => _yearIsCommon;
            set => SetProperty(ref _yearIsCommon, value);
        }

        public string Year
        {
            get => _year;
            set => SetProperty(ref _year, value);
        }

        public bool TrackNumberIsCommon
        {
            get => _trackNumberIsCommon;
            set => SetProperty(ref _trackNumberIsCommon, value);
        }

        public string TrackNumber
        {
            get => _trackNumber;
            set => SetProperty(ref _trackNumber, value);
        }

        public bool TrackCountIsCommon
        {
            get => _trackCountIsCommon;
            set => SetProperty(ref _trackCountIsCommon, value);
        }

        public string TrackCount
        {
            get => _trackCount;
            set => SetProperty(ref _trackCount, value);
        }

        public DelegateCommand ApplyCommand { get; }

        public EditControlViewModel() =>
            ApplyCommand = new(() =>
            {
                if (_audioFiles != null)
                    foreach (var audioFile in _audioFiles)
                    {
                        if (SongTitleIsCommon)
                            audioFile.Metadata.SongTitle = SongTitle;
                        if (ArtistIsCommon)
                            audioFile.Metadata.Artist = Artist;
                        if (AlbumIsCommon)
                            audioFile.Metadata.Album = Album;
                        if (AlbumArtistIsCommon)
                            audioFile.Metadata.AlbumArtist = AlbumArtist;
                        if (ComposerIsCommon)
                            audioFile.Metadata.Composer = Composer;
                        if (GenreIsCommon)
                            audioFile.Metadata.Genre = Genre;
                        if (CommentIsCommon)
                            audioFile.Metadata.Comment = Comment;
                        if (DayIsCommon)
                            audioFile.Metadata.Day = Day;
                        if (MonthIsCommon)
                            audioFile.Metadata.Month = Month;
                        if (YearIsCommon)
                            audioFile.Metadata.Year = Year;
                        if (TrackNumberIsCommon)
                            audioFile.Metadata.TrackNumber = TrackNumber;
                        if (TrackCountIsCommon)
                            audioFile.Metadata.TrackCount = TrackCount;
                    }

                RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
            });

        public void OnDialogOpened(IDialogParameters parameters)
        {
            _audioFiles = parameters.GetValue<List<AudioFileViewModel>>("AudioFiles");

            IsMultiple = _audioFiles!.Count > 1;
            Title = _isMultiple ? $"Editing {_audioFiles.Count} files" : "Editing 1 file";

            var thisType = GetType();

            foreach (var propertyName in thisType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(prop => prop.PropertyType == typeof(string) &&
                               !prop.Name.Equals("Title", StringComparison.Ordinal))
                .Select(prop => prop.Name))
            {
                var propertyInfo = typeof(AudioMetadataViewModel).GetProperty(propertyName);
                if (propertyInfo == null) continue;

                var firstValue = (string) propertyInfo.GetValue(_audioFiles[0].Metadata)!;

                if (_audioFiles.TrueForAll(audioFile =>
                    ((string) propertyInfo.GetValue(audioFile.Metadata)!).Equals(firstValue, StringComparison.Ordinal)))
                {
                    thisType.GetProperty($"{propertyName}IsCommon")!.SetValue(this, true);
                    thisType.GetProperty(propertyName)!.SetValue(this, firstValue);
                }
                else
                {
                    thisType.GetProperty($"{propertyName}IsCommon")!.SetValue(this, false);
                    thisType.GetProperty(propertyName)!.SetValue(this, string.Empty);
                }
            }
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public event Action<IDialogResult>? RequestClose;
    }
}
