using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject player;
    public TileBehaviour[,] tileMap;
    List<TileBehaviour> path;
    public TileBehaviour currentTile;
    int pathProgress = 0;

    // Use this for initialization
    void Start () {
        path = new List<TileBehaviour>();
	}
	
	// Update is called once per frame
	void Update () {
		if(path.Count > 0)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, path[pathProgress].transform.position, 0.5f);

            if (Vector3.Distance(player.transform.position, path[pathProgress].transform.position) == 0)
                NextTile();

            
        }
	}

    public void NextTile()
    {
        //gameObject.GetComponent<TileTicker>().TileTick(player.transform.position);
        Debug.Log("Next Tile");
        currentTile = path[pathProgress];
        if (pathProgress != path.Count-1)
            pathProgress = pathProgress + 1;
        else
            path = new List<TileBehaviour>();
    }

    public void PathTo(TileBehaviour goal)
    {
        path = FindPath(currentTile, goal);
        pathProgress = 0;
        Debug.Log("Start: " + currentTile.name);
        TileBehaviour prev = currentTile;
        foreach(TileBehaviour tb in path)
        {
            Debug.Log("next:" + tb.name);
            Vector3 start = new Vector3(prev.transform.position.x, prev.transform.position.y + 5f, prev.transform.position.z);
            Vector3 next = new Vector3(tb.transform.position.x, tb.transform.position.y + 5f, tb.transform.position.z);
            Debug.DrawLine(start, next, Color.red, 5f);
            prev = tb;
        }
    }

    protected List<TileBehaviour> FindPath(TileBehaviour origin, TileBehaviour goal)
    {
        AStar pathFinder = new AStar();
        pathFinder.FindPath(origin, goal, tileMap, false);
        return pathFinder.TileBehavioursFromPath();
    }
}
