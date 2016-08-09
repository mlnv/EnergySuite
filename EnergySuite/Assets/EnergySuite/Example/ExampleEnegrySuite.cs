using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace EnergySuite
{
    public class ExampleEnegrySuite : MonoBehaviour
    {
        #region Public Vars

        public Text CurrentAmountText;
        public Text TimeLeftText;
        public Slider TimeLeftSlider;
        public Button AddEnergyButton;
        public Button UseEnergyButton;

        #endregion

        #region Private Vars

        #endregion

        void OnEnable()
        {
            Time.timeScale = 0;
            EnergySuiteManager.OnEnergyChanged += OnEnergyAdded;
            EnergySuiteManager.OnTimeLeftChanged += OnTimeLeftChanged;
            AddEnergyButton.onClick.AddListener(OnAddEnergyButtonClicked);
            UseEnergyButton.onClick.AddListener(OnUseEnergyButtonClicked);
        }

        void OnDisable()
        {
            EnergySuiteManager.OnEnergyChanged -= OnEnergyAdded;
            EnergySuiteManager.OnTimeLeftChanged -= OnTimeLeftChanged;
            AddEnergyButton.onClick.RemoveListener(OnAddEnergyButtonClicked);
            UseEnergyButton.onClick.RemoveListener(OnUseEnergyButtonClicked);
        }

        void Start()
        {
            CurrentAmountText.text = EnergySuiteManager.Amount + "/" + EnergySuiteManager.MaxAmount;
        }

        #region Event Handlers

        void OnAddEnergyButtonClicked()
        {
            EnergySuiteManager.Add(1);
        }

        void OnUseEnergyButtonClicked()
        {
            EnergySuiteManager.Use(1);
        }

        void OnEnergyAdded(int amount)
        {
            CurrentAmountText.text = amount + "/" + EnergySuiteManager.MaxAmount;
        }

        void OnTimeLeftChanged(TimeSpan timeLeft)
        {
            string formatString = string.Format("{0:00}:{1:00}", timeLeft.Minutes, timeLeft.Seconds);
            TimeLeftText.text = formatString;

            TimeLeftSlider.value = EnergySuiteManager.ConvertToSliderValue(timeLeft);
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
