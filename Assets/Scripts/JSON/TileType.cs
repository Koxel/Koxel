using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileType {
    public string name;
    public Color defaultRGB;
    public Color hoverRGB;

    public TileType(string name, Color defaultRGB, Color hoverRGB)
    {
        this.name = name;
        this.defaultRGB = defaultRGB;
        this.hoverRGB = hoverRGB;
    }
}
