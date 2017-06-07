using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour {

	private Animator anim;
	private int speed = 5;

	void Start() {
		anim = GetComponent<Animator>();
	}

	void Update () {
		if (Input.GetKey(KeyCode.Space)) {
			anim.Play("Jump");
		} else if (Input.GetKey(KeyCode.C)) {
			anim.Play("Sit");
		} else if (Input.GetMouseButton(0)) {
			anim.Play("Hit");
		} else if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.D)) {
			if (Input.GetKey(KeyCode.LeftShift)) {
				anim.Play("Run");
				speed = 10;
			} else {
				anim.Play("Walk");
				speed = 5;
			}
		} else {
			anim.Play("Idle");
		}

		if (Input.GetKey (KeyCode.W)) {
			transform.Translate(Vector3.forward * Time.deltaTime*speed, Space.Self);
		}
		if (Input.GetKey (KeyCode.S)) {
			transform.Translate(Vector3.back * Time.deltaTime*speed, Space.Self);

		}
		if (Input.GetKey (KeyCode.A)) {
			transform.Translate(Vector3.left * Time.deltaTime*speed, Space.Self);

		}
		if (Input.GetKey (KeyCode.D)) {
			transform.Translate(Vector3.right * Time.deltaTime*speed, Space.Self);
		}
		
	}

	void FixedUpdate(){
	}
}