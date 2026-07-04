using System;        // стандартная библиотека
using BuildSystem;   // нужен для TilemapLayerType
using Core.Building; // нужен для TilemapLayerRegistry
using UnityEngine;   // Unity
using UnityEngine.Tilemaps; // нужен для Tilemap
using Zenject;       // нужен для [Inject]

// [RequireComponent] — Unity автоматически добавит Tilemap если его нет на объекте
[RequireComponent(typeof(Tilemap))]

/*Вешается на каждый тайлмап на сцене. При старте регистрирует себя в реестре: "я слой Fence," +
    " вот мой Tilemap". Больше ничего не делает.*/

public class TilemapLayer : MonoBehaviour 
{
    // Тип этого слоя — назначается в инспекторе (Ground? Garden? Buildings?)
    public FlagsTilemapLayerType LayerType;

    private TilemapLayerRegistry _layerRegistry; // реестр всех слоёв — придёт через Zenject

    // Публичное свойство: читать можно снаружи, но записать только здесь
    public Tilemap Tilemap { get; private set; }

    // [Inject] — Zenject видит этот метод и передаёт зависимости автоматически
    // Не нужно писать FindObjectOfType<TilemapLayerRegistry>()
    [Inject]
    public void Construction(TilemapLayerRegistry registry)
    {
        _layerRegistry = registry; // получаем реестр
    }

    // Awake — вызывается раньше Start
    // Получаем компонент Tilemap с этого же GameObject
    private void Awake()
    {
        Tilemap = GetComponent<Tilemap>(); // GetComponent — берём компонент с этого объекта
    }

    // Start — вызывается после Awake, когда все объекты уже инициализированы
    // Регистрируем себя в реестре: "я слой типа Garden, вот мой Tilemap"
    private void Start()
    {
        _layerRegistry.Register(LayerType, Tilemap);
    }
}