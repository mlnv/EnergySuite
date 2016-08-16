using UnityEngine;
using System.Collections;
using Prime31.StateKitLite;

namespace EnergySuite
{
	public enum State
	{
		Init,
		Adding,
		Full
	}

	public class EnergySuiteValueBehaviour : StateKitLite<State>
	{
		#region Public Vars

		public TimeServerHandler TimeServHandler;
		public TimeBasedValue TimeBasedValue;

		#endregion

		#region Private Vars

		#endregion

		#region Event Handlers

		void Init_Enter()
		{
			if (EnergySuiteBehaviour.CurrentTimeSec > EnergySuiteBehaviour.TimeServ.GetLastClosedTime())
			{
				TimeServHandler.CheckAmountAdded();
			}
			else {
				TimeServHandler.CheckAmountAdded(0);
			}

			if (TimeBasedValue.IsFull())
				currentState = State.Full;
			else
				currentState = State.Adding;
		}

		void Adding_Tick()
		{
			if (TimeBasedValue.IsFull())
				currentState = State.Full;
			else
				TimeServHandler.Update();
		}

		void Full_Exit()
		{
			TimeBasedValue.SetTimeLastAdded();
		}

		#endregion

		#region Public Methods

		public void Add(int amount, bool setTime = true, long customTime = -1)
		{
			TimeBasedValue.Add(amount);

			if (setTime)
			{
				if (customTime == -1)
					TimeBasedValue.SetTimeLastAdded();
				else
					TimeBasedValue.SetTimeLastAdded(customTime);
			}

			if (TimeBasedValue.IsFull())
				currentState = State.Full;
		}

		public bool Use(int amount)
		{
			currentState = State.Adding;
			return TimeBasedValue.Use(amount);
		}

		public void CustomInit(TimeBasedValue timeBasedValue)
		{
			TimeServHandler = new TimeServerHandler(this, timeBasedValue);
			TimeBasedValue = timeBasedValue;
			initialState = State.Init;
		}

		#endregion

		#region Private Methods

		#endregion
	}
}
