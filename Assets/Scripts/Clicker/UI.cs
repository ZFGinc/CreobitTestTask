using UnityEngine;
using UnityEngine.SceneManagement;

namespace Clicker
{
    public class UI : MonoBehaviour
    {
        [SerializeField]
        private string _menuSceneName = "Menu";

        public void ReturnToMenu()
        {
            SceneManager.LoadScene(_menuSceneName);
        }
    }
}