using System;
using ColorMatch.Core.Enums;
using ColorMatch.Core.Model;
using ColorMatch.Core.Services;
using ColorMatch.Presentation.Config;
using UnityEngine;
using VContainer;

namespace ColorMatch.Presentation.Gameplay
{
    /// <summary>
    /// Bridges spawn requests from the logic core to actual pooled figures,
    /// resolving color and shape into concrete sprites and a world position.
    /// </summary>
    public sealed class FigureSpawner : MonoBehaviour
    {
        [Serializable]
        private struct ShapeSprite
        {
            public FigureShape Shape;
            public Sprite Sprite;
        }

        [SerializeField] private FigurePool _pool;
        [SerializeField] private PlayArea _playArea;
        [SerializeField] private ShapeSprite[] _shapeSprites;

        private GameSession _session;
        private ColorPalette _palette;

        [Inject]
        public void Construct(GameSession session, GameConfig config)
        {
            _session = session;
            _palette = config.Palette;
            _session.FigureRequested += OnFigureRequested;
        }

        private void OnDestroy()
        {
            if (_session != null)
                _session.FigureRequested -= OnFigureRequested;
        }

        private void OnFigureRequested(FigureSpawnRequest request)
        {
            FigureView figure = _pool.Get();
            figure.transform.position = new Vector3(
                _playArea.WorldX(request.NormalizedX),
                _playArea.SpawnY,
                0f);

            figure.Initialize(
                request.Color,
                _palette.Resolve(request.Color),
                SpriteFor(request.Shape),
                request.FallSpeed,
                _playArea.DespawnY);
        }

        private Sprite SpriteFor(FigureShape shape)
        {
            for (int i = 0; i < _shapeSprites.Length; i++)
                if (_shapeSprites[i].Shape == shape)
                    return _shapeSprites[i].Sprite;
            return null;
        }
    }
}
