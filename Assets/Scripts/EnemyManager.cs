using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	public GameObject EnemyPrefab;

	// Use this for initialization
	void Start () {
		var horizontalObstacles = GameObject.FindGameObjectsWithTag("horizontalobstacle");
		var verticalObstacles = GameObject.FindGameObjectsWithTag("verticalobstacle");

		foreach (var obstacle in horizontalObstacles) {
			Create(obstacle, "right");
		}
		foreach (var obstacle in verticalObstacles) {
			Create(obstacle, "up");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Create (GameObject obstacle, string direction) {
		var boxCollider = obstacle.GetComponent<BoxCollider2D>();

		var enemy = Instantiate(EnemyPrefab).transform;
		enemy.position = boxCollider.bounds.center;

		var controller = enemy.GetComponent<EnemyController>();
		controller.direction = direction;
		controller.boxColliderPath = boxCollider;
	}
}
