///-----------------------------------------------------------------

/// <copyright file="MainWindow.xaml.cs" group="Abdessalam Ait Haqi & Pavel Sushko">

///     Copyright (c) 2021 Abdessalam Ait Haqi & Pavel Sushko

/// </copyright>

/// <author> Abdessalam Ait Haqi </author>

/// <contributor> Pavel Sushko </contributor>

///-----------------------------------------------------------------
namespace PharmacyAbdelPavel
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows;
    using Microsoft.Win32;
    using Models;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region On launch

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LoadDefaultSettings();
        }

        /// <summary>
        /// Loads the default settings.
        /// </summary>
        private void LoadDefaultSettings()
        {
            // Loads the first cabinet number
            string value = Data.ReadSingleValue(Data.FirstCabSetting);
            Medicine.FirstCabinetNumber = Data.IsValidFirstCabNum(value) ? int.Parse(value) : Data.DefaultFirstCabNumber;

            // Loads the last cabinet number
            value = Data.ReadSingleValue(Data.LastCabSetting);
            Medicine.LastCabinetNumber = Data.IsValidLastCabNum(value) ? int.Parse(value) : Data.DefaultLastCabNumber;

            // Loads the lowest minimum quantity
            value = Data.ReadSingleValue(Data.LowestMinimumSetting);
            Medicine.LowestMinimumQuantity = Data.IsValidLowestMin(value) ? int.Parse(value) : Data.DefaultLowestMinQty;

            // Loads the list of suppliers
            List<string> values = Data.ReadMultipleValues(Data.SupplierListSetting);
            Medicine.ApprovedSuppliers = Data.IsValidListOfSupplier(values) ? values : Data.DefaultApprovedSupplier;

            // Loads the inventory
            dgInventory.ItemsSource = Inventory.InventoryList;

            // Sets a double click event handler for the inventory items
            dgInventory.MouseDoubleClick += BtnModify_Click;
        }

        #endregion On launch

        /// <summary>
        /// Add new medicine to the medicine list from a csv file
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnImport_Click(object sender, RoutedEventArgs e)
        {
            // Create a new OpenFileDialog object
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "csv",
                Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt",
                FilterIndex = 1,
                InitialDirectory = Regex.Replace(Directory.GetCurrentDirectory(), @"(\\.[^\\]*){4}$", "TestFolder"),
                Title = "Select a CSV file"
            };

            // Show the OpenFileDialog by calling ShowDialog method
            _ = openFileDialog.ShowDialog();

            // Get the selected file name
            string filePath = openFileDialog.FileName;

            // Check if the user selected a file
            if (!string.IsNullOrEmpty(filePath))
            {
                // Read the file to an array of strings
                List<string> results = Data.ReadMultipleValues(filePath);

                if (results.Count < 1)
                {
                    _ = MessageBox.Show("Invalid file format. Please check the file and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }

                // Iterate through the array, create a medicine from it, and add it to the medecineList
                foreach (string line in results)
                {
                    // Split the line into an array of strings
                    string[] values = line.Split(',');

                    // Skips the header and all duplicates
                    if (values[0] == "Name" || Inventory.InventoryList.Any(medicine => medicine.ToCSV() == line))
                        continue;

                    // Check that the array has the correct number of values
                    if (values.Length != 6)
                    {
                        _ = MessageBox.Show("Invalid file format. Please check the file and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        return;
                    }

                    // Create a string builder for the error message
                    StringBuilder errorMessage = new StringBuilder();

                    // Extract the values from the array and parse them when needed
                    string name = values[0];
                    bool validAvail = int.TryParse(values[1], out int availableQuantity);
                    bool validMin = int.TryParse(values[2], out int minimumQuantity);
                    bool validCabNum = int.TryParse(values[3], out int cabinetNumber) && cabinetNumber >= Medicine.FirstCabinetNumber && cabinetNumber <= Medicine.LastCabinetNumber;
                    string supplier = values[4];
                    string category = values[5];

                    // Check if all the values are valid
                    if (!validAvail)
                        _ = errorMessage.AppendLine($"Invalid available quantity: {values[1]}");

                    if (!validMin)
                        _ = errorMessage.AppendLine($"Invalid minimum quantity: {values[2]}");

                    if (!validCabNum)
                        _ = errorMessage.AppendLine($"Invalid cabinet number: {values[3]}");

                    if (!Medicine.ApprovedSuppliers.Contains(supplier))
                        _ = errorMessage.AppendLine($"Invalid supplier: {supplier}");

                    if (!Medicine.Categories.Contains(category))
                        _ = errorMessage.AppendLine($"Invalid category: {category}");

                    // If there are any errors, show them to the user
                    if (errorMessage.Length > 0)
                    {
                        _ = MessageBox.Show(errorMessage.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        continue;
                    }

                    // Create a new medicine and add it to the Inventory
                    try
                    {
                        Inventory.AddToInventoryList(new Medicine(name, availableQuantity, minimumQuantity, cabinetNumber, supplier, category));
                    }
                    catch (Exception er)
                    {
                        _ = MessageBox.Show(er.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    // Refreshes the displayed inventory
                    dgInventory.Items.Refresh();
                }
            }
        }

        #region Exporting

        /// <summary>
        /// Saves the list of medicine that is in stock to a csv file
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnInStockReport_Click(object sender, RoutedEventArgs e)
        {
            // Notify the user that the file was saved successfully
            if (Data.ExportToFile(Inventory.InventoryList.Where(medicine => medicine.AvailableQuantity >= medicine.MinimumQuantity).ToList()))
                _ = MessageBox.Show("The in stock inventory was exported successfully", "Input Accepted", MessageBoxButton.OK);
            else
                _ = MessageBox.Show("The in stock inventory could not be exported", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnOutOfStockReport_Click(object sender, RoutedEventArgs e)
        {
            if (Data.ExportToFile(Inventory.ShoppingList()))
                _ = MessageBox.Show("The out of stock inventory was exported successfully", "Input Accepted", MessageBoxButton.OK);
            else
                _ = MessageBox.Show("The out of stock inventory could not be exported", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Save the current inventory list to a CSV or TXT file
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Notify the user that the file was saved successfully
            if (Data.ExportToFile(Inventory.InventoryList))
                _ = MessageBox.Show("The inventory was exported successfully", "Input Accepted", MessageBoxButton.OK);
        }

        #endregion Exporting

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Check that the sender is a value in the text box
            // Search the list of in stock medication for a match even if just in part of the name
            // Display all results that match the name
        }

        /// <summary>
        /// Opens the <see cref="MedicineWindow"/> window to add a new medicine to the inventory.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MedicineWindow medecineWindow = new MedicineWindow();

            _ = medecineWindow.ShowDialog();

            dgInventory.Items.Refresh();
        }

        /// <summary>
        /// Removes the selected medicine from the inventory.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dgInventory.SelectedItem != null)
            {
                Inventory.RemoveFromInventoryList(dgInventory.SelectedItem as Medicine);

                _ = MessageBox.Show("The medical item was removed successfully", "Input Accepted", MessageBoxButton.OK);
            }
            else
                _ = MessageBox.Show(" You must select the item you wish to remove", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);

            dgInventory.Items.Refresh();
        }

        /// <summary>
        /// Opens the <see cref="ModifyWindow"/> window to modify the selected medicine.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnModify_Click(object sender, RoutedEventArgs e)
        {
            if (dgInventory.SelectedItem != null)
            {
                ModifyWindow modifyWindow = new ModifyWindow(dgInventory.SelectedItem as Medicine);

                _ = modifyWindow.ShowDialog();
                _ = MessageBox.Show("The medical item was modified successfully", "Input Accepted", MessageBoxButton.OK);

                dgInventory.Items.Refresh();
            }
            else
                _ = MessageBox.Show(" You must select the item you wish to modify", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// Opens the <see cref="SettingsWindow"/> window to modify the settings of the medicine.
        /// </summary>
        /// <param name="sender"> The sender of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();

            _ = settingsWindow.ShowDialog();
        }
    }
}