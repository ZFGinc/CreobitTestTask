using System;
using UnityEngine;

namespace Runner
{
    public class Timer : MonoBehaviour
    {
        public event Action NewRecord;

        private DateTime _startDateTime;
        private DateTime _endDateTime;

        private const string KeyForSaveRecordMinutes = "_recordMinutes";
        private const string KayForSaveRecordSeconds = "_recordSeconds";

        private void Start()
        {
            _startDateTime = DateTime.Now;
        }

        public TimeSpan GetOffsetDateTime()
        {
            _endDateTime = DateTime.Now;

            TimeSpan result = _endDateTime - _startDateTime;
            CheckRecordDestination(result);

            return result;
        }

        private void CheckRecordDestination(TimeSpan result)
        {
            if (PlayerPrefs.HasKey(KeyForSaveRecordMinutes) && PlayerPrefs.HasKey(KayForSaveRecordSeconds))
            {
                int minutes = PlayerPrefs.GetInt(KeyForSaveRecordMinutes);
                int seconds = PlayerPrefs.GetInt(KayForSaveRecordSeconds);

                TimeSpan oldRecord = new TimeSpan(0, minutes, seconds);

                if (oldRecord > result)
                {
                    NewRecord?.Invoke();
                    SaveRecord(result);
                }
            }
            else
            {
                NewRecord?.Invoke();
                SaveRecord(result);
            }
        }

        private void SaveRecord(TimeSpan result)
        {
            PlayerPrefs.SetInt(KeyForSaveRecordMinutes, result.Minutes);
            PlayerPrefs.SetInt(KayForSaveRecordSeconds, result.Seconds);
        }
    }
}