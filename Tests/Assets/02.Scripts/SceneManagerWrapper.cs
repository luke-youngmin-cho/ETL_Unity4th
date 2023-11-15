using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerWrapper : MonoBehaviour
{
    public static SceneManagerWrapper instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject().AddComponent<SceneManagerWrapper>();
            return _instance;
        }
    }
    private static SceneManagerWrapper _instance;
    private List<ISceneListener> _listeners = new List<ISceneListener>();

    public void Register(ISceneListener listener)
    {
        _listeners.Add(listener);
        //SceneManager.sceneUnloaded += (scene) => listener.OnBeforeSceneUnloaded();
        //SceneManager.sceneLoaded += (scene, mode) => listener.OnAfterSceneloaded();
    }

    public void Unregister(ISceneListener listener)
    {
        _listeners.Remove(listener);
    }

    public void LoadScene(string sceneName)
    {
        foreach (var listener in _listeners)
            listener.OnBeforeSceneUnloaded();

        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(C_LoadSceneAsync(sceneName));
    }

    private IEnumerator C_LoadSceneAsync(string sceneName)
    {
        foreach (var listener in _listeners)
            listener.OnBeforeSceneUnloaded();

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (operation.isDone == false)
        {
            yield return null;
        }

        foreach (var listener in _listeners)
            listener.OnAfterSceneloaded();
    }
}
