/**************************************************************************************************
* PUPManager.cs controls the actiona and dispositoin of the shiled Pickup object
* when encounterd by the Player object.  Shield time value controlled from Inspector.
***************************************************************************************************/
using UnityEngine;

public class PUPManager : MonoBehaviour {

     private GameController gameController;
     public GameObject explosion;
     public int scoreValue;
     public int timeIncrement;

     // Control the correct gameController at each level.
     private void Start() {
          GameObject gameControllerObject = GameObject.FindWithTag("GameController");
          if (gameControllerObject != null) {
               gameController = gameControllerObject.GetComponent<GameController>();
          }
          if (gameController == null) {
               Debug.Log("Cannot find 'GameController' script.");
          }

     }

     // Triggers the desired action when the applied object's collider is entered.
     void OnTriggerEnter(Collider other) {
          if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Boss")
              || other.CompareTag("Boss_1") || other.CompareTag("Boss_2") || other.CompareTag("Boss_3")
              || other.CompareTag("Boss_4") || other.CompareTag("Boss_5") || other.CompareTag("Boss_6")
              || other.CompareTag("PlayerBolt") ) {
               return;
          }

          // Give feedback that the pickup was made.
          if (explosion != null) {
               Instantiate(explosion, transform.position, transform.rotation);
          }

          // Add the Shiled time to the applied game object (Player). Time controllable from Inspector.
          if (other.CompareTag("Player")) {
               GameState.timeToShieldDown += timeIncrement;
               Destroy(gameObject);
          }

          gameController.AddScore(scoreValue);
     }

}
