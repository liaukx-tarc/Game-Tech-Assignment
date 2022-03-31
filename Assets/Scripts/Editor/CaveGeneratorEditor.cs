using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(CaveGenerator))]
public class CaveGeneratorEditor : Editor
{
    bool isFloodFill = true;


    public override void OnInspectorGUI()
    {
        CaveGenerator generator = (CaveGenerator)target;

        //Camera
        GUILayout.Label("Main Camera");
        generator.mainCamera = EditorGUILayout.ObjectField("Camera", generator.mainCamera, typeof(GameObject), true) as GameObject;
        GUILayout.Space(8);

        //Tilemap
        GUILayout.Label("The target tilemap");
        generator.pathMap = EditorGUILayout.ObjectField("Path",generator.pathMap, typeof(Tilemap), true) as Tilemap;
        GUILayout.Space(8);

        generator.wallMap = EditorGUILayout.ObjectField("Wall", generator.wallMap, typeof(Tilemap), true) as Tilemap;
        GUILayout.Space(8);

        //Map Size
        GUILayout.Label("The size of cave");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Width");
        generator.width = EditorGUILayout.IntField(generator.width);

        GUILayout.Label("Height");
        generator.height = EditorGUILayout.IntField(generator.height);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);

        //Generate Chance
        GUILayout.Label("Chance to generate path");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Chance");
        generator.generateChance = (int)EditorGUILayout.Slider(generator.generateChance, 0, 100);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);

        //Repeat Times
        GUILayout.Label("Cellular Automata");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Repeat Steps");
        generator.repeatTimes = EditorGUILayout.IntField(generator.repeatTimes);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);

        //Death Birth Limit
        GUILayout.Label("The Birth & Death Limit");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Birth");
        generator.birthLimit = EditorGUILayout.IntField(generator.birthLimit);

        GUILayout.Label("Death");
        generator.deathLimit = EditorGUILayout.IntField(generator.deathLimit);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);

        //Flood Fill
        EditorGUILayout.BeginHorizontal();
        isFloodFill = EditorGUILayout.Toggle("Largest Cave Only", isFloodFill);
        if (isFloodFill)
        {
            generator.isMapCheck = true;
        }
        else
            generator.isMapCheck = false;
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);
        //Cave Percentage
        GUILayout.Label("The minimum size of largest cave over the map");
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Percentage");
        generator.cavePercentage = (int)EditorGUILayout.Slider(generator.cavePercentage, 0, 100);

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);


        //Tile
        GUILayout.Label("Tiles");
        GUILayout.Label("Rock");
        generator.tile[0] = EditorGUILayout.ObjectField(generator.tile[0], typeof(Tile), true) as Tile;
        GUILayout.Space(3);

        GUILayout.Label("Path");
        generator.tile[1] = EditorGUILayout.ObjectField(generator.tile[1], typeof(Tile), true) as Tile;
        GUILayout.Space(3);

        GUILayout.Label("Wall");
        generator.tile[2] = EditorGUILayout.ObjectField(generator.tile[2], typeof(Tile), true) as Tile;
        GUILayout.Space(3);

        GUILayout.Label("Start Point");
        generator.tile[3] = EditorGUILayout.ObjectField(generator.tile[3], typeof(Tile), true) as Tile;
        GUILayout.Space(3);

        GUILayout.Label("End Point");
        generator.tile[4] = EditorGUILayout.ObjectField(generator.tile[4], typeof(Tile), true) as Tile;
        GUILayout.Space(3);

        //EditorGUILayout.EndHorizontal();
        GUILayout.Space(8);

        if (GUILayout.Button("Generate Map"))
        {
            generator.wallMap.ClearAllTiles();
            generator.pathMap.ClearAllTiles();
            generator.mainCamera.SetActive(true);
            generator.player.SetActive(false);
            generator.GenerateCave();
        }

        if(GUILayout.Button("Clear"))
        {
            generator.wallMap.ClearAllTiles();
            generator.pathMap.ClearAllTiles();
            generator.mainCamera.SetActive(true);
            generator.player.SetActive(false);
        }
        GUILayout.Space(8);

        //Player
        GUILayout.Label("Player Object");
        generator.player = EditorGUILayout.ObjectField("Player", generator.player, typeof(GameObject), true) as GameObject;
        GUILayout.Space(8);

        if (GUILayout.Button("Create Player"))
        {
            generator.CreatePlayer();
        }
    }
}
