using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour {

	public int tileX;
    public int tileY;
    public TileMap map;

    public List<Node> currentPath = null;

    void Update()
    {
        if(currentPath != null)
        {
            int currNode = 0;

            while(currNode < currentPath.Count - 1)
            {
                Vector3 begin = new Vector3();
                Vector3 end = new Vector3();

                if (currentPath[currNode].y % 2 == 0 || currentPath[currNode].y == 0) 
                    begin = new Vector3(currentPath[currNode].x * map.GetComponent<TileMap>().hexOffsetX,
                                        .5f, 
                                        currentPath[currNode].y * map.GetComponent<TileMap>().hexOffsetY);

                else if(currentPath[currNode].y % 2 == 1)
                    begin = new Vector3(
                                        currentPath[currNode].x * map.GetComponent<TileMap>().hexOffsetX + map.GetComponent<TileMap>().hexOddRowOffsetX,
                                        .5f, 
                                        currentPath[currNode].y * map.GetComponent<TileMap>().hexOffsetY
                                        );

                if (currentPath[currNode + 1].y % 2 == 0 || currentPath[currNode + 1].y == 0)
                    end =   new Vector3(currentPath[currNode + 1].x * map.GetComponent<TileMap>().hexOffsetX, 
                                        .5f, 
                                        currentPath[currNode + 1].y * map.GetComponent<TileMap>().hexOffsetY);

                else if (currentPath[currNode + 1].y % 2 == 1)
                    end =   new Vector3(currentPath[currNode + 1].x * map.GetComponent<TileMap>().hexOffsetX + map.GetComponent<TileMap>().hexOddRowOffsetX,
                                        .5f, 
                                        currentPath[currNode + 1].y * map.GetComponent<TileMap>().hexOffsetY);

                Debug.DrawLine(begin, end);
 
                currNode++;
            }
        }
    }

    public void MoveNextTile()
    {
        if (currentPath == null)
            return;

        currentPath.RemoveAt(0);

        //transform.position = new Vector3(currentPath[0].x * map.GetComponent<TileMap>().hexOffsetX, 0f, currentPath[0].y * map.GetComponent<TileMap>().hexOffsetY);

        if (currentPath[0].y % 2 == 0 || currentPath[0].y == 0)
            transform.position = new Vector3(currentPath[0].x * map.GetComponent<TileMap>().hexOffsetX,
                                0f,
                                currentPath[0].y * map.GetComponent<TileMap>().hexOffsetY);

        else if (currentPath[0].y % 2 == 1)
            transform.position = new Vector3(
                                currentPath[0].x * map.GetComponent<TileMap>().hexOffsetX + map.GetComponent<TileMap>().hexOddRowOffsetX,
                                0f,
                                currentPath[0].y * map.GetComponent<TileMap>().hexOffsetY
                                );

        tileX = currentPath[0].x;
        tileY = currentPath[0].y;

        if (currentPath.Count == 1)
        {
            currentPath = null;
        }
    }
}
