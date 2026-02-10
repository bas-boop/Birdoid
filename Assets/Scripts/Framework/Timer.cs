using UnityEngine;
using UnityEngine.Events;

namespace Framework
{
    public sealed class Timer : MonoBehaviour
    {
        [SerializeField] private float time;
        [SerializeField] private bool canCountOnStart;
        
        [Space(20)]
        [SerializeField] private UnityEvent onTimerStart = new();
        [SerializeField] private UnityEvent onTimerStop = new();
        [SerializeField] private UnityEvent onTimerDone = new();
        
        public bool IsCounting { get; private set; }
        private float _currentTimer;

        private bool _canCount;

        private void Start()
        {
            _currentTimer = time;
            
            if (canCountOnStart)
                StartTimer();
        }

        private void Update()
        {
            if (_canCount)
                UpdateTimer();
        }

        public void StartTimer()
        {
            _canCount = true;
            onTimerStart?.Invoke();
        }

        public void RestartTimer()
        {
            _currentTimer = time;
            StartTimer();
        }

        public void StopTimer()
        {
            _currentTimer = time;
            _canCount = false;
            onTimerStop?.Invoke();
        }

        public float GetCurrentTime() => _currentTimer;

        private void UpdateTimer()
        {
            _currentTimer -= Time.deltaTime;
            IsCounting = true;
            
            if (_currentTimer > 0)
                return;
            
            _currentTimer = 0;
            _canCount = false;
            IsCounting = false;
            onTimerDone?.Invoke();
        }
    }
}