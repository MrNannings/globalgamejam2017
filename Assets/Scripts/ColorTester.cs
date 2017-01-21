using System.Linq;
using UnityEngine;
using System.Collections.Generic;

namespace GlobalGameJam2017 {

	[RequireComponent (typeof(SpriteRenderer), typeof(Rigidbody2D))]
	public class ColorTester : MonoBehaviour {

		public float speed;
		public AnimationCurve curve;
		public Transform child;
        public List<SinusWaveNode> sinusWaveNodesList;
        public float updateInterval = 0.5F; //time between nodes
        private double lastInterval;

        public GameObject touchParticle;

        private bool grounded;
		private Vector2 lastGroundedPosition;
		private Vector3 originalScale;
		private float kickAnimationTime = -1;

		private new Rigidbody2D rigidbody;
		private SpriteRenderer spriteRenderer;   

        private void Awake () {
			rigidbody = GetComponent<Rigidbody2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();

			originalScale = child.localScale;
		}

		private void Start () {
            sinusWaveNodesList = new List<SinusWaveNode>();
            ScoreManager.Instance.Init();
        }

		private void Update () {
			var force = Vector2.zero;

			var gravity = GravityLine.Instance.GetValue(transform.position);

			if (grounded) {
				//				rigidbody.position += new Vector2(Input.GetAxis("Horizontal"), 0) * Time.deltaTime;

				force += new Vector2(Input.GetAxis("Horizontal"), 0) * Time.deltaTime * speed;
				force += new Vector2(0, Input.GetButtonDown("Jump") ? 160 * gravity : 0);

				gravity = 0;
			}
			else {
				force += new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * speed, 0);
				//				force += new Vector2(0, Input.GetButton("Jump") ? 10 * -gravity : 0);

				if (SinusWave.Instance.Distance(transform.position) < 0.3f && Input.GetKey(KeyCode.E)) {
					
				}
			}


			spriteRenderer.color = Color.Lerp(Color.green, Color.red, (gravity + 1f) / 2);

			rigidbody.gravityScale = (gravity);
			rigidbody.AddForce(force);

			KickAnimation();

            CheckTouchWave();

        }

		private void OnCollisionEnter2D (Collision2D collision) {
			var gravity = GravityLine.Instance.GetValue(transform.position);

			var direction = (Vector2.up * gravity).normalized;
			var otherDirection = (transform.position - collision.transform.position).normalized;

			var onTop = Vector2.Dot(direction, otherDirection) > 0;

			Debug.Log(onTop ? "top" : "bottom");

			if (onTop) {
				lastGroundedPosition = transform.position;
				grounded = true;
			}
			else {
				transform.position = lastGroundedPosition;
				rigidbody.velocity = Vector2.zero;
			}
		}

		private void OnCollisionExit2D (Collision2D collision) {
			if (grounded) {}

			grounded = false;
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

			if (AnalyzeSound.Instance != null && AnalyzeSound.Instance.PitchValue > 50) {
				kickAnimationTime = 0;
				child.localScale = originalScale;
			}
		}


        public void CheckTouchWave()
        {
            var positionOnSinus = new Vector3(transform.position.x, SinusWave.Instance.GetValue(transform.position));

            if (Vector3.Distance(transform.position, positionOnSinus) < 0.5f)
            {
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