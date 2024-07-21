using UnityEngine;
using UnityEngine.UI;

namespace Clicker
{
    [RequireComponent(typeof(Clicker))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class View : MonoBehaviour
    {
        [SerializeField]
        private Sprite _down, _up;

        [Space]
        [SerializeField]
        private Text _clicksView;

        [Space]
        [SerializeField]
        private GameObject _arrow;

        private Clicker _clicker;
        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _clicker = GetComponent<Clicker>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnEnable()
        {
            _clicker.Up += OnUp;
            _clicker.Down += OnDown;
        }

        private void OnDisable()
        {
            _clicker.Up -= OnUp;
            _clicker.Down -= OnDown;
        }

        private void OnUp()
        {
            _spriteRenderer.sprite = _up;
            _clicksView.text = _clicker.Clicks.ToString();
        }

        private void OnDown()
        {
            _arrow.SetActive(false);
            _spriteRenderer.sprite = _down;
        }
    }
}