using System;
using ColorMatch.Core.Enums;
using ColorMatch.Core.Model;
using ColorMatch.Core.Storage;

namespace ColorMatch.Core.Services
{
    /// <summary>
    /// Coordinates a single play session: timer, basket color, spawning and
    /// scoring. Presentation drives it via Start/Tick and reports catches;
    /// the session raises events describing what changed.
    /// </summary>
    public sealed class GameSession
    {
        private readonly CountdownTimer _timer;
        private readonly BasketColorCycler _basket;
        private readonly SpawnDirector _spawner;
        private readonly ScoreService _score;
        private readonly IHighScoreStorage _highScores;

        public bool IsRunning { get; private set; }

        public FigureColor BasketColor => _basket.CurrentColor;
        public int Score => _score.Current;
        public float TimeRemaining => _timer.Remaining;

        public event Action<int> ScoreChanged;
        public event Action<float> TimeChanged;
        public event Action<FigureColor> BasketColorChanged;
        public event Action<FigureSpawnRequest> FigureRequested;

        /// <summary>Raised when the session ends, carrying the final result.</summary>
        public event Action<SessionResult> Ended;

        public GameSession(
            CountdownTimer timer,
            BasketColorCycler basket,
            SpawnDirector spawner,
            ScoreService score,
            IHighScoreStorage highScores)
        {
            _timer = timer ?? throw new ArgumentNullException(nameof(timer));
            _basket = basket ?? throw new ArgumentNullException(nameof(basket));
            _spawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
            _score = score ?? throw new ArgumentNullException(nameof(score));
            _highScores = highScores ?? throw new ArgumentNullException(nameof(highScores));

            // Re-broadcast the inner services as this session's public surface,
            // so the presentation layer subscribes to one object only.
            _score.ScoreChanged += s => ScoreChanged?.Invoke(s);
            _timer.TimeChanged += t => TimeChanged?.Invoke(t);
            _basket.ColorChanged += c => BasketColorChanged?.Invoke(c);
            _spawner.FigureRequested += r => FigureRequested?.Invoke(r);
            _timer.Finished += End;
        }

        /// <summary>Begins a fresh run.</summary>
        public void Start()
        {
            _score.Reset();
            _timer.Reset();
            _basket.Reset();
            _spawner.Reset();
            IsRunning = true;
        }

        public void Tick(float deltaTime)
        {
            if (!IsRunning) return;

            _basket.Tick(deltaTime);
            _spawner.Tick(deltaTime);
            _timer.Tick(deltaTime); // last: it may end the run this frame
        }

        /// <summary>Reports a figure caught by the basket and applies scoring.</summary>
        public void ReportCatch(FigureColor figureColor)
        {
            if (!IsRunning) return;

            bool matched = ColorMatchRules.IsMatch(_basket.CurrentColor, figureColor);
            _score.ApplyCatch(matched);
        }

        private void End()
        {
            if (!IsRunning) return;
            IsRunning = false;

            int finalScore = _score.Current;
            int previousBest = _highScores.Load();
            bool isNewHighScore = finalScore > previousBest;
            if (isNewHighScore)
                _highScores.Save(finalScore);

            int best = isNewHighScore ? finalScore : previousBest;
            Ended?.Invoke(new SessionResult(finalScore, best, isNewHighScore));
        }
    }
}
