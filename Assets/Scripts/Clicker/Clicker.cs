using System;
using UnityEngine;

namespace Clicker
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(ClicksStorage))]
    public class Clicker : MonoBehaviour
    {
        public event Action Down, Up;

        private ClicksStorage _clicksStorage;

        public int Clicks => _clicksStorage.Clicks;

        private void Awake()
        {
            _clicksStorage = GetComponent<ClicksStorage>();
        }

        private void Start()
        {
            Up?.Invoke();
        }

        private void OnMouseDown()
        {
            Down.Invoke();
        }

        private void OnMouseUp()
        {
            _clicksStorage.AddClicks();
            Up.Invoke();
        }
    }
}