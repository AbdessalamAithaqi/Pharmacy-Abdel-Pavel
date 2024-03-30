///-----------------------------------------------------------------

/// <copyright file="Medicine.cs" group="Abdessalam Ait Haqi & Pavel Sushko">

///     Copyright (c) 2021 Abdessalam Ait Haqi & Pavel Sushko

/// </copyright>

/// <author> Abdessalam Ait Haqi </author>

/// <contributor> Pavel Sushko </contributor>

///-----------------------------------------------------------------
namespace PharmacyAbdelPavel.Models
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Medicine class
    /// </summary>
    public class Medicine
    {
        #region Data members

        private static readonly List<string> _categories = new List<string> {
            "Liquid",
            "Sublingual tablet",
            "Bucal tablets",
            "Capsules",
            "Suppositories",
            "Drops",
            "Inhalers",
            "Injections",
            "Implants/Patches",
            "Supplements",
            "Other"
        };

        private static List<string> approvedSuppliers;
        private static int lowestMinimumQuantity;
        private static int firstCabinetNumber;
        private static int lastCabinetNumber;
        private string name;
        private int availableQuantity;
        private int minimumQuantity;
        private int cabinetNumber;
        private string supplier;
        private string category;

        #endregion Data members

        #region Properties

        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentException("The name of the medecine is a requiered infromation");

                name = value;
            }
        }

        public int AvailableQuantity
        {
            get => availableQuantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("The available quantity cannot be lower than 0");

                availableQuantity = value;
            }
        }

        public int MinimumQuantity
        {
            get => minimumQuantity;
            set
            {
                if (value < lowestMinimumQuantity)
                    throw new ArgumentException($"The minimum quantity cannot be lower than {lowestMinimumQuantity}");

                minimumQuantity = value;
            }
        }

        public int CabinetNumber
        {
            get => cabinetNumber;
            set
            {
                if (value < firstCabinetNumber || value > lastCabinetNumber)
                    throw new ArgumentException($"The cabinet number cannot be smaller than {firstCabinetNumber} or " +
                        $"bigger than {lastCabinetNumber}");

                cabinetNumber = value;
            }
        }

        public string Supplier
        {
            get => supplier;
            set
            {
                if (!approvedSuppliers.Contains(value))
                    throw new ArgumentException("The supplier must be one of the company's approved suppliers");

                supplier = value;
            }
        }

        public string Category
        {
            get => category;
            set
            {
                if (!_categories.Contains(value))
                    throw new ArgumentException("The category must be one of the categories of medecine");

                category = value;
            }
        }

        public static int LowestMinimumQuantity
        {
            get => lowestMinimumQuantity;
            set
            {
                if (value < 0)
                    throw new ArgumentException("The lowest minimum quantity cannot be negative");

                lowestMinimumQuantity = value;
            }
        }

        public static int FirstCabinetNumber
        {
            get => firstCabinetNumber;
            set
            {
                if (value < 0)
                    throw new ArgumentException("The number of the first cabinet must be positive");

                firstCabinetNumber = value;
            }
        }

        public static int LastCabinetNumber
        {
            get => lastCabinetNumber;
            set
            {
                if (value < firstCabinetNumber)
                    throw new ArgumentException("The number of the last cabinet must come after the number of the " +
                        "first cabinet");

                lastCabinetNumber = value;
            }
        }

        public static List<string> ApprovedSuppliers
        {
            get => approvedSuppliers;
            set
            {
                if (value == null || value.Count < 1)
                    throw new ArgumentException("The list of approved suppliers must contain at least one supplier");

                approvedSuppliers = value;
            }
        }

        public static List<string> Categories => _categories;

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="name"> The name of the medicine. </param>
        /// <param name="availableQuantity"> The available quantity of the medicine. </param>
        /// <param name="minimumQuantity"> The minimum quantity of the medicine. </param>
        /// <param name="cabinetNumber"> The cabinet number of the medicine. </param>
        /// <param name="supplier"> The supplier of the medicine. </param>
        /// <param name="category"> The category of the medicine. </param>
        public Medicine(string name, int availableQuantity, int minimumQuantity, int cabinetNumber, string supplier, string category)
        {
            Name = name;
            AvailableQuantity = availableQuantity;
            MinimumQuantity = minimumQuantity;
            CabinetNumber = cabinetNumber;
            Supplier = supplier;
            Category = category;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Method that adds a supplier to the list of approved suppliers
        /// </summary>
        /// <param name="value"> The supplier to be added. </param>
        /// <returns> True if the supplier was added, false otherwise. </returns>
        public static bool AddAuthorizedSupplier(string value)
        {
            if (approvedSuppliers.Contains(value))
                return false;

            approvedSuppliers.Add(value);

            return true;
        }

        /// <summary>
        /// Removes all instances of the authorized supplier name from the list of authorized suppliers
        /// </summary>
        /// <param name="value"> The name of the authorized supplier to remove. </param>
        /// <returns> True if the supplier was removed, false otherwise. </returns>
        public static bool RemoveAuthorizedSupplier(string value)
        {
            if (!approvedSuppliers.Contains(value))
                return false;

            for (int i = 0; i < approvedSuppliers.Count; i++)
                // If it is List<String>
                if (approvedSuppliers[i] == value)
                    approvedSuppliers.RemoveAt(i);

            return true;
        }

        /// <summary>
        /// Method that returns the CSV string representation of the medicine
        /// </summary>
        /// <returns> The CSV string representation of the medicine. </returns>
        public string ToCSV()
        {
            return $"{name},{availableQuantity},{minimumQuantity},{cabinetNumber},{supplier},{category}";
        }

        #endregion Methods
    }
}