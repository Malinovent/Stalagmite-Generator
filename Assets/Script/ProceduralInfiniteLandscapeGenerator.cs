using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralInfiniteLandscapeGenerator : MonoBehaviour
{
    [SerializeField] private PropGenChunk LandscapePrefab;

    [SerializeField] private Material material;
    
    [SerializeField] private int xResolution = 20;
    [SerializeField] private int zResolution = 20;

    [SerializeField] private float meshScale = 1;
    [SerializeField] private float yScale = 1;


    [SerializeField, Range(1, 8)] private int octaves = 1;
    [SerializeField] private float lacunarity = 2;
    [SerializeField, Range(0, 1)] private float gain = 0.5f;
    [SerializeField] private float perlinScale = 1;

    private void Awake()
    {
        PropGenChunk topLeft = CreateTerrainChunk(new Vector2(0, 2 * zResolution));
        PropGenChunk topMiddle = CreateTerrainChunk(new Vector2(xResolution, 2 * zResolution));
        PropGenChunk topRight = CreateTerrainChunk(new Vector2(2*xResolution, 2 * zResolution));

        PropGenChunk middleLeft = CreateTerrainChunk(new Vector2(0, zResolution));
        PropGenChunk middle = CreateTerrainChunk(new Vector2(xResolution, zResolution));
        PropGenChunk middleRight = CreateTerrainChunk(new Vector2(2*xResolution, zResolution));

        PropGenChunk bottomLeft = CreateTerrainChunk(new Vector2(0, 0));
        PropGenChunk bottomMiddle = CreateTerrainChunk(new Vector2(xResolution, 0));
        PropGenChunk bottomRigjt = CreateTerrainChunk(new Vector2(2*xResolution, 0));
    }

    private PropGenChunk CreateTerrainChunk(Vector2 position)
    {
        PropGenChunk chunk = Instantiate(LandscapePrefab);
        chunk.InitInfiniteLandscape(material, xResolution, zResolution, meshScale, yScale, octaves, lacunarity, gain, perlinScale, position);
        chunk.transform.SetParent(transform);

        return chunk;
    }
}
