using UnityEngine;
using System.Collections;
using System;

namespace EnergySuite
{
	public class TimeServerHandler
	{
		#region Public Vars

		#endregion

		#region Private Vars

		const float _timeToCheck = 1f;

		readonly EnergySuiteValueBehaviour _energySuiteValueBehaviour;
		readonly TimeBasedValue _timeBasedValue;

		bool _fromPaused;
		bool _waitForCheck = true;
		float _timeToCheckTemp;

		#endregion

		public TimeServerHandler(EnergySuiteValueBehaviour energySuiteValueBehaviour, TimeBasedValue timeBasedValue)
		{
			_timeBasedValue = timeBasedValue;
			_energySuiteValueBehaviour = energySuiteValueBehaviour;
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
				EnergySuiteBehaviour.TimeServ.SetLastClosedTime();
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
			EnergySuiteBehaviour.TimeServ.SetLastClosedTime();
			_fromPaused = false;
			_waitForCheck = true;
		}

		#region Event Handlers

		#endregion

		#region Public Methods

		public void SetLastTimeAdded(long time = -1)
		{
			if (time == -1)
				_timeBasedValue.SetTimeLastAdded();
			else
				_timeBasedValue.SetTimeLastAdded(time);
		}

		public void CheckAmountAdded(long lastClosedTime = -1)
		{
			int result = 0;

			if (lastClosedTime == -1)
				lastClosedTime = EnergySuiteBehaviour.TimeServ.GetLastClosedTime();

			TimeSpan timeLeftToAddEnergy = _timeBasedValue.GetTimeToNextAdd(lastClosedTime);
			long totalTimeToAddEnergy = _timeBasedValue.GetTimeToAddEnergy();

			long lastTimeAdded;

			long timeAddedEnergy = lastClosedTime + (long)timeLeftToAddEnergy.TotalSeconds;

			if (timeAddedEnergy > EnergySuiteBehaviour.CurrentTimeSec)
			{
				_waitForCheck = false;
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
				_energySuiteValueBehaviour.Add(result, true, lastTimeAdded);
			}

			_waitForCheck = false;
		}

		public void CheckCanAddOne()
		{
			if (_timeBasedValue.GetTimeToNextAdd() >= TimeSpan.Zero)
			{
				EnergySuiteManager.OnTimeLeftChanged(_timeBasedValue.GetTimeToNextAdd(), _timeBasedValue);
			}
			else
			{
				_energySuiteValueBehaviour.Add(1);
				EnergySuiteManager.OnTimeLeftChanged(_timeBasedValue.GetTimeToNextAdd(), _timeBasedValue);
			}
		}

		#endregion

		#region Private Methods

		#endregion
	}
}
