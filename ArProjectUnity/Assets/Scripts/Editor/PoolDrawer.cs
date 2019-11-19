using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(PoolItem))]
public class PoolDrawer : PropertyDrawer
{
    SerializedProperty nameP, goP, amountP;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect upperRect = new Rect(position.x, position.y, position.width, position.height/2);
        Rect goRect = new Rect(position.x, upperRect.y+GetPropertyHeight(property,label)/2, position.width * 0.7f, position.height / 2) ;
        Rect amountRect = new Rect(position.x+ position.width * 0.7f+5, upperRect.y+ GetPropertyHeight(property, label)/2, position.width * 0.3f-5, position.height/2);

        nameP = property.FindPropertyRelative("name");
        goP = property.FindPropertyRelative("go");
        amountP = property.FindPropertyRelative("poolSize");

        EditorGUI.PropertyField(upperRect, nameP,GUIContent.none);
        EditorGUI.PropertyField(goRect, goP,GUIContent.none);
        EditorGUI.PropertyField(amountRect, amountP,GUIContent.none);
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label)*2;
    }
}
