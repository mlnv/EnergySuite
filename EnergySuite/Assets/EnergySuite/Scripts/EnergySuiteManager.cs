using UnityEngine;
using System;
using System.Collections;

namespace EnergySuite
{
    public class EnergySuiteManager : Singleton<EnergySuiteManager>
    {
        #region Public Vars

        public Action<int> OnEnergyChanged = delegate
        {
        };

        public Action<TimeSpan> OnTimeLeftChanged = delegate
        {
        };

        public int Amount
        {
            get
            {
                if (_energyInventory != null)
                    return _energyInventory.Amount;
                else
                    return 0;
            }
            private set{ _amount = value; }
        }

        #endregion

        #region Private Vars

        TimeServerHandler _timeServerHandler;
        EnergyInventory _energyInventory;
        int _amount;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _timeServerHandler = new TimeServerHandler();
            _energyInventory = new EnergyInventory();

            _energyInventory.OnEnergyIncreased += OnEnergyIncreasedHandler;
            _energyInventory.OnEnergyDecreased += OnEnergyDecreasedHandler;

            _timeServerHandler.OnTimeLeftChanged += OnTimeLeftChangedTimeServerHandler;

            _timeServerHandler.CheckAmountAdded();
        }

        void OnDisable()
        {
            _energyInventory.OnEnergyIncreased -= OnEnergyIncreasedHandler;
            _energyInventory.OnEnergyDecreased -= OnEnergyDecreasedHandler;
            _timeServerHandler.OnTimeLeftChanged -= OnTimeLeftChangedTimeServerHandler;
        }

        void Update()
        {
            _timeServerHandler.Update();
        }

        void OnDestroy()
        {
            _timeServerHandler.OnDestroy();
            _energyInventory.OnDestroy();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            _timeServerHandler.OnApplicationPause(pauseStatus);
        }

        #region Event Handlers

        void OnEnergyIncreasedHandler(int amount)
        {
            OnEnergyChanged(amount);
        }

        void OnEnergyDecreasedHandler(int amount)
        {
            OnEnergyChanged(amount);
        }

        void OnTimeLeftChangedTimeServerHandler(TimeSpan timeLeft)
        {
            OnTimeLeftChanged(timeLeft.Add(new TimeSpan(0,0,1)));
        }

        #endregion

        #region Public Methods

        public void AddEnergy(int amount, DateTime? customDateTime = null)
        {
            _energyInventory.Add(amount);

            if (customDateTime == null)
                _timeServerHandler.SetLastTimeAdded();
            else
                _timeServerHandler.SetLastTimeAdded(customDateTime);

            _timeServerHandler.CheckCanAddOne();
        }

        public bool UseEnergy(int amount)
        {
            return _energyInventory.Use(amount);
        }

        /// <summary>
        /// Converts to slider value.
        /// </summary>
        /// <returns>The to slider value. 0 - 1</returns>
        /// <param name="timeLeft">Time left to add one</param>
        public float ConvertToSliderValue(TimeSpan timeLeft)
        {
            float result = 0f;

            TimeSpan timeToAddEnergy = _timeServerHandler.GetTimeToAddEnergy();
            long timeToAddDuration = timeToAddEnergy.Ticks;
            long timeLeftDuration = timeLeft.Ticks;

            long percent = (100 * timeLeftDuration) / timeToAddDuration;

            result = percent / 100f;

            return result;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
