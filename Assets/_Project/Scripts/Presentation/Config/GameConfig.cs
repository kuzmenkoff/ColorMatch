using System.Collections.Generic;
using ColorMatch.Core.Config;
using ColorMatch.Core.Enums;
using UnityEngine;

namespace ColorMatch.Presentation.Config
{
    /// <summary>
    /// Designer-facing tuning for a session, stored as an asset so values can be
    /// balanced in the Inspector. Builds the plain-C# config objects the logic
    /// core consumes.
    /// </summary>
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ColorMatch/Game Config")]
    public sealed class GameConfig : ScriptableObject
    {
        [Header("References")]
        [SerializeField] private ColorPalette _palette;

        [Header("Level")]
        [SerializeField] private float _levelDuration = 60f;
        [SerializeField] private float _basketColorInterval = 3f;

        [Header("Scoring")]
        [SerializeField] private int _pointsPerMatch = 10;
        [SerializeField] private int _penaltyPerMismatch = 5;
        [SerializeField] private bool _clampScoreToZero = true;

        [Header("Difficulty (start -> end over ramp)")]
        [SerializeField] private float _startFallSpeed = 2.5f;
        [SerializeField] private float _endFallSpeed = 6f;
        [SerializeField] private float _startSpawnInterval = 1.2f;
        [SerializeField] private float _endSpawnInterval = 0.45f;
        [SerializeField] private float _rampDuration = 60f;

        [Header("Figures")]
        [SerializeField]
        private FigureShape[] _shapes =
            { FigureShape.Circle, FigureShape.Square, FigureShape.Triangle, FigureShape.Star };

        public ColorPalette Palette => _palette;
        public float LevelDuration => _levelDuration;
        public float BasketColorInterval => _basketColorInterval;
        public IReadOnlyList<FigureColor> Colors => _palette.Colors;
        public IReadOnlyList<FigureShape> Shapes => _shapes;

        public ScoreConfig BuildScoreConfig()
            => new ScoreConfig(_pointsPerMatch, _penaltyPerMismatch, _clampScoreToZero);

        public DifficultyConfig BuildDifficultyConfig()
            => new DifficultyConfig(
                _startFallSpeed, _endFallSpeed,
                _startSpawnInterval, _endSpawnInterval,
                _rampDuration);
    }
}
