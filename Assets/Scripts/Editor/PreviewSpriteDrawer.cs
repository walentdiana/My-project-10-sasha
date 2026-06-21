using UnityEngine; // Unity
using UnityEditor; // Unity Editor (этот код работает ТОЛЬКО в редакторе, не в билде)

namespace BuildSystem
{
    // Кастомный редактор для BuildTileData
    // [CustomEditor] — говорит Unity: когда выбран BuildTileData, показывай этот инспектор
    // Editor — базовый класс для кастомных инспекторов
    [CustomEditor(typeof(BuildTileData))]
    public class PreviewSpriteDrawer : Editor
    {
        // OnInspectorGUI — Unity вызывает это чтобы нарисовать инспектор
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); // сначала рисуем стандартные поля (TileName, Tile, Icon)

            BuildTileData data = (BuildTileData)target; // target = объект который редактируем

            if (!data.Icon) // нет иконки — не рисуем превью
                return;

            GUILayout.Space(15); // отступ 15 пикселей

            Texture2D tex = data.Icon.texture; // текстура спрайта
            Rect rect = data.Icon.rect;        // прямоугольник спрайта на текстуре (если атлас)

            // UV координаты — нормализованные (0..1) координаты спрайта на текстуре
            // Нужно если несколько спрайтов упакованы в одну текстуру (атлас)
            Rect uv = new Rect(
                rect.x / tex.width,   // левый край: пиксель / ширина = доля
                rect.y / tex.height,  // нижний край
                rect.width / tex.width,  // ширина в долях
                rect.height / tex.height // высота в долях
            );

            // Резервируем прямоугольник 128x128 в инспекторе для превью
            Rect previewRect = GUILayoutUtility.GetRect(
                128, 128,
                GUILayout.ExpandWidth(false),  // не растягивать по ширине
                GUILayout.ExpandHeight(false)  // не растягивать по высоте
            );

            // Центрируем превью по горизонтали
            previewRect.x = (EditorGUIUtility.currentViewWidth - 128) / 2;

            // Рисуем спрайт в прямоугольнике с UV координатами
            GUI.DrawTextureWithTexCoords(previewRect, tex, uv);
        }
    }
}