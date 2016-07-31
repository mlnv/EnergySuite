using UnityEngine;
using System.Collections;
using System;

namespace EnergySuite
{
    public class TimeServerHandler
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

        const float _timeToCheck = 1f;

        TimeServer _timeServer;
        bool _fromPaused;
        bool _waitForCheck = true;
        float _timeToCheckTemp;

        #endregion

        public TimeServerHandler()
        {
            _timeServer = new TimeServer();
        }

        public void Update()
        {
            if (_waitForCheck)
                return;

            if (_timeToCheckTemp > 0f)
            {
                _timeToCheckTemp -= Time.unscaledDeltaTime;
            }
            else
            {
                _timeToCheckTemp = _timeToCheck;
                CheckCanAddOne();
            }
        }

        public void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                _timeServer.SetLastClosedTime();
                _fromPaused = true;
            }
            else if (_fromPaused)
            {
                _fromPaused = false;
                CheckAmountAdded();
            }
        }

        public void OnDestroy()
        {
            _timeServer.SetLastClosedTime();
            _fromPaused = false;
            _waitForCheck = true;
        }

        #region Event Handlers

        #endregion

        #region Public Methods

        public void SetLastTimeAdded()
        {
            _timeServer.SetTimeLastAdded();
        }

        #endregion

        #region Private Methods

        void CheckCanAddOne()
        {
            if (_timeServer.GetTimeToNextAdd() > TimeSpan.Zero)
            {
                OnTimeLeftChanged(_timeServer.GetTimeToNextAdd());
            }
            else
            {
                OnEnergyAdded(1);
                _timeServer.SetTimeLastAdded();
            }
        }

        public void CheckAmountAdded()
        {
            int result = 0;

            DateTime lastClosedTime = _timeServer.GetLastClosedTime();
            TimeSpan timeLeftToAddEnergy = _timeServer.GetTimeToNextAdd();
            TimeSpan timeToAddEnergy = GetTimeToAddEnergy();

            DateTime lastTimeAdded = DateTime.Now;
            DateTime timeAddedEnergy = lastClosedTime.Add(timeLeftToAddEnergy);

            if (timeAddedEnergy > DateTime.Now)
            {
                _waitForCheck = false;
                return;
            }

            result++;
            lastTimeAdded = timeAddedEnergy;

            while (true)
            {
                DateTime newLastTimeAdded = lastTimeAdded.Add(timeToAddEnergy);

                if (newLastTimeAdded > DateTime.Now)
                    break;

                result++;
                lastTimeAdded = newLastTimeAdded;
            }

            if (result > 0)
            {
                OnEnergyAdded(result);
                _timeServer.SetTimeLastAdded(lastTimeAdded);
            }

            _waitForCheck = false;
        }

        TimeSpan GetTimeToAddEnergy()
        {
            TimeSpan timeToAddEnergyMinutes = TimeSpan.FromMinutes(EnergySuiteConfig.TimeToReloadMinutes);
            TimeSpan timeToAddEnergySeconds = TimeSpan.FromSeconds(EnergySuiteConfig.TimeToReloadSeconds);
            return timeToAddEnergyMinutes.Add(timeToAddEnergySeconds);
        }

        #endregion
    }
}
