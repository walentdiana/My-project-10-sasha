using UnityEngine; // Unity (не используется напрямую но нужен для зависимостей)
using Zenject;     // нужен для IInitializable, [Inject]

namespace Data.Metadata
{
    // Инициализатор реестра метаданных
    // Нужен потому что TileMetadataRegistry — ScriptableObject (не MonoBehaviour)
    // и не имеет Awake/Start. Zenject вызовет Initialize() вместо них.
    public class TileMetadataRegistryInitializer : IInitializable
    {
        private TileMetadataRegistry _registry; // реестр который нужно инициализировать

        // Zenject передаёт реестр через этот метод
        [Inject]
        public void Construction(TileMetadataRegistry registry)
        {
            _registry = registry; // запоминаем
        }

        // IInitializable.Initialize() — Zenject вызывает это при старте (вместо Start)
        public void Initialize()
        {
            _registry.Initialize(); // строим кэш и сортируем записи
        }
    }
}