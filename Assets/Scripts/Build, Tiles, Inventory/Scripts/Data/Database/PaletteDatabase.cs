using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    // Единственный ассет на весь проект.
    // Содержит все BuildPalette — и locked, и unlocked.
    // PaletteUIManager сам отфильтрует нужные.
    [CreateAssetMenu(fileName = "PaletteDatabase", menuName = "Build System/Palette Database")]
    public class PaletteDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        private Dictionary<int, BuildPalette> _palettesDatabase;
        [field: SerializeField] public BuildPalette[] Palettes { get; private set; }
        
        public void OnAfterDeserialize()
        {
            _palettesDatabase = new Dictionary<int, BuildPalette>();
            for (int i = 0; i < Palettes.Length; i++)
            {
                Palettes[i].Id = i;
                _palettesDatabase.Add(i, Palettes[i]);
            }
        }

        public void OnBeforeSerialize(){}
        
        public bool TryGetPalette(int id, out BuildPalette palette) =>
        _palettesDatabase.TryGetValue(id, out palette);
    }
}