using System;
using ColorMatch.Core.Enums;

namespace ColorMatch.Core.StateMachine
{
    /// <summary>
    /// Tracks the high-level screen state (menu / playing / game over) and
    /// notifies listeners on each transition.
    /// </summary>
    public sealed class GameStateMachine
    {
        public GameState Current { get; private set; } = GameState.MainMenu;

        /// <summary>Raised after a transition, carrying the new state.</summary>
        public event Action<GameState> StateChanged;

        public void GoToMainMenu() => Transition(GameState.MainMenu);
        public void StartGame() => Transition(GameState.Playing);
        public void GoToGameOver() => Transition(GameState.GameOver);

        private void Transition(GameState next)
        {
            if (Current == next) return;
            Current = next;
            StateChanged?.Invoke(next);
        }
    }
}
