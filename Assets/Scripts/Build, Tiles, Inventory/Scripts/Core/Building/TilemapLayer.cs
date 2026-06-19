using System;
using BuildSystem;
using Core.Building;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

[RequireComponent(typeof(Tilemap))]
public class TilemapLayer : MonoBehaviour
{
    public TilemapLayerType LayerType;

    private TilemapLayerRegistry _layerRegistry;
    public Tilemap Tilemap { get; private set; }

    [Inject]
    public void Construction(TilemapLayerRegistry registry)
    {
        _layerRegistry = registry;
    }
    
    private void Awake()
    {
        Tilemap = GetComponent<Tilemap>();
    }

    private void Start()
    {
        _layerRegistry.Register(LayerType, Tilemap);
    }
}