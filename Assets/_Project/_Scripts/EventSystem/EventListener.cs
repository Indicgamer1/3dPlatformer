using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Platformer
{
    public abstract class EventListener<T> : MonoBehaviour
    {
        [SerializeField] protected EventChannel<T> eventChannel;
        [SerializeField] protected UnityEvent<T> unityEvent;

        private void OnEnable()
        {
            eventChannel.AddListener(this);
        }

        private void OnDisable()
        {
            eventChannel.RemoveListener(this);
        }

        public void Raise(T value)
        {
            unityEvent.Invoke(value);
        }
    }
}