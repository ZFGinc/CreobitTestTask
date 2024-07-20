using UnityEngine;

namespace Clicker
{
    public class ClicksStorage : MonoBehaviour
    {
        private int _clicks = 0;

        private const string NameData = "_countClicks";

        public int Clicks => _clicks;

        public void AddClicks()
        {
            _clicks++;
        }

        private void OnEnable()
        {
            LoadClicks();
        }

        private void OnDisable()
        {
            SaveClicks();
        }

        private void LoadClicks()
        {
            _clicks = PlayerPrefs.GetInt(NameData, 0);
        }

        private void SaveClicks()
        {
            PlayerPrefs.SetInt(NameData, _clicks);
        }
    }
}