using UnityEngine;
using System.Collections;
using System;

namespace EnergySuite
{
	public static class EnergySuiteManager
	{
		#region Public Vars

		/// <summary>
		/// The amount changed.
		/// </summary>
		public static Action<int, TimeBasedValue> OnAmountChanged = delegate
		{
		};

		/// <summary>
		/// Time left to add 1.
		/// </summary>
		public static Action<TimeSpan, TimeBasedValue> OnTimeLeftChanged = delegate
		{
		};

		#endregion

		#region Private Vars

		#endregion

		#region Event Handlers

		#endregion

		#region Public Methods

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
			float result = 0f;

			long timeToAddDuration = timeBasedValue.GetTimeToAddEnergy();
			long timeLeftDuration = (long)timeLeft.TotalSeconds;

			long percent = (100 * timeLeftDuration) / timeToAddDuration;
			result = percent / 100f;

			return result;
		}

		#endregion

		#region Private Methods

		#endregion
	}
}
