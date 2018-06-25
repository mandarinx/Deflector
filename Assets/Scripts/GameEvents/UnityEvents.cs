using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

[Serializable]
public class UnityIntEvent : UnityEvent<int> {}

[Serializable]
public class UnityObjectEvent : UnityEvent<Object> {}

[Serializable]
public class UnityVector3Event : UnityEvent<Vector3> {}
