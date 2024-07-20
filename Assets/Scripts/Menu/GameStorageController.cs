using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Menu
{
    public class GameStorageController : MonoBehaviour
    {
        public event Action Loaded, Unloaded;

        [SerializeField] 
        private AssetReference _scene;

        private SceneInstance _instanceScene;

        public SceneInstance SceneInstance => _instanceScene;

        public void LoadAsset()
        {
            _scene.LoadSceneAsync().Completed += OnSceneLoaded;
        }

        public void UnloadAsset()
        {
            Addressables.UnloadSceneAsync(_instanceScene);
            Unloaded.Invoke();
        }

        private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> handle)
        {
            if(handle.Status == AsyncOperationStatus.Succeeded)
            {
                _instanceScene = handle.Result;
                Loaded.Invoke();
            }
        }
    }
}