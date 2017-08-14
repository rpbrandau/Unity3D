/*****************************************************************************************************
* ShieldManager.cs controls the application of a Forward shield on the Player object when a shield 
* Pickup game object is triggered by the Player (user has to overrun the shield game object).
******************************************************************************************************/
using UnityEngine;

public class ShieldManager : MonoBehaviour {

     public GameObject shield;
     public GameObject explosion;

     // No shield on start at each level
     void Start () {
          shield.SetActive(false);
          GameState.timeToShieldDown = 0;
	}

     /* Finer grained time to shield down at time expiration - works better at level change event as
        Player can still be destroyed on a dispatched shot after Boss destruction. */
     void FixedUpdate () {
          if (GameState.timeToShieldDown > 0)
               shield.SetActive(true);
          else
               shield.SetActive(false);

          if(GameState.timeToShieldDown > 0)
               GameState.timeToShieldDown -= Time.deltaTime;
	}

     /* Provide a visual indication to player when shield collider is hit by an enemy object or shot. 
        Actual destruction of enemy is accomplishe dvia the Enemy object's DestroyBycontact.cs script. */
     void OnTriggerEnter(Collider other) {
          if ( other.CompareTag("Enemy") || other.CompareTag("Boss_1") || other.CompareTag("Boss_2")
            || other.CompareTag("Boss_3") || other.CompareTag("Boss_4") || other.CompareTag("Boss_5") 
            || other.CompareTag("Boss_6")) {
               Instantiate(explosion, other.transform.position, other.transform.rotation);
          }
     }

}

/* CITATIONS:
[1] https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html
*/
