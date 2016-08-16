using UnityEngine;
using System;

namespace EnergySuite
{
    public class TimeServer
    {
        #region Public Vars

        #endregion

        #region Private Vars

        const string LastClosedTimeKey = "LastClosedTime";

        #endregion

        public TimeServer()
        {
            ZPlayerPrefs.Initialize(EnergySuiteConfig.Password, EnergySuiteConfig.PasswordSalt);
        }

        #region Event Handlers

        #endregion

        #region Public Methods

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

        void SetLastClosedTimeHandle()
        {
            ZPlayerPrefs.SetString(LastClosedTimeKey, EnergySuiteBehaviour.CurrentTimeSec.ToString());
        }

        long GetLastClosedTimeHandle()
        {
            string timeString = ZPlayerPrefs.GetString(LastClosedTimeKey, EnergySuiteBehaviour.CurrentTimeSec.ToString());
            return (long)Convert.ToDouble(timeString);
        }

        #endregion
    }
}
