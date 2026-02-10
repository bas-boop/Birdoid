#if UNITY_EDITOR

using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace Swzwij.FolderArchitecture
{
    /// <summary>
    /// Defines an editor tool to create a standard folder structure for Unity projects.
    /// </summary>
    public class FolderArchitect : EditorWindow
    {
        const string ROOT_FOLDER = "Assets/";

        /// <summary>
        /// Provides a list of essential folders for a typical Unity project.
        /// </summary>
        private static readonly List<string> _folders = new()
        {
            "Art",
            "Art/Animations",
            "Art/Materials",
            "Art/Models",
            "Art/Rigs",
            "Art/Shaders",
            "Art/Textures",
            "Audio",
            "Audio/Sound Effects",
            "Audio/Music",
            "Editor",
            "Plugins",
            "Prefabs",
            "Resources",
            "Scenes",
            "Scripts",
            "Scripts/Utils",
            "Settings"
        };

        /// <summary>
        /// Generates the defined folder structure within the Unity project.
        /// </summary>
        [MenuItem("Tools/Folder Architect/Generate Default Architecture")]
        private static void Generate() => GenerateCustomArchitecture(null);

        /// <summary>
        /// Generates a custom folder structure based on a provided architecture list.
        /// </summary>
        /// <param name="architecture">A list of folders to create.</param>
        public static void GenerateCustomArchitecture(List<string> architecture)
        {
            List<string> folderArchitecture = architecture ?? _folders;

            foreach (string folder in folderArchitecture)
            {
                string path = ROOT_FOLDER + folder;

                if (Directory.Exists(path))
                    continue;

                Directory.CreateDirectory(path);
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Converts a newline-separated string representation of a folder structure into a list of folders.
        /// </summary>
        /// <param name="architecture">A string where each line represents a folder.</param>
        /// <returns>A list of folder paths.</returns>
        public static List<string> ProcessArchitecture(string architecture)
        {
            string[] lines = architecture.Split('\n');
            List<string> folderList = new();

            foreach (string line in lines)
                folderList.Add(line.Trim());

            return folderList;
        }
    }
}

#endif