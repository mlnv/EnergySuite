using UnityEngine;
using System;
using System.Collections.Generic;

namespace EnergySuite
{
	public class EnergySuiteBehaviour : Singleton<EnergySuiteBehaviour>
	{
		#region Public Vars

		public static TimeServer TimeServ;
		public static long CurrentTimeSec;

		#endregion

		#region Private Vars

		Dictionary<TimeValue, EnergySuiteValueBehaviour> _valueBehaviours = new Dictionary<TimeValue, EnergySuiteValueBehaviour>();
		static bool _settingUp = true;
		static float _storedTickCount;
		static Action _onUpdateTimeComplete = delegate
		{

		};

		#endregion

		protected override void Awake()
		{
			base.Awake();
			TimeServ = new TimeServer();

			UpdateCurrentTime(delegate
			{
				var enumerator = EnergySuiteConfig.StoredInfo.GetEnumerator();
				while (enumerator.MoveNext())
				{
					GameObject go = new GameObject();
					go.name = "EnergySuiteValueBehvaiour_" + enumerator.Current.Value.Type.ToString();
					go.transform.parent = transform;
					go.AddComponent<EnergySuiteValueBehaviour>().CustomInit(enumerator.Current.Value);
					_valueBehaviours.Add(enumerator.Current.Key, go.GetComponent<EnergySuiteValueBehaviour>());
				}
			});
		}

		void Update()
		{
			if (_settingUp)
				return;

			_storedTickCount += Time.unscaledDeltaTime;
			if (_storedTickCount > 1)
			{
				CurrentTimeSec++;
				_storedTickCount = 0;
			}
		}

		void OnDestroy()
		{
			var enumerator = _valueBehaviours.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.TimeServHandler.OnDestroy();
				enumerator.Current.Value.TimeBasedValue.SaveAmount();
			}
		}

		void OnApplicationPause(bool pauseStatus)
		{
			UpdateCurrentTime(delegate
			{
				var enumerator = _valueBehaviours.GetEnumerator();
				while (enumerator.MoveNext())
				{
					enumerator.Current.Value.TimeServHandler.OnApplicationPause(pauseStatus);

					if (pauseStatus)
						enumerator.Current.Value.TimeBasedValue.SaveAmount();
				}
			});
		}

		#region Event Handlers

		#endregion

		#region Public Methods

		public static void UpdateCurrentTime(Action onComplete)
		{
			if (onComplete != null)
				_onUpdateTimeComplete = onComplete;

			_settingUp = true;
#if UNITY_IOS && !UNITY_EDITOR
            iOSBridge.GetCurrentMediaTime();
#elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaObject jo = new AndroidJavaObject("android.os.SystemClock");
            long time = jo.CallStatic<long>("elapsedRealtime"); 
            long timeSec = time/1000;
            CallbackGetTimeStatic(timeSec.ToString());
#elif UNITY_STANDALONE || UNITY_EDITOR
			int time = Environment.TickCount;

			if (time < 0)
				time = Int32.MaxValue + Environment.TickCount;

			int timeSec = time / 1000;
			CallbackGetTimeStatic(timeSec.ToString());
#endif
		}

		public static void CallbackGetTimeStatic(string value)
		{
			CurrentTimeSec = (long)Convert.ToDouble(value);
			_settingUp = false;
			_onUpdateTimeComplete();
		}

		public void CallbackGetTime(string value)
		{
			EnergySuiteBehaviour.CallbackGetTimeStatic(value);
		}

		public void Add(TimeValue type, int amount, bool setTime = true, long customTime = -1)
		{
			_valueBehaviours[type].Add(amount, setTime, customTime);
		}

		public bool Use(TimeValue type, int amount)
		{
			return _valueBehaviours[type].Use(amount);
		}

		#endregion

		#region Private Methods

		#endregion
	}
}
