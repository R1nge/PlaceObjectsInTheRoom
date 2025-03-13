using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Assets.Scripts.Services
{
    public abstract class GenericStateMachine<T, T1> where T : Enum where T1 : IState
    {
        protected Dictionary<T, T1> States;
        protected T CurrentStateType;
        protected T1 CurrentState;
        protected T PreviousStateType;
        protected T1 PreviousState;
        protected readonly Dictionary<T, T1> NotExitedStates = new();

        public virtual void SwitchState(T stateType)
        {
            if (Equals(CurrentStateType, stateType))
            {
                Debug.LogError($"Trying to switch to the same state {stateType}");
                return;
            }

            if (States.TryGetValue(stateType, out var newState))
            {
                CurrentState?.Exit();
                CurrentState = newState;
                CurrentStateType = stateType;
                CurrentState.Enter();
            }
        }

        public virtual void SwitchStateAndExitFromAllPreviousStates(T stateType)
        {
            SwitchState(stateType);

            foreach (var state in NotExitedStates.Values)
            {
                state.Exit();
            }

            NotExitedStates.Clear();
        }

        public virtual void SwitchWithoutExitFromPreviousState(T stateType)
        {
            if (Equals(CurrentStateType, stateType))
            {
                Debug.LogError($"Trying to switch to the same state {stateType}");
                return;
            }

            NotExitedStates.TryAdd(stateType, CurrentState);

            PreviousStateType = CurrentStateType;
            PreviousState = CurrentState;

            CurrentState = States[stateType];
            CurrentStateType = stateType;

            NotExitedStates.TryAdd(PreviousStateType, PreviousState);

            CurrentState.Enter();
        }

        public virtual void SwitchToPreviousState()
        {
            if (PreviousState == null)
            {
                Debug.LogError("No previous state");
                return;
            }

            SwitchState(PreviousStateType);
        }
    }
}