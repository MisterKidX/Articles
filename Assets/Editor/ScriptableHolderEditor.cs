using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptableHolder<>), true)]
public class ScriptableHolderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SerializedProperty valueProp = serializedObject.FindProperty("Value");

        if (valueProp != null && valueProp.propertyType == SerializedPropertyType.Generic)
        {
            var SOType = target.GetType();
            var type = SOType.GetField("Value").FieldType;
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
            {
                SerializedProperty fieldProp = valueProp.FindPropertyRelative(field.Name);

                if (fieldProp != null)
                    EditorGUILayout.PropertyField(fieldProp, new GUIContent(ObjectNames.NicifyVariableName(field.Name)));
            }
        }
        else if (valueProp != null && valueProp.propertyType == SerializedPropertyType.ObjectReference)
            EditorGUILayout.HelpBox("You cannot wrap values that derive from UnityEngine.Object.", MessageType.Warning);
        else
            EditorGUILayout.HelpBox("No valid 'Value' property found.", MessageType.Warning);

        serializedObject.ApplyModifiedProperties();
    }

    // this is instead of setting the icon manualy for each new derived ScriptableHolder
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        SerializedProperty valueProp = serializedObject.FindProperty("Value");

        if (valueProp != null && valueProp.propertyType == SerializedPropertyType.Generic)
        {
            var icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/ScriptableHolder Icon.png");

            EditorGUIUtility.SetIconForObject(target, icon);
        }
        else
        {
            EditorGUIUtility.SetIconForObject(target, EditorGUIUtility.IconContent("console.warnicon@2x").image as Texture2D);
        }

        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }
}