using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace GlobalGameJam2017 {

	[RequireComponent (typeof(SpriteRenderer), typeof(Rigidbody2D))]
	public class ColorTester : MonoBehaviour {

		public bool debug = true;
		public float jumpStrength = 3;
		public float speed;
		public Vector2 raycastTarget = new Vector2(0.2f, 0.3f);
		public AnimationCurve curve;
		public Transform child;
        public List<SinusWaveNode> sinusWaveNodesList;
        public float updateInterval = 0.5F; //time between nodes
        private double lastInterval;
        public Animator playerAnimator;
        public Animator shadowAnimator;

        public GameObject touchParticle;

        private bool grounded;
		private bool jumped;
		private Vector2 lastGroundedPosition;
		private Vector3 originalScale;
		private float kickAnimationTime = -1;
		private float timeSinceGrounded = 0;

		private new Rigidbody2D rigidbody;
		private SpriteRenderer spriteRenderer; 
        
        private AnalyzeSound AnalyzeSoundKick;
        private SoundsController soundsController;  

        private void Awake () {
			rigidbody = GetComponent<Rigidbody2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();

			originalScale = child.localScale;

            AnalyzeSoundKick = GameObject.Find("MusicOut Kick").GetComponent<AnalyzeSound>();
            soundsController = GameObject.Find("Sounds Controller").GetComponent<SoundsController>();
        }

		private void Start () {
            sinusWaveNodesList = new List<SinusWaveNode>();
            ScoreManager.Instance.Init();
        }

		private void Update () {
			if (CheckHittingPlatform()) {
				if (!grounded) {
					soundsController.PlaySound(3);
				}

				lastGroundedPosition = transform.position;
				grounded = true;
			}
			else {
				grounded = false;
			}

			var force = Vector2.zero;

			var gravity = GravityLine.Instance.GetValue(transform.position);

			if (grounded) {
				rigidbody.velocity -= rigidbody.velocity * 8f * Time.deltaTime;

				force += new Vector2(Input.GetAxis("Horizontal"), 0) * Time.deltaTime * 700 * speed;

				gravity = 0;
				jumped = false;
				timeSinceGrounded = 0;
			}
			else {
				if (Input.GetAxis("Horizontal") == 0 && Mathf.Abs(rigidbody.velocity.x) > 0.01f) {
					force.x = -rigidbody.velocity.x * 1 * Time.deltaTime;
				}
				else {
					force += new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * 40 * speed, 0);
				}

				timeSinceGrounded += Time.deltaTime;
			}

			if (timeSinceGrounded < 0.18f) {
				if (Input.GetButtonDown("Jump") && timeSinceGrounded == 0) {
					force += new Vector2(0, 30 * jumpStrength * GravityLine.Instance.GetValue(transform.position));
					jumped = true;
                    soundsController.PlaySound(8);
				}
				else if (Input.GetButton("Jump") && jumped) {
					force += new Vector2(0, 200 * jumpStrength * GravityLine.Instance.GetValue(transform.position) * Time.deltaTime);
				}
			}

			force = force.normalized * Mathf.Max(force.magnitude, 3f);

			rigidbody.gravityScale = (gravity);
			rigidbody.AddForce(force);

			KickAnimation();

            CheckTouchWave();

            //vertical flip
            if (gravity != 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs( transform.localScale.y) * gravity, transform.localScale.z);           
            }
            //horizontal flip
			if (Mathf.Abs(rigidbody.velocity.x) > 0.002f) {
				transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * Mathf.Sign(rigidbody.velocity.x), transform.localScale.y, transform.localScale.z);
			}

			playerAnimator.SetBool("grounded", grounded);
            playerAnimator.SetFloat("velocityX", Mathf.Abs(rigidbody.velocity.x));
            playerAnimator.SetFloat("velocityY", rigidbody.velocity.y * gravity);
            //Debug.Log(rigidbody.velocity.y * gravity);

			shadowAnimator.SetBool("grounded", grounded);
            shadowAnimator.SetFloat("velocityX", Mathf.Abs(rigidbody.velocity.x));
            shadowAnimator.SetFloat("velocityY", rigidbody.velocity.y * gravity);
        }

		private bool CheckHittingPlatform (float direction = 1) {
			var gravity = GravityLine.Instance.GetValue(transform.position);
			var rayY = raycastTarget.y;

            //easy fix
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y) * gravity, transform.localScale.z);

            var onTop = false;

			var hit = Physics2D.Raycast(transform.position + new Vector3(raycastTarget.x, 0), Vector2.down * gravity * direction, rayY, LayerMask.GetMask("Platforms"));
			if (hit.collider != null) {
				onTop = true;
            }

			hit = Physics2D.Raycast(transform.position + new Vector3(-raycastTarget.x, 0), Vector2.down * gravity * direction, rayY, LayerMask.GetMask("Platforms"));
			if (hit.collider != null) {
				onTop = true;
            }

			return onTop;
		}

        private void GettingHit()
        {
            soundsController.PlaySound(14);
            transform.position = lastGroundedPosition;
            rigidbody.velocity = Vector2.zero;
        }

		private void OnCollisionEnter2D (Collision2D collision) {
			if (CheckHittingPlatform(-1) || !grounded && !collision.gameObject.CompareTag("Platform")) {
                GettingHit();
            }
		}
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                GettingHit();
            }
        }

        private void KickAnimation () {
			if (kickAnimationTime >= 0) {
				child.localScale = originalScale + Vector3.one * curve.Evaluate(kickAnimationTime) * 0.2f;

				kickAnimationTime += Time.deltaTime;

				if (kickAnimationTime > curve.keys.Last().time) {
					kickAnimationTime = -1;
				}
				return;
			}

			if (AnalyzeSoundKick != null && AnalyzeSoundKick.PitchValue > 50) {
				kickAnimationTime = 0;
				child.localScale = originalScale;
			}
		}


        public void CheckTouchWave()
        {
            var positionOnSinus = new Vector3(transform.position.x, SinusWave.Instance.GetValue(transform.position));

            if (Vector3.Distance(transform.position, positionOnSinus) < 0.5f)
            {
                ShowTouchParticle(positionOnSinus);
                soundsController.PlaySound(2);

                float timeNow = Time.realtimeSinceStartup;
                if (timeNow > lastInterval + updateInterval)
                {
                    lastInterval = timeNow;
                    sinusWaveNodesList.Add(new SinusWaveNode(sinusWaveNodesList.Count, 10, positionOnSinus));
                    if (sinusWaveNodesList.Count > 1)
                    {
                        ScoreManager.Instance.IncreaseScore(
                            ScoreManager.Instance.CalculateScoreBetweenNode(
                                sinusWaveNodesList[sinusWaveNodesList.Count - 1], sinusWaveNodesList[sinusWaveNodesList.Count - 2]
                            )
                        );
                    }
                }

            }
        }

        public void ShowTouchParticle(Vector3 position)
        {
            if(!touchParticle.GetComponent<ParticleSystem>().isPlaying)
            {
                touchParticle.GetComponent<ParticleSystem>().Clear();
                touchParticle.transform.position = position;
                touchParticle.GetComponent<ParticleSystem>().Play();
            }

        }

    }

}