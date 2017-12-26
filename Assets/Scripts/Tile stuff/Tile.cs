﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    public Vector3 coords;
    public Chunk chunk;

    public string biome;
    public string tileType;

    private MaterialPropertyBlock _propBlock;
    private Renderer _renderer;

    private void Awake()
    {
        _propBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();

        biome = "hi";
        tileType = "cutie";
    }

    public void SetColor(Color color)
    {
        float random = 0f;//Random.Range(-0.05f, 0.05f);
        Color newColor = new Color(color.r + random, color.g + random, color.b + random, color.a);
        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", newColor);
        _renderer.SetPropertyBlock(_propBlock);
    }
}
