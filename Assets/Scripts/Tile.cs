using UnityEngine;
using System.Collections.Generic;

public class Tile
{
    public string name;
    public string type;
    public Color defaultRGB;
    public Color hoverRGB;

    public List<Tile> neighbours;
    public int x;
    public int y;

    public Tile()
    {
        neighbours = new List<Tile>();
    }

    public float DistanceTo(Tile otherTile)
    {
        return Vector2.Distance(
                new Vector2(x, y),
                new Vector2(otherTile.x, otherTile.y)
            );
    }
}
