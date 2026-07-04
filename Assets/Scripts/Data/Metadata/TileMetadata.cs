using BuildSystem;          // нужен для TilemapLayerType
using Inventory.Item;       // нужен для ToolCapability
using UnityEngine;          // Unity
using UnityEngine.Tilemaps; // нужен для TileBase

namespace Data.Metadata
{
    // Метаданные тайла — описывает что можно сделать с тайлом и что получится
    // ScriptableObject — один файл на каждый тип интерактивного тайла
    //
    // Пример: трава
    //   SourceLayer = Ground       (ищем траву на слое Ground)
    //   RequiredCapability = Till  (нужна лопата чтобы взаимодействовать)
    //   ResultTile = грядка        (трава превращается в грядку)
    //   ResultLayer = Soil         (грядка кладётся на слой Soil)
    //   TimeToNextState = -1       (не меняется само по себе)
    [CreateAssetMenu(fileName = "TileMetadata", menuName = "Build System/Tile Metadata")]
    public class TileMetadata : ScriptableObject
    {
        public FlagsTilemapLayerType SourceLayer;      // на каком слое ищем этот тайл
        public ToolCapability RequiredCapability; // какой инструмент нужен (лопата=Till, топор=Chop)
        public TileBase ResultTile;              // во что превращается тайл после взаимодействия
        public FlagsTilemapLayerType ResultLayer;     // на каком слое размещается результат

        // Система времени — тайл меняется сам через некоторое время
        // Пример: политая грядка → через 10 секунд → сухая грядка
        public TileMetadata NextState;       // следующее состояние (null = не меняется)
        public float TimeToNextState = -1f;  // через сколько секунд (-1 = никогда)
    }
}