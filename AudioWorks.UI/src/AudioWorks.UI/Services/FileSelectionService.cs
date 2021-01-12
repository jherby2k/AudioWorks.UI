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
using System.Text;
using System.Windows.Forms;
using AudioWorks.Api;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace AudioWorks.UI.Services
{
    // ReSharper disable once ClassNeverInstantiated.Global
    sealed class FileSelectionService : IFileSelectionService
    {
        public IEnumerable<string> SelectFiles()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = GetFilter(),
                Title = "Select Audio File(s)"
            };

            var showResult = dialog.ShowDialog();
            if (showResult.HasValue && showResult.Value)
                return dialog.FileNames;
            return Array.Empty<string>();
        }

        public string SelectDirectory()
        {
            using (var dialog = new FolderBrowserDialog
            {
                Description = "Select a Directory to Search",
                UseDescriptionForTitle = true
            })
            {
                var showResult = dialog.ShowDialog();
                return showResult == DialogResult.OK ? dialog.SelectedPath : string.Empty;
            }
        }

        static string GetFilter()
        {
            var formatInfos = AudioFileManager.GetFormatInfo();
            var patterns = new List<string>();
            var filterOptions = new List<string>();

            foreach (var formatInfo in formatInfos)
            {
                var pattern = $"*{formatInfo.Extension}";
                patterns.Add($"*{formatInfo.Extension}");
                filterOptions.Add($"{formatInfo.Format} Files|{pattern}");
            }

            return new StringBuilder("All Audio Files|").AppendJoin(";", patterns).Append('|')
                .AppendJoin('|', filterOptions).ToString();
        }
    }
}
