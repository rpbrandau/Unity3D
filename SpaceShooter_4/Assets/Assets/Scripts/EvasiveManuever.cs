/*****************************************************************************************************
* EvasiveManuever.cs is applied to Enemy ships in order to "auto" move them in random but predefined
* pattersns. All public variables are controllable from the inspector, including the boundary limits
* placed on the applied game object.  Mover.cs MUST also be applied for this script to work as current
* speed is derived from there.
******************************************************************************************************/
using System.Collections;
using UnityEngine;

public class EvasiveManuever : MonoBehaviour {

     private float currentSpeed;
     private float targetManuever;
     private Rigidbody rb;
     
     public float dodge;
     public float smoothing;
     public float tilt;
     public Vector2 startWait;
     public Vector2 manueverTime;
     public Vector2 manueverWait;
     public Boundary boundary;
     public float timeRemaining;    

	void Start () {
          rb = GetComponent<Rigidbody>();
          currentSpeed = rb.velocity.z;
          StartCoroutine (Evade());	
	}
	
     // Runs continuously for the life of the applied game object (Enemy).
     IEnumerator Evade() {
          yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

          while (true) {
               targetManuever = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
               yield return new WaitForSeconds(Random.Range(manueverTime.x, manueverTime.y));
               targetManuever = 0;
               yield return new WaitForSeconds(Random.Range(manueverWait.x, manueverWait.y));
          }
     }

     // Runs at default 0.02, thus 50 physics updates per frame Update() to give applied object smooth travel.
     void FixedUpdate () {
          float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManuever, Time.deltaTime * smoothing);
          rb.velocity = new Vector3(newManeuver, 0.0f, currentSpeed);
          rb.position = new Vector3 (
               Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
               0.0f,
               Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
          );
          rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
          timeRemaining -= Time.deltaTime;
          if (timeRemaining < 0) {
               boundary.zMin = -20;
          }
     }
     
}

/* CITATIONS:
[1] https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/extending-space-shooter-enemies-more-hazards?playlist=17147
[2] http://answers.unity3d.com/questions/225213/c-countdown-timer.html
[3] https://unity3d.com/learn/tutorials/topics/scripting/update-and-fixedupdate
*/
