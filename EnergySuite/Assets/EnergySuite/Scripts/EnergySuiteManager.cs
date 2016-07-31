using UnityEngine;
using System;
using System.Collections;

namespace EnergySuite
{
    public class EnergySuiteManager : Singleton<EnergySuiteManager>
    {
        #region Public Vars

        public Action<int> OnEnergyAdded = delegate
        {
        };

        public Action<TimeSpan> OnTimeLeftChanged = delegate
        {
        };

        #endregion

        #region Private Vars

        TimeServerHandler _timeServerHandler;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _timeServerHandler = new TimeServerHandler();

            _timeServerHandler.OnEnergyAdded += OnEnergyAddedTimeServerHandler;
            _timeServerHandler.OnTimeLeftChanged += OnTimeLeftChangedTimeServerHandler;

            _timeServerHandler.CheckAmountAdded();
        }

        void OnDisable()
        {
            _timeServerHandler.OnEnergyAdded -= OnEnergyAddedTimeServerHandler;
            _timeServerHandler.OnTimeLeftChanged -= OnTimeLeftChangedTimeServerHandler;
        }

        void Update()
        {
            _timeServerHandler.Update();
        }

        void OnDestroy()
        {
            _timeServerHandler.OnDestroy();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            _timeServerHandler.OnApplicationPause(pauseStatus);
        }

        #region Event Handlers

        void OnEnergyAddedTimeServerHandler(int amount)
        {
            OnEnergyAdded(amount);
        }

        void OnTimeLeftChangedTimeServerHandler(TimeSpan timeLeft)
        {
            OnTimeLeftChanged(timeLeft);
        }

        #endregion

        #region Public Methods

        public void AddEnergy(int amount)
        {
            OnEnergyAdded(amount);
            _timeServerHandler.SetLastTimeAdded();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
