using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {
    public GameObject playerPrefab;
    public GameObject player;
    public Dictionary<Vector2, TileBehaviour> tileMap;
    public int radius;
    List<TileBehaviour> path;
    public TileBehaviour currentTile;
    int pathProgress = 0;
    public float playerSpeed = .5f;
    
    void Start () {
        path = new List<TileBehaviour>();
	}
	
	void Update () {
        // Move over the path
		if(path.Count > 0)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, path[pathProgress].transform.position, playerSpeed);

            if (Vector3.Distance(player.transform.position, path[pathProgress].transform.position) == 0)
                NextTile();
        }
	}

    // Pathfinding and such
    public void NextTile()
    {
        currentTile = path[pathProgress];
        if (pathProgress != path.Count-1)
            pathProgress = pathProgress + 1;
        else
            path = new List<TileBehaviour>();
    }

    public void PixelPath(TileBehaviour goal)
    {
        path = CreateAStarPath(currentTile, goal);
        pathProgress = 0;
    }

    public float HexDistance(TileBehaviour a, TileBehaviour b)
    {
        Vector3 cubeA = a.coordinates;
        Vector3 cubeB = b.coordinates;
        return Mathf.Max(Mathf.Abs(cubeA.x - cubeB.x), Mathf.Abs(cubeA.y - cubeB.y), Mathf.Abs(cubeA.z - cubeB.z));
    }

    public List<TileBehaviour> CreateAStarPath(TileBehaviour start, TileBehaviour goal)
    {
        PriorityQueue<TileBehaviour> frontier;
        Dictionary<TileBehaviour, TileBehaviour> came_from;
        Dictionary<TileBehaviour, double> cost_so_far;
        List<TileBehaviour> path;
        TileBehaviour current;

        frontier = new PriorityQueue<TileBehaviour>();
        frontier.Enqueue(start, 0);

        came_from = new Dictionary<TileBehaviour, TileBehaviour>();
        cost_so_far = new Dictionary<TileBehaviour, double>();
        
        came_from[start] = start;
        cost_so_far[start] = 0;
        
        while (frontier.Count > 0)
        {
            current = frontier.Dequeue();

            if (current == goal)
                break;

            foreach (TileBehaviour next in current.Neighbours())
            {
                var new_cost = cost_so_far[current] + current.moveCost;
                if (!cost_so_far.ContainsKey(next) || new_cost < cost_so_far[next])
                {
                    cost_so_far[next] = new_cost;

                    var heuristic = HexDistance(goal, next);
                    var priority = new_cost + heuristic;
                    frontier.Enqueue(next, priority);
                    came_from[next] = current;
                }
            }
        }

        current = goal;
        path = new List<TileBehaviour>();
        path.Add(current);
        while(current != start)
        {
            current = came_from[current];
            path.Add(current);
        }
        path.Add(start);
        path.Reverse();
        return path;
    }
}