using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventService
{

    public EventController<int> OnColorButtonClicked {  get; private set; }
    public EventController OnColorTrialCompleted { get; private set; }
    public EventController OnMathTrialCompleted { get; private set; }

    public EventController<int> OnTrialCompleted {  get; private set; }

    public EventService()
    {
        OnColorButtonClicked = new EventController<int>();
        OnColorTrialCompleted = new EventController();
        OnMathTrialCompleted = new EventController();
        OnTrialCompleted = new EventController<int>();
    }
}

public class EventController<T>
{
    public event Action<T> baseEvent;
    public void InvokeEvent(T type) => baseEvent?.Invoke(type);
    public void AddListener(Action<T> listener) => baseEvent += listener;
    public void RemoveListener(Action<T> listener) => baseEvent -= listener;
}

public class EventController
{
    public event Action baseEvent;
    public void InvokeEvent() => baseEvent?.Invoke();
    public void AddListener(Action listener) => baseEvent += listener;
    public void RemoveListener(Action listener) => baseEvent -= listener;

}
