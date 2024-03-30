///-----------------------------------------------------------------

/// <copyright file="ModifyWindow.xaml.cs" group="Abdessalam Ait Haqi & Pavel Sushko">

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
    /// Interaction logic for ModifyWindow.xaml
    /// </summary>
    public partial class ModifyWindow : Window
    {
        private readonly Medicine _medicine;

        public ModifyWindow(Medicine medicine)
        {
            _medicine = medicine;

            InitializeComponent();
            LoadOptions();
        }

        public void LoadOptions()
        {
            // Loads the name
            txtbName.Text = _medicine.Name;

            // Loads the available quantity
            txtbAvailableQty.Text = _medicine.AvailableQuantity.ToString();

            // Loads the minimum quantity
            txtbMinQty.Text = _medicine.MinimumQuantity.ToString();

            // Loads the cabinet number
            txtbCabNum.Text = _medicine.CabinetNumber.ToString();

            // Loads the supplier list
            cmbSupplier.ItemsSource = Medicine.ApprovedSuppliers;

            // Loads the category list
            cmbCategory.ItemsSource = Medicine.Categories;
        }

        private void BtnModName_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtbName.Text))
            {
                Medicine beforeUpdate = _medicine;
                _medicine.Name = txtbName.Text;
                Medicine afterUpdate = _medicine;
                Inventory.UpdateItemInList(beforeUpdate, afterUpdate);
            }
            else
                MessageBox.Show("Please enter a name", "Invalid Name", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnModAvailQty_Click(object sender, RoutedEventArgs e)
        {
            bool valid = int.TryParse(txtbAvailableQty.Text, out int input);

            if (valid && input >= 0)
            {
                Medicine beforeUpdate = _medicine;
                _medicine.AvailableQuantity = input;
                Medicine afterUpdate = _medicine;
                Inventory.UpdateItemInList(beforeUpdate, afterUpdate);
            }
            else
                _ = MessageBox.Show("The available quantity must be a positive number", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnModMinQty_Click(object sender, RoutedEventArgs e)
        {
            bool valid = int.TryParse(txtbMinQty.Text, out int input);

            if (valid && input >= 0)
            {
                Medicine beforeUpdate = _medicine;
                _medicine.MinimumQuantity = input;
                Medicine afterUpdate = _medicine;
                Inventory.UpdateItemInList(beforeUpdate, afterUpdate);
            }
            else
                _ = MessageBox.Show("The minimum quantity must be a positive number", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnModCabNum_Click(object sender, RoutedEventArgs e)
        {
            bool valid = int.TryParse(txtbCabNum.Text, out int input);

            if (valid && input >= Data.DefaultFirstCabNumber && input <= Data.DefaultLastCabNumber)
            {
                Medicine beforeUpdate = _medicine;
                _medicine.CabinetNumber = input;
                Medicine afterUpdate = _medicine;
                Inventory.UpdateItemInList(beforeUpdate, afterUpdate);
            }
            else
                _ = MessageBox.Show($"The cabinet number must be between {Data.DefaultFirstCabNumber} and {Data.DefaultLastCabNumber}", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnModSupplier_Click(object sender, RoutedEventArgs e)
        {
            if (cmbSupplier.SelectedIndex != -1)
            {
                Medicine beforeUpdate = _medicine;
                _medicine.Supplier = cmbSupplier.SelectedItem.ToString();
                Medicine afterUpdate = _medicine;
                Inventory.UpdateItemInList(beforeUpdate, afterUpdate);
            }
            else
                _ = MessageBox.Show("Please select a supplier", "Invalid Supplier", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnModCat_Click(object sender, RoutedEventArgs e)
        {
            if (cmbCategory.SelectedIndex != -1)
            {
                Medicine beforeUpdate = _medicine;
                _medicine.Category = cmbCategory.SelectedItem.ToString();
                Medicine afterUpdate = _medicine;
                Inventory.UpdateItemInList(beforeUpdate, afterUpdate);
            }
            else
                _ = MessageBox.Show("Please select a category", "Invalid Category", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}