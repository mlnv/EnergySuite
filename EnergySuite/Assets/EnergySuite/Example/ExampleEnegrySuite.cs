using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace EnergySuite
{
    public class ExampleEnegrySuite : MonoBehaviour
    {
        #region Public Vars

        public int CurrentAmount
        {
            get{ return _currentAmount; }
            set
            {
                _currentAmount = value;
                CurrentAmountText.text = _currentAmount.ToString();
            }
        }

        public Text CurrentAmountText;
        public Text TimeLeftText;

        public EnergySuiteManager EnergySuiteManager;

        #endregion

        #region Private Vars

        int _currentAmount;
        const string AmountKey = "Amount";

        #endregion

        void OnEnable()
        {
            CurrentAmount = LoadAmount();

            EnergySuiteManager.OnEnergyAdded += OnEnergyAdded;
            EnergySuiteManager.OnTimeLeftChanged += OnTimeLeftChanged;
        }

        void OnDisable()
        {
            EnergySuiteManager.OnEnergyAdded -= OnEnergyAdded;
            EnergySuiteManager.OnTimeLeftChanged -= OnTimeLeftChanged;
        }

        void OnDestroy()
        {
            SaveAmount(CurrentAmount);
        }

        #region Event Handlers

        void OnEnergyAdded(int amount)
        {
            CurrentAmount += amount;
        }

        void OnTimeLeftChanged(TimeSpan timeLeft)
        {
            string formatString = string.Format("{0:00}:{1:00}", timeLeft.Minutes, timeLeft.Seconds);
            TimeLeftText.text = formatString;
        }

        #endregion

        #region Public Methods

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
