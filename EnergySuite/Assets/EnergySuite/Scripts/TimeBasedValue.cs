using UnityEngine;
using System.Collections;
using System;

namespace EnergySuite
{
	public class TimeBasedValue
	{
		#region Public Vars

		public TimeValue Type;
		public long LastTimeAdded;
		public int TimeToReloadMin;
		public int TimeToReloadSec;
		public int MaxAmount;

		public int Amount { get; private set; }

		#endregion

		#region Private Vars

		#endregion

		public TimeBasedValue(TimeValue type, int timeToReloadMin, int timeToReloadSec, int maxAmount)
		{
			ZPlayerPrefs.Initialize(EnergySuiteConfig.Password, EnergySuiteConfig.PasswordSalt);

			Type = type;
			TimeToReloadMin = timeToReloadMin;
			TimeToReloadSec = timeToReloadSec;
			MaxAmount = maxAmount;
			LastTimeAdded = GetTimeFromLastAdded();

			Amount = LoadAmount();
		}

		#region Event Handlers

		#endregion

		#region Public Methods

		public void Add(int amount)
		{
			if (Amount + amount <= MaxAmount)
				Amount += amount;
			else
			{
				Amount = MaxAmount;
			}

			EnergySuiteManager.OnAmountChanged(Amount, this);
		}

		public bool Use(int amount)
		{
			if (Amount - amount >= 0)
				Amount -= amount;
			else
				return false;

			EnergySuiteManager.OnAmountChanged(Amount, this);
			return true;
		}

		public bool IsFull()
		{
			return Amount >= MaxAmount;
		}

		public TimeSpan GetTimeToNextAdd(long fromTime = -1)
		{
			return GetTimeLeft(fromTime);
		}

		public void SetTimeLastAdded(long time = -1)
		{
			SetTimeLastAddedHandle(time);
		}

		public void SaveAmount()
		{
			ZPlayerPrefs.SetInt(GetAmountKey(), Amount);
		}

		public long GetTimeToAddEnergy()
		{
			return TimeToReloadMin * 60 + TimeToReloadSec;
		}

		#endregion

		#region Private Methods

		int LoadAmount()
		{
			return ZPlayerPrefs.GetInt(GetAmountKey(), 0);
		}

		void SetTimeLastAddedHandle(long time = -1)
		{
			long timeToSave;

			if (time == -1)
				timeToSave = EnergySuiteBehaviour.CurrentTimeSec;
			else
				timeToSave = time;

			LastTimeAdded = timeToSave;
			ZPlayerPrefs.SetString(GetLastTimeAddedKey(), timeToSave.ToString());
		}

		TimeSpan GetTimeLeft(long fromTime = -1)
		{
			long currentTime = 0;

			if (fromTime == -1)
				currentTime = EnergySuiteBehaviour.CurrentTimeSec;
			else
				currentTime = fromTime;

			long timeToAddEnergy = LastTimeAdded + GetTimeToAddEnergy();
			int seconds = (int)(timeToAddEnergy - currentTime);
			return new TimeSpan(0, 0, seconds);
		}

		long GetTimeFromLastAdded()
		{
			if (!ZPlayerPrefs.HasKey(GetLastTimeAddedKey()))
				SetTimeLastAddedHandle();

			string timeString = ZPlayerPrefs.GetString(GetLastTimeAddedKey(), EnergySuiteBehaviour.CurrentTimeSec.ToString());

			return (long)Convert.ToDouble(timeString);
		}

		string GetLastTimeAddedKey()
		{
			return EnergySuiteConfig.LastTimeAddedPrefixKey + Type.ToString();
		}

		string GetAmountKey()
		{
			return EnergySuiteConfig.AmountPrefixKey + Type.ToString();
		}

		#endregion
	}
}
