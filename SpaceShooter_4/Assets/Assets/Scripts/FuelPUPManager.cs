/***********************************************************************************
* FuelPUPManager.cs controls the actiona and dispositoin of the Fuel Picup object
* when encounterd by the Player object.  Fuel value controlled from Inspector panel.
************************************************************************************/
using UnityEngine;

public class FuelPUPManager : MonoBehaviour {

    private GameController gameController;
    public GameObject explosion;
    public int fuelValue;

    // Control the correct gameController at each level.
    void Start() {
         GameObject gameControllerObject = GameObject.FindWithTag("GameController");
         if (gameControllerObject != null) {
             gameController = gameControllerObject.GetComponent<GameController>();
         }
         if (gameController == null) {
             Debug.Log("Cannot find 'GameController', script.");
         }
     }
     
     // Triggers the desired action when the applied object's collider is entered.
     void OnTriggerEnter(Collider other) {
          if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Boss")
             || other.CompareTag("Boss_1") || other.CompareTag("Boss_2") || other.CompareTag("Boss_3")
             || other.CompareTag("Boss_4") || other.CompareTag("Boss_5") || other.CompareTag("Boss_6")
             || other.CompareTag("PlayerBolt") || other.CompareTag("Untagged")) {
               return;
          }

          // Give feedback that the pickup was made.
          if (explosion != null) {
               Instantiate(explosion, transform.position, transform.rotation);
          }
          // Add the fuel to the applied game object (Player)
          if (other.CompareTag("Player")) {
               gameController.AddFuel(fuelValue);
               Destroy(gameObject);
          }
     }

}
