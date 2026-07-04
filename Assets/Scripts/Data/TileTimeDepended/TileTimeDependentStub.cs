using Data.Metadata; // нужен для TileMetadata
using UnityEngine;   // нужен для Vector3Int

namespace BuildSystem.TileTimeDependent
{
    // Интерфейс системы времени для тайлов
    // Описывает: "я умею регистрировать тайлы которые меняются со временем"
    // Сейчас реализован заглушкой, потом заменим на реальную систему
    public interface ITileTimeDependent
    {
        // Зарегистрировать тайл: "этот тайл в клетке cell должен измениться через metadata.TimeToNextState секунд"
        void RegisterTimedTile(Vector3Int cell, FlagsTilemapLayerType layer, TileMetadata metadata);

        // Убрать регистрацию тайла (например тайл уже изменился или убран)
        void UnregisterTimedTile(Vector3Int cell, FlagsTilemapLayerType layer);
    }

    // Заглушка — пустая реализация интерфейса
    // Игра компилируется и работает, но тайлы пока не меняются со временем
    // TODO: заменить когда будет готова система времени
    public class TileTimeDependentStub : ITileTimeDependent
    {
        // Должна регистрировать тайл для изменения — пока ничего не делает
        public void RegisterTimedTile(Vector3Int cell, FlagsTilemapLayerType layer, TileMetadata metadata)
        {
            // TODO: система времени — запустить таймер для этого тайла
        }

        // Должна отменять регистрацию — пока ничего не делает
        public void UnregisterTimedTile(Vector3Int cell, FlagsTilemapLayerType layer)
        {
            // TODO: система времени — остановить таймер для этого тайла
        }
    }
}