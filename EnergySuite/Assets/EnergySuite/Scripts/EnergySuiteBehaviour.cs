using UnityEngine;
using System;
using MonsterLove.StateMachine;

namespace EnergySuite
{
    public class EnergySuiteBehaviour : Singleton<EnergySuiteBehaviour>
    {
        public enum State
        {
            Init,
            Adding,
            Full
        }

        #region Public Vars

        public static TimeServerHandler TimeServHandler;
        public static EnergyInventory EnergyInventory;

        public static long CurrentTimeSec;

        #endregion

        #region Private Vars

        Action<int> OnEnergyChanged = delegate
        {
        };

        Action<TimeSpan> OnTimeLeftChanged = delegate
        {
        };

        StateMachine<State> _stateMachine;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            EnergySuiteBehaviour.UpdateCurrentTime();

            if (EnergyInventory == null)
                CreateInventoryInstance();

            TimeServHandler = new TimeServerHandler(this);
            _stateMachine = GetComponent<StateMachineRunner>().Initialize<State>(this);
            _stateMachine.ChangeState(State.Init);

            EnergyInventory.OnEnergyIncreased += OnEnergyIncreasedHandler;
            EnergyInventory.OnEnergyDecreased += OnEnergyDecreasedHandler;

            TimeServHandler.OnTimeLeftChanged += OnTimeLeftChangedTimeServerHandler;

            OnEnergyChanged += OnEnergyChangedHandler;
            OnTimeLeftChanged += OnTimeLeftChangedHandler;
        }

        void OnDisable()
        {
            EnergyInventory.OnEnergyIncreased -= OnEnergyIncreasedHandler;
            EnergyInventory.OnEnergyDecreased -= OnEnergyDecreasedHandler;
            TimeServHandler.OnTimeLeftChanged -= OnTimeLeftChangedTimeServerHandler;
            OnEnergyChanged -= OnEnergyChangedHandler;
            OnTimeLeftChanged -= OnTimeLeftChangedHandler;
        }

        void OnDestroy()
        {
            TimeServHandler.OnDestroy();
            EnergyInventory.SaveAmount();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            TimeServHandler.OnApplicationPause(pauseStatus);

            if (pauseStatus)
                EnergyInventory.SaveAmount();
        }

        #region Event Handlers

        void Init_Enter()
        {
            if (CurrentTimeSec > TimeServHandler.TimeServ.GetLastClosedTime())
                TimeServHandler.CheckAmountAdded();
            else
                TimeServHandler.CheckAmountAdded(0);

            if (EnergyInventory.IsFull())
                _stateMachine.ChangeState(State.Full);
            else
                _stateMachine.ChangeState(State.Adding);
        }

        void Adding_Update()
        {
            if (EnergyInventory.IsFull())
                _stateMachine.ChangeState(State.Full);
            else
                TimeServHandler.Update();
        }

        void Full_Exit()
        {
            TimeServHandler.SetLastTimeAdded();
        }

        void OnEnergyIncreasedHandler(int amount)
        {
            OnEnergyChanged(amount);
        }

        void OnEnergyDecreasedHandler(int amount)
        {
            OnEnergyChanged(amount);
        }

        void OnTimeLeftChangedTimeServerHandler(TimeSpan timeLeft)
        {
            OnTimeLeftChanged(timeLeft.Add(new TimeSpan(0, 0, 1)));
        }

        void OnEnergyChangedHandler(int amount)
        {
            EnergySuiteManager.OnEnergyChanged(amount);
        }

        void OnTimeLeftChangedHandler(TimeSpan timeLeft)
        {
            EnergySuiteManager.OnTimeLeftChanged(timeLeft);
        }

        #endregion

        #region Public Methods

        public void AddEnergy(int amount, bool setTime = true, long customTime = -1)
        {
            EnergyInventory.Add(amount);

            if (setTime)
            {
                if (customTime == -1)
                    TimeServHandler.SetLastTimeAdded();
                else
                    TimeServHandler.SetLastTimeAdded(customTime);
            }

            if (EnergyInventory.IsFull())
                _stateMachine.ChangeState(State.Full);
        }

        public bool UseEnergy(int amount)
        {
            _stateMachine.ChangeState(State.Adding);
            return EnergyInventory.Use(amount);
        }

        public static void CreateInventoryInstance()
        {
            EnergyInventory = new EnergyInventory();
        }

        public static void UpdateCurrentTime()
        {
            #if UNITY_IOS && !UNITY_EDITOR
            iOSBridge.GetCurrentMediaTime();
            #elif UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaObject jo = new AndroidJavaObject("android.os.SystemClock");
            long time = jo.CallStatic<long>("elapsedRealtime"); 
            long timeSec = time/1000;
            CallbackGetTimeStatic(timeSec.ToString());
            #elif UNITY_STANDALONE || UNITY_EDITOR
            int time = Environment.TickCount;
            int timeSec = time / 1000;
            CallbackGetTimeStatic(timeSec.ToString());
            #endif
        }

        public static void CallbackGetTimeStatic(string value)
        {
            CurrentTimeSec = (long)Convert.ToDouble(value);
        }

        public void CallbackGetTime(string value)
        {
            EnergySuiteBehaviour.CallbackGetTimeStatic(value);
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
