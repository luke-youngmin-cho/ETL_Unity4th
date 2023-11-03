using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.GameElements
{
    public class Portal : MonoBehaviour
    {
        public string currentScene;
        public string destinationScene;
        public bool isBusy;
        public Portal destination;

        public void Teleport(Transform target)
        {
            if (isBusy)
                return;

            if (destination)
            {
                target.position = destination.transform.position;
                Busy(1.0f);
                destination.Busy(1.0f);
            }
            else if (string.IsNullOrEmpty(destinationScene) == false)
            {
                PortalManager.instance.TeleportScene(currentScene, destinationScene);
            }
        }

        public void Ready()
        {
            isBusy = false;
        }

        public void Busy(float duration)
        {
            isBusy = true;
            Invoke("Ready", duration);
        }
    }
}