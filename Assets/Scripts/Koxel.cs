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
}
