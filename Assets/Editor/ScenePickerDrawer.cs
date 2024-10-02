using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.Reflection;
using System.Linq;

namespace DBD.UnityCodingTools.Editor
{
    [CustomPropertyDrawer(typeof(ScenePickerAttribute))]
    [InitializeOnLoad]
    public class ScenePickerDrawer : PropertyDrawer
    {
        static ScenePickerDrawer()
        {
            EditorBuildSettings.sceneListChanged -= BuildListChangedHandler;
            EditorBuildSettings.sceneListChanged += BuildListChangedHandler;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
                StringGUI(position, property, label);
            else if (property.propertyType == SerializedPropertyType.Integer)
                IntGUI(position, property, label);
            else
                EditorGUILayout.HelpBox(nameof(ScenePickerAttribute)
                    + " may only be used on strings and integers", MessageType.Warning);
        }

        private void IntGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var scene = EditorSceneManager.GetSceneByBuildIndex(property.intValue);
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path);

            var scenesInBuildCount = EditorSceneManager.sceneCountInBuildSettings;
            GUIContent[] scenes = new GUIContent[scenesInBuildCount];
            for (int i = 0; i < scenes.Length; i++)
            {
                var guiContent = EditorGUIUtility.IconContent("d_UnityLogo");
                scenes[i] = new GUIContent(guiContent);
                scenes[i].text = " " + GetSceneName(i);
            }

            EditorGUI.BeginChangeCheck();
            var newSceneIndex = EditorGUILayout.Popup(property.intValue, scenes);

            if (EditorGUI.EndChangeCheck())
                property.intValue = newSceneIndex;
        }

        private void StringGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var savedGUID = EditorPrefs.GetString(property.stringValue, "");

            var oldScenePath = AssetDatabase.GUIDToAssetPath(savedGUID);
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(oldScenePath);

            if (oldScene != null)
            {
                EditorPrefs.DeleteKey(property.stringValue);
                property.stringValue = oldScene.name;
                EditorPrefs.SetString(property.stringValue, savedGUID);
            }

            EditorGUI.BeginChangeCheck();
            var newScene = EditorGUI.ObjectField(position, property.displayName, oldScene, typeof(SceneAsset), false) as SceneAsset;

            if (EditorGUI.EndChangeCheck())
            {
                var newPath = AssetDatabase.GetAssetPath(newScene);
                var guid = AssetDatabase.GUIDFromAssetPath(newPath);
                property.stringValue = GetSceneName(newPath);

                EditorPrefs.DeleteKey(property.stringValue);
                property.stringValue = newScene.name;
                EditorPrefs.SetString(property.stringValue, guid.ToString());
            }
        }

        private string GetSceneName(int buildIndex)
        {
            var scene = EditorBuildSettings.scenes[buildIndex];
            var startIndex = scene.path.LastIndexOf('/');
            var endIndex = scene.path.LastIndexOf('.');
            return scene.path.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        private string GetSceneName(string path)
        {
            var startIndex = path.LastIndexOf('/');
            var endIndex = path.LastIndexOf('.');
            return path.Substring(startIndex + 1, endIndex - startIndex - 1);
        }

        private static void BuildListChangedHandler()
        {
            Debug.Log("Changes were made to the build settings!");

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                //Debug.Log($"Scanning assembly: {assembly.GetName().Name}");

                var types = assembly.GetTypes();

                foreach (var type in types)
                {
                    // Get fields decorated with TestAttribute
                    var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                        .Where(f => f.GetCustomAttribute<ScenePickerAttribute>() != null && f.FieldType == typeof(int));

                    foreach (var field in fields)
                    {
                        var attribute = field.GetCustomAttribute<ScenePickerAttribute>();
                        object b = Activator.CreateInstance(type);
                        var val = field.GetValue(b);
                        Debug.Log($"Field: {field.Name}, Contatinig Type: {type.FullName}, Value: {val}");
                    }
                }
            }
        }
    }
}