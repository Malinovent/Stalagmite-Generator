using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator
{
    public int octaves;         // Can also be called layers. Increasing this increases the level of detail in the terrain
    public float gain;          // Determines how fast the amplitude changes for each octave. Can also be called persistence
    public float lacunarity;    // Determines how fast the frequency changes for each octave
    public float perlinScale;

    public NoiseGenerator()
    { 
    
    }

    public NoiseGenerator(int octaves, float gain, float lacunarity, float perlinScale)
    {
        this.octaves = octaves;
        this.gain = gain;
        this.lacunarity = lacunarity;
        this.perlinScale = perlinScale;
    }

    public float GetValueNoise()
    {
        return Random.value;
    }

    public float GetPerlinNoise(float x, float z)
    {
        return (2 * Mathf.PerlinNoise(x, z) - 1);
    }

    public float GetFractalNoise(float x, float z)
    {
        float fractalNoise = 0;

        float frequency = 1;
        float amplitude = 1;

        for (int i = 0; i < octaves; i++)
        {
            float xVal = x * frequency * perlinScale;
            float zVal = z * frequency * perlinScale;

            fractalNoise += amplitude * GetPerlinNoise(xVal, zVal);

            amplitude *= gain;
            frequency *= lacunarity;
        }



        return fractalNoise;
    }


}
