using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyController : MonoBehaviour {

    public BoxCollider2D boxColliderPath;
    public AnimationCurve curve;
    public string direction = "up";
    public Sprite enemyHorizontalSprite;

    private Vector3 orginalsize;
    private AnalyzeSound AnalyzeSoundKick;
    private float kickAnimationTime = -1;
    private float timeSinceGrounded = 0;
    
    private void Awake()
    {
        AnalyzeSoundKick = GameObject.Find("MusicOut Kick").GetComponent<AnalyzeSound>();
    }

    // Use this for initialization
    void Start () {
        
        if (direction == "right" || direction == "left")
        {
            transform.rotation *= Quaternion.Euler(0, 0, -90f);
            if(enemyHorizontalSprite != null) GetComponent<SpriteRenderer>().sprite = enemyHorizontalSprite;
        }
        orginalsize = transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {

		var animationSpeedIncrease = 1 + curve.Evaluate(kickAnimationTime) * 0.6f;

        if (direction == "up")
        {
            if (boxColliderPath.bounds.max.y > transform.position.y)
            {
                transform.position += new Vector3(0, 3, 0) * Time.deltaTime * animationSpeedIncrease;
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
                transform.position -= new Vector3(0, 3, 0) * Time.deltaTime * animationSpeedIncrease;
            }
            else
            {
                direction = "up";
                transform.localScale = new Vector3(orginalsize.x, orginalsize.y, 1);
            }
        }
        if (direction == "right")
        {
            if (boxColliderPath.bounds.max.x > transform.position.x)
            {
                transform.position += new Vector3(3, 0, 0) * Time.deltaTime * animationSpeedIncrease;
            }
            else
            {
                direction = "left";
                //transform.localScale = new Vector3(-orginalsize.x, -orginalsize.y, 1);
                transform.rotation *= Quaternion.Euler(0, 0, 180f);
            }
        }
        else if (direction == "left")
        {
            if (boxColliderPath.bounds.min.x < transform.position.x)
            {
                transform.position -= new Vector3(3, 0, 0) * Time.deltaTime * animationSpeedIncrease;
            }
            else
            {
                direction = "right";
                transform.rotation *= Quaternion.Euler(0, 0, -180f);
                //transform.localScale = new Vector3(-orginalsize.x, orginalsize.y, 1);
            }
        }

        KickAnimation();

    }

    private void KickAnimation() {
	    var sign = direction == "up" ? 1 : -1;

        if (kickAnimationTime >= 0)
        {
            transform.localScale = orginalsize * sign + Vector3.one * curve.Evaluate(kickAnimationTime) * 0.13f;

            kickAnimationTime += Time.deltaTime;

            if (kickAnimationTime > curve.keys.Last().time)
            {
                kickAnimationTime = -1;
            }
            return;
        }

        if (AnalyzeSoundKick != null && AnalyzeSoundKick.PitchValue > 50)
        {
            kickAnimationTime = 0;
            transform.localScale = orginalsize * sign;
        }
    }
}
