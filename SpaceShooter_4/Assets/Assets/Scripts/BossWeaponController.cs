/******************************************************************************************
* BossWeaponController.cs is the auto firing controller for Enemy Boss ships.  Attach to
* any game asset that requires auto shooting capability and set the various parmaters in
* the Inspector.  Upto 6 spawn locations provided, and null on any asset is acceptable.
*******************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossWeaponController : MonoBehaviour {

     private AudioSource audioSource;
     private float fireRate;
     private int sceneID;

     // The type of weapon to be fired.
     public GameObject shot_1;
     public GameObject shot_2;
     public GameObject shot_3;
     public GameObject shot_4;
     public GameObject shot_5;
     public GameObject shot_6;

     // Where to spawn the shot from.
     public Transform shotSpawn_1;
     public Transform shotSpawn_2;
     public Transform shotSpawn_3;
     public Transform shotSpawn_4;
     public Transform shotSpawn_5;
     public Transform shotSpawn_6;

     public float delay;

     // Set the firing rates by level (sceneID) on Start.
     void Start() {
          audioSource = GetComponent<AudioSource>();
          sceneID = SceneManager.GetActiveScene().buildIndex;
          if (sceneID == 0)
               fireRate = 1.0f;
          else if (sceneID == 1)
               fireRate = 1.8f;
          else if (sceneID == 2)
               fireRate = 1.6f;
          else if (sceneID == 3)
               fireRate = 1.6f;
          else if (sceneID == 4)
               fireRate = 1.4f;
          else if (sceneID == 5)
               fireRate = 1.4f;
          InvokeRepeating("Fire", delay, fireRate);
     }

     // Firing routines by level with audio calls.  Null in Inspector acceptable.
     void Fire() {
          if (shot_1 != null) {
               Instantiate(shot_1, shotSpawn_1.position, shotSpawn_1.rotation);
               audioSource.Play();
          }
          if (shot_2 != null) {
               Instantiate(shot_2, shotSpawn_2.position, shotSpawn_2.rotation);
               audioSource.Play();
          }
          if (shot_3 != null) {
               Instantiate(shot_3, shotSpawn_3.position, shotSpawn_3.rotation);
               audioSource.Play();
          }
          if (shot_4 != null) {
               Instantiate(shot_4, shotSpawn_4.position, shotSpawn_4.rotation);
               audioSource.Play();
          }
          if (shot_5 != null) {
               Instantiate(shot_5, shotSpawn_5.position, shotSpawn_5.rotation);
               audioSource.Play();
          }
          if (shot_6 != null) {
               Instantiate(shot_6, shotSpawn_6.position, shotSpawn_6.rotation);
               audioSource.Play();
          }
     }

}
