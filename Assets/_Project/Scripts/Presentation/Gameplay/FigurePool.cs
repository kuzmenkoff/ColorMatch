using System.Collections.Generic;
using UnityEngine;

namespace ColorMatch.Presentation.Gameplay
{
    /// <summary>
    /// Simple pool of FigureView instances to avoid per-spawn allocations.
    /// </summary>
    public sealed class FigurePool : MonoBehaviour
    {
        [SerializeField] private FigureView _prefab;
        [SerializeField] private int _prewarm = 16;

        private readonly Queue<FigureView> _idle = new Queue<FigureView>();

        private void Awake()
        {
            for (int i = 0; i < _prewarm; i++)
                _idle.Enqueue(CreateInstance());
        }

        /// <summary>Takes a figure from the pool, creating one if empty.</summary>
        public FigureView Get()
        {
            FigureView figure = _idle.Count > 0 ? _idle.Dequeue() : CreateInstance();
            figure.gameObject.SetActive(true);
            return figure;
        }

        private FigureView CreateInstance()
        {
            FigureView figure = Instantiate(_prefab, transform);
            figure.Released += Return;
            figure.gameObject.SetActive(false);
            return figure;
        }

        private void Return(FigureView figure)
        {
            figure.gameObject.SetActive(false);
            _idle.Enqueue(figure);
        }
    }
}