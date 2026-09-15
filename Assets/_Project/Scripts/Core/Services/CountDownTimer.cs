using System;

namespace ColorMatch.Core.Services
{
    /// <summary>
    /// Counts down from a fixed duration. Driven by explicit ticks.
    /// </summary>
    public sealed class CountdownTimer
    {
        private readonly float _duration;

        public float Remaining { get; private set; }
        public bool IsFinished => Remaining <= 0f;

        /// <summary>Raised after each tick with the remaining seconds.</summary>
        public event Action<float> TimeChanged;

        /// <summary>Raised once when the countdown reaches zero.</summary>
        public event Action Finished;

        public CountdownTimer(float duration)
        {
            if (duration <= 0f) throw new ArgumentOutOfRangeException(nameof(duration));
            _duration = duration;
        }

        /// <summary>Restarts the countdown at the full duration.</summary>
        public void Reset()
        {
            Remaining = _duration;
            TimeChanged?.Invoke(Remaining);
        }

        public void Tick(float deltaTime)
        {
            if (IsFinished) return;

            Remaining -= deltaTime;
            if (Remaining <= 0f)
            {
                Remaining = 0f;
                TimeChanged?.Invoke(Remaining);
                Finished?.Invoke();
                return;
            }

            TimeChanged?.Invoke(Remaining);
        }
    }
}
