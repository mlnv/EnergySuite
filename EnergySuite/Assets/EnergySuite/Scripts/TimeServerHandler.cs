using UnityEngine;
using System.Collections;
using System;

namespace EnergySuite
{
	public class TimeServerHandler
	{
        private const float timeToCheck = 1f;

        private readonly EnergySuiteValueBehaviour energySuiteValueBehaviour;
        private readonly TimeBasedValue timeBasedValue;

        private bool fromPaused;
        private bool waitForCheck = true;
        private float timeToCheckTemp;

		public TimeServerHandler(EnergySuiteValueBehaviour energySuiteValueBehaviour, TimeBasedValue timeBasedValue)
		{
			this.timeBasedValue = timeBasedValue;
			this.energySuiteValueBehaviour = energySuiteValueBehaviour;
		}

		public void Update()
		{
            if (this.waitForCheck)
            {
                return;
            }

			if (this.timeToCheckTemp > 0f)
			{
				this.timeToCheckTemp -= Time.unscaledDeltaTime;
			}
			else
			{
				this.timeToCheckTemp = timeToCheck;
				this.CheckCanAddOne();
			}
		}

		public void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				EnergySuiteBehaviour.TimeServ.SetLastClosedTime();
				this.fromPaused = true;
			}
			else if (this.fromPaused)
			{
				this.fromPaused = false;
				this.CheckAmountAdded();
			}
		}

		public void OnDestroy()
		{
			EnergySuiteBehaviour.TimeServ.SetLastClosedTime();
			this.fromPaused = false;
			this.waitForCheck = true;
		}

		public void SetLastTimeAdded(long time = -1)
		{
            if (time == -1)
            {
                timeBasedValue.SetTimeLastAdded();
            }
            else
            {
                timeBasedValue.SetTimeLastAdded(time);
            }
		}

		public void CheckAmountAdded(long lastClosedTime = -1)
		{
			var result = 0;

            if (lastClosedTime == -1)
            {
                lastClosedTime = EnergySuiteBehaviour.TimeServ.GetLastClosedTime();
            }

			var timeLeftToAddEnergy = timeBasedValue.GetTimeToNextAdd(lastClosedTime);
			var totalTimeToAddEnergy = timeBasedValue.GetTimeToAddEnergy();
			var timeAddedEnergy = lastClosedTime + (long)timeLeftToAddEnergy.TotalSeconds;

			if (timeAddedEnergy > EnergySuiteBehaviour.CurrentTimeSec)
			{
				this.waitForCheck = false;
				return;
			}

			result++;
			var lastTimeAdded = timeAddedEnergy;

			while (true)
			{
				var newLastTimeAdded = lastTimeAdded + totalTimeToAddEnergy;

                if (newLastTimeAdded > EnergySuiteBehaviour.CurrentTimeSec)
                {
                    break;
                }

				result++;
				lastTimeAdded = newLastTimeAdded;
			}

			if (result > 0)
			{
				energySuiteValueBehaviour.Add(result, true, lastTimeAdded);
			}

			this.waitForCheck = false;
		}

		public void CheckCanAddOne()
		{
			if (timeBasedValue.GetTimeToNextAdd() >= TimeSpan.Zero)
			{
				EnergySuiteManager.OnTimeLeftChanged(timeBasedValue.GetTimeToNextAdd(), timeBasedValue);
			}
			else
			{
				energySuiteValueBehaviour.Add(1);
				EnergySuiteManager.OnTimeLeftChanged(timeBasedValue.GetTimeToNextAdd(), timeBasedValue);
			}
		}
	}
}
