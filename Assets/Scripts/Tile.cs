using UnityEngine;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using LitJson;

public class Tile : MonoBehaviour {

	public string jsonFolders = "/JSON files/Events/";
	private string jsonString;
	public string[] randomEventsList;
	//public string[] ScheduledEventsList;

	public string biome;
	public string region;


	void Start () {
		// Only if there are events given
		if (randomEventsList.Length > 0) {
			ParseJSON ();
		}

		// Set the biome name if missing
		if (biome == "")
			biome = gameObject.name.Replace("Tile", "");
	}

	// Tile Tick is called from the Game manager, instead of Update()
	public void TileTick (){
		// Check if an event should run
		// add a list of possible events (activate function with: gameObject.SendMessage (name, args);)

		Debug.Log (name);
	}

	private void iParsedJSON (List<object> args){

	}


	// To parse JSON at beginning
	private void ParseJSON () {
		// Parse all given files
		for (int fileNr = 0; fileNr != randomEventsList.Length; fileNr++) {  
			// Create an object of the text
			jsonString = File.ReadAllText (Application.dataPath + jsonFolders + randomEventsList [fileNr] + ".json");
			JsonData itemData = JsonMapper.ToObject (jsonString)["EventData"];

			// Seperate the blocks
			for (int dataNr = 0; dataNr != itemData.Count; dataNr++) { 
				var name = (string) itemData [dataNr] ["Name"];

				// Create an empty list to store the attributes (every new data block)
				List<object> args = new List<object>();
				// Add name, biome(s) and player requirement
				// (always required)
				args.Add (itemData [dataNr] ["Name"]);
				args.Add (itemData [dataNr] ["Biome"]);
				args.Add (itemData [dataNr] ["Player"]);

				// Scroll through all the atributes the event has
				for (int attribute = args.Count+1; attribute != itemData [dataNr].Count; attribute++) {
					// Add them to the list
					args.Add (itemData [dataNr] [attribute]);
				}

				// Call the method with using the name and args
				iParsedJSON(args);
			}
		}
	}

	public List<Transform> FindConnectedTiles() {
		Transform parent = transform.parent;
		List<Transform> connectedTiles = new List<Transform>();

		foreach (Transform tile in parent.transform){
			//Debug.Log (tile.name + Vector3.Distance (transform.position, tile.position));
			if (tile != transform){
				if (Vector3.Distance (transform.position, tile.position) < 2f) { 
					connectedTiles.Add (tile);
				}
			}
		}
		return connectedTiles;
	}
}