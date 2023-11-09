using UnityEngine;

namespace Test.Testris
{
    public class PlayManager : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Map.TryMove(Coord.right))
                {

                }
            }
        }
    }
}