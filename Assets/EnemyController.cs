using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public BoxCollider2D boxColliderPath;
    private string direction = "up";
    private Vector3 orginalsize;

	// Use this for initialization
	void Start () {
        orginalsize = transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {

        if (direction == "up")
        {
            if (boxColliderPath.bounds.max.y > transform.position.y)
            {
                transform.position += new Vector3(0, 3, 0) * Time.deltaTime;
            }
            else
            {
                direction = "down";
                transform.localScale = new Vector3(orginalsize.x, -orginalsize.y, 1);
            }
        }
        else if (direction == "down")
        {
            if (boxColliderPath.bounds.min.y < transform.position.y)
            {
                transform.position -= new Vector3(0, 3, 0) * Time.deltaTime;
            }
            else
            {
                direction = "up";
                transform.localScale = new Vector3(orginalsize.x, orginalsize.y, 1);
            }
        }

    }
}
