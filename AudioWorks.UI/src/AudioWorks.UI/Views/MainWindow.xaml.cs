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

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace AudioWorks.UI.Views
{
    public sealed partial class MainWindow
    {
        public MainWindow() => InitializeComponent();

        void DataGrid_OnPreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Prevent the context menu from showing on column headers
            var source = (DependencyObject) e.OriginalSource;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            while (source != null && !(source is DataGridColumnHeader))
                source = VisualTreeHelper.GetParent(source);

            if (source != null)
                e.Handled = true;
        }

        void DataGrid_OnPreviewDragOver(object sender, DragEventArgs e)
        {
            if (((DataObject) e.Data).ContainsFileDropList())
                e.Effects = DragDropEffects.Link;
            e.Handled = true;
        }

        void ExitMenuItem_OnClick(object sender, RoutedEventArgs e) => Close();
    }
}
