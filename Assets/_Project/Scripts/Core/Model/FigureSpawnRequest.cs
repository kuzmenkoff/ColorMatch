using ColorMatch.Core.Enums;

namespace ColorMatch.Core.Model
{
    /// <summary>
    /// A spawn request produced by the logic layer. The presentation layer
    /// turns it into an actual pooled sprite.
    /// </summary>
    public readonly struct FigureSpawnRequest
    {
        public readonly FigureColor Color;
        public readonly FigureShape Shape;
        public readonly float NormalizedX; // 0..1 — horizontal spawn position
        public readonly float FallSpeed;   // world units per second

        public FigureSpawnRequest(FigureColor color, FigureShape shape, float normalizedX, float fallSpeed)
        {
            Color = color;
            Shape = shape;
            NormalizedX = normalizedX;
            FallSpeed = fallSpeed;
        }
    }
}
