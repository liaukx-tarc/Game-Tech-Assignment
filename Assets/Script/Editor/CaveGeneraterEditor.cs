using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(CaveGenerater))]
public class CaveGeneraterEditor : Editor
{
    bool isFloodFill = true;


    public override void OnInspectorGUI()
    {
        CaveGenerater generater = (CaveGenerater)target;

        //Tilemap
        GUILayout.Label("The target tilemap");
        generater.tilemap = EditorGUILayout.ObjectField("Tilemap",generater.tilemap, typeof(Tilemap), true) as Tilemap;
        GUILayout.Space(8);

        //Map Size
        GUILayout.Label("The size of cave");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Width");
        generater.width = EditorGUILayout.IntField(generater.width);

        GUILayout.Label("Height");
        generater.height = EditorGUILayout.IntField(generater.height);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);

        //Generate Chance
        GUILayout.Label("Chance to generate path");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Chance");
        generater.generateChance = (int)EditorGUILayout.Slider(generater.generateChance, 0, 100);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);

        //Repeat Times
        GUILayout.Label("Cellular Automata");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Repeat Steps");
        generater.repeatTimes = EditorGUILayout.IntField(generater.repeatTimes);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);

        //Death Birth Limit
        GUILayout.Label("The Birth & Death Limit");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Birth");
        generater.birthLimit = EditorGUILayout.IntField(generater.birthLimit);

        GUILayout.Label("Death");
        generater.deathLimit = EditorGUILayout.IntField(generater.deathLimit);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);

        //Flood Fill
        isFloodFill = EditorGUILayout.Toggle("Floodfill Algorithm", isFloodFill);
        if (isFloodFill)
        {
            generater.isMapCheck = true;
        }
        else
            generater.isMapCheck = false;

        //Cave Percentage
        GUILayout.Label("The minimum % of largest cave over the map");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Percentage");
        generater.cavePercentage = (int)EditorGUILayout.Slider(generater.cavePercentage, 0, 100);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);


        //Tile
        GUILayout.Label("The tile");
        GUILayout.Label("Rock");
        generater.tile[0] = EditorGUILayout.ObjectField(generater.tile[0], typeof(Tile), true) as Tile;
        GUILayout.Space(3);

        GUILayout.Label("Path");
        generater.tile[1] = EditorGUILayout.ObjectField(generater.tile[1], typeof(Tile), true) as Tile;
        GUILayout.Space(3);

        GUILayout.Label("Wall");
        generater.tile[2] = EditorGUILayout.ObjectField(generater.tile[2], typeof(Tile), true) as Tile;
        GUILayout.Space(3);

        GUILayout.Label("Start Point");
        generater.tile[3] = EditorGUILayout.ObjectField(generater.tile[3], typeof(Tile), true) as Tile;
        GUILayout.Space(3);

        GUILayout.Label("End Point");
        generater.tile[4] = EditorGUILayout.ObjectField(generater.tile[4], typeof(Tile), true) as Tile;
        GUILayout.Space(3);

        //EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);

        if (GUILayout.Button("Generate Map"))
        {
            generater.tilemap.ClearAllTiles();
            generater.GenerateCave();
        }

        if(GUILayout.Button("Reset"))
        {
            generater.tilemap.ClearAllTiles();
        }
    }
}
