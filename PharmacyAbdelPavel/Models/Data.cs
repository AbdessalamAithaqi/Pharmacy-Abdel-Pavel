///-----------------------------------------------------------------

/// <copyright file="Data.cs" group="Abdessalam Ait Haqi & Pavel Sushko">

///     Copyright (c) 2021 Abdessalam Ait Haqi & Pavel Sushko

/// </copyright>

/// <author> Abdessalam Ait Haqi </author>

/// <contributor> Pavel Sushko </contributor>

///-----------------------------------------------------------------
namespace PharmacyAbdelPavel.Models
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Microsoft.Win32;

    /// <summary>
    /// Class that allows for data manipulation.
    /// </summary>
    public static class Data
    {
        #region Global constants

        private static readonly string _firstCabSetting = "../../../Settings/SettingFirstCab.txt";
        private static readonly string _lastCabSetting = "../../../Settings/SettingLastCab.txt";
        private static readonly string _lowestMinimumSetting = "../../../Settings/SettingLowestMin.txt";
        private static readonly string _supplierListSetting = "../../../Settings/SettingSupplierList.txt";
        private static readonly int _defaultFirstCabNumber = 0;
        private static readonly int _defaultLastCabNumber = int.MaxValue;
        private static readonly int _defaultLowestMinQty = 0;
        private static readonly List<string> _defaultApprovedSupplier = new List<string> { "Any" };
        private static string saveFile = "";

        #endregion Global constants

        #region Properties

        public static string FirstCabSetting => _firstCabSetting;
        public static string LastCabSetting => _lastCabSetting;
        public static string LowestMinimumSetting => _lowestMinimumSetting;
        public static string SupplierListSetting => _supplierListSetting;
        public static int DefaultFirstCabNumber => _defaultFirstCabNumber;
        public static int DefaultLastCabNumber => _defaultLastCabNumber;
        public static int DefaultLowestMinQty => _defaultLowestMinQty;
        public static List<string> DefaultApprovedSupplier => _defaultApprovedSupplier;

        #endregion Properties

#nullable enable

        /// <summary>
        /// Reads a single value from a file.
        /// </summary>
        /// <param name="filePath"> The path to the file. </param>
        /// <returns> The value read from the file. </returns>
        public static string? ReadSingleValue(string filePath)
        {
            if (File.Exists(filePath))
                try
                {
                    return File.ReadAllLines(filePath)[0];
                }
                catch (Exception)
                {
                    return null;
                }

            return null;
        }

        /// <summary>
        /// Reads a list of values from a file.
        /// </summary>
        /// <param name="filePath"> The path to the file. </param>
        /// <returns> The list of values read from the file. </returns>
        public static List<string>? ReadMultipleValues(string filePath)
        {
            if (File.Exists(filePath))
                try
                {
                    return new List<string>(File.ReadAllLines(filePath));
                }
                catch (Exception)
                {
                    return null;
                }

            return null;
        }

#nullable disable

        /// <summary>
        /// Checks if the value is a valid number for the first cabinet number.
        /// </summary>
        /// <param name="value"> The value to check. </param>
        /// <returns> True if the value is valid, false otherwise. </returns>
        public static bool IsValidFirstCabNum(string value)
        {
            return int.TryParse(value, out int number) && number >= 0 && number <= Medicine.LastCabinetNumber;
        }

        /// <summary>
        /// Checks if the value is a valid number for the last cabinet number.
        /// </summary>
        /// <param name="value"> The value to check. </param>
        /// <returns> True if the value is valid, false otherwise. </returns>
        public static bool IsValidLastCabNum(string value)
        {
            return int.TryParse(value, out int number) && number >= 0 && number >= Medicine.FirstCabinetNumber;
        }

        /// <summary>
        /// Checks if the value is a valid number for the first medicine number.
        /// </summary>
        /// <param name="value"> The value to check. </param>
        /// <returns> True if the value is valid, false otherwise. </returns>
        public static bool IsValidLowestMin(string value)
        {
            return int.TryParse(value, out int number) && number >= 0;
        }

        /// <summary>
        /// Checks if the value is a valid number for the last medicine number.
        /// </summary>
        /// <param name="values"> The list of suppliers to check. </param>
        /// <returns> True if the value is valid, false otherwise. </returns>
        public static bool IsValidListOfSupplier(List<string> values)
        {
            return values != null && values.Count > 0;
        }

        /// <summary>
        /// Updates the setting file with the new value.
        /// </summary>
        /// <param name="filePath"> The path to the file. </param>
        /// <param name="value"> The new value. </param>
        /// <returns> True if the update was successful, false otherwise. </returns>
        public static bool UpdateSettingFile(string filePath, string newValue)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);

                // If the directory doesn't exist, create it.
                if (!Directory.Exists(dir))
                    _ = Directory.CreateDirectory(dir);

                // Write to file, overwriting the file if it exists.
                File.WriteAllText(filePath, newValue);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Updates the settings file for the chosen medicine.
        /// </summary>
        /// <param name="filePath"> The path to the file. </param>
        /// <param name="newValues"> The new values to write. </param>
        /// <returns> True if the update was successful, false otherwise. </returns>
        public static bool UpdateSettingFile(string filePath, List<string> newValues)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);

                // If the directory doesn't exist, create it.
                if (!Directory.Exists(dir))
                    _ = Directory.CreateDirectory(dir);

                // Write to file, overwriting the file if it exists.
                File.WriteAllLines(filePath, newValues);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds to the setting file with the new value.
        /// </summary>
        /// <param name="filePath"> The path to the file. </param>
        /// <param name="value"> The new value. </param>
        /// <returns> True if the update was successful, false otherwise. </returns>
        public static bool AddToSettingFile(string filePath, string newValue)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);

                // If the directory doesn't exist, create it.
                if (!Directory.Exists(dir))
                    _ = Directory.CreateDirectory(dir);

                // Write to file, add to the file if it exists.
                File.AppendAllText(filePath, $"\n{newValue}");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Adds to the setting file with the new values.
        /// </summary>
        /// <param name="filePath"> The path to the file. </param>
        /// <param name="newValues"> The new values to write. </param>
        /// <returns> True if the update was successful, false otherwise. </returns>
        public static bool AddToSettingFile(string filePath, List<string> newValues)
        {
            try
            {
                string dir = Path.GetDirectoryName(filePath);

                // If the directory doesn't exist, create it.
                if (!Directory.Exists(dir))
                    _ = Directory.CreateDirectory(dir);

                // Write to file, add to the file if it exists.
                File.AppendAllLines(filePath, newValues);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Exports a list of medicine to a file
        /// </summary>
        /// <param name="medicineList"> The list of medicine to export. </param>
        /// <returns> True if the export was successful, false otherwise. </returns>
        public static bool ExportToFile(List<Medicine> medicineList)
        {
            if (string.IsNullOrEmpty(saveFile))
            {
                // Create a new SaveFileDialog object
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    CheckFileExists = false,
                    CheckPathExists = false,
                    DefaultExt = "csv",
                    Filter = "CSV files (*.csv)|*.csv|Text files (*.txt)|*.txt",
                    FilterIndex = 1,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Title = "Select a file to save to"
                };

                // Open the SaveFileDialog by calling ShowDialog method
                _ = saveFileDialog.ShowDialog();

                saveFile = saveFileDialog.FileName;
            }

            try
            {
                if (!string.IsNullOrEmpty(saveFile))
                {
                    // Create a string builder with the CSV header
                    StringBuilder sb = new StringBuilder("Name,Available Quantity,Minimum Quantity,Cabinet Number,Supplier,Category\n");

                    // Iterate through each medicine in the inventory list and append it to the string builder in it's CSV format
                    foreach (Medicine medicine in medicineList)
                        _ = sb.AppendLine(medicine.ToCSV());

                    // Write the string builder to the file
                    File.WriteAllText(saveFile, sb.ToString());

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}