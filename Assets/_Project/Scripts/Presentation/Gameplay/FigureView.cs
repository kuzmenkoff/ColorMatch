using System;
using ColorMatch.Core.Enums;
using UnityEngine;

namespace ColorMatch.Presentation.Gameplay
{
    /// <summary>
    /// Visual for a falling figure. Holds its logical color for catch checks,
    /// falls at a constant speed, and releases itself back to the pool once it
    /// drops below the play area or is caught.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Collider2D))]
    public sealed class FigureView : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Rigidbody2D _body;
        private float _despawnY;
        private bool _released;

        /// <summary>Logical color used by the catch rule.</summary>
        public FigureColor Color { get; private set; }

        /// <summary>Raised when the figure should return to its pool.</summary>
        public event Action<FigureView> Released;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _body = GetComponent<Rigidbody2D>();
        }

        /// <summary>Configures the figure for a new fall.</summary>
        public void Initialize(FigureColor color, Color rgba, Sprite sprite, float fallSpeed, float despawnY)
        {
            Color = color;
            _renderer.color = rgba;
            _renderer.sprite = sprite;
            _despawnY = despawnY;
            _released = false;
            _body.linearVelocity = Vector2.down * fallSpeed;
        }

        private void Update()
        {
            if (!_released && transform.position.y <= _despawnY)
                Release();
        }

        /// <summary>Returns the figure to the pool (caught or off-screen).</summary>
        public void Release()
        {
            if (_released)
                return;

            _released = true;
            _body.linearVelocity = Vector2.zero;
            Released?.Invoke(this);
        }
    }
}
