using UnityEngine;
[RequireComponent(typeof(MapDisplayScr))]
public class MapGeneratorScr : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColorMap,
        Mesh
    }

    #region Public Fields
    [Header("Range")]
    public int mapWidth = 100;
    public int mapHeight = 100;
    public float noiseScale = 0.3f;
    [Space]

    [Header("Detail")]
    public int octaves = 4;
    [Range(0, 1)] public float persistance = 0.5f;
    public float lacunarity = 2f;
    [Space]

    [Header("Mesh")]
    public float meshHeightMultiplier = 100f;
    public AnimationCurve animationCurve;
    [Space]

    [Header("PRNG")]
    public int seed;
    public Vector2 offset;
    [Space]

    [Header("Region")]
    public DrawMode dMode;
    public TerrainType[] regions;
    [Space]

    [Header("Auto/Manual")]
    public bool autoUpdate = false;

    #endregion

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            } 
        }

        MapDisplayScr display = GetComponent<MapDisplayScr>();
        if (dMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }
        else if (dMode == DrawMode.ColorMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
        else if (dMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, animationCurve), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
        }
    }

    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
        if (noiseScale < 0.0001f)
        {
            noiseScale = 0.0001f;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
