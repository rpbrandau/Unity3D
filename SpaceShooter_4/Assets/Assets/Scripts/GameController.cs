/********************************************************************************************
* GameController.cs is used to control game actions such as spawning waves of enemies,
* level control, scoring, and scene transition fading calls.  See GameState.cs for dependency.
**********************************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

     private bool gameOver;
     private bool restart;
     private float fadeTime;
     private bool allSpawnsCompleted;

     public GameObject[] hazards;
     public GameObject[] bosses;
     public GUIText gameOverText;
     public GUIText restartText;
     public GUIText scoreText;
     public GUIText missionText;
     public GUIText message;
     public GUIText fuelText;
     public Vector3 spawnValues;
     public int hazardCount;
     public int bossCount;
     public int waveCount;
     public float spawnWait;
     public float startWait;
     public float waveWait;

     // Parallel arrays to control and manage Boss behaviour.
     int[] registerBossType;
     bool[] isDead;


     private void Start() {
          registerBossType = new int[bossCount];
          isDead = new bool[bossCount];
          for (int i = 0; i < bossCount; i++) {
               registerBossType[i] = 0;
               isDead[i] = false;     
          }
          gameOver = false;
          gameOverText.text = ""; 
          restart = false;
          restartText.text = "";
          allSpawnsCompleted = false;
          UpdateScore();
          GameState.fuelAmount = 100;
          UpdateFuel();
          GameState.bossesNotDestroyed = 0;
          GameState.playerDestroyed = 0;
          StartCoroutine (SpawnWaves());
          StartCoroutine(DecreaseFuel());
     }


     private void Update() {

          // Ends game if Boss not destroyed and Player object still active.
          if (allSpawnsCompleted == true && GameState.bossesNotDestroyed > 0) {
               ResetState(true);
               StartCoroutine(LoadLevelZero());
          }

          // Change levels if Boss is destroyed by Player.
          if (AllBossesAreDead() == true) {
               Scene scene = SceneManager.GetActiveScene();
               if (scene.name != "Winner") {
                    StartCoroutine(ChangeLevel(scene));
               }
          }

          // Auto restart game if Player is destroyed.
          if (gameOver) {
               ResetState(true);
               StartCoroutine(LoadLevelZero());
          }

          // Esc to quit from any level.
          if (Input.GetKeyDown(KeyCode.Escape) == true) { // WILL NOT WORK IN EDITOR! OK in builds.
               Application.Quit();
          }

          // J to jump levels from any level.  Pressed from Level 7 will retain points.
          if (Input.GetKeyDown(KeyCode.J) == true) {
               Scene scene = SceneManager.GetActiveScene();
               if (scene.name != "Winner") {
                    ResetState(false);
                    StartCoroutine(ChangeLevel(scene));
               }
               else {
                    ResetState(true);
                    StartCoroutine(LoadLevelZero());
               }
          }

          // P to pause game from any level.
          if (Input.GetKeyDown(KeyCode.P)) {
               if (Time.timeScale == 1) {
                    Time.timeScale = 0;
               }
               else {
                    Time.timeScale = 1;
               }
          }

          // R for Replay if game over condition [restart == true].  Full GameState reset. 
          if (restart) {
               if (Input.GetKeyDown(KeyCode.R)) {
                    ResetState(true);
                    StartCoroutine(LoadLevelZero());
               }
          }

          // Conditional restart - Level 7 will retain points, all others will not. 
          if (Input.GetKeyDown(KeyCode.R)) {
               Scene scene = SceneManager.GetActiveScene();
               if (scene.name == "Winner") {
                    ResetState(false);
               }
               else {
                    ResetState(true);
               }
               StartCoroutine(LoadLevelZero());
          }
     }

     // Controls the behavior and number of enemy object waves spawned.  See Inspector panel at each level. 
     IEnumerator SpawnWaves() {
          yield return new WaitForSeconds(startWait);
          while (waveCount > 0) {
               for (int i = 0; i < hazardCount; i++) {
                    GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                    Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                    Quaternion spawnRotation = Quaternion.identity;
                    Instantiate(hazard, spawnPosition, spawnRotation);
                    yield return new WaitForSeconds(spawnWait);
               }

               waveCount--;
               yield return new WaitForSeconds(waveWait);

               // If Player survived all initial waves, spawn any number of Bosses to overcome and advance to next level.
               if (waveCount == 0 && GameState.playerDestroyed == 0) {
                    for (int i = 0; i < bossCount; i++) {
                         GameObject boss = bosses[Random.Range(0, bosses.Length)];
                         Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                         Quaternion spawnRotation = Quaternion.identity;
                         Instantiate(boss, spawnPosition, spawnRotation);

                         if (boss.tag == "Boss_1") {
                              registerBossType[i] = 1;
                         }
                         else if(boss.tag == "Boss_2") {
                              registerBossType[i] = 2;
                         }
                         else if (boss.tag == "Boss_3") {
                              registerBossType[i] = 3;
                         }
                         else if (boss.tag == "Boss_4") { 
                              registerBossType[i] = 4;
                         }
                         else if (boss.tag == "Boss_5") { 
                              registerBossType[i] = 5;
                         }
                        else if (boss.tag == "Boss_6") { 
                              registerBossType[i] = 6;
                         }

                    yield return new WaitForSeconds(spawnWait);
                    }
               }

               if (gameOver) {
                    restartText.text = "Press 'R' for Restart";
                    restart = true;
                    ResetState(true);
                    break;
               }
          }

          allSpawnsCompleted = true;
     }

     // Decrement fuel once per second.
     IEnumerator DecreaseFuel() {
          yield return new WaitForSeconds(1f);
          GameState.fuelAmount--;
          UpdateFuel();
          if(GameState.fuelAmount <= 0) {
               GameOver();
          }
          StartCoroutine(DecreaseFuel());
     }

     // Called by FuelPUPManager on Player encountering Fuel pickup.
     void UpdateFuel() {
          fuelText.text = "Fuel: " + GameState.fuelAmount;
     }

     // Manages Player's on board fuel amount - mac fuel is 100 units.
     public void AddFuel(int fuelValue) {
          GameState.fuelAmount += fuelValue;
          GameState.fuelAmount = Mathf.Clamp(GameState.fuelAmount, 0, 100);
          UpdateFuel();
     }

     // Manages level change from _GM.cs as applied to each level.
     IEnumerator ChangeLevel(Scene scene) {
          fadeTime = GameObject.Find("_GM").GetComponent<Fader>().BeginFade(1);
          ResetState(false);
          yield return new WaitForSeconds(fadeTime);
          SceneManager.LoadScene(scene.buildIndex + 1);
     }

     // Manages transition to start level from any level as called from various.
     IEnumerator LoadLevelZero() {
          fadeTime = GameObject.Find("_GM").GetComponent<Fader>().BeginFade(1);
          yield return new WaitForSeconds(fadeTime);
          SceneManager.LoadScene(0);
     }

     // Boss scoring and calls to manage the death of a Boss - all levels except Winner.
     public void AddScore(int newScoreValue) {
          GameState.score += newScoreValue;
          UpdateScore();
          if (newScoreValue == 100) {
               ManageBossDeath(1);
          }
          else if (newScoreValue == 250) {
               ManageBossDeath(2);
          }
          else if (newScoreValue == 350) {
               ManageBossDeath(3);
          }
          else if (newScoreValue == 500) { 
               ManageBossDeath(4);
          }
          else if (newScoreValue == 600) {
               ManageBossDeath(5);
        }
          else if (newScoreValue == 1000) {
               ManageBossDeath(6);
        }
     }

     // Manages appropriate boss type and its death resolution at each level except Winner.
     private int ManageBossDeath(int bossType) {
          for (int i = 0; i < bossCount; i++) {
               if (registerBossType[i] == bossType) {
                    if(isDead[i] == false) {
                         isDead[i] = true;
                         return 0;
                    }
               }
          }
          return 0;
     }

     // Test for all bosses at a particular level have been defeated.
     private bool AllBossesAreDead() {
          for (int i = 0; i < bossCount; i++) {
               if (isDead[i] == false) {
                    return false;
               }
          }
          return true;
     }

     // Keeps current score updated and displayed to user.
     void UpdateScore() {
          scoreText.text = "Score: " + GameState.score;
      }

     // Reset GameState to initial values, sans current score on user decision at Winner level.
     private void ResetState(bool alsoResetScore) {
          GameState.bossesNotDestroyed = 0;
          GameState.playerDestroyed = 0;
          for (int i = 0; i < bossCount; i++) {
               registerBossType[i] = 0;
               isDead[i] = false;
          }
          if (alsoResetScore) {
               GameState.score = 0;
          }
     }

     // Triggers reset on Player destruction.
     public void GameOver() {
          gameOverText.text = "Game Over!";
          gameOver = true;
          restart = true;
     }

}

/* CITATIONS:
[1] http://answers.unity3d.com/questions/1113318/applicationloadlevelapplicationloadedlevel-obsolet.html
[2] http://answers.unity3d.com/questions/698531/how-to-make-esc-button-quit.html
[3] https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.GetSceneAt.html
[4] http://answers.unity3d.com/questions/447142/how-to-program-a-pauseunpause-button-in-c.html
[5] http://answers.unity3d.com/questions/1189512/how-do-i-check-if-all-booleans-in-an-array-are-tru.html
[6] https://stackoverflow.com/questions/5678216/all-possible-c-sharp-array-initialization-syntaxes
*/
