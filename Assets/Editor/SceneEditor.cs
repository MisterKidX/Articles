using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(Scene))]
public class SceneEditor : PropertyDrawer
{
    // TODO: give scene editor another look - Popup for int based loading, like in
    // the attribute for integers. Make this a menuitem
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // https://docs.unity3d.com/ScriptReference/PropertyDrawer.html
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        var relative = property.FindPropertyRelative("_guid");
        // we are using GUID so if the scene changes directory or renamed
        // we can still hold a reference to it
        var oldScenePath = AssetDatabase.GUIDToAssetPath(relative.stringValue);
        var pathToSave = oldScenePath;
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(oldScenePath);

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUI.ObjectField(position, property.displayName, oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            var newPath = AssetDatabase.GetAssetPath(newScene);
            pathToSave = newPath;
            var guid = AssetDatabase.GUIDFromAssetPath(newPath);

            relative.stringValue = guid.ToString();

            var sceneObject = fieldInfo.GetValue(property.serializedObject.targetObject) as Scene;
        }

        property.FindPropertyRelative("_path").stringValue = pathToSave;

        EditorGUI.EndProperty();
    }
}
