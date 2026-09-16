using UnityEngine;

namespace ColorMatch.Presentation.Gameplay
{
    /// <summary>
    /// Defines the horizontal span and vertical bounds of the playfield in
    /// world units, and maps the logic layer's normalized X to a world X.
    /// </summary>
    public sealed class PlayArea : MonoBehaviour
    {
        [SerializeField] private float _halfWidth = 2.5f;
        [SerializeField] private float _spawnY = 6f;
        [SerializeField] private float _despawnY = -6f;
        [SerializeField] private float _basketY = -4f;

        public float SpawnY => _spawnY;
        public float DespawnY => _despawnY;
        public float BasketY => _basketY;
        public float HalfWidth => _halfWidth;

        /// <summary>Maps a normalized X in [0,1] to a world X within the field.</summary>
        public float WorldX(float normalized) => Mathf.Lerp(-_halfWidth, _halfWidth, normalized);
    }
}
