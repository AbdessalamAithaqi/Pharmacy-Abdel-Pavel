///-----------------------------------------------------------------

/// <copyright file="SettingsWindow.xaml.cs" group="Abdessalam Ait Haqi & Pavel Sushko">

///     Copyright (c) 2021 Abdessalam Ait Haqi & Pavel Sushko

/// </copyright>

/// <author> Abdessalam Ait Haqi </author>

/// <contributor> Pavel Sushko </contributor>

///-----------------------------------------------------------------
namespace PharmacyAbdelPavel
{
    using System.Collections.Generic;
    using System.Windows;
    using Models;
    using Microsoft.Win32;
    using System.IO;

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsWindow"/> class.
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
            LoadCurrentSettings();
        }

        #region On Load Methods

        /// <summary>
        /// Loads the current settings.
        /// </summary>
        private void LoadCurrentSettings()
        {
            LoadFirstCabinetNumber();
            LoadLastCabinetNumber();
            LoadLowestMinimumQuantity();
            LoadAuthorizedSuppliers();
        }

        /// <summary>
        /// Loads the first cabinet number.
        /// </summary>
        private void LoadFirstCabinetNumber()
        {
            txtbFirstCabNum.Text = Medicine.FirstCabinetNumber.ToString();
        }

        /// <summary>
        /// Loads the last cabinet number.
        /// </summary>
        private void LoadLastCabinetNumber()
        {
            txtbLastCabNum.Text = Medicine.LastCabinetNumber.ToString();
        }

        /// <summary>
        /// Loads the lowest minimum quantity.
        /// </summary>
        private void LoadLowestMinimumQuantity()
        {
            txtbLowestMinQty.Text = Medicine.LowestMinimumQuantity.ToString();
        }

        /// <summary>
        /// Loads the authorized suppliers.
        /// </summary>
        private void LoadAuthorizedSuppliers()
        {
            lbAuthSupp.ItemsSource = Medicine.ApprovedSuppliers;
        }

        #endregion On Load Methods

        #region On click Methods

        /// <summary>
        /// Handles the Click event of the btnFirstCabNum control.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnFirstCabNum_Click(object sender, RoutedEventArgs e)
        {
            string input = txtbFirstCabNum.Text;

            if (Data.IsValidFirstCabNum(input))
            {
                if (Data.UpdateSettingFile(Data.FirstCabSetting, input))
                {
                    // Update the medicine class
                    Medicine.FirstCabinetNumber = int.Parse(input);

                    // Update new value in settings file
                    File.WriteAllText(Data.FirstCabSetting, input);

                    _ = MessageBox.Show($"The value of the first cabinet is now {Medicine.FirstCabinetNumber}", "Input Accepted", MessageBoxButton.OK);
                }
                else
                    _ = MessageBox.Show("The number of the first cabinet was not saved. Please try again", "Failed to save", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _ = MessageBox.Show("The number of the first cabinet must be a positive number and lower than the last cabinet nuber", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);

                LoadFirstCabinetNumber();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnLastCabNum control.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnLastCabNum_Click(object sender, RoutedEventArgs e)
        {
            string input = txtbLastCabNum.Text;

            if (Data.IsValidLastCabNum(input))
            {
                if (Data.UpdateSettingFile(Data.LastCabSetting, input))
                {
                    // Update new value in medecine
                    Medicine.LastCabinetNumber = int.Parse(input);

                    // Update new value in settings file
                    File.WriteAllText(Data.LastCabSetting, input);

                    _ = MessageBox.Show($"The value of the last cabinet is now {Medicine.LastCabinetNumber}", "Input Accepted", MessageBoxButton.OK);
                }
                else
                    _ = MessageBox.Show("The number of the last cabinet was not saved. Please try again", "Failed to save", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _ = MessageBox.Show("The number of the last cabinet must be a positive number and higher than the last cabinet number", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);

                LoadLastCabinetNumber();
            }
        }

        /// <summary>
        /// Handles the Click event of the btnLowestMinQty control.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnLowestMinQty_Click(object sender, RoutedEventArgs e)
        {
            string input = txtbLowestMinQty.Text;

            if (Data.IsValidLowestMin(input))
            {
                if (Data.UpdateSettingFile(Data.LowestMinimumSetting, input))
                {
                    // Update new value in medecine
                    Medicine.LowestMinimumQuantity = int.Parse(input);

                    // Update new value in settings file
                    File.WriteAllText(Data.LowestMinimumSetting, input);

                    _ = MessageBox.Show($"The value of the lowest minimum quantity is now {Medicine.LowestMinimumQuantity}", "Input Accepted", MessageBoxButton.OK);
                }
                else
                    _ = MessageBox.Show("The lowest minimum quantity was not saved. Please try again", "Failed to save", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                _ = MessageBox.Show("The lowest minimum quantity must be a positive number", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);

                LoadLowestMinimumQuantity();
            }
        }

        /// <summary>
        /// Adds a supplier to the authorized suppliers list.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnAddSupp_Click(object sender, RoutedEventArgs e)
        {
            SupplierWindow supplierWindow = new SupplierWindow();

            _ = supplierWindow.ShowDialog();

            lbAuthSupp.Items.Refresh();
        }

        /// <summary>
        /// Imports an authorized supplier.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnImportSupp_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;

            // Open file explorer
            OpenFileDialog fileExplorer = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "txt files (*.txt)|*.txt"
            };

            if (fileExplorer.ShowDialog() == true)
                // Select desired file
                filePath = fileExplorer.FileName;

            // Read file
            // Turn into string list
            List<string> newList = new List<string>(Data.ReadMultipleValues(filePath));

            if (Data.IsValidListOfSupplier(newList))
                if (Data.UpdateSettingFile(Data.SupplierListSetting, newList))
                {
                    Medicine.ApprovedSuppliers = newList;

                    LoadAuthorizedSuppliers();

                    _ = MessageBox.Show($"The list of authorized suppliers was updated successfully", "Input Accepted", MessageBoxButton.OK);
                }
                else
                    _ = MessageBox.Show("The supplier list was not saved. Please try again", "Failed to save", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                _ = MessageBox.Show("The file does not contain a valid list of suppliers", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Removes an authorized supplier.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnRemoveSupp_Click(object sender, RoutedEventArgs e)
        {
            string value = lbAuthSupp.SelectedItem.ToString();

            if (value != null)
                if (Medicine.RemoveAuthorizedSupplier(value) && Data.UpdateSettingFile(Data.SupplierListSetting, Medicine.ApprovedSuppliers))
                {
                    lbAuthSupp.Items.Refresh();
                    _ = MessageBox.Show($"The authorised supplier \"{value}\" was removed from the list of authorised suppliers", "Input Accepted", MessageBoxButton.OK);
                }
                else
                    _ = MessageBox.Show($"The supplier \"{value}\" was not found. Please try again", "Failed to save", MessageBoxButton.OK, MessageBoxImage.Warning);
            else
                _ = MessageBox.Show("No authorised supplier was selected. Please select the authorized supplier to be removed", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion On click Methods
    }
}