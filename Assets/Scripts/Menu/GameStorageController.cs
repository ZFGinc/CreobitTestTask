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
        private AsyncOperationHandle<SceneInstance> _unloadHandle;

        public SceneInstance SceneInstance => _loadHandle.Result;

        public void LoadAsset()
        {
            _scene.LoadSceneAsync(LoadSceneMode.Single, false).Completed += OnSceneLoaded;
            Loading?.Invoke();
        }

        public void UnloadAsset()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable) Failed?.Invoke();

            Addressables.UnloadSceneAsync(SceneInstance).Completed += OnSceneUnloaded;
        }

        private void OnSceneUnloaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _unloadHandle = handle;
                Addressables.Release(_unloadHandle);
                Unloaded?.Invoke();
            }
        }

        private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
                _loadHandle = handle;
                Loaded?.Invoke();
            }
        }

        private void OnDestroy()
        {
            Addressables.Release(_loadHandle);
        }
    }
}