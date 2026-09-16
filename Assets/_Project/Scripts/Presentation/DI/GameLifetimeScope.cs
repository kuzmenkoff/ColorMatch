using ColorMatch.Core.Services;
using ColorMatch.Core.StateMachine;
using ColorMatch.Core.Storage;
using ColorMatch.Presentation.Config;
using ColorMatch.Presentation.Gameplay;
using ColorMatch.Presentation.Services;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ColorMatch.Presentation.DI
{
    /// <summary>
    /// Composition root: builds the logic core from the GameConfig asset and
    /// exposes it to the presentation layer through dependency injection.
    /// </summary>
    public sealed class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameConfig _config;

        protected override void Configure(IContainerBuilder builder)
        {
            // Config asset and the plain-C# configs derived from it.
            builder.RegisterInstance(_config);
            builder.RegisterInstance(_config.BuildScoreConfig());
            builder.RegisterInstance(_config.BuildDifficultyConfig());

            // Engine-backed implementations of the core abstractions.
            builder.Register<IRandom, UnityRandom>(Lifetime.Singleton);
            builder.Register<IHighScoreStorage, PlayerPrefsHighScoreStorage>(Lifetime.Singleton);

            // Core services whose dependencies are all resolvable by the container.
            builder.Register<DifficultyCurve>(Lifetime.Singleton);
            builder.Register<ScoreService>(Lifetime.Singleton);
            builder.Register<GameStateMachine>(Lifetime.Singleton);

            // Core services that need primitive config values -> built via factories.
            builder.Register(_ =>
                new CountdownTimer(_config.LevelDuration),
                Lifetime.Singleton);

            builder.Register(container =>
                new BasketColorCycler(
                    _config.Colors,
                    _config.BasketColorInterval,
                    container.Resolve<IRandom>()),
                Lifetime.Singleton);

            builder.Register(container =>
                new SpawnDirector(
                    container.Resolve<DifficultyCurve>(),
                    _config.Colors,
                    _config.Shapes,
                    container.Resolve<IRandom>()),
                Lifetime.Singleton);

            // Aggregate session; every dependency is registered above, so
            // VContainer resolves its constructor automatically.
            builder.Register<GameSession>(Lifetime.Singleton);

            // Inject the scene MonoBehaviours that depend on the session.
            builder.RegisterComponentInHierarchy<GameLoopDriver>();
            builder.RegisterComponentInHierarchy<BasketView>();
            builder.RegisterComponentInHierarchy<FigureSpawner>();
        }
    }
}
