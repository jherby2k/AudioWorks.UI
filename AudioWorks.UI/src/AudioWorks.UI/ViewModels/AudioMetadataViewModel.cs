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
using System.IO;
using System.Windows.Media.Imaging;
using AudioWorks.Common;
using Prism.Mvvm;

namespace AudioWorks.UI.ViewModels
{
    public sealed class AudioMetadataViewModel : BindableBase
    {
        AudioMetadata _savedValues;
        AudioMetadata _metadata;
        bool _songTitleIsModified;
        bool _artistIsModified;
        bool _albumIsModified;
        bool _albumArtistIsModified;
        bool _composerIsModified;
        bool _genreIsModified;
        bool _commentIsModified;
        bool _dayIsModified;
        bool _monthIsModified;
        bool _yearIsModified;
        bool _trackNumberIsModified;
        bool _trackCountIsModified;
        bool _trackPeakIsModified;
        bool _albumPeakIsModified;
        bool _trackGainIsModified;
        bool _albumGainIsModified;
        Lazy<BitmapImage?> _coverImage;

        public string SongTitle
        {
            get => _metadata.Title;
            set
            {
                _metadata.Title = value;
                RaisePropertyChanged();
                SongTitleIsModified = !value.Equals(_savedValues.Title, StringComparison.Ordinal);
            }
        }

        public bool SongTitleIsModified
        {
            get => _songTitleIsModified;
            private set => SetProperty(ref _songTitleIsModified, value);
        }

        public string Artist
        {
            get => _metadata.Artist;
            set
            {
                _metadata.Artist = value;
                RaisePropertyChanged();
                ArtistIsModified = !value.Equals(_savedValues.Artist, StringComparison.Ordinal);
            }
        }

        public bool ArtistIsModified
        {
            get => _artistIsModified;
            private set => SetProperty(ref _artistIsModified, value);
        }

        public string Album
        {
            get => _metadata.Album;
            set
            {
                _metadata.Album = value;
                RaisePropertyChanged();
                AlbumIsModified = !value.Equals(_savedValues.Album, StringComparison.Ordinal);
            }
        }

        public bool AlbumIsModified
        {
            get => _albumIsModified;
            private set => SetProperty(ref _albumIsModified, value);
        }

        public string AlbumArtist
        {
            get => _metadata.AlbumArtist;
            set
            {
                _metadata.AlbumArtist = value;
                RaisePropertyChanged();
                AlbumArtistIsModified = !value.Equals(_savedValues.AlbumArtist, StringComparison.Ordinal);
            }
        }

        public bool AlbumArtistIsModified
        {
            get => _albumArtistIsModified;
            private set => SetProperty(ref _albumArtistIsModified, value);
        }

        public string Composer
        {
            get => _metadata.Composer;
            set
            {
                _metadata.Composer = value;
                RaisePropertyChanged();
                ComposerIsModified = !value.Equals(_savedValues.Composer, StringComparison.Ordinal);
            }
        }

        public bool ComposerIsModified
        {
            get => _composerIsModified;
            private set => SetProperty(ref _composerIsModified, value);
        }

        public string Genre
        {
            get => _metadata.Genre;
            set
            {
                _metadata.Genre = value;
                RaisePropertyChanged();
                GenreIsModified = !value.Equals(_savedValues.Genre, StringComparison.Ordinal);
            }
        }

        public bool GenreIsModified
        {
            get => _genreIsModified;
            private set => SetProperty(ref _genreIsModified, value);
        }

        public string Comment
        {
            get => _metadata.Comment;
            set
            {
                _metadata.Comment = value;
                RaisePropertyChanged();
                CommentIsModified = !value.Equals(_savedValues.Comment, StringComparison.Ordinal);
            }
        }

        public bool CommentIsModified
        {
            get => _commentIsModified;
            private set => SetProperty(ref _commentIsModified, value);
        }

        public string Day
        {
            get => _metadata.Day;
            set
            {
                _metadata.Day = value;
                RaisePropertyChanged();
                DayIsModified = !value.Equals(_savedValues.Day, StringComparison.Ordinal);
            }
        }

        public bool DayIsModified
        {
            get => _dayIsModified;
            private set => SetProperty(ref _dayIsModified, value);
        }

        public string Month
        {
            get => _metadata.Month;
            set
            {
                _metadata.Month = value;
                RaisePropertyChanged();
                MonthIsModified = !value.Equals(_savedValues.Month, StringComparison.Ordinal);
            }
        }

        public bool MonthIsModified
        {
            get => _monthIsModified;
            private set => SetProperty(ref _monthIsModified, value);
        }

        public string Year
        {
            get => _metadata.Year;
            set
            {
                _metadata.Year = value;
                RaisePropertyChanged();
                YearIsModified = !value.Equals(_savedValues.Year, StringComparison.Ordinal);
            }
        }

        public bool YearIsModified
        {
            get => _yearIsModified;
            private set => SetProperty(ref _yearIsModified, value);
        }

        public string TrackNumber
        {
            get => _metadata.TrackNumber;
            set
            {
                _metadata.TrackNumber = value;
                RaisePropertyChanged();
                TrackNumberIsModified = !value.Equals(_savedValues.TrackNumber, StringComparison.Ordinal);
            }
        }

        public bool TrackNumberIsModified
        {
            get => _trackNumberIsModified;
            private set => SetProperty(ref _trackNumberIsModified, value);
        }

        public string TrackCount
        {
            get => _metadata.TrackCount;
            set
            {
                _metadata.TrackCount = value;
                RaisePropertyChanged();
                TrackCountIsModified = !value.Equals(_savedValues.TrackCount, StringComparison.Ordinal);
            }
        }

        public bool TrackCountIsModified
        {
            get => _trackCountIsModified;
            private set => SetProperty(ref _trackCountIsModified, value);
        }

        public string TrackPeak
        {
            get => _metadata.TrackPeak;
            private set
            {
                _metadata.TrackPeak = value;
                RaisePropertyChanged();
                TrackPeakIsModified = !value.Equals(_savedValues.TrackPeak, StringComparison.Ordinal);
            }
        }

        public bool TrackPeakIsModified
        {
            get => _trackPeakIsModified;
            private set => SetProperty(ref _trackPeakIsModified, value);
        }

        public string AlbumPeak
        {
            get => _metadata.AlbumPeak;
            private set
            {
                _metadata.AlbumPeak = value;
                RaisePropertyChanged();
                AlbumPeakIsModified = !value.Equals(_savedValues.AlbumPeak, StringComparison.Ordinal);
            }
        }

        public bool AlbumPeakIsModified
        {
            get => _albumPeakIsModified;
            private set => SetProperty(ref _albumPeakIsModified, value);
        }

        public string TrackGain
        {
            get => _metadata.TrackGain;
            private set
            {
                _metadata.TrackGain = value;
                RaisePropertyChanged();
                TrackGainIsModified = !value.Equals(_savedValues.TrackGain, StringComparison.Ordinal);
            }
        }

        public bool TrackGainIsModified
        {
            get => _trackGainIsModified;
            private set => SetProperty(ref _trackGainIsModified, value);
        }

        public string AlbumGain
        {
            get => _metadata.AlbumGain;
            private set
            {
                _metadata.AlbumGain = value;
                RaisePropertyChanged();
                AlbumGainIsModified = !value.Equals(_savedValues.AlbumGain, StringComparison.Ordinal);
            }
        }

        public bool AlbumGainIsModified
        {
            get => _albumGainIsModified;
            private set => SetProperty(ref _albumGainIsModified, value);
        }

        public BitmapImage? CoverImage => _coverImage.Value;

        public bool Modified =>
            SongTitleIsModified || ArtistIsModified || AlbumIsModified || AlbumArtistIsModified || ComposerIsModified ||
            GenreIsModified || CommentIsModified || DayIsModified || MonthIsModified || YearIsModified ||
            TrackNumberIsModified || TrackCountIsModified || TrackPeakIsModified || AlbumPeakIsModified ||
            TrackGainIsModified || AlbumGainIsModified;

        public AudioMetadataViewModel(AudioMetadata metadata)
        {
            PropertyChanged += (_, e) =>
            {
                if (e.PropertyName != null && e.PropertyName.EndsWith("IsModified", StringComparison.Ordinal))
                    RaisePropertyChanged(nameof(Modified));
            };

            _savedValues = new(metadata);
            _metadata = metadata;
            _coverImage = new(() => LoadImage(metadata.CoverArt));
        }

        internal void UpdateModel(AudioMetadata metadata)
        {
            _savedValues = new(metadata);
            _metadata = metadata;
            _coverImage = new(() => LoadImage(metadata.CoverArt));
            Refresh();
        }

        internal void Refresh()
        {
            SongTitle = _metadata.Title;
            Artist = _metadata.Artist;
            Album = _metadata.Album;
            AlbumArtist = _metadata.AlbumArtist;
            Composer = _metadata.Composer;
            Genre = _metadata.Genre;
            Comment = _metadata.Comment;
            Day = _metadata.Day;
            Month = _metadata.Month;
            Year = _metadata.Year;
            TrackNumber = _metadata.TrackNumber;
            TrackCount = _metadata.TrackCount;
            TrackPeak = _metadata.TrackPeak;
            AlbumPeak = _metadata.AlbumPeak;
            TrackGain = _metadata.TrackGain;
            AlbumGain = _metadata.AlbumGain;
        }

        static unsafe BitmapImage? LoadImage(ICoverArt? coverArt)
        {
            if (coverArt == null) return null;

            fixed (byte* dataAddress = coverArt.Data)
                using (var stream = new UnmanagedMemoryStream(dataAddress, coverArt.Data.Length))
                {
                    var result = new BitmapImage();
                    result.BeginInit();
                    result.StreamSource = stream;
                    result.CacheOption = BitmapCacheOption.OnLoad;
                    result.EndInit();
                    result.Freeze();
                    return result;
                }
        }
    }
}