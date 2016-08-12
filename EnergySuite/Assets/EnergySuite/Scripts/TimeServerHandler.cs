using UnityEngine;
using System.Collections;
using System;

namespace EnergySuite
{
    public class TimeServerHandler
    {
        #region Public Vars

        public Action<TimeSpan> OnTimeLeftChanged = delegate
        {
        };

        #endregion

        #region Private Vars

        const float _timeToCheck = 0.5f;

        public readonly TimeServer TimeServ;
        readonly EnergySuiteBehaviour _energySuiteBehaviour;

        bool _fromPaused;
        bool _waitForCheck = true;
        float _timeToCheckTemp;

        #endregion

        public TimeServerHandler(EnergySuiteBehaviour energySuiteBehaviour)
        {
            TimeServ = new TimeServer();
            _energySuiteBehaviour = energySuiteBehaviour;
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
                TimeServ.SetLastClosedTime();
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
            TimeServ.SetLastClosedTime();
            _fromPaused = false;
            _waitForCheck = true;
        }

        #region Event Handlers

        #endregion

        #region Public Methods

        public void SetLastTimeAdded(long time = -1)
        {
            if (time == -1)
                TimeServ.SetTimeLastAdded();
            else
                TimeServ.SetTimeLastAdded(time);
        }

        public void CheckAmountAdded(long lastClosedTime = -1)
        {
            int result = 0;

            if (lastClosedTime == -1)
                lastClosedTime = TimeServ.GetLastClosedTime();

            TimeSpan timeLeftToAddEnergy = TimeServ.GetTimeToNextAdd();
            TimeSpan timeToAddEnergy = GetTimeToAddEnergy();

            long lastTimeAdded;

            long timeAddedEnergy = lastClosedTime + timeLeftToAddEnergy.Seconds;

            if (timeAddedEnergy > EnergySuiteBehaviour.CurrentTimeSec)
            {
                _waitForCheck = false;
                return;
            }

            result++;
            lastTimeAdded = timeAddedEnergy;

            while (true)
            {
                long newLastTimeAdded = lastTimeAdded + timeToAddEnergy.Seconds;

                if (newLastTimeAdded > EnergySuiteBehaviour.CurrentTimeSec)
                    break;

                result++;
                lastTimeAdded = newLastTimeAdded;
            }

            if (result > 0)
            {
                _energySuiteBehaviour.AddEnergy(result, true, lastTimeAdded);
            }

            _waitForCheck = false;
        }

        public TimeSpan GetTimeToAddEnergy()
        {
            TimeSpan timeToAddEnergyMinutes = TimeSpan.FromMinutes(EnergySuiteConfig.TimeToReloadMinutes);
            TimeSpan timeToAddEnergySeconds = TimeSpan.FromSeconds(EnergySuiteConfig.TimeToReloadSeconds);
            return timeToAddEnergyMinutes.Add(timeToAddEnergySeconds);
        }

        public void CheckCanAddOne()
        {
            EnergySuiteBehaviour.UpdateCurrentTime();

            if (TimeServ.GetTimeToNextAdd() >= TimeSpan.Zero)
            {
                OnTimeLeftChanged(TimeServ.GetTimeToNextAdd());
            }
            else
            {
                _energySuiteBehaviour.AddEnergy(1);
                OnTimeLeftChanged(TimeServ.GetTimeToNextAdd());
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
