using UnityEditor;
using UnityEngine;
using System;
using System.IO;

[CustomEditor(typeof(HexGrid))]
public class HexGridEditor : Editor
{
    private static bool editing = false;
    private static HexGrid hexGrid = null;

    private const string buttonText_open = "Load from file";
    private const string buttonText_close = "Save";

    private static int[][] allTiles;
    private static string[] tileTypes;
    private static Texture2D[] icons;

    public override void OnInspectorGUI()
    {
        if (hexGrid == null)
        {
            Init(target);
        }

        //Title
        GUILayout.Box("Hex Grid Editor", GUILayout.ExpandWidth(true));

        ShowFields();

        Color c = GUI.color;
        GUI.color = new Color32(64, 168, 64, 254);
        if (GUILayout.Button((editing ? buttonText_close : buttonText_open)))
        {
            GUI.color = c;
            if (editing)
                Save();
            else
                Load();
            editing = !editing;
        }
        GUI.color = c;
        if (editing)
        {
            ShowEditor();
            DrawGrid();
            ClearButton();
        }
    }

    private void Save()
    {
        using (FileStream fs = new FileStream(System.IO.Path.Combine(Application.streamingAssetsPath, HexGrid.savePath), FileMode.OpenOrCreate, FileAccess.Write))
        {
            //Write map radius
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(hexGrid.mapRadius);

            //Write all tiles
            int d = hexGrid.mapRadius * 2 + 1;
            for (int i = 0; i < d; i++)
            {
                int count = (int)d - Mathf.Abs(hexGrid.mapRadius - i);
                int startIndex = Mathf.Max(hexGrid.mapRadius - i, 0);
                for (int j = startIndex; j < startIndex + count; j++)
                {
                    bw.Write(allTiles[i][j]);
                }
            }
        }
        Debug.Log("grid.data saved");
    }

    private static void ClearButton()
    {
        Color c = GUI.color;
        GUI.color = new Color32(168, 64, 64, 254);
        if (GUILayout.Button("Clear"))
        {
            ResetTiles();
        }
        GUI.color = c;
    }

    private static void ResetTiles()
    {
        int size = hexGrid.mapRadius * 2 + 1;
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                allTiles[i][j] = 0;
    }

    private void Load()
    {
        if (File.Exists(System.IO.Path.Combine(Application.streamingAssetsPath, HexGrid.savePath)) == false)
        {
            ResetTiles();
            return;
        }
        using (FileStream fs = new FileStream(System.IO.Path.Combine(Application.streamingAssetsPath, HexGrid.savePath), FileMode.Open, FileAccess.Read))
        {
            BinaryReader br = new BinaryReader(fs);
            int radius = br.ReadInt32();
            hexGrid.mapRadius = radius;
            int d = hexGrid.mapRadius * 2 + 1;
            for (int i = 0; i < d; i++)
            {
                int count = (int)d - Mathf.Abs(hexGrid.mapRadius - i);
                int startIndex = Mathf.Max(hexGrid.mapRadius - i, 0);
                for (int j = startIndex; j < startIndex + count; j++)
                {
                    allTiles[i][j] = br.ReadInt32();
                }
            }
        }
        Debug.Log("grid.data loaded");
    }

    private static void ShowFields()
    {
        EditorGUI.BeginChangeCheck();
        hexGrid.mapRadius = EditorGUILayout.IntField("Radius", hexGrid.mapRadius);
        if (EditorGUI.EndChangeCheck())
        {
            ResetTiles();
            return;
        }
    }

    private static void Init(UnityEngine.Object target)
    {
        if (icons == null || tileTypes == null)
            LoadResources();

        hexGrid = (HexGrid)target;
        int size = hexGrid.mapRadius * 2 + 1;
        allTiles = new int[size][];
        for (int i = 0; i < size; i++)
            allTiles[i] = new int[size];
        selectedTile = 0;
    }

    private static void LoadResources()
    {
        //Setup strings from enum
        string[] types = Enum.GetNames(typeof(TileType));
        int count = types.Length - 1;

        //load icons
        tileTypes = new string[count];
        icons = new Texture2D[count];
        for (int i = 0; i < count; i++)
        {
            tileTypes[i] = types[i + 1];
            icons[i] = Resources.Load<Texture2D>("HexEditor/editor_" + tileTypes[i].ToLower());
        }
    }

    private static int selectedTile = 0;
    private static void ShowEditor()
    {
        GUILayout.Label("Select tile type:");
        GUILayout.BeginHorizontal();
        selectedTile = GUILayout.SelectionGrid(selectedTile, icons, tileTypes.Length, EditorStyles.miniButtonMid);
        GUILayout.EndHorizontal();
    }

    private static void DrawGrid()
    {
        float w = EditorGUIUtility.currentViewWidth;
        int r = hexGrid.mapRadius;
        int d = r * 2 + 1;
        float tw = w / d;
        float y = GUILayoutUtility.GetLastRect().y + 48;
        GUILayout.FlexibleSpace();

        for (int i = 0; i < d; i++)
        {
            int count = (int)d - Mathf.Abs(hexGrid.mapRadius - i);                        
            int startIndex = Mathf.Max(hexGrid.mapRadius - i, 0);                         
            for (int j = startIndex; j < startIndex + count; j++)
            {
                Rect rect = new Rect(j * tw + (i - r) * tw / 2, y + i * tw, tw, tw);
                if (GUI.Button(rect, icons[allTiles[i][j]], GUIStyle.none))
                    allTiles[i][j] = selectedTile;
            }
        }
    }
}
