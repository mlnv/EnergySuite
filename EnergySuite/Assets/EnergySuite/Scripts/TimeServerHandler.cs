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
			if (waitForCheck)
				return;

			if (timeToCheckTemp > 0f)
			{
				timeToCheckTemp -= Time.unscaledDeltaTime;
			}
			else
			{
				timeToCheckTemp = timeToCheck;
				CheckCanAddOne();
			}
		}

		public void OnApplicationPause(bool pauseStatus)
		{
			if (pauseStatus)
			{
				EnergySuiteBehaviour.TimeServ.SetLastClosedTime();
				fromPaused = true;
			}
			else if (fromPaused)
			{
				fromPaused = false;
				CheckAmountAdded();
			}
		}

		public void OnDestroy()
		{
			EnergySuiteBehaviour.TimeServ.SetLastClosedTime();
			fromPaused = false;
			waitForCheck = true;
		}

		public void SetLastTimeAdded(long time = -1)
		{
			if (time == -1)
				timeBasedValue.SetTimeLastAdded();
			else
				timeBasedValue.SetTimeLastAdded(time);
		}

		public void CheckAmountAdded(long lastClosedTime = -1)
		{
			int result = 0;

			if (lastClosedTime == -1)
				lastClosedTime = EnergySuiteBehaviour.TimeServ.GetLastClosedTime();

			TimeSpan timeLeftToAddEnergy = timeBasedValue.GetTimeToNextAdd(lastClosedTime);
			long totalTimeToAddEnergy = timeBasedValue.GetTimeToAddEnergy();

			long lastTimeAdded;

			long timeAddedEnergy = lastClosedTime + (long)timeLeftToAddEnergy.TotalSeconds;

			if (timeAddedEnergy > EnergySuiteBehaviour.CurrentTimeSec)
			{
				waitForCheck = false;
				return;
			}

			result++;
			lastTimeAdded = timeAddedEnergy;

			while (true)
			{
				long newLastTimeAdded = lastTimeAdded + totalTimeToAddEnergy;

				if (newLastTimeAdded > EnergySuiteBehaviour.CurrentTimeSec)
					break;

				result++;
				lastTimeAdded = newLastTimeAdded;
			}

			if (result > 0)
			{
				energySuiteValueBehaviour.Add(result, true, lastTimeAdded);
			}

			waitForCheck = false;
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
