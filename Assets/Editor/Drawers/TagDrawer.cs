using UnityEditor;
using UnityEngine;

using Framework.Attributes;

namespace Editor.Drawers
{
    /// <summary>
    /// Strings will be shown as a dropdown menu with the tags options set in this unity project.
    /// </summary>
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagDrawer : PropertyDrawer
    {
        private const string UNTAGGED = "Untagged";
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUI.BeginProperty(position, label, property);
                property.stringValue = EditorGUI.TagField(position, label, property.stringValue);

                if (property.stringValue == string.Empty)
                    property.stringValue = UNTAGGED;
                
                EditorGUI.EndProperty();
            }
            else
                EditorGUI.PropertyField(position, property, label);
        }
    }
}