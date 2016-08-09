using UnityEngine;
using System;
using System.Collections;
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

        public static TimeServerHandler TimeServerHandler;
        public static EnergyInventory EnergyInventory;

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
            if (EnergyInventory == null)
                CreateInventoryInstance();
            
            TimeServerHandler = new TimeServerHandler(this);
            _stateMachine = GetComponent<StateMachineRunner>().Initialize<State>(this);
            _stateMachine.ChangeState(State.Init);

            EnergyInventory.OnEnergyIncreased += OnEnergyIncreasedHandler;
            EnergyInventory.OnEnergyDecreased += OnEnergyDecreasedHandler;

            TimeServerHandler.OnTimeLeftChanged += OnTimeLeftChangedTimeServerHandler;

            OnEnergyChanged += OnEnergyChangedHandler;
            OnTimeLeftChanged += OnTimeLeftChangedHandler;
        }

        void OnDisable()
        {
            EnergyInventory.OnEnergyIncreased -= OnEnergyIncreasedHandler;
            EnergyInventory.OnEnergyDecreased -= OnEnergyDecreasedHandler;
            TimeServerHandler.OnTimeLeftChanged -= OnTimeLeftChangedTimeServerHandler;
            OnEnergyChanged -= OnEnergyChangedHandler;
            OnTimeLeftChanged -= OnTimeLeftChangedHandler;
        }

        void OnDestroy()
        {
            TimeServerHandler.OnDestroy();
            EnergyInventory.SaveAmount();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            TimeServerHandler.OnApplicationPause(pauseStatus);

            if (pauseStatus)
                EnergyInventory.SaveAmount();
        }

        #region Event Handlers

        void Init_Enter()
        {
            TimeServerHandler.CheckAmountAdded();

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
                TimeServerHandler.Update();
        }

        void Full_Exit()
        {
            TimeServerHandler.SetLastTimeAdded();
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

        public void AddEnergy(int amount, bool setTime = true, DateTime? customDateTime = null)
        {
            EnergyInventory.Add(amount);

            if (setTime)
            {
                if (customDateTime == null)
                    TimeServerHandler.SetLastTimeAdded();
                else
                    TimeServerHandler.SetLastTimeAdded(customDateTime);
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

        #endregion

        #region Private Methods

        #endregion
    }
}
