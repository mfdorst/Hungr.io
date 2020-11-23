using UnityEngine;
using UnityEditor;

public class MapGenerator : EditorWindow
{
    private GroundTile _tile;
    private Vector2 _origin = new Vector2(-14, -10);
    private Vector2 _dimensions = new Vector2(28, 20);
    private Color _barrenColor;
    private Color _abundantColor;
    private float _perlinClampFactor = 10;

    [MenuItem("Window/Map Generator")]
    public static void OpenWindow()
    {
        GetWindow<MapGenerator>("Map Generator");
    }

    public void OnGUI()
    {
        _tile = (GroundTile)EditorGUILayout.ObjectField(_tile, typeof(GroundTile), true);
        _origin = EditorGUILayout.Vector2Field("Origin", _origin);
        _dimensions = EditorGUILayout.Vector2Field("Dimensions", _dimensions);
        _barrenColor = EditorGUILayout.ColorField("Barren Color", _barrenColor);
        _abundantColor = EditorGUILayout.ColorField("Abundant Color", _abundantColor);
        _perlinClampFactor = EditorGUILayout.FloatField("Perlin Clamp Factor", _perlinClampFactor);

        if (GUILayout.Button("Generate")) GenerateMap();
    }

    private void GenerateMap()
    {
        var parent = new GameObject("Ground Tiles");
        for (var x = 0; x < _dimensions.x; x++)
        {
            for (var y = 0; y < _dimensions.y; y++)
            {
                var child = (GroundTile)PrefabUtility.InstantiatePrefab(_tile);
                var transform = child.transform;
                transform.SetParent(parent.transform);
                transform.position = new Vector3(_origin.x + x + 0.5f, _origin.y + y + 0.5f);
                child.name = $"Ground Tile ({x}, {y})";
                var clampedX = x / _perlinClampFactor;
                var clampedY = y / _perlinClampFactor;
                var color = Color.Lerp(_barrenColor, _abundantColor, Mathf.PerlinNoise(clampedX, clampedY));
                child.GetComponent<SpriteRenderer>().color = color;
            }
        }
    }
}
