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
        public Button AddEnergyButton;
        public Button UseEnergyButton;

        #endregion

        #region Private Vars

        int _currentAmount;
        const string AmountKey = "Amount";

        #endregion

        void OnEnable()
        {
            CurrentAmount = LoadAmount();
            Time.timeScale = 0;
            EnergySuiteManager.Instance.OnEnergyAdded += OnEnergyAdded;
            EnergySuiteManager.Instance.OnTimeLeftChanged += OnTimeLeftChanged;
            AddEnergyButton.onClick.AddListener(OnAddEnergyButtonClicked);
            UseEnergyButton.onClick.AddListener(OnUseEnergyButtonClicked);
        }

        void OnDisable()
        {
            if (EnergySuiteManager.Instance != null)
            {
                EnergySuiteManager.Instance.OnEnergyAdded -= OnEnergyAdded;
                EnergySuiteManager.Instance.OnTimeLeftChanged -= OnTimeLeftChanged;
            }
            AddEnergyButton.onClick.RemoveListener(OnAddEnergyButtonClicked);
            UseEnergyButton.onClick.RemoveListener(OnUseEnergyButtonClicked);
        }

        void OnDestroy()
        {
            SaveAmount(CurrentAmount);
        }

        #region Event Handlers

        void OnAddEnergyButtonClicked()
        {
            EnergySuiteManager.Instance.AddEnergy(1);
        }

        void OnUseEnergyButtonClicked()
        {
            if (CurrentAmount - 1 >= 0)
                CurrentAmount--;
        }

        void OnEnergyAdded(int amount)
        {
            if (CurrentAmount + amount <= EnergySuiteConfig.MaxAmount)
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
