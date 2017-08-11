/* DestroyBycontact.cs used by Asteroids, Enmey Bolts, and Enemy ships (ramming works!) to 
 * destroy the Player game Object. */
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class DestroyByContact : MonoBehaviour {

     public GameObject explosion;
     public GameObject playerExplosion;
     public int scoreValue;
     public int takeDamage;

     private GameController gameController;


     private void Start() {
          GameObject gameControllerObject = GameObject.FindWithTag("GameController");
          if (gameControllerObject != null) {
               gameController = gameControllerObject.GetComponent<GameController>();
          }
          if (gameController == null) {
               Debug.Log("Cannot find 'GameController' script.");
          }

     }


     void OnTriggerEnter(Collider other) {
          if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Boss") 
              || other.CompareTag("Boss_1") || other.CompareTag("Boss_2") || other.CompareTag("Boss_3")
              || other.CompareTag("Boss_4") || other.CompareTag("Boss_5") || other.CompareTag("Boss_6")
              || other.CompareTag("ShieldPUP") ) {
                    return;
          }

          takeDamage--;

          if (takeDamage <= 0) {
               if (explosion != null) {
                    Instantiate(explosion, transform.position, transform.rotation);
               }

               if (other.CompareTag("Player")) {
                    Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    GameState.playerDestroyed++;
                    gameController.GameOver();
               }

               gameController.AddScore(scoreValue);

               if (gameObject.CompareTag("Shield") == false)
                    Destroy(gameObject);

               //explodes the Player Object if it runs into the Boss
               else if (takeDamage > 0 && other.CompareTag("Player"))
                {
                    Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
                    GameState.playerDestroyed++;
                    gameController.GameOver();
                }
               else
                    gameObject.SetActive(false);
          }

          if (other.gameObject.CompareTag("Shield") == false)
               Destroy(other.gameObject);
     }

}

/* CITATIONS:
[1] https://stackoverflow.com/questions/13725619/cant-catch-nullreferenceexception & elected not to use...
[2] https://gamedev.stackexchange.com/questions/136674/nullreferenceexception-in-unity
*/
