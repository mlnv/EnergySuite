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

        public void OnDestroy()
        {
            SaveAmount(Amount);
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

        #endregion

        #region Private Methods

        void SaveAmount(int amount)
        {
            PlayerPrefs.SetInt(AmountKey, amount);
        }

        int LoadAmount()
        {
            return PlayerPrefs.GetInt(AmountKey, 0);
        }

        #endregion
    }
}
