﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls all the Player actions like jump, run, shoot
/// </summary>

public class Player : MonoBehaviour {
	// Player attributes
	// First original attributes from the Player class 
	private float horizontalSpeed;
	private float verticalSpeed = 600f;
	private int health;
	private int damage;
	private bool facingRight;
	private bool isFacingRight;
	private bool isGrounded;
	private bool canDoubleJump;
	private bool isJumping = false;
	private bool isTalking;
	private bool leftPressed;
	private bool rightPressed;

	//Second attributes from unity classes
	private Animator anim;
	private Rigidbody2D rd;
	private SpriteRenderer sr; // para voltear la orientacion del sprite y que pueda ver a la izquierda o a la derecha

	// Classes
	public Feet feet;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		rd = GetComponent<Rigidbody2D> ();
		sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		 * this line checks if the box created in the Feet class is ovelaped with 
		 * the layer Ground
		*/
		isGrounded = feet.IsOverlaping ();

		/*
			GetAxisRaw returns 1, 0 o -1 depending on the movement in an axis, in
			this case Horizontal axis. 
			So if the player is moving in the right, GetAxisRaw returns 1
			if the player is moving in the left, GetAxisRaw returns -1
			and if the player is not moving in, GetAxisRaw returns 0.

			5f is the true value of the velocity so if you want to make the player
			faster o slower change that value.
		*/ 
		horizontalSpeed = 5f * (Input.GetAxisRaw ("Horizontal"));
		if(horizontalSpeed != 0){
			moveHorizontal ();
		}else{
			stopMoving ();
		}

		if (Input.GetButtonDown ("Jump")) {
			jump ();
		}

		showFaling ();

		//Update the GUI

		if(leftPressed){
			horizontalSpeed = -5F;
			moveHorizontal ();
		}

		if(rightPressed){
			horizontalSpeed = 5F;
			moveHorizontal ();
		}
		//checkGrounded ();
	}	

	/*
		Stops the movement and avoid the on ice movement when the player is 
		stoping
	*/
	public void stopMoving(){
		rd.velocity = new Vector2(0f, rd.velocity.y);
		if (!isJumping) {
			anim.SetInteger ("state", 0);
		}
	}
		
	public void moveHorizontal(){
		//send the volocity to the player in order to move it.
		rd.velocity = new Vector2(horizontalSpeed, rd.velocity.y);

		//Change the orientation of the sprite 
		if(horizontalSpeed < 0){
			/*
				If the player is moving to the left the sprite is facing the lef
				otherwise is facing the right
			*/ 
			sr.flipX = false;
			isFacingRight = false;
		}else if (horizontalSpeed > 0){
			sr.flipX = true;
			isFacingRight = true;
		}

		/* 
		 * In case the player is not jumping the states machine
		 * will show the running state
		*/ 
		if (!isJumping) {
			anim.SetInteger ("state", 1);
		}
	}

	void showFaling(){
		/*
		 * If the vertical velocity is negative the Player must be
		 * falling so the states machine will set the falling state
		*/
		if (rd.velocity.y < 0) {
			anim.SetInteger ("state", 3);
		}
	}

	public void jump(){
		if(isGrounded){
			isJumping = true;
			rd.AddForce (new Vector2(0f, verticalSpeed));
			anim.SetInteger ("state", 2);
			/*Debug.Log ("salta");
			int estado = anim.GetInteger("state");
			Debug.Log (estado);*/
			canDoubleJump = true;
		}

		if (canDoubleJump && !isGrounded) {
			rd.AddForce (new Vector2(0f, verticalSpeed));
			anim.SetInteger ("state", 2);
			canDoubleJump = false;
		}
	}

	/*public void checkGrounded(){
		if (isGrounded) {
			isJumping = false;
			anim.SetInteger ("state", 0);
		}
	}*/

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.CompareTag ("Ground")) {
			isJumping = false;
			anim.SetInteger ("state", 0);	
		}
	}

	//Methods for GUI
	public void MobileMoveLeft (){
		leftPressed = true;
	}

	public void MobileMoveRight (){
		rightPressed = true;
	}

	public void MobileMoveStop (){
		leftPressed = false;
		rightPressed = false;
		stopMoving ();
	}

	public void MobileMoveJump (){
		jump ();
	}

}
