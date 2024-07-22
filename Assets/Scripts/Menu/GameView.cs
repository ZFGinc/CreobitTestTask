using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    [RequireComponent(typeof(GameStorageController))]
    public class GameView : MonoBehaviour
    {
        [SerializeField]
        private Button _play, _load, _unload;

        [Space] 
        [SerializeField]
        private GameObject _loadingPanel, _failedPanel;

        private GameStorageController _gameStorageController;

        private void Awake()
        {
            _gameStorageController = GetComponent<GameStorageController>();
        }

        private void OnEnable()
        {
            _gameStorageController.Loaded += OnSceneLoaded;
            _gameStorageController.Unloaded += OnSceneUnloaded;
            _gameStorageController.Loading += OnLoding;
            _gameStorageController.Failed += OnFailedLoad;
        }

        private void OnDisable()
        {
            _gameStorageController.Loaded -= OnSceneLoaded;
            _gameStorageController.Unloaded -= OnSceneUnloaded;
            _gameStorageController.Loading -= OnLoding;
            _gameStorageController.Failed -= OnFailedLoad;
        }

        private void OnFailedLoad()
        {
            _failedPanel.SetActive(true);
        }

        private void OnSceneUnloaded()
        {
            _play.interactable = false;
            _unload.interactable = false;
            _load.interactable = true;
        }

        private void OnSceneLoaded()
        {
            _play.interactable = true;
            _unload.interactable = true;
            _load.interactable = false;

            _loadingPanel.SetActive(false);
        }

        private void OnLoding()
        {
            _loadingPanel.SetActive(true);
        }

        public void LoadGame()
        {
            _gameStorageController.LoadScene();
        }
    }
}