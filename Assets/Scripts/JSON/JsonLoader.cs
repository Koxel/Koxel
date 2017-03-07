using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class JsonLoader : MonoBehaviour {

    public List<Tile> Tiles = new List<Tile>();

	// Use this for initialization
	void Start () {
        // First setup a Tile object for every kind of tile defined in the Tile Json files
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Resources/Tiles");
        FileInfo[] info = dir.GetFiles("*.json");
        
        foreach (FileInfo file in info)
        {
            CreateNewTile(JsonMapper.ToObject(File.ReadAllText(file.FullName)));
        }

        // We're ready, start Generation
        GetComponent<WorldGenerator>().Generate(Tiles);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void CreateNewTile(JsonData jsonData)
    {
        Debug.Log(jsonData["name"]);
        string tileName = jsonData["name"].ToString();
        Color tileDefaultColor = new Color((int)jsonData["defaultRGB"][0], (int)jsonData["defaultRGB"][1], (int)jsonData["defaultRGB"][2], (int)jsonData["defaultRGB"][3]);
        Color tileHoverColor = new Color((int)jsonData["hoverRGB"][0], (int)jsonData["hoverRGB"][1], (int)jsonData["hoverRGB"][2], (int)jsonData["hoverRGB"][3]);

        Tiles.Add(
            new Tile
            {
                name = tileName,
                defaultRGB = tileDefaultColor,
                hoverRGB = tileHoverColor
            }
        );
    }
}
