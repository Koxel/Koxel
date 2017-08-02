using System;
using UnityEngine;

namespace Koxel
{
    public class HexData
    {
        private float hexSize;
        private float hexHeight;
        private float hexWidth;

        public HexData(float hexSize)
        {
            this.hexSize = hexSize;

            hexHeight = 2f;
            hexWidth = Mathf.Sqrt(3) / 2 * hexHeight;

            hexHeight *= this.hexSize; hexWidth *= this.hexSize;
        }

        public float Size()
        {
            return this.hexSize;
        }

        public float Height()
        {
            return this.hexHeight;
        }

        public float Width()
        {
            return this.hexWidth;
        }
    }

    public class HexCalc
    {
        public Vector2 CubeToOddR(Vector3 cube)
        {
            int col = (int)(cube.x + (int)(cube.y - (Math.Abs(cube.y) % 2)) / 2);
            int row = (int)cube.y;
            return new Vector2(col, row);
        }

        public Vector3 OddRToCube(Vector2 hex)
        {
            float x = hex.x - (hex.y - (Math.Abs(hex.y) % 2)) / 2;
            float z = hex.y;
            float y = -x - z;
            return new Vector3(x, y, z);
        }
    }
}
