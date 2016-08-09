using UnityEngine;
using System.Collections;
using System;

namespace EnergySuite
{
    public class EnergyInventory
    {
        #region Public Vars

        public int Amount { get; private set; }

        public Action<int> OnEnergyIncreased = delegate
        {
        };

        public Action<int> OnEnergyDecreased = delegate
        {
        };

        #endregion

        #region Private Vars

        int _maxAmount;
        const string AmountKey = "Amount";

        #endregion

        public EnergyInventory()
        {
            Amount = LoadAmount();
            _maxAmount = EnergySuiteConfig.MaxAmount;
        }

        #region Event Handlers

        #endregion

        #region Public Methods

        public void Add(int amount)
        {
            if (Amount + amount <= _maxAmount)
                Amount += amount;
            else
            {
                Amount = _maxAmount;
            }

            OnEnergyIncreased(Amount);
        }

        public bool Use(int amount)
        {
            if (Amount - amount >= 0)
                Amount -= amount;
            else
                return false;

            OnEnergyDecreased(Amount);
            return true;
        }

        public bool IsFull()
        {
            return Amount >= _maxAmount;
        }

        public void SaveAmount()
        {
            PlayerPrefs.SetInt(AmountKey, Amount);
        }

        #endregion

        #region Private Methods

        int LoadAmount()
        {
            return PlayerPrefs.GetInt(AmountKey, 0);
        }

        #endregion
    }
}
