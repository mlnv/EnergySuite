using UnityEngine;
using System;

namespace EnergySuite
{
    public class TimeServer
    {
        private const string LastClosedTimeKey = "LastClosedTime";

        public TimeServer()
        {
            ZPlayerPrefs.Initialize(EnergySuiteConfig.Password, EnergySuiteConfig.PasswordSalt);
        }

        public void SetLastClosedTime()
        {
            this.SetLastClosedTimeHandle();
        }

        public long GetLastClosedTime()
        {
            return this.GetLastClosedTimeHandle();
        }

        private void SetLastClosedTimeHandle()
        {
            ZPlayerPrefs.SetString(LastClosedTimeKey, EnergySuiteBehaviour.CurrentTimeSec.ToString());
        }

        private long GetLastClosedTimeHandle()
        {
            var timeString = ZPlayerPrefs.GetString(LastClosedTimeKey, EnergySuiteBehaviour.CurrentTimeSec.ToString());
            return (long)Convert.ToDouble(timeString);
        }
    }
}
