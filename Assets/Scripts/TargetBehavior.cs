using UnityEngine;
using System.Collections;

public class TargetBehavior : MonoBehaviour
{

	// target impact on game
	public int scoreAmount = 0;
	public float timeAmount = 0.0f;
	public bool is_vibrate = false;

	// explosion when hit?
	public GameObject explosionPrefab;

	// when collided with another gameObject
	void OnCollisionEnter (Collision newCollision)
	{
		// exit if there is a game manager and the game is over
		if (GameManager.gm) {
			if (!GameManager.gm.is_play)
				return;
		}

		// only do stuff if hit by a projectile
		if (newCollision.gameObject.tag == "Projectile") {
			if (explosionPrefab) {
				// Instantiate an explosion effect at the gameObjects position and rotation
				GameObject obj_explosion=Instantiate (explosionPrefab, transform.position, transform.rotation);
				if (GameObject.Find("Game Manager").GetComponent<GameManager>().carrot.get_status_sound())
					obj_explosion.GetComponent<AudioSource>().enabled = true;
				else
					obj_explosion.GetComponent<AudioSource>().enabled = false;
			}

			// if game manager exists, make adjustments based on target properties
			if (GameManager.gm) {
				GameManager.gm.targetHit (scoreAmount, timeAmount);
				if (this.is_vibrate) GameManager.gm.play_vibrate();
			}
				
			Destroy (newCollision.gameObject);
			Destroy (gameObject);
		}
	}
}
