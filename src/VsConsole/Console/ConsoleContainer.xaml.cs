// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.PackageManagement;
using NuGet.PackageManagement.UI;

namespace NuGetConsole
{
    /// <summary>
    /// Interaction logic for ConsoleContainer.xaml
    /// </summary>
    public partial class ConsoleContainer : UserControl
    {
        public ConsoleContainer(
            ISolutionManager solutionManager,
            IProductUpdateService productUpdateService,
            IPackageRestoreManager packageRestoreManager,
            IDeleteOnRestartManager deleteOnRestartManager,
            IVsShell4 shell)
        {

            InitializeComponent();

            var productBar = new ProductUpdateBar(productUpdateService);
            productBar.SetValue(Grid.RowProperty, 0);
            RootLayout.Children.Add(productBar);

            var restoreBar = new PackageRestoreBar(solutionManager, packageRestoreManager);
            restoreBar.SetValue(Grid.RowProperty, 1);
            RootLayout.Children.Add(restoreBar);

            var restartBar = new RestartRequestBar(deleteOnRestartManager, shell);
            restartBar.SetValue(Grid.RowProperty, 2);
            RootLayout.Children.Add(restartBar);

            // Set DynamicResource binding in code
            // The reason we can't set it in XAML is that the VsBrushes class come from either
            // Microsoft.VisualStudio.Shell.10 or Microsoft.VisualStudio.Shell.11 assembly,
            // depending on whether NuGet runs inside VS10 or VS11.
            InitializeText.SetResourceReference(TextBlock.ForegroundProperty, VsBrushes.WindowTextKey);
        }

        public void AddConsoleEditor(UIElement content)
        {
            Grid.SetRow(content, 4);
            RootLayout.Children.Add(content);
        }

        public void NotifyInitializationCompleted()
        {
            RootLayout.Children.Remove(InitializeText);
        }
    }
}
