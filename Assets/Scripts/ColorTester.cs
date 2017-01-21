using System.Linq;
using UnityEngine;

namespace GlobalGameJam2017 {

	[RequireComponent (typeof(SpriteRenderer), typeof(Rigidbody2D))]
	public class ColorTester : MonoBehaviour {

		public float speed;
		public AnimationCurve curve;
		public Transform child;

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

		private void Start () {}

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
			}


			spriteRenderer.color = Color.Lerp(Color.green, Color.red, (gravity + 1f) / 2);

			rigidbody.gravityScale = (gravity);
			rigidbody.AddForce(force);

			KickAnimation();
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

	}

}