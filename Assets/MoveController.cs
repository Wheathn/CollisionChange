using UnityEngine;
using UnityEngine.UI;
using TrueSync;
using System.Collections.Generic;


public class MoveController : TrueSyncBehaviour {
	
	private const byte INPUT_KEY_HORIZONTAL = 1;
	private const byte INPUT_KEY_JUMP = 2;
	private const byte INPUT_KEY_VERT = 3;

	FP speedX = 1.6f;
	bool grounded = false;
	TSCollider col;


	public override void OnSyncedStart(){
	}

	void Update () {
	}

	// Sets player inputs.
	public override void OnSyncedInput() {		
		int input = 0;
		if (Input.GetKey(KeyCode.A)) {
			input = -1;
		} else if (Input.GetKey(KeyCode.D)) {
			input = 1;
		}
		TrueSyncInput.SetInt(INPUT_KEY_HORIZONTAL, input);

		int jump = 0;
		if (Input.GetKey (KeyCode.Space)) {
			jump = 1;
		}
		TrueSyncInput.SetInt (INPUT_KEY_JUMP, jump);

		int vertical = 0;
		if (Input.GetKey (KeyCode.S)) {
			vertical = -1;
		}
		TrueSyncInput.SetInt (INPUT_KEY_VERT, vertical);
	}

	public override void OnSyncedUpdate ()
	{
		int directionInput = TrueSyncInput.GetInt (INPUT_KEY_HORIZONTAL);
		int jumpInput = TrueSyncInput.GetInt (INPUT_KEY_JUMP);
		int vertInput = TrueSyncInput.GetInt (INPUT_KEY_VERT);

		// Move
		if (directionInput < 0) {
			tsRigidBody.velocity = new TSVector (-speedX, tsRigidBody.velocity.y, 0);
		} else if (directionInput > 0) {
			tsRigidBody.velocity = new TSVector (speedX, tsRigidBody.velocity.y, 0);
		} else if (directionInput == 0) {
			tsRigidBody.velocity = new TSVector (0, tsRigidBody.velocity.y, 0);
		}

		// Start Jump from Ground
		if (jumpInput == 1 && grounded) {
			tsRigidBody.velocity = new TSVector (tsRigidBody.velocity.x, 9.5f, 0);
		}


		// Fall through platforms
		if (vertInput == -1 || jumpInput == 1) {
			tsCollider.isTrigger = true;
			print ("TRIGGERED, Input: "+ vertInput);
		}
		if (tsRigidBody.velocity.y < 1f && vertInput == 0){
			tsCollider.isTrigger = false;
			print ("not triggered, Input: "+ vertInput);
		}
	}

	public void OnSyncedCollisionStay(TSCollision col){
		if (col.gameObject.tag == "Ground")
			grounded = true;
	}
	public void OnSyncedCollisionExit(TSCollision col){
		if (col.gameObject.tag == "Ground")
			grounded = false;
	}
	public void OnSyncedTriggerExit(TSCollision col){
		if (col.gameObject.tag == "Ground")
			grounded = false;
	}

}