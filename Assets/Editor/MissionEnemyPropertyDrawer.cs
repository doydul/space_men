using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(MissionEnemyProfile.MissionEnemy))]
public class MissionEnemyPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        var properties = new List<SerializedProperty>();
        properties.Add(property.FindPropertyRelative("randomized"));
        properties.Add(property.FindPropertyRelative("threat"));
        
        if (property.FindPropertyRelative("randomized").boolValue) {
            properties.Add(property.FindPropertyRelative("difficulty"));
            properties.Add(property.FindPropertyRelative("subLevel"));
        } else {
            properties.Add(property.FindPropertyRelative("type"));
        }

        var lineHeight = EditorGUIUtility.singleLineHeight;
        
        int i = 0;
        foreach (var prop in properties) {
            var rect = new Rect(position.x, position.y + lineHeight * i, position.width, lineHeight);
            EditorGUI.PropertyField(rect, prop);
            i++;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return  EditorGUIUtility.singleLineHeight * (property.FindPropertyRelative("randomized").boolValue ? 4 : 3);
    }
}