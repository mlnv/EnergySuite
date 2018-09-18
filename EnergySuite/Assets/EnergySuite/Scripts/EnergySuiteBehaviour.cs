using UnityEngine;
using System;
using System.Collections.Generic;

namespace EnergySuite
{
	public class EnergySuiteBehaviour : Singleton<EnergySuiteBehaviour>
	{
		public static TimeServer TimeServ;
		public static long CurrentTimeSec;

        private Dictionary<TimeValue, EnergySuiteValueBehaviour> valueBehaviours = new Dictionary<TimeValue, EnergySuiteValueBehaviour>();
        private static bool settingUp = true;
        private static float storedTickCount;
        private static Action onUpdateTimeComplete = delegate
		{

		};

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
					valueBehaviours.Add(enumerator.Current.Key, go.GetComponent<EnergySuiteValueBehaviour>());
				}
			});
		}

		private void Update()
		{
			if (settingUp)
				return;

			storedTickCount += Time.unscaledDeltaTime;
			if (storedTickCount > 1)
			{
				CurrentTimeSec++;
				storedTickCount = 0;
			}
		}

		private void OnDestroy()
		{
			var enumerator = valueBehaviours.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Value.TimeServHandler.OnDestroy();
				enumerator.Current.Value.TimeBasedValue.SaveAmount();
			}
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			UpdateCurrentTime(delegate
			{
				var enumerator = valueBehaviours.GetEnumerator();
				while (enumerator.MoveNext())
				{
					enumerator.Current.Value.TimeServHandler.OnApplicationPause(pauseStatus);

					if (pauseStatus)
						enumerator.Current.Value.TimeBasedValue.SaveAmount();
				}
			});
		}

		public static void UpdateCurrentTime(Action onComplete)
		{
			if (onComplete != null)
				onUpdateTimeComplete = onComplete;

			settingUp = true;
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
			settingUp = false;
			onUpdateTimeComplete();
		}

		public void CallbackGetTime(string value)
		{
			EnergySuiteBehaviour.CallbackGetTimeStatic(value);
		}

		public void Add(TimeValue type, int amount, bool setTime = true, long customTime = -1)
		{
			valueBehaviours[type].Add(amount, setTime, customTime);
		}

		public bool Use(TimeValue type, int amount)
		{
			return valueBehaviours[type].Use(amount);
		}
	}
}
