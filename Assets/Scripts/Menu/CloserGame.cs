using UnityEngine;

namespace Menu
{
    public class CloserGame : MonoBehaviour
    {
        public void CloseGame()
        {
            Resources.UnloadUnusedAssets();
            Application.Quit();
        }
    }
}