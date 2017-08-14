/***********************************************************************************************************
* DestroyByBoundary.cs is utilzed to remove gameObjects once "off screen".  Additioanlly, if Game Object is
* taged as a boss type, bossesNotDestroyed is incremented, thus triggering a game ending event.
************************************************************************************************************/
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

     void OnTriggerExit(Collider other) {
          // These tags trigger a game ending event via the GameState.cs script.
          if(other.tag == "Boss" || other.tag == "Boss_1" || other.tag == "Boss_2" || other.tag == "Boss_3"
            || other.tag == "Boss_4" || other.tag == "Boss_5" || other.tag == "Boss_6") {
               GameState.bossesNotDestroyed++;
          }
          // Destroy an object that contacts the boundary.
          Destroy(other.gameObject);
     }
}

/* CITATION:
[1] https://unity3d.com/learn/tutorials/projects/space-shooter-tutorial/boundary?playlist=17147
*/
