using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public abstract class EventChannel<T> : ScriptableObject
    {
        protected HashSet<EventListener<T>> listeners = new();

        public void Invoke(T value)
        {
            foreach (var listener in listeners)
            {
                listener.Raise(value);
            }
        }
        
        public void AddListener(EventListener<T> listener) => listeners.Add(listener);
        public void RemoveListener(EventListener<T> listener) => listeners.Remove(listener);
    }
    
    public readonly struct Empty { }
    
    [CreateAssetMenu(fileName = "EmptyEventChannel", menuName = "Events/EmptyEventChannel")]
    public class EventChannel : EventChannel<Empty> { }
}