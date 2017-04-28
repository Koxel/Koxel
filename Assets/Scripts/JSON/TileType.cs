﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileType {
    public string name;
    public string Hname;
    public Color defaultColor;
    public Color hoverColor;
    public float moveCost;

    public TileType(string Hname, string name, Color defaultColor, Color hoverColor, float moveCost)
    {
        this.Hname = Hname;
        this.name = name;
        this.defaultColor = defaultColor;
        this.hoverColor = hoverColor;
        this.moveCost = moveCost;
    }
}
