using UnityEngine;
using System;
using System.Collections;
using MonsterLove.StateMachine;

namespace EnergySuite
{
    public class EnergySuiteManager : Singleton<EnergySuiteManager>
    {
        public enum State
        {
            Init,
            Adding,
            Full
        }

        #region Public Vars

        public Action<int> OnEnergyChanged = delegate
        {
        };

        public Action<TimeSpan> OnTimeLeftChanged = delegate
        {
        };

        public int Amount
        {
            get
            {
                if (_energyInventory != null)
                    return _energyInventory.Amount;
                else
                    return 0;
            }
            private set{ _amount = value; }
        }

        public static StateMachine<State> StateMachine;

        #endregion

        #region Private Vars

        TimeServerHandler _timeServerHandler;
        EnergyInventory _energyInventory;
        int _amount;

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _energyInventory = new EnergyInventory();
            _timeServerHandler = new TimeServerHandler();
            StateMachine = GetComponent<StateMachineRunner>().Initialize<State>(this);
            StateMachine.ChangeState(State.Init);

            _energyInventory.OnEnergyIncreased += OnEnergyIncreasedHandler;
            _energyInventory.OnEnergyDecreased += OnEnergyDecreasedHandler;

            _timeServerHandler.OnTimeLeftChanged += OnTimeLeftChangedTimeServerHandler;
        }

        void OnDisable()
        {
            _energyInventory.OnEnergyIncreased -= OnEnergyIncreasedHandler;
            _energyInventory.OnEnergyDecreased -= OnEnergyDecreasedHandler;
            _timeServerHandler.OnTimeLeftChanged -= OnTimeLeftChangedTimeServerHandler;
        }

        void OnDestroy()
        {
            _timeServerHandler.OnDestroy();
            _energyInventory.OnDestroy();
        }

        void OnApplicationPause(bool pauseStatus)
        {
            _timeServerHandler.OnApplicationPause(pauseStatus);
        }

        #region Event Handlers

        void Init_Enter()
        {
            _timeServerHandler.CheckAmountAdded();

            if (_energyInventory.IsFull())
                StateMachine.ChangeState(State.Full);
            else 
                StateMachine.ChangeState(State.Adding);
        }

        void Adding_Update()
        {
            if (_energyInventory.IsFull())
                StateMachine.ChangeState(State.Full);
            else
                _timeServerHandler.Update();
        }

        void Full_Exit()
        {
            _timeServerHandler.SetLastTimeAdded();
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

        #endregion

        #region Public Methods

        public void AddEnergy(int amount, bool setTime = true, DateTime? customDateTime = null)
        {
            _energyInventory.Add(amount);

            if (setTime)
            {
                if (customDateTime == null)
                    _timeServerHandler.SetLastTimeAdded();
                else
                    _timeServerHandler.SetLastTimeAdded(customDateTime);
            }

            if(_energyInventory.IsFull())
                StateMachine.ChangeState(State.Full);
        }

        public bool UseEnergy(int amount)
        {
            StateMachine.ChangeState(State.Adding);
            return _energyInventory.Use(amount);
        }

        /// <summary>
        /// Converts to slider value.
        /// </summary>
        /// <returns>The to slider value. 0 - 1</returns>
        /// <param name="timeLeft">Time left to add one</param>
        public float ConvertToSliderValue(TimeSpan timeLeft)
        {
            float result = 0f;

            TimeSpan timeToAddEnergy = _timeServerHandler.GetTimeToAddEnergy();
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
