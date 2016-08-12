using UnityEngine;
using System;

namespace EnergySuite
{
    public class TimeServer
    {
        #region Public Vars

        #endregion

        #region Private Vars

        const string LastTimeEnergyAddedKey = "LastTimeEnergyAdded";
        const string LastClosedTimeKey = "LastClosedTime";

        #endregion

        public TimeServer()
        {
            ZPlayerPrefs.Initialize(EnergySuiteConfig.Password, EnergySuiteConfig.PasswordSalt);
        }

        #region Event Handlers

        #endregion

        #region Public Methods

        public void SetTimeLastAdded(long time = -1)
        {
            SetTimeLastAddedHandle(time);
        }

        public TimeSpan GetTimeToNextAdd()
        {
            return GetTimeLeft();
        }

        public void SetLastClosedTime()
        {
            SetLastClosedTimeHandle();
        }

        public long GetLastClosedTime()
        {
            return GetLastClosedTimeHandle();
        }

        #endregion

        #region Private Methods

        void SetTimeLastAddedHandle(long time = -1)
        {
            long timeToSave;

            if (time == -1)
                timeToSave = EnergySuiteBehaviour.CurrentTimeSec;
            else
                timeToSave = time;

            ZPlayerPrefs.SetString(LastTimeEnergyAddedKey, timeToSave.ToString());
        }

        void SetLastClosedTimeHandle()
        {
            ZPlayerPrefs.SetString(LastClosedTimeKey, EnergySuiteBehaviour.CurrentTimeSec.ToString());
        }

        long GetLastClosedTimeHandle()
        {
            string timeString = ZPlayerPrefs.GetString(LastClosedTimeKey, EnergySuiteBehaviour.CurrentTimeSec.ToString());
            return (long)Convert.ToDouble(timeString);
        }

        TimeSpan GetTimeLeft()
        {
            long timeToAddEnergy = GetTimeFromLastAdded() + EnergySuiteConfig.TimeToReloadMinutes * 60 + EnergySuiteConfig.TimeToReloadSeconds;
            int seconds = (int)(timeToAddEnergy - EnergySuiteBehaviour.CurrentTimeSec);
            return new TimeSpan(0,0, seconds);
        }

        long GetTimeFromLastAdded()
        {
            if (!ZPlayerPrefs.HasKey(LastTimeEnergyAddedKey))
                SetTimeLastAddedHandle();
            
            string timeString = ZPlayerPrefs.GetString(LastTimeEnergyAddedKey, EnergySuiteBehaviour.CurrentTimeSec.ToString());

            return (long)Convert.ToDouble(timeString);
        }

        #endregion
    }
}
