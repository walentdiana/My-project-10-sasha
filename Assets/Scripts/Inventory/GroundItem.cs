using Inventory.Item;
using UnityEditor;
using UnityEngine;

namespace Inventory
{
    public class GroundItem : MonoBehaviour, ISerializationCallbackReceiver
    {
        public ItemObject item;

        public void OnBeforeSerialize()
        {
            GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
            EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
        }

        public void OnAfterDeserialize()
        {
            
        }
    }
}