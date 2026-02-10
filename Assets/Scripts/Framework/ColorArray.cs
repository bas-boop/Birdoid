using UnityEngine;

namespace Framework
{
    public static class ColorArray
    {
        public static Color GetColor(int target) => Colors[(target % Colors.Length + Colors.Length) % Colors.Length];

        private static readonly Color[] Colors =
        {
            Color.red,
            Color.blue,
            Color.green,
            Color.yellow,
            Color.cyan,
            Color.magenta,
            new Color(1.0f, 0.5f, 0.0f), // Orange
            new Color(0.5f, 0.0f, 1.0f), // Purple
            new Color(0.0f, 0.5f, 0.5f), // Teal
            new Color(0.5f, 0.5f, 0.0f), // Olive
            new Color(1.0f, 0.0f, 0.5f), // Hot Pink
            new Color(0.5f, 1.0f, 0.0f), // Lime
            new Color(0.0f, 0.5f, 1.0f), // Sky Blue
            new Color(1.0f, 0.25f, 0.25f), // Light Red
            new Color(0.25f, 1.0f, 0.25f), // Light Green
            new Color(0.25f, 0.25f, 1.0f), // Light Blue
            new Color(1.0f, 1.0f, 0.25f), // Light Yellow
            new Color(1.0f, 0.5f, 0.5f), // Light Pink
            new Color(0.5f, 1.0f, 0.5f), // Mint
            new Color(0.5f, 0.5f, 1.0f), // Lavender
            new Color(0.25f, 0.0f, 0.5f), // Deep Purple
            new Color(0.5f, 0.25f, 0.0f), // Brown
            new Color(0.0f, 0.25f, 0.5f), // Dark Blue
            new Color(0.25f, 0.5f, 0.0f), // Dark Green
            new Color(0.5f, 0.0f, 0.25f), // Maroon
            new Color(0.0f, 0.5f, 0.25f), // Sea Green
            new Color(0.8f, 0.6f, 0.2f)  // Gold
        };
    }
}