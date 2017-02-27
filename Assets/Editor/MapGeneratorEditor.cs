using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                for (int i = 0; i < mapGen.gameObject.transform.childCount; i++)
                {
                    GameObject.DestroyImmediate(mapGen.gameObject.transform.GetChild(i).gameObject);
                }
                mapGen.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            for (int i = 0; i < mapGen.gameObject.transform.childCount; i++)
            {
                GameObject.Destroy(mapGen.gameObject.transform.GetChild(i).gameObject);
            }
            mapGen.GenerateMap();
        }
    }
}