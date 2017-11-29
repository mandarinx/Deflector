// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.Events;

namespace RoboRyanTron.Unite2017.Events
{
    
    [Serializable]
    public class UnityGameObjectEvent : UnityEvent<GameObject> {}

    public class GameObjectEventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public GameObjectEvent Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityGameObjectEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(GameObject go)
        {
            Response.Invoke(go);
        }
    }
}