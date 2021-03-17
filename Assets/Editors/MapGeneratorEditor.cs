using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGeneratorScr))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGeneratorScr mapGen = (MapGeneratorScr) target;

        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            mapGen.GenerateMap();
        }
    }
}
