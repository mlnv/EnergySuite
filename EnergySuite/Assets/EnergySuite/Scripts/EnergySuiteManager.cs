using UnityEngine;
using System.Collections;
using System;

namespace EnergySuite
{
	public static class EnergySuiteManager
	{
		/// <summary>
		/// The amount changed.
		/// </summary>
		public static Action<int, TimeBasedValue> OnAmountChanged = delegate { };

		/// <summary>
		/// Time left to add 1.
		/// </summary>
		public static Action<TimeSpan, TimeBasedValue> OnTimeLeftChanged = delegate { };

		/// <summary>
		/// Add the specified amount.
		/// </summary>
		/// <param name="amount">Amount.</param>
		public static void Add(TimeValue type, int amount)
		{
			EnergySuiteBehaviour.Instance.Add(type, amount, false);
		}

		/// <summary>
		/// Use the specified amount.
		/// </summary>
		/// <returns>True - succesfully, False - not enough energy to use.</returns>
		/// <param name="amount">Amount.</param>
		public static bool Use(TimeValue type, int amount)
		{
			return EnergySuiteBehaviour.Instance.Use(type, amount);
		}

		/// <summary>
		/// Gets the current amount.
		/// </summary>
		/// <returns>The amount.</returns>
		/// <param name="timeValue">Time value.</param>
		public static int GetAmount(TimeValue timeValue)
		{
			return EnergySuiteConfig.StoredInfo[timeValue].Amount;
		}

		public static int GetMaxAmount(TimeValue timeValue)
		{
			return EnergySuiteConfig.StoredInfo[timeValue].MaxAmount;
		}

		/// <summary>
		/// Converts amount to slider value. (0 - 1)
		/// </summary>
		/// <returns>Slider value. 0 - 1</returns>
		/// <param name="timeLeft">Time left to add one energy</param>
		public static float ConvertToSliderValue(TimeSpan timeLeft, TimeBasedValue timeBasedValue)
		{
			var result = 0f;

			long timeToAddDuration = timeBasedValue.GetTimeToAddEnergy();
			var timeLeftDuration = (long)timeLeft.TotalSeconds;

			var percent = (100 * timeLeftDuration) / timeToAddDuration;
			result = percent / 100f;

			return result;
		}
	}
}
