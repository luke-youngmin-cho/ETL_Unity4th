using System;
using UnityEngine;

public class SceneListener : MonoBehaviour, ISceneListener
{
    public event Action onSomethingHappened;

    private void Awake()
    {
        onSomethingHappened += () => Debug.Log("Something will be happened !!");

        SceneManagerWrapper.instance.Register(this);
    }

    private void OnDestroy()
    {
        SceneManagerWrapper.instance.Unregister(this);
    }

    public void OnAfterSceneloaded()
    {
    }

    public void OnBeforeSceneUnloaded()
    {
        onSomethingHappened = null;
    }
}