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

using System.Windows;

namespace AudioWorks.UI.ViewModels
{
    public sealed class MainWindowViewModelBindingProxy : Freezable
    {
        protected override Freezable CreateInstanceCore() => new MainWindowViewModelBindingProxy();

        public MainWindowViewModel Data
        {
            get => (MainWindowViewModel) GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(MainWindowViewModel),
                typeof(MainWindowViewModelBindingProxy));
    }
}
