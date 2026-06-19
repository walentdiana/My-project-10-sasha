using System.Collections.Generic; // нужен для Dictionary
using BuildSystem;                 // нужен для TilemapLayerType
using UnityEngine;                 // нужен для Debug, MonoBehaviour
using UnityEngine.Tilemaps;       // нужен для Tilemap

namespace Core.Building
{
    // Реестр всех тайлмапов сцены
    // MonoBehaviour — живёт на сцене
    // Хранит словарь: тип слоя → Tilemap
    // TilePainter и ToolModeController спрашивают здесь: "дай мне Tilemap для слоя Garden"
    public class TilemapLayerRegistry : MonoBehaviour
    {
        // readonly — нельзя заменить сам словарь после создания
        // new() — сокращение от new Dictionary<TilemapLayerType, Tilemap>()
        private readonly Dictionary<TilemapLayerType, Tilemap> _layers = new();

        // Регистрирует тайлмап — вызывается каждым TilemapLayer при старте
        public void Register(TilemapLayerType type, Tilemap tilemap)
        {
            if (_layers.ContainsKey(type)) // уже есть такой тип?
            {
                // Предупреждение в консоль (не ошибка, игра не сломается)
                Debug.LogWarning($"TilemapLayerRegistry: layer {type} already registered, overwriting.");
                _layers[type] = tilemap; // перезаписываем
                return;
            }
            _layers[type] = tilemap; // добавляем новый
        }

        // Безопасный поиск — возвращает false если не нашли (не бросает исключение)
        // out Tilemap tilemap — выходной параметр, заполняется если нашли
        public bool TryGetLayer(TilemapLayerType type, out Tilemap tilemap)
            => _layers.TryGetValue(type, out tilemap); // true = нашли, false = нет

        // Небезопасный поиск — бросает ошибку в консоль если нет
        // Используется когда слой ТОЧНО должен быть (TilePainter)
        public Tilemap GetLayer(TilemapLayerType type)
        {
            if (_layers.TryGetValue(type, out var tilemap))
                return tilemap; // нашли — возвращаем

            // Не нашли — это серьёзная проблема, логируем ошибку
            Debug.LogError($"TilemapLayerRegistry: layer {type} not found.");
            return null; // возвращаем null (вызывающий код должен проверить)
        }
    }
}