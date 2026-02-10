using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Swzwij.FolderArchitecture
{
    #if UNITY_EDITOR
    
    /// <summary>
    /// Provides a custom editor window for creating folder structures within a Unity project.
    /// </summary>
    public class FolderArchitectWindow : EditorWindow
    {
        /// <summary>
        /// Holds a list of desired folder paths (one per line) to be created within the project.
        /// </summary>
        [Multiline] private string inputText = 
            "Art\r\n" +
            "Art/Animations\r\n" +
            "Art/Materials\r\n" +
            "Art/Models\r\n" +
            "Art/Rigs\r\n" +
            "Art/Shaders\r\n" +
            "Art/Textures\r\n" +
            "Audio\r\n" +
            "Audio/Sound Effects\r\n" +
            "Audio/Music\r\n" +
            "Editor\r\n" +
            "Plugins\r\n" +
            "Prefabs\r\n" +
            "Resources\r\n" +
            "Scenes\r\n" +
            "Scripts\r\n" +
            "Scripts/Utils\r\n" +
            "Settings";

        /// <summary>
        /// Opens the Folder Architect window in the Unity Editor.
        /// </summary>
        [MenuItem("Tools/Folder Architect/Create Custom Architecture")]
        private static void Init()
        {
            FolderArchitectWindow window = (FolderArchitectWindow)GetWindow(typeof(FolderArchitectWindow));
            window.Show();
            window.titleContent = new GUIContent("Architecture Creator");
        }

        /// <summary>
        /// Renders the GUI for the Folder Architect window and handles folder creation logic. 
        /// </summary>
        private void OnGUI()
        {
            List<string> formatedStructure;
            inputText = EditorGUILayout.TextArea(inputText, GUILayout.ExpandHeight(true));

            if (GUILayout.Button("Generate Architecture"))
            {
                formatedStructure = FolderArchitect.ProcessArchitecture(inputText);
                FolderArchitect.GenerateCustomArchitecture(formatedStructure);
            }
        }
    }
    
    #endif
}