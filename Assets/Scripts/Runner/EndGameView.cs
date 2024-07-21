using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Runner
{
    [RequireComponent(typeof(Timer))]
    public class EndGameView : MonoBehaviour
    {
        [SerializeField]
        private Text _textTime;

        [Space]
        [SerializeField]
        private GameObject _endGamePanel;
        [SerializeField]
        private GameObject _newRecordSplash;

        [Space]
        [SerializeField] 
        private CharacterMove _characterMove;

        private Timer _timer;

        private void Awake()
        {
            _timer = GetComponent<Timer>();
        }

        private void OnEnable()
        {
            _characterMove.EndGame += OnEndGame;
            _timer.NewRecord += OnNewRecord;
        }

        private void OnDisable()
        {
            _characterMove.EndGame -= OnEndGame;
            _timer.NewRecord -= OnNewRecord;
        }

        private void OnNewRecord()
        {
            _newRecordSplash.SetActive(true);
        }

        private void OnEndGame()
        {
            TimeSpan _result = _timer.GetOffsetDateTime();

            StringBuilder stringBuilder = new StringBuilder();

            if (_result.Minutes < 10) stringBuilder.Append("0");
            stringBuilder.Append(_result.Minutes);
            stringBuilder.Append(":");
            if(_result.Seconds < 10) stringBuilder.Append("0");
            stringBuilder.Append(_result.Seconds);

            _textTime.text = stringBuilder.ToString();

            _endGamePanel.SetActive(true);
        }
    }
}