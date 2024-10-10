using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

[CustomPropertyDrawer(typeof(Scene))]
public class SceneEditor : PropertyDrawer
{
    // DONE: handle scene renames
    // DONE: handle scene change dir
    // DONE: handle build index change
    // TODO: handle all in build
    // TODO: handle scene change GUID
    // TODO: create a Scene view like the integer [ScenePicker].
    // make it part of the attribute's constructor.
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        var guidProperty = property.FindPropertyRelative("_guid");
        var pathProperty = property.FindPropertyRelative("_path");
        //var nameProperty = property.FindPropertyRelative("_name");
        //var indexProperty = property.FindPropertyRelative("_index");

        // we are using GUID so if the scene changes directory or renamed
        // we can still hold a reference to it
        var oldScenePath = AssetDatabase.GUIDToAssetPath(guidProperty.stringValue);
        SceneAsset oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(oldScenePath);

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUI.ObjectField(position, property.displayName, oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            var guid = AssetDatabase.GUIDFromAssetPath(newPath);

            guidProperty.stringValue = guid.ToString();
            pathProperty.stringValue = newPath.ToString();
        }

        //nameProperty.stringValue = newScene.name;
        //indexProperty.intValue = SceneUtility.GetBuildIndexByScenePath(newPath);

        EditorGUI.EndProperty();
    }
}
