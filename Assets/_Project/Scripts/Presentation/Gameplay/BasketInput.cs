using UnityEngine;
using UnityEngine.InputSystem;

namespace ColorMatch.Presentation.Gameplay
{
    /// <summary>
    /// Moves the basket horizontally to follow a pressed pointer (mouse or
    /// touch), clamped to the play area.
    /// </summary>
    public sealed class BasketInput : MonoBehaviour
    {
        [SerializeField] private PlayArea _playArea;
        [SerializeField] private Camera _camera;

        private void Reset() => _camera = Camera.main;

        private void Awake()
        {
            if (_camera == null)
                _camera = Camera.main;
        }

        private void Update()
        {
            Pointer pointer = Pointer.current;
            if (pointer == null || !pointer.press.isPressed)
                return;

            Vector2 screen = pointer.position.ReadValue();
            float worldX = _camera.ScreenToWorldPoint(screen).x;
            worldX = Mathf.Clamp(worldX, -_playArea.HalfWidth, _playArea.HalfWidth);

            Vector3 pos = transform.position;
            pos.x = worldX;
            transform.position = pos;
        }
    }
}
