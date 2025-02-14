using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathData))]
public class PathEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PathData pathData = (PathData)target;
        EditableLine line = FindObjectOfType<EditableLine>();

        if (line == null)
        {
            EditorGUILayout.HelpBox("Не найден EditableLine в сцене!", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Сохранить путь"))
        {
            pathData.SavePath(line);
            EditorUtility.SetDirty(pathData);
            AssetDatabase.SaveAssets();
        }

        if (GUILayout.Button("Загрузить путь"))
        {
            pathData.LoadPath(line);
        }
    }
}
