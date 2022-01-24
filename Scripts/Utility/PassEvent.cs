using UnityEngine.Events;

[System.Serializable]
public class PassEvent : UnityEvent { }

[System.Serializable]
public class PassPageEvent : UnityEvent<Pages> { }
