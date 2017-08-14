/*************************************************************************************
 * DestroybyTime.cs is used by the various explosion objects to ensure their removal 
 * from memory.  Failure to utilize this script on objects not moving and eventually
 * contacting the game boundary will eventually crash a windows system.
 *************************************************************************************/
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

     public float lifetime;
 
	void Start () {
          Destroy(gameObject, lifetime);
	}
}

/* CITATIONS:
[1] https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/explosions?playlist=17147 
*/
