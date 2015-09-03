using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System;

public class LoadWorld : MonoBehaviour {

	public void startStage(){
		Application.LoadLevel("Learning");
	}

	public void loadWorld() {

		string vectorSave = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\SandBox\\Worlds\\myVectors.werld";
		string tileSave = vectorSave.Replace ("myVectors.werld", "myTiles.werld");

		if (!File.Exists (vectorSave)) {
			Debug.Log("World does not exist");
			return;
		}

		try {
			Stream stream = new FileStream(vectorSave, FileMode.Open);
			Stream stream2 = new FileStream(tileSave, FileMode.Open);

			var serializer = new XmlSerializer(typeof(Vector3[]));
			Vector3[] vecs = (Vector3[]) serializer.Deserialize(stream);

			var ser = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			string[] tiles = (string[]) ser.Deserialize(stream2);

			CreateMap.map = new Hashtable();
			for(int i =0; i < vecs.Length; i++) {
				CreateMap.map.Add(vecs[i], tiles[i]);
			}

		} catch (Exception ioe) {
			Debug.Log (ioe.Message);
			Debug.Log (ioe.StackTrace);
		}

		startStage();
	}

	public void saveWorld() {
		string vectorSave = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\SandBox\\Worlds\\myVectors.werld";
		string tileSave = vectorSave.Replace ("myVectors.werld", "myTiles.werld");
		Stream stream, stream2;
		if (!File.Exists (vectorSave)) {
			if(!Directory.Exists(vectorSave.Replace("myVectors.werld","")))
				Directory.CreateDirectory(vectorSave.Replace("myVectors.werld",""));

			stream = File.Create (vectorSave);
			stream2 = File.Create (tileSave);


		} else {
			File.Delete(vectorSave);
			File.Delete(tileSave);
			stream = new FileStream(vectorSave, FileMode.Create);
			stream2 = new FileStream(vectorSave, FileMode.Create);
		}

		try {

			// Creating arrays from hashtable
			Vector3[] vecs = new Vector3[CreateMap.map.Keys.Count];
			CreateMap.map.Keys.CopyTo(vecs, 0);

			string[] tiles = new string[vecs.Length];
			for(int i =0; i < vecs.Length; i++) {
				System.Object obj = (string) CreateMap.map[vecs[i]];
				if(obj != null && obj.GetType() == typeof(string))
					tiles[i] = (string) obj;
				else
					tiles[i] = "";
			}

			// Writing vectors to file
			var serializer = new XmlSerializer(typeof(Vector3[]));
			serializer.Serialize(stream, vecs);

			// Writing tiles to file
			var ser = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			ser.Serialize(stream2, tiles);


		} catch (Exception ioe) {
			Debug.Log (ioe.Message);
		} finally {
			stream.Close();
			stream2.Close();
		}
	}

	public void openSettings() {
		
	}
}
