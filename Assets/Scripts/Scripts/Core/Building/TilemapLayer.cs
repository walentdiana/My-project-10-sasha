using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BuildSystem
{
    public class TilemapLayer : MonoBehaviour
    {
        [SerializeField] private TilePainter _painter;
        public TilemapLayerType Type;
        public Tilemap Tilemap { get; private set; }

        private void Awake()
        {
            Tilemap = GetComponent<Tilemap>();
            _painter.RegisterLayer(this);
        }
    }
}