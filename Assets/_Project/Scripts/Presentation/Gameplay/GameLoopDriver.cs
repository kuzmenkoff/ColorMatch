using ColorMatch.Core.Services;
using UnityEngine;
using VContainer;

namespace ColorMatch.Presentation.Gameplay
{
    /// <summary>
    /// Drives the session clock. Temporary auto-start for gameplay testing.
    /// </summary>
    public sealed class GameLoopDriver : MonoBehaviour
    {
        private GameSession _session;

        [Inject]
        public void Construct(GameSession session) => _session = session;

        private void Start() => _session.Start();

        private void Update() => _session.Tick(Time.deltaTime);
    }
}
