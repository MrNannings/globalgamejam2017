using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour {

	public GameObject CollectablePrefab;

	// Use this for initialization
	void Start () {
		var collectables = GameObject.FindGameObjectsWithTag("keycube");

		foreach (var collectable in collectables) {
			var obj = Instantiate(CollectablePrefab);
			obj.transform.position = collectable.GetComponent<BoxCollider2D>().bounds.center + Vector3.back;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
