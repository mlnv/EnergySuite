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

        #region Event Handlers

        #endregion

        #region Public Methods

        public void SetTimeLastAdded(DateTime? dateTime = null)
        {
            SetTimeLastAddedHandle(dateTime);
        }

        public TimeSpan GetTimeToNextAdd()
        {
            return GetTimeLeft();
        }

        public void SetLastClosedTime()
        {
            SetLastClosedTimeHandle();
        }

        public DateTime GetLastClosedTime()
        {
            return GetLastClosedTimeHandle();
        }

        #endregion

        #region Private Methods

        void SetTimeLastAddedHandle(DateTime? dateTime)
        {
            DateTime dt;

            if (dateTime == null)
                dt = DateTime.Now;
            else
            {
                dt = (DateTime)dateTime;
            }

            PlayerPrefs.SetString(LastTimeEnergyAddedKey, dt.ConvertToUnixTimestamp().ToString());
        }

        void SetLastClosedTimeHandle()
        {
            PlayerPrefs.SetString(LastClosedTimeKey, DateTime.Now.ConvertToUnixTimestamp().ToString());
        }

        DateTime GetLastClosedTimeHandle()
        {
            string timeString = PlayerPrefs.GetString(LastClosedTimeKey, DateTime.Now.ConvertToUnixTimestamp().ToString());
            double unixTime = Convert.ToInt64(timeString);
            return new DateTime().ConvertFromUnixTimestamp(unixTime);
        }

        TimeSpan GetTimeLeft()
        {
            DateTime timeToAddEnergy = GetTimeFromLastAdded().AddMinutes(EnergySuiteConfig.TimeToReloadMinutes).AddSeconds(EnergySuiteConfig.TimeToReloadSeconds);
            TimeSpan result = timeToAddEnergy.Subtract(DateTime.Now);
            return result;
        }

        DateTime GetTimeFromLastAdded()
        {
            if (!PlayerPrefs.HasKey(LastTimeEnergyAddedKey))
                SetTimeLastAddedHandle(null);
            
            string timeString = PlayerPrefs.GetString(LastTimeEnergyAddedKey, DateTime.Now.ConvertToUnixTimestamp().ToString());

            double unixTime = Convert.ToInt64(timeString);
            return new DateTime().ConvertFromUnixTimestamp(unixTime);
        }

        #endregion
    }
}
