using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
    private static bool editing = false;
    private static HexGrid hexGrid = null;

    private const string buttonText_open = "Edit";
    private const string buttonText_close = "Save";
    private static int tileTypeSelected = 0;
    private static int[][] allTiles;

    private static string[] tileTypes = { "Empty", "Blocked", "Wood", "Stone" };
    private static Texture2D[] icons;

    public override void OnInspectorGUI()
    {
        if (hexGrid == null)
        {
            Init(target);
        }

        //Title
        GUILayout.Box("Hex Grid Editor", GUILayout.ExpandWidth(true));

        ShowFields(editing);

        if (GUILayout.Button((editing?buttonText_close:buttonText_open)))
            editing = !editing;
        if (editing)
        {
            ShowEditor();
            DrawGrid();
        }
    }

    private static void ShowFields(bool active)
    {
        EditorGUI.BeginDisabledGroup(active);
        EditorGUI.BeginChangeCheck();
        hexGrid.mapRadius = EditorGUILayout.IntField("Radius", hexGrid.mapRadius);
        if (EditorGUI.EndChangeCheck())
        {
            hexGrid = null;
            return;
        }
        EditorGUI.EndDisabledGroup();
    }

    private static void Init(Object target)
    {
        hexGrid = (HexGrid)target;
        int size = hexGrid.mapRadius * 2 + 1;
        allTiles = new int[size][];
        for (int i = 0; i < size; i++)
            allTiles[i] = new int[size];

        //load icons
        //{ "Empty", "Blocked", "Wood", "Stone" };
        icons = new Texture2D[4];
        icons[0] = Resources.Load<Texture2D>("HexEditor/editor_empty");
        icons[1] = Resources.Load<Texture2D>("HexEditor/editor_blocked");
        icons[2] = Resources.Load<Texture2D>("HexEditor/editor_tree");
        icons[3] = Resources.Load<Texture2D>("HexEditor/editor_stone");
    }

    private static int selectedTile = 0;
    private static void ShowEditor()
    {
        GUILayout.Label("Select tile type:");
        GUILayout.BeginHorizontal();
        selectedTile = GUILayout.SelectionGrid(selectedTile, tileTypes, tileTypes.Length, EditorStyles.miniButtonMid);
        GUILayout.EndHorizontal();
    }

    private static void DrawGrid()
    {
        float w = EditorGUIUtility.currentViewWidth;
        int r = hexGrid.mapRadius;
        int d = r * 2 + 1;
        float tw = w / d;
        float y = GUILayoutUtility.GetLastRect().y + 32;
        GUILayout.FlexibleSpace();

        for (int i = 0; i < d; i++)
        {
            int count = (int)d - Mathf.Abs(hexGrid.mapRadius - i);                        //How many tiles in a row
            int startIndex = Mathf.Max(hexGrid.mapRadius - i, 0);                           //Starting collumn in 2d array
            for (int j = startIndex; j < startIndex + count; j++)
            {
                Rect rect = new Rect(j * tw + (i - r) * tw / 2, y + i * tw, tw, tw);
                if (GUI.Button(rect, icons[allTiles[i][j]], GUIStyle.none))
                    allTiles[i][j] = selectedTile;
            }
        }
    }


}
