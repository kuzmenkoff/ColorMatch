using System;
using System.Collections.Generic;
using ColorMatch.Core.Enums;
using UnityEngine;

namespace ColorMatch.Presentation.Config
{
    /// <summary>
    /// Maps each logical <see cref="FigureColor"/> to a concrete RGBA value and
    /// defines which colors are in play.
    /// </summary>
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "ColorMatch/Color Palette")]
    public sealed class ColorPalette : ScriptableObject
    {
        [Serializable]
        private struct Entry
        {
            public FigureColor Color;
            public Color Rgba;
        }

        [SerializeField] private Entry[] _entries;

        private FigureColor[] _cachedColors;

        /// <summary>The logical colors available for gameplay, in configured order.</summary>
        public IReadOnlyList<FigureColor> Colors
        {
            get
            {
                if (_cachedColors == null || _cachedColors.Length != _entries.Length)
                {
                    _cachedColors = new FigureColor[_entries.Length];
                    for (int i = 0; i < _entries.Length; i++)
                        _cachedColors[i] = _entries[i].Color;
                }
                return _cachedColors;
            }
        }

        /// <summary>Resolves a logical color to its RGBA value.</summary>
        public Color Resolve(FigureColor color)
        {
            for (int i = 0; i < _entries.Length; i++)
            {
                if (_entries[i].Color == color)
                    return _entries[i].Rgba;
            }
            return Color.magenta; // visible fallback for an unmapped color
        }
    }
}
