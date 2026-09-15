using System;
using System.Collections.Generic;
using ColorMatch.Core.Enums;

namespace ColorMatch.Core.Services
{
    /// <summary>
    /// Owns the basket's current color and switches it to a random color from
    /// the available set at a fixed interval. A switch never repeats the
    /// current color (unless only one color is available).
    /// </summary>
    public sealed class BasketColorCycler
    {
        private readonly IReadOnlyList<FigureColor> _colors;
        private readonly float _interval;
        private readonly IRandom _random;

        private float _timeUntilChange;

        public FigureColor CurrentColor { get; private set; }

        /// <summary>Raised whenever the basket switches color.</summary>
        public event Action<FigureColor> ColorChanged;

        public BasketColorCycler(IReadOnlyList<FigureColor> colors, float interval, IRandom random)
        {
            if (colors == null || colors.Count == 0)
                throw new ArgumentException("At least one color is required.", nameof(colors));
            if (interval <= 0f) throw new ArgumentOutOfRangeException(nameof(interval));

            _colors = colors;
            _interval = interval;
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        /// <summary>Restarts the cycle and broadcasts an initial color (any of them).</summary>
        public void Reset()
        {
            _timeUntilChange = _interval;
            CurrentColor = _colors[_random.Range(0, _colors.Count)];
            ColorChanged?.Invoke(CurrentColor); // always announce the starting color
        }

        public void Tick(float deltaTime)
        {
            _timeUntilChange -= deltaTime;
            if (_timeUntilChange <= 0f)
            {
                _timeUntilChange += _interval;
                TrySetColor(PickDifferentColor());
            }
        }

        // Picks a random color guaranteed to differ from the current one.
        private FigureColor PickDifferentColor()
        {
            if (_colors.Count == 1)
                return _colors[0];

            int currentIndex = IndexOfCurrent();
            int roll = _random.Range(0, _colors.Count - 1);
            if (roll >= currentIndex) roll++;
            return _colors[roll];
        }

        private int IndexOfCurrent()
        {
            for (int i = 0; i < _colors.Count; i++)
            {
                if (_colors[i] == CurrentColor)
                    return i;
            }
            return 0; // current color not in the set yet (before the first pick)
        }

        private void TrySetColor(FigureColor color)
        {
            if (color == CurrentColor)
                return;

            CurrentColor = color;
            ColorChanged?.Invoke(color);
        }
    }
}
