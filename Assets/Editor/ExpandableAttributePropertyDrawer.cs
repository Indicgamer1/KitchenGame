using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ExpandableAttribute))]
public class ExpandableAttributePropertyDrawer : PropertyDrawer
{
    Editor m_editor = null;


    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label,true);

        if (property.objectReferenceValue == null)
            return;
        if(property.isExpanded == EditorGUI.Foldout(position, property.isExpanded, GUIContent.none))
        {
            EditorGUI.indentLevel++;

             Rect rect = EditorGUILayout.BeginVertical(GUI.skin.box);

            if(!m_editor)
            Editor.CreateCachedEditor(property.objectReferenceValue, null, ref m_editor);
            m_editor.OnInspectorGUI();

            EditorGUILayout.EndVertical();

            DrawOutlineBox(rect, Color.blue, 1);
            EditorGUI.indentLevel--;
        }
    }

    void DrawOutlineBox(Rect rect,Color color,int thickness)
    {
        //Draw top line
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, thickness), color);

        //Draw bottom line
        EditorGUI.DrawRect(new Rect(rect.x, rect.yMax -thickness, rect.width, thickness), color);

        //Draw left line
        EditorGUI.DrawRect(new Rect(rect.x, rect.y, thickness,rect.height), color);

        //Draw right line
        EditorGUI.DrawRect(new Rect(rect.xMax -thickness, rect.y, thickness, rect.height ), color);
    }
}
