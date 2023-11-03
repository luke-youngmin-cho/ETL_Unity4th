using Platformer.GameElements;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public static PortalManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("PortalManager").AddComponent<PortalManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    private static PortalManager _instance;


    public void TeleportScene(string from, string to)
    {
        StartCoroutine(C_TeleportScene(from, to));
    }

    private IEnumerator C_TeleportScene(string from, string to)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(to);

        while (asyncOperation.isDone == false)
            yield return null;

        FindObjectsOfType<Portal>()
            .FirstOrDefault(portal => portal.currentScene == to &&
                                      portal.destinationScene == from);
    }
}