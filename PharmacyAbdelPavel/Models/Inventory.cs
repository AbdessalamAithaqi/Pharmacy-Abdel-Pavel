///-----------------------------------------------------------------

/// <copyright file="Inventory.cs" group="Abdessalam Ait Haqi & Pavel Sushko">

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
    /// Class of the medicine inventory.
    /// </summary>
    public static class Inventory
    {
        #region Data members

        private static List<Medicine> medicineList = new List<Medicine> { };

        #endregion Data members

        #region Properties

        public static List<Medicine> InventoryList
        {
            get => medicineList;
            set
            {
                if (value == null || value.Count < 1)
                    throw new ArgumentException("The list of medecine must contain at least one value");

                medicineList = value;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Adds a new medicine to the inventory.
        /// </summary>
        /// <param name="value"> The medicine to be added. </param>
        public static void AddToInventoryList(Medicine value)
        {
            if (value != null)
                medicineList.Add(value);
        }

        /// <summary>
        /// Removes a medicine from the inventory.
        /// </summary>
        /// <param name="value"> The medicine to be removed. </param>
        public static void RemoveFromInventoryList(Medicine value)
        {
            if (medicineList.Contains(value))
                _ = medicineList.Remove(value);
        }

        public static void UpdateItemInList(Medicine oldValue, Medicine newValue)
        {
            if (medicineList.Contains(oldValue))
            {
                RemoveFromInventoryList(oldValue);
                AddToInventoryList(newValue);
            }
        }

        public static List<Medicine> ShoppingList()
        {
            List<Medicine> shoppingList = new List<Medicine>();

            foreach (Medicine medecine in medicineList)
                if (medecine.AvailableQuantity < medecine.MinimumQuantity)
                    shoppingList.Add(medecine);

            return shoppingList;
        }

        #endregion Methods
    }
}