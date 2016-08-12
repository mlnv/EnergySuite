using UnityEngine;
using System.Collections;
using System;

namespace EnergySuite
{
    public static class EnergySuiteManager
    {
        #region Public Vars

        /// <summary>
        /// Amount of energy changed.
        /// </summary>
        public static Action<int> OnEnergyChanged = delegate
        {
        };

        /// <summary>
        /// Time left to add 1 energy.
        /// </summary>
        public static Action<TimeSpan> OnTimeLeftChanged = delegate
        {
        };

        /// <summary>
        /// Gets the current amount of energy.
        /// </summary>
        /// <value>The amount.</value>
        public static int Amount
        {
            get
            {
                if (EnergySuiteBehaviour.EnergyInventory == null)
                {
                    EnergySuiteBehaviour.CreateInventoryInstance();
                    return EnergySuiteBehaviour.EnergyInventory.Amount;
                }
                else
                {
                    return EnergySuiteBehaviour.EnergyInventory.Amount;
                }
            }
        }

        /// <summary>
        /// Gets the max amount of energy.
        /// </summary>
        /// <value>Max amount.</value>
        public static int MaxAmount
        {
            get
            { 
                return EnergySuiteConfig.MaxAmount;
            }
            private set
            { 
                EnergySuiteConfig.MaxAmount = value;
            }
        }

        #endregion

        #region Private Vars

        #endregion

        #region Event Handlers

        #endregion

        #region Public Methods

        /// <summary>
        /// Add the specified amount of energy.
        /// </summary>
        /// <param name="amount">Amount.</param>
        public static void Add(int amount)
        {
            EnergySuiteBehaviour.Instance.AddEnergy(amount, false);
        }

        /// <summary>
        /// Use the specified amount of energy.
        /// </summary>
        /// <returns>True - succesfully, False - not enough energy to use.</returns>
        /// <param name="amount">Amount.</param>
        public static bool Use(int amount)
        {
            return EnergySuiteBehaviour.Instance.UseEnergy(amount);
        }

        /// <summary>
        /// Converts amlunt of energy to slider value.
        /// </summary>
        /// <returns>Slider value. 0 - 1</returns>
        /// <param name="timeLeft">Time left to add one energy</param>
        public static float ConvertToSliderValue(TimeSpan timeLeft)
        {
            float result = 0f;

            TimeSpan timeToAddEnergy = EnergySuiteBehaviour.TimeServHandler.GetTimeToAddEnergy();
            long timeToAddDuration = timeToAddEnergy.Ticks;
            long timeLeftDuration = timeLeft.Ticks;

            long percent = (100 * timeLeftDuration) / timeToAddDuration;

            result = percent / 100f;

            return result;
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
