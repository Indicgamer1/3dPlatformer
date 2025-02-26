using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer
{
    public class StateMachine
    {
        private class StateNode
        {
            public IState state { get; private set; }
            public HashSet<ITransition> Transitions {get;}

            public StateNode(IState state)
            {
                this.state = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState state, IPredicate predicate)
            {
                Transitions.Add(new Transition(state, predicate));
            }
        }

        private StateNode current;
        private Dictionary<Type, StateNode> nodes = new();
        private HashSet<ITransition> anyTransitions = new();

        public void Update()
        {
            ITransition transition = GetTransition();
            
            if(transition != null)
                ChangeStateTo(transition.To);
            
            current.state?.Update();
        }

        public void FixedUpdate()
        {
            current.state?.FixedUpdate();
        }

        public void SetState(IState state)
        {
            current = nodes[state.GetType()];
            current.state?.OnEnter();
        }
        
        private void ChangeStateTo(IState newState)
        {
            if (newState == current.state) return;
            
            var oldState = current.state;
            current = nodes[newState.GetType()];
            
            oldState.OnExit();
            current.state?.OnEnter();
        }

        private ITransition GetTransition()
        {
            foreach (var transition in anyTransitions)
                if(transition.Condition.Evaluate())
                    return transition;


            foreach (var transition in current.Transitions)
                if(transition.Condition.Evaluate())
                    return transition;
            
            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate predicate)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).state, predicate);
        }

        public void AddAnyTransition(IState to, IPredicate predicate)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).state, predicate));
        }
        private StateNode GetOrAddNode(IState state)
        {
            StateNode node;
            if (nodes.ContainsKey(state.GetType()))
            {
                node = nodes[state.GetType()];
            }
            else
            {
                node = new StateNode(state); 
                nodes.Add(state.GetType(), node);
            }
            
            return node;
        }
    }
}