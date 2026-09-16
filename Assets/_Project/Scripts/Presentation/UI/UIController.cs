using ColorMatch.Core.Enums;
using ColorMatch.Core.Services;
using ColorMatch.Core.StateMachine;
using ColorMatch.Core.Storage;
using ColorMatch.Presentation.Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace ColorMatch.Presentation.UI
{
    /// <summary>
    /// Presents all screens (menu, HUD, game over) and the buttons that move
    /// between them. A thin view over the session and the state machine.
    /// </summary>
    public sealed class UIController : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject _menuPanel;
        [SerializeField] private GameObject _hudPanel;
        [SerializeField] private GameObject _gameOverPanel;

        [Header("Menu")]
        [SerializeField] private TMP_Text _menuBestText;
        [SerializeField] private Button _playButton;

        [Header("HUD")]
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _timeText;

        [Header("Game Over")]
        [SerializeField] private TMP_Text _finalScoreText;
        [SerializeField] private TMP_Text _bestText;
        [SerializeField] private GameObject _newRecordLabel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;

        private GameSession _session;
        private GameStateMachine _state;
        private GameFlowController _flow;
        private IHighScoreStorage _highScores;

        [Inject]
        public void Construct(
            GameSession session,
            GameStateMachine state,
            GameFlowController flow,
            IHighScoreStorage highScores)
        {
            _session = session;
            _state = state;
            _flow = flow;
            _highScores = highScores;

            _playButton.onClick.AddListener(_flow.StartGame);
            _restartButton.onClick.AddListener(_flow.StartGame);
            _menuButton.onClick.AddListener(_flow.GoToMenu);

            _state.StateChanged += ShowScreenFor;
            _session.ScoreChanged += OnScoreChanged;
            _session.TimeChanged += OnTimeChanged;
            _session.Ended += OnSessionEnded;

            ShowScreenFor(_state.Current);
        }

        private void OnDestroy()
        {
            if (_state != null) _state.StateChanged -= ShowScreenFor;
            if (_session != null)
            {
                _session.ScoreChanged -= OnScoreChanged;
                _session.TimeChanged -= OnTimeChanged;
                _session.Ended -= OnSessionEnded;
            }
        }

        private void ShowScreenFor(GameState state)
        {
            _menuPanel.SetActive(state == GameState.MainMenu);
            _hudPanel.SetActive(state == GameState.Playing);
            _gameOverPanel.SetActive(state == GameState.GameOver);

            if (state == GameState.MainMenu)
                _menuBestText.text = $"Best: {_highScores.Load()}";
        }

        private void OnScoreChanged(int score) => _scoreText.text = $"Score: {score}";

        private void OnTimeChanged(float remaining)
            => _timeText.text = $"Time: {Mathf.CeilToInt(remaining)}";

        private void OnSessionEnded(SessionResult result)
        {
            _finalScoreText.text = $"Score: {result.Score}";
            _bestText.text = $"Best: {result.HighScore}";
            _newRecordLabel.SetActive(result.IsNewHighScore);
        }
    }
}
