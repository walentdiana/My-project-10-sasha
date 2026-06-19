using System.Collections.Generic;
using UnityEngine;

namespace BuildSystem
{
    [CreateAssetMenu(fileName = "PaletteDatabase", menuName = "Build System/Palette Database")]
    public class PaletteDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        [field: SerializeField] public BuildPalette[] Palettes { get; private set; }

        private Dictionary<int, BuildPalette> _palettesDatabase;
        
        public void OnAfterDeserialize()
        {
            _palettesDatabase = new Dictionary<int, BuildPalette>();
            for (int i = 0; i < Palettes.Length; i++)
            {
                Palettes[i].Id = i;
                _palettesDatabase.Add(i, Palettes[i]);
            }
        }

        public void OnBeforeSerialize(){ }
        
        public bool TryGetPalette(int id, out BuildPalette palette) => 
            _palettesDatabase.TryGetValue(id, out palette);

    }
}