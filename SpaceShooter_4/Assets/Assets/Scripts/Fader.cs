/***********************************************************************************
* Fader.cs is used to control the image presented and the amount of time the
* image is presented between levels on a level change event.  Image opacity and time
* of presentation controllable form the Inspector panel.  fadeOutTexture is the
* actual image or texture to be presented to the user - load in Inpsector panel.
* See _GM for applying fadeSpeed.
************************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fader : MonoBehaviour {

     public Texture2D fadeOutTexture;
     public float fadeSpeed = 1.0f;

     private int drawDepth = -1000;
     private float alpha = 1.0f;
     private float fadeDir = -1;

     // What is presented to the user and for how long.
	void OnGUI () {
          alpha += fadeDir * fadeSpeed * Time.deltaTime;
          alpha = Mathf.Clamp01(alpha);
          GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
          GUI.depth = drawDepth;
          GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

     // Sets the direction of fade, which is inconsequental here.
     public float BeginFade(int direction) {
          fadeDir = direction;
          return (fadeSpeed);
     }

     // Start the fade.
     void OnEnable() {
          SceneManager.sceneLoaded += OnSceneLoaded;
     }

     // Stop the fade.
     void OnDisable() {
          SceneManager.sceneLoaded -= OnSceneLoaded;
     }

     // The actual call to present the image.
     private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
          BeginFade(-1);
     }

}

/* CITATIONS:
[1] Fader script adpted from: https://www.youtube.com/watch?v=0HwZQt94uHQ
[2] Updated to v5.6.2 with http://answers.unity3d.com/questions/1174255/since-onlevelwasloaded-is-deprecated-in-540b15-wha.html
*/
