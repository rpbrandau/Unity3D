/**************************************************************************************************
* BGSCroller.cs used on all game levels to scroll the background image as loaded in the Inspector. 
* The bahaviour is modified via the two publci variables.
**************************************************************************************************/
using UnityEngine;

public class BGScroller : MonoBehaviour {

     public float scrollSpeed;
     public float tileSizeZ;

     private Vector3 startPosition;

     private void Start() {
          startPosition = transform.position;
     }

     void Update () {
          float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);
          transform.position = startPosition + Vector3.forward * newPosition;
     }

}

/* CITATION:
 [1] https://unity3d.com/learn/tutorials/projects/space-shooter/adding-a-background?playlist=17147
*/
