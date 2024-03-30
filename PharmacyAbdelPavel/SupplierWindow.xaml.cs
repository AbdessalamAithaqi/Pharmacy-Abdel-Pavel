///-----------------------------------------------------------------

/// <copyright file="SupplierWindow.xaml.cs" group="Abdessalam Ait Haqi & Pavel Sushko">

///     Copyright (c) 2021 Abdessalam Ait Haqi & Pavel Sushko

/// </copyright>

/// <author> Abdessalam Ait Haqi </author>

/// <contributor> Pavel Sushko </contributor>

///-----------------------------------------------------------------
namespace PharmacyAbdelPavel
{
    using System.Windows;
    using PharmacyAbdelPavel.Models;

    /// <summary>
    /// Interaction logic for SupplierWindow.xaml
    /// </summary>
    public partial class SupplierWindow : Window
    {
        public SupplierWindow()
        {
            InitializeComponent();
        }

        private void BtnSubAdd_Click(object sender, RoutedEventArgs e)
        {
            string input = txtbCompanyName.Text;
            //submits the value
            if (!string.IsNullOrEmpty(input))
            {
                if (Data.AddToSettingFile(Data.SupplierListSetting, input))
                {
                    _ = Medicine.AddAuthorizedSupplier(input);
                    _ = MessageBox.Show($"The supplier \"{input}\" was added to the list of authorized suppliers successfully", "Input Accepted", MessageBoxButton.OK);
                    txtbCompanyName.Text = string.Empty;
                }
                else
                    _ = MessageBox.Show("The supplier name was not saved. Please try again", "Failed to save", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
                _ = MessageBox.Show("The name of the supplier cannot be empty", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnSubClose_Click(object sender, RoutedEventArgs e)
        {
            string input = txtbCompanyName.Text;
            if (!string.IsNullOrEmpty(input))
            {
                if (Data.AddToSettingFile(Data.SupplierListSetting, input))
                {
                    _ = Medicine.AddAuthorizedSupplier(input);
                    _ = MessageBox.Show($"The supplier \"{input}\" was added to the list of authorized suppliers successfully", "Input Accepted", MessageBoxButton.OK);
                    Close();
                }
                else
                    _ = MessageBox.Show("The supplier name was not saved. Please try again", "Failed to save", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
                _ = MessageBox.Show("The name of the supplier cannot be empty", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}