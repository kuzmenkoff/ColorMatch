using ColorMatch.Core.Services;
using LitMotion;
using UnityEngine;
using VContainer;

namespace ColorMatch.Presentation.Gameplay
{
    /// <summary>
    /// Plays juice on the basket in response to catches: a scale pulse for a
    /// correct figure and a rotation shake for a wrong one, each with a sound.
    /// </summary>
    public sealed class BasketFeedback : MonoBehaviour
    {
        [Header("Audio")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _correctClip;
        [SerializeField] private AudioClip _wrongClip;

        [Header("Pulse (correct)")]
        [SerializeField] private float _pulseScale = 1.25f;
        [SerializeField] private float _pulseDuration = 0.22f;

        [Header("Shake (wrong)")]
        [SerializeField] private float _shakeAngle = 12f;
        [SerializeField] private float _shakeDuration = 0.3f;
        [SerializeField] private float _shakeFrequency = 40f;

        private GameSession _session;
        private Vector3 _baseScale;
        private MotionHandle _scaleHandle;
        private MotionHandle _rotationHandle;

        [Inject]
        public void Construct(GameSession session)
        {
            _session = session;
            _session.FigureCaught += OnFigureCaught;
        }

        private void Awake() => _baseScale = transform.localScale;

        private void OnDestroy()
        {
            if (_session != null)
                _session.FigureCaught -= OnFigureCaught;
        }

        private void OnFigureCaught(bool matched)
        {
            if (matched)
            {
                PlayPulse();
                PlayClip(_correctClip);
            }
            else
            {
                PlayShake();
                PlayClip(_wrongClip);
            }
        }

        // Single grow-and-settle bump using a sine profile (0 -> peak -> 0).
        private void PlayPulse()
        {
            if (_scaleHandle.IsActive()) _scaleHandle.Cancel();
            transform.localScale = _baseScale;

            _scaleHandle = LMotion.Create(0f, 1f, _pulseDuration)
                .WithOnComplete(() => transform.localScale = _baseScale)
                .Bind(t =>
                {
                    float bump = Mathf.Sin(t * Mathf.PI) * (_pulseScale - 1f);
                    transform.localScale = _baseScale * (1f + bump);
                });
        }

        // Decaying Z-rotation wobble. Uses rotation (not position) so it never
        // fights the drag input, which writes position.
        private void PlayShake()
        {
            if (_rotationHandle.IsActive()) _rotationHandle.Cancel();
            transform.localRotation = Quaternion.identity;

            _rotationHandle = LMotion.Create(0f, 1f, _shakeDuration)
                .WithOnComplete(() => transform.localRotation = Quaternion.identity)
                .Bind(t =>
                {
                    float damper = 1f - t; // decays to zero
                    float angle = Mathf.Sin(t * _shakeFrequency) * _shakeAngle * damper;
                    transform.localRotation = Quaternion.Euler(0f, 0f, angle);
                });
        }

        private void PlayClip(AudioClip clip)
        {
            if (_audioSource != null && clip != null)
                _audioSource.PlayOneShot(clip);
        }
    }
}
