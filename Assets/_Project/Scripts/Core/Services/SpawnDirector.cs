using System;
using System.Collections.Generic;
using ColorMatch.Core.Enums;
using ColorMatch.Core.Model;

namespace ColorMatch.Core.Services
{
    /// <summary>
    /// Decides when figures spawn and what they are. Cadence and fall speed
    /// come from the difficulty curve; color and shape are uniform random.
    /// Emits a request for the presentation layer to realize.
    /// </summary>
    public sealed class SpawnDirector
    {
        private readonly DifficultyCurve _difficulty;
        private readonly IReadOnlyList<FigureColor> _colors;
        private readonly IReadOnlyList<FigureShape> _shapes;
        private readonly IRandom _random;

        private float _elapsed;
        private float _timeUntilSpawn;

        /// <summary>Raised when a new figure should be spawned.</summary>
        public event Action<FigureSpawnRequest> FigureRequested;

        public SpawnDirector(
            DifficultyCurve difficulty,
            IReadOnlyList<FigureColor> colors,
            IReadOnlyList<FigureShape> shapes,
            IRandom random)
        {
            _difficulty = difficulty ?? throw new ArgumentNullException(nameof(difficulty));
            if (colors == null || colors.Count == 0)
                throw new ArgumentException("At least one color is required.", nameof(colors));
            if (shapes == null || shapes.Count == 0)
                throw new ArgumentException("At least one shape is required.", nameof(shapes));

            _colors = colors;
            _shapes = shapes;
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        /// <summary>Resets elapsed time and schedules the first spawn.</summary>
        public void Reset()
        {
            _elapsed = 0f;
            _timeUntilSpawn = _difficulty.Evaluate(0f).SpawnInterval;
        }

        public void Tick(float deltaTime)
        {
            _elapsed += deltaTime;
            _timeUntilSpawn -= deltaTime;

            if (_timeUntilSpawn <= 0f)
            {
                DifficultySnapshot snapshot = _difficulty.Evaluate(_elapsed);
                _timeUntilSpawn += snapshot.SpawnInterval;
                FigureRequested?.Invoke(BuildRequest(snapshot.FallSpeed));
            }
        }

        private FigureSpawnRequest BuildRequest(float fallSpeed)
        {
            FigureColor color = _colors[_random.Range(0, _colors.Count)];
            FigureShape shape = _shapes[_random.Range(0, _shapes.Count)];
            float normalizedX = _random.NextFloat();
            return new FigureSpawnRequest(color, shape, normalizedX, fallSpeed);
        }
    }
}
