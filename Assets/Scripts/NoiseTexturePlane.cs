using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;

public class NoiseTexturePlane : MonoBehaviour {

    public int width = 50;
    public int height = 50;
    public float scale = 20f;
    public long seed = 13;

    Simplex simplex;

    //Edit this.
    float Noise(int xx, int yy)
    {
        float x = (float)xx / width * scale;
        float y = (float)yy / width * scale;

        float noise = 0;
        noise += (float)simplex.Evaluate(x, y);
        //noise = Mathf.PerlinNoise(x, y);
        ///Noise may only range from 0 to 1.
        ///Todo: limit it :P
        ///for now just divide back and stuff
        return noise;
    }


    
	void Start () {
        simplex = new Simplex(seed);

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
	}
	
	Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color = NoiseColor(x, y);
                texture.SetPixel(x, y, color); ;
            }
        }
        texture.Apply();
        return texture;
    }

    Color NoiseColor(int x, int y)
    {
        float value = Noise(x, y);
        return new Color(value, value, value);
    }
}
