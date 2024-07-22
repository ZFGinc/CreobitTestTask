using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Menu
{
    public class GameStorageController : MonoBehaviour
    {
        public event Action Loaded, Unloaded, Loading, Failed;

        [SerializeField] 
        private AssetReference _scene;

        private AsyncOperationHandle<SceneInstance> _loadHandle;
        private string _path;

        public SceneInstance SceneInstance => _loadHandle.Result;
        private string NameKey => _scene.ToString();

        private void Awake()
        {
            if (PlayerPrefs.HasKey(NameKey))
            {
                _path = PlayerPrefs.GetString(NameKey);
                LoadLocalAsset();
            }
        }

        public void LoadScene()
        {
            SceneInstance.ActivateAsync();
        }

        public void LoadAsset()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Failed?.Invoke();
                return;
            }

            Loading?.Invoke();
            _scene.LoadSceneAsync(LoadSceneMode.Single, false).Completed += OnSceneLoaded;
        }

        public void UnloadAsset()
        {
            Addressables.UnloadSceneAsync(SceneInstance).Completed += OnSceneUnloaded;
        }

        private void LoadLocalAsset()
        {
            if (_path == string.Empty) return;

            Loading?.Invoke();
            Addressables.LoadSceneAsync(_path, LoadSceneMode.Single, false).Completed += OnSceneLoaded;
        }

        private void OnSceneUnloaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                PlayerPrefs.DeleteKey(NameKey);
                _path = string.Empty;
                Unloaded?.Invoke();
            }
        }

        private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadHandle = handle;
                _path = handle.Result.Scene.path;

                PlayerPrefs.SetString(NameKey, _path);
                Loaded?.Invoke();
            }
        }
    }
}