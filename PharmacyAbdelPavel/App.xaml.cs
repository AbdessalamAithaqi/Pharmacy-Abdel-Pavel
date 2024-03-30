///-----------------------------------------------------------------
/// <copyright file="App.xaml.cs" group="Abdessalam Ait Haqi & Pavel Sushko">
///     Copyright (c) 2021 Abdessalam Ait Haqi & Pavel Sushko
/// </copyright>
/// <author> Initial Contributor </author>
/// <contributor> Second Contributor </contributor>
///-----------------------------------------------------------------
namespace PharmacyAbdelPavel
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message,"Error",MessageBoxButton.OK,MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}