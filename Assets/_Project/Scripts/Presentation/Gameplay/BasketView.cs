using ColorMatch.Core.Enums;
using ColorMatch.Core.Services;
using ColorMatch.Presentation.Config;
using UnityEngine;
using VContainer;

namespace ColorMatch.Presentation.Gameplay
{
    /// <summary>
    /// Renders the basket's current color and reports caught figures to the
    /// session. Needs a trigger collider covering the basket mouth.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class BasketView : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private GameSession _session;
        private ColorPalette _palette;

        private void Awake() => _renderer = GetComponent<SpriteRenderer>();

        [Inject]
        public void Construct(GameSession session, GameConfig config)
        {
            _session = session;
            _palette = config.Palette;
            _session.BasketColorChanged += OnBasketColorChanged;
        }

        private void OnDestroy()
        {
            if (_session != null)
                _session.BasketColorChanged -= OnBasketColorChanged;
        }

        private void OnBasketColorChanged(FigureColor color)
            => _renderer.color = _palette.Resolve(color);

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out FigureView figure))
                return;

            _session.ReportCatch(figure.Color);
            figure.Release();
        }
    }
}
