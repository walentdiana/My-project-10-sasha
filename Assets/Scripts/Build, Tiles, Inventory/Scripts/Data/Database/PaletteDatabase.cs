using System.Collections.Generic; // нужен для Dictionary
using UnityEngine;                 // Unity

namespace BuildSystem
{
    // База данных всех палитр — один .asset файл на весь проект
    // Аналог ItemDatabaseObject, но для палитр тайлов
    // ISerializationCallbackReceiver — пересобирает словарь при загрузке
    [CreateAssetMenu(fileName = "PaletteDatabase", menuName = "Build System/Palette Database")]
    public class PaletteDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        // Словарь для быстрого поиска палитры по ID
        // Не сохраняется — пересобирается при загрузке
        private Dictionary<int, BuildPalette> _palettesDatabase;

        [field: SerializeField] public BuildPalette[] Palettes { get; private set; } // все палитры

        // Вызывается после загрузки — пересобираем словарь и проставляем ID
        public void OnAfterDeserialize()
        {
            _palettesDatabase = new Dictionary<int, BuildPalette>(); // создаём пустой словарь

            for (int i = 0; i < Palettes.Length; i++)
            {
                Palettes[i].Id = i;                    // ID = индекс в массиве
                _palettesDatabase.Add(i, Palettes[i]); // добавляем в словарь
            }
        }

        // Вызывается перед сохранением — ничего не делаем
        public void OnBeforeSerialize() { }

        // Ищет палитру по ID — безопасный поиск (не бросает исключение)
        // out — выходной параметр, возвращает найденную палитру
        public bool TryGetPalette(int id, out BuildPalette palette) =>
            _palettesDatabase.TryGetValue(id, out palette); // true = нашли, false = нет
    }
}