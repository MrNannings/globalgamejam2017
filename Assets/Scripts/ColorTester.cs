using UnityEngine;

namespace GlobalGameJam2017 {

	[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
	public class ColorTester : MonoBehaviour {

		private new Rigidbody2D rigidbody;
		private SpriteRenderer spriteRenderer;

		private void Awake () {
			rigidbody = GetComponent<Rigidbody2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void Start () {
			
		}

		private void Update () {
//			transform.position += new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * 4, 0);

//			transform.position -= new Vector3(0, GravityLine.Instance.GetValue(transform.position) * Time.deltaTime * 8);

			var gravity = GravityLine.Instance.GetValue(transform.position);

			spriteRenderer.color = Color.Lerp(Color.green, Color.red, (gravity + 1f) / 2);

			rigidbody.gravityScale = (gravity);
			rigidbody.AddForce(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * 50, Input.GetButtonUp("Jump") ? 50 : 0));
		}

		private void OnCollision2D () {
			
		}

	}

}