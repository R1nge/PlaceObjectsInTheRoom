using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Assets.Scripts.Services
{
    public abstract class GenericAsyncStateMachine<T, T1> where T : Enum where T1 : IAsyncState
    {
        protected readonly Dictionary<T, T1> States = new Dictionary<T, T1>();
        protected T CurrentStateType;
        protected T1 CurrentState;
        protected T PreviousStateType;
        protected T1 PreviousState;
        protected readonly Dictionary<T, T1> NotExitedStates = new();

        public virtual async UniTask SwitchState(T stateType, int delayMilliseconds = 0)
        {
            if (Equals(CurrentStateType, stateType))
            {
                Debug.LogError($"Already in {CurrentStateType} state");
            }

            if (delayMilliseconds > 0)
            {
                await UniTask.Delay(delayMilliseconds);
            }

            PreviousStateType = CurrentStateType;
            PreviousState = CurrentState;

            if (PreviousState != null)
            {
                PreviousState.Exit();
                NotExitedStates.Remove(PreviousStateType);
            }

            CurrentStateType = stateType;
            if (States.TryGetValue(stateType, out var newState))
            {
                CurrentState = newState;
                await CurrentState.Enter();
            }
            else
            {
                Debug.LogError("State not found for " + stateType);
            }
        }

        public virtual async UniTask SwitchStateAndExitFromAllPreviousStates(T stateType, int delayMilliseconds = 0)
        {
            await SwitchState(stateType, delayMilliseconds);

            var values = NotExitedStates.Values;

            foreach (var state in values)
            {
                if (state != null)
                {
                    await state.Exit();
                }
            }

            NotExitedStates.Clear();
        }

        public virtual async UniTask SwitchToPreviousState(int delayMilliseconds = 0)
        {
            if (PreviousState == null)
            {
                Debug.LogError("No previous state");
                return;
            }

            await SwitchState(PreviousStateType, delayMilliseconds);
        }

        public async UniTask SwitchWithoutExitFromPreviousState(T stateType, int delayMilliseconds = 0)
        {
            if (Equals(CurrentStateType, stateType))
            {
                Debug.LogError($"Already in {CurrentStateType} state");
                return;
            }

            if (delayMilliseconds > 0)
            {
                await UniTask.Delay(delayMilliseconds);
            }

            NotExitedStates.TryAdd(stateType, CurrentState);
            
            PreviousStateType = CurrentStateType;
            PreviousState = CurrentState;

            CurrentState = States[stateType];
            CurrentStateType = stateType;

            NotExitedStates.TryAdd(PreviousStateType, PreviousState);
            
            await CurrentState.Enter();
        }
        
        public void AddState(T stateType, T1 state) => States.Add(stateType, state);

        public void RemoveState(T stateType) => States.Remove(stateType);
    }
}