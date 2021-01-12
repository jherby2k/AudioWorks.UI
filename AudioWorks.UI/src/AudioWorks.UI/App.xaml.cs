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

using AudioWorks.UI.Services;
using AudioWorks.UI.ViewModels;
using AudioWorks.UI.Views;
using ControlzEx.Theming;
using MahApps.Metro.Controls.Dialogs;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;

namespace AudioWorks.UI
{
    public sealed partial class App
    {
        protected override Window CreateShell() => Container.Resolve<MainWindow>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IFileSelectionService, FileSelectionService>();
            containerRegistry.RegisterSingleton<IMetadataSettingService, MetadataSettingService>();
            containerRegistry.RegisterSingleton<IAnalysisSettingService, AnalysisSettingService>();
            containerRegistry.RegisterSingleton<IEncoderSettingService, EncoderSettingService>();
            containerRegistry.RegisterDialog<EditControl, EditControlViewModel>();
            containerRegistry.RegisterDialog<AnalysisControl, AnalysisControlViewModel>();
            containerRegistry.RegisterDialog<EncoderControl, EncoderControlViewModel>();
            containerRegistry.RegisterDialogWindow<CustomDialogWindow>();
            containerRegistry.RegisterForNavigation<EncoderSelectionControl>();
            containerRegistry.RegisterForNavigation<EncoderDestinationControl>();

            // MahApps.Metro dialog service
            containerRegistry.RegisterInstance(DialogCoordinator.Instance);
        }

        protected override IModuleCatalog CreateModuleCatalog() =>
            new DirectoryModuleCatalog { ModulePath = "Modules" };

        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
            ThemeManager.Current.SyncTheme();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            ContainerLocator.Current.Resolve<IMetadataSettingService>().Save();
            ContainerLocator.Current.Resolve<IAnalysisSettingService>().Save();
            ContainerLocator.Current.Resolve<IEncoderSettingService>().Save();

            base.OnExit(e);
        }
    }
}
