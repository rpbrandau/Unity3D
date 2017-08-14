/*********************************************************************************
* TextTimer.cs controls, via Inpspector, the duration of message display to user. 
**********************************************************************************/
using UnityEngine;

public class TextTimer : MonoBehaviour {

     public float time;

     void Start() {
          Destroy(gameObject, time);
     }
}

/* CITATIONS:
[1] http://answers.unity3d.com/questions/970467/how-to-make-disappear-a-gui-text-after-an-amount-o.html
*/