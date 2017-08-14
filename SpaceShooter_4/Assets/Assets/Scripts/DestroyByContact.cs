/**************************************************************************************************************************
* DestroyByContact.cs used by objects other than the Player to control their behaviour on being hit by a PlayerBolt tagged 
* wepon.  The bahaviour is modified when the Player has a shield enabled. 
***************************************************************************************************************************/
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

     public GameObject explosion;
     public GameObject playerExplosion;
     public int scoreValue;
     public int takeDamage;

     private GameController gameController;

     // Manage instance of the gameController.
     private void Start() {
          GameObject gameControllerObject = GameObject.FindWithTag("GameController");
          if (gameControllerObject != null) {
               gameController = gameControllerObject.GetComponent<GameController>();
          }
          if (gameController == null) {
               Debug.Log("Cannot find 'GameController' script.");
          }

     }

     // Manage what happens when two gameObjects collide.
     void OnTriggerEnter(Collider other) {

          // This group of objects, identified by tag, is ignored. (Player object DOES NOT have a DestroyByContact method applied!
          if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Boss") || other.CompareTag("Boss_1") ||
              other.CompareTag("Boss_2") || other.CompareTag("Boss_3") || other.CompareTag("Boss_4") || other.CompareTag("Boss_5") ||
              other.CompareTag("Boss_6") || other.CompareTag("Fuel") ) {
               return;
          }

          // Hide the PlayerBolts when multiple hits are required so they don't appear to "pass through".
          if (other.CompareTag("PlayerBolt")) {
               other.gameObject.SetActive(false);  // DestroyByBoundary manages destruction of PlayerBolts that miss target.
          }

          takeDamage--;

          // Object has been hit required number of times to be destroyed.
          if (takeDamage <= 0) {
               if (explosion != null) {
                    Instantiate(explosion, transform.position, transform.rotation);
               }
               // But if the Player ran into the enemy object, it's destroyed and the game ends...
               if (other.CompareTag("Player")) {
                    Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    other.gameObject.SetActive(false);
                    GameState.playerDestroyed++;
                    gameController.GameOver();
               }

               gameController.AddScore(scoreValue);

               // But if the object wasn't a Player and its anything other than a shield, then it that object is destroyed. 
               if (gameObject.CompareTag("Shield") == false)
                    Destroy(gameObject);
               //else
                 //   gameObject.SetActive(false);
          }

          // Explodes the Player Object if it runs into the Boss.
          else if (takeDamage > 0) {
               if (other.CompareTag("Player")) {
                    Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    Destroy(other.gameObject);
                    GameState.playerDestroyed++;
                    gameController.GameOver();
               }
               // Unless the shield was up! Then the object that ran into the shield is destryed if it has a takeDamage var.
               else if (other.gameObject.CompareTag("Shield")) {
                    takeDamage = 0;
               }
          }
     }

}

/* CITATIONS:
[1] https://stackoverflow.com/questions/13725619/cant-catch-nullreferenceexception & elected not to use...
[2] https://gamedev.stackexchange.com/questions/136674/nullreferenceexception-in-unity
*/
