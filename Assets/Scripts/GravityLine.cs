using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalGameJam2017 {

	public class GravityLine : MonoBehaviour {

		public static GravityLine Instance { get; private set; }

		void Awake () {
			if (Instance != null) {
				Debug.LogError("GravityLine instance already set");
			}

			Instance = this;
		}

		// Use this for initialization
		void Start () {}

		// Update is called once per frame
		void Update () {}

		public float GetValue (Vector2 pos) {
			return SinusWave.Instance.GetValue(pos) < pos.y ? 1 : -1;

			return Mathf.Sign(pos.y / 10);
		}

	}

}