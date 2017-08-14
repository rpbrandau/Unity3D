/**************************************************************************************
* PlayerController.cs is used by Player game Object to control boundaries, motion,
* and weapons instantiation.
***************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

// Instatiate the boundaries - dimensional paramters controlled from Inspector.
[System.Serializable]
public class Boundary {
     public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

     public Rigidbody rb;
     public float speed;
     public float tilt;
     public Boundary boundary;
     public GameObject shot;
     public float fireRate;
     public AudioSource ac;

     // As many as 7 weapon lacations available to prefab developer.
     public Transform shotSpawn;
     public Transform shotSpawn_1;
     public Transform shotSpawn_2;
     public Transform shotSpawn_3;
     public Transform shotSpawn_4;
     public Transform shotSpawn_5;
     public Transform shotSpawn_6;

     private float nextFire;

     void Start() {
          rb = GetComponent<Rigidbody>();
          ac = GetComponent<AudioSource>();
     }

     /* How the Player object is equipped with weapon types, rate, & spawn position @ each level.
        Consider null test and add all instatntiate weapons, though minimized for perfomance here. */
     private void Update() {
          if (Input.GetButton("Fire1") && Time.time > nextFire) {
               nextFire = Time.time + fireRate;
               Scene currentScene = SceneManager.GetActiveScene(); // [1]
               string sceneName = currentScene.name;
               if (sceneName == "Main") {
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
               }
               else if (sceneName == "Level_2") {
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
               }
               else if (sceneName == "Level_3") {
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
               }
               else if (sceneName == "Level_4") {
                    Instantiate(shot, shotSpawn_1.position, shotSpawn_1.rotation);
                    Instantiate(shot, shotSpawn_2.position, shotSpawn_2.rotation);
               }
               else if(sceneName == "Level_5") {
                    Instantiate(shot, shotSpawn_1.position, shotSpawn_1.rotation);
                    Instantiate(shot, shotSpawn_2.position, shotSpawn_2.rotation);
               }
               else if(sceneName == "Level_6") {
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                    Instantiate(shot, shotSpawn_1.position, shotSpawn_1.rotation);
                    Instantiate(shot, shotSpawn_2.position, shotSpawn_2.rotation);
               }
               ac.Play();
          }
    
     }

     // Update and move Player object via user input at defualt 0.02 (50 times per frame) while retaining boundary limits.
     private void FixedUpdate() {
          float moveHorizontal = Input.GetAxis("Horizontal");
          float moveVertical = Input.GetAxis("Vertical");

          Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
          rb.velocity = movement * speed;

          rb.position = new Vector3(
               Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
               0.0f,
               Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
               );

          rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
     }

}

/* CITATIONS:
[1] http://answers.unity3d.com/questions/1173303/how-to-check-which-scene-is-loaded-and-write-if-co.html
[2] https://unity3d.com/learn/tutorials/projects/space-shooter/the-player-gameobject?playlist=17147
 */
