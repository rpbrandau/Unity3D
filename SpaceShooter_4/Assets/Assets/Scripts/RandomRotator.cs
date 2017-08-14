/******************************************************************************
* RandomRotator.cs is the script applied to asteroids to randomly rotate them.
*******************************************************************************/
using UnityEngine;

public class RandomRotator : MonoBehaviour {

     public float tumble;
     public Rigidbody rb;

     private void Start() {
          rb = GetComponent<Rigidbody>();
          rb.angularVelocity = Random.insideUnitSphere * tumble;
     }

}

/* CITATIONS:
[1] https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/creating-hazards?playlist=17147
*/
