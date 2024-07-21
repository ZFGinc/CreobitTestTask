using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ReturnToMenu : MonoBehaviour
    {
        [SerializeField]
        private string _menuSceneName = "Menu";

        public void Return()
        {  
            SceneManager.LoadScene(_menuSceneName);
        }
    }
}