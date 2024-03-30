///-----------------------------------------------------------------

/// <copyright file="MedicineWindow.xaml.cs" group="Abdessalam Ait Haqi & Pavel Sushko">

///     Copyright (c) 2021 Abdessalam Ait Haqi & Pavel Sushko

/// </copyright>

/// <author> Abdessalam Ait Haqi </author>

/// <contributor> Pavel Sushko </contributor>

///-----------------------------------------------------------------
namespace PharmacyAbdelPavel
{
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using Models;

    /// <summary>
    /// Interaction logic for MedicineWindow.xaml
    /// </summary>
    public partial class MedicineWindow : Window
    {
        private bool isValid = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicineWindow"/> class.
        /// </summary>
        public MedicineWindow()
        {
            InitializeComponent();
            LoadOptions();
        }

        /// <summary>
        /// Loads the options.
        /// </summary>
        private void LoadOptions()
        {
            LoadAuthorizedSuppliers();
            LoadCategories();
        }

        /// <summary>
        /// Loads the authorized suppliers.
        /// </summary>
        private void LoadAuthorizedSuppliers()
        {
            cmbSuppliers.ItemsSource = Medicine.ApprovedSuppliers;
        }

        /// <summary>
        /// Loads the categories.
        /// </summary>
        private void LoadCategories()
        {
            cmbCategories.ItemsSource = Medicine.Categories;
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender"> The source of the event. </param>
        /// <param name="e"> The event arguments. </param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckUserInput())
            {
                Inventory.AddToInventoryList(GetUserInput());
                _ = MessageBox.Show("The medecine was added to the inventory successfully", "Successful upload", MessageBoxButton.OK);

                ClearForm();

                isValid = true;
            }
        }

        private void ClearForm()
        {
            foreach (object uiItem in AddMedGrid.Children)
            {
                if (uiItem is TextBox)
                    (uiItem as TextBox).Clear();
                else if (uiItem is ComboBox)
                    (uiItem as ComboBox).SelectedIndex = -1;
            }
        }

        private bool CheckUserInput()
        {
            StringBuilder errorMessage = new StringBuilder();

            if (string.IsNullOrEmpty(nameInput.Text))
                _ = errorMessage.AppendLine("Name is a required field");

            if (string.IsNullOrEmpty(availableQuantityInput.Text))
                _ = errorMessage.AppendLine("Available quanity is a required field");

            if (string.IsNullOrEmpty(minimumQuantityInput.Text))
                _ = errorMessage.AppendLine("Minimum quanity is a required field");

            if (string.IsNullOrEmpty(cabinetNumberInput.Text))
                _ = errorMessage.AppendLine("Cabinet number is a required field");

            if (cmbSuppliers.SelectedIndex == -1)
                _ = errorMessage.AppendLine("Supplier name is a required field");

            if (cmbCategories.SelectedIndex == -1)
                _ = errorMessage.AppendLine("Category is a required field");

            if (!string.IsNullOrEmpty(errorMessage.ToString()))
            {
                // Check if available quantity is number || is positive number
                if (!(int.TryParse(availableQuantityInput.Text, out int number) && number >= 0))
                    _ = errorMessage.AppendLine("Available quantity must be a positive integer");

                // Check if minimum quantity is number
                if (!(int.TryParse(minimumQuantityInput.Text, out number) && number >= Medicine.LowestMinimumQuantity))
                    _ = errorMessage.AppendLine($"Minimum quantity must be a positive integer bigger than or equalt to {Medicine.LowestMinimumQuantity}");

                if (!(int.TryParse(cabinetNumberInput.Text, out number) && number >= Medicine.FirstCabinetNumber && number <= Medicine.LastCabinetNumber))
                    _ = errorMessage.AppendLine($"Cabinet number must be a positive integer between {Medicine.FirstCabinetNumber} and {Medicine.LastCabinetNumber}");
            }

            if (string.IsNullOrEmpty(errorMessage.ToString()))
                return true;

            _ = MessageBox.Show(errorMessage.ToString(), "Missing fields", MessageBoxButton.OK, MessageBoxImage.Error);

            return false;
        }

        private Medicine GetUserInput()
        {
            return new Medicine(nameInput.Text, int.Parse(availableQuantityInput.Text), int.Parse(minimumQuantityInput.Text), int.Parse(cabinetNumberInput.Text), cmbSuppliers.SelectedItem.ToString(), cmbCategories.SelectedItem.ToString());
        }

        private void BtnSaveClose_Click(object sender, RoutedEventArgs e)
        {
            BtnSave_Click(sender, e);

            if (isValid)
                Close();
        }
    }
}