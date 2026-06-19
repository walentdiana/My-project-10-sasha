using UnityEngine;
using UnityEditor;

namespace BuildSystem
{
    [CustomEditor(typeof(BuildTileData))]
    public class PreviewSpriteDrawer : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            BuildTileData data = (BuildTileData)target;

            if (!data.Icon)
                return;
            
                GUILayout.Space(15);
                
                Texture2D tex = data.Icon.texture;
                Rect rect = data.Icon.rect;

                Rect uv = new Rect(
                    rect.x / tex.width,
                    rect.y / tex.height,
                    rect.width / tex.width,
                    rect.height / tex.height);
                
                Rect previewRect = GUILayoutUtility.GetRect(
                    128,
                    128,
                    GUILayout.ExpandWidth(false),
                    GUILayout.ExpandHeight(false));
                
                previewRect.x = (EditorGUIUtility.currentViewWidth - 128) / 2;
                
                GUI.DrawTextureWithTexCoords(previewRect, tex, uv);
            
        }
    }
}