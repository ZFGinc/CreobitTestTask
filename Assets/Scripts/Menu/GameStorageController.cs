using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Collections;

namespace Menu
{
    public class GameStorageController : MonoBehaviour
    {
        public event Action Loaded, Unloaded, Loading, Failed;

        [SerializeField] 
        private AssetReference _scene;

        private Camera _menuCamera;
        private AsyncOperationHandle<SceneInstance> _loadHandle;
        private string _path;
        private bool _isLoaded;

        public SceneInstance SceneInstance => _loadHandle.Result;
        public Scene Scene => SceneInstance.Scene;
        private string NameKey => _scene.ToString();

        private const string MenuSceneName = "Menu";

        private void Awake()
        {
            _menuCamera = Camera.main;

            CheckLocalAssets();
        }

        public void LoadGame()
        {
            Loading?.Invoke();

            StartCoroutine(LoadGameAsync());
        }

        public void UnloadAsset()
        {
            if (_isLoaded)
            {
                Loading?.Invoke();
                Addressables.UnloadSceneAsync(SceneInstance, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects).Completed += OnSceneUnloaded;
            }

            if(PlayerPrefs.HasKey(NameKey)) PlayerPrefs.DeleteKey(NameKey);

            Unloaded?.Invoke();
        }

        public void LoadAsset()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Failed?.Invoke();
                return;
            }

            Loading?.Invoke();
            _scene.LoadSceneAsync(LoadSceneMode.Additive, false).Completed += OnSceneLoaded;
        }

        private void CheckLocalAssets()
        {
            if (PlayerPrefs.HasKey(NameKey))
            {
                Loading?.Invoke();

                _path = PlayerPrefs.GetString(NameKey);

                Loaded?.Invoke();
            }
        }

        private void LoadLocalAsset()
        {
            if (_path == string.Empty) return;

            Loading?.Invoke();
            Addressables.LoadSceneAsync(_path, LoadSceneMode.Additive, false).Completed += OnSceneLocalLoaded;
        }

        private IEnumerator LoadGameAsync()
        {
            LoadLocalAsset();

            while(!_isLoaded) yield return null;

            var operation = SceneInstance.ActivateAsync();

            yield return new WaitUntil(() => operation.isDone);

            _menuCamera.gameObject.SetActive(false);
        }

        private void OnSceneUnloaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded )
            {
                _path = string.Empty;
                _isLoaded = false;

                Unloaded?.Invoke();
            }
        }

        private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if(handle.Status == AsyncOperationStatus.Succeeded && handle.IsDone)
            {
                _loadHandle = handle;
                _path = handle.Result.Scene.path;

                PlayerPrefs.SetString(NameKey, _path);
                
                SceneManager.LoadScene(MenuSceneName);
            }
        }

        private void OnSceneLocalLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded && handle.IsDone)
            {
                _loadHandle = handle;
                _isLoaded = true;

                Loaded?.Invoke();
            }
        }
    }
}