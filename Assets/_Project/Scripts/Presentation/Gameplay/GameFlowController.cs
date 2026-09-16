using ColorMatch.Core.Enums;
using ColorMatch.Core.Services;
using ColorMatch.Core.StateMachine;
using UnityEngine;
using VContainer;

namespace ColorMatch.Presentation.Gameplay
{
    /// <summary>
    /// Owns the game flow: starts a run, ticks the session while playing,
    /// and transitions to game-over when the session ends.
    /// </summary>
    public sealed class GameFlowController : MonoBehaviour
    {
        private GameSession _session;
        private GameStateMachine _state;

        [Inject]
        public void Construct(GameSession session, GameStateMachine state)
        {
            _session = session;
            _state = state;
            _session.Ended += OnSessionEnded;
        }

        private void OnDestroy()
        {
            if (_session != null)
                _session.Ended -= OnSessionEnded;
        }

        private void Update()
        {
            if (_state.Current == GameState.Playing)
                _session.Tick(Time.deltaTime);
        }

        /// <summary>Starts a fresh run and enters the Playing state.</summary>
        public void StartGame()
        {
            _session.Start();
            _state.StartGame();
        }

        /// <summary>Returns to the main menu.</summary>
        public void GoToMenu() => _state.GoToMainMenu();

        private void OnSessionEnded(SessionResult result) => _state.GoToGameOver();
    }
}
