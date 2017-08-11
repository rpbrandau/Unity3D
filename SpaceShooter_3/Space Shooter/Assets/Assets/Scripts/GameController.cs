/* GameController.cs is used to control game actions such as spawning waves of enemies,
 * level control, scoring, and scene transition fading. */
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
          GameState.fuelAmount = 50;
          UpdateFuel();
          GameState.bossesNotDestroyed = 0;
          GameState.playerDestroyed = 0;
          StartCoroutine (SpawnWaves());
          StartCoroutine(DecreaseFuel());
     }


     private void Update() {

          if (restart) {
               if (Input.GetKeyDown(KeyCode.R)) {
                    ResetState(true);
                    StartCoroutine(LoadLevelZero());
               }
          }

          if (AllBossesAreDead() == true) {
               Scene scene = SceneManager.GetActiveScene();
               if (scene.name != "Winner") {
                    StartCoroutine(ChangeLevel(scene));
               }
          }

          if (allSpawnsCompleted == true && GameState.bossesNotDestroyed > 0) {
               ResetState(true);
               StartCoroutine(LoadLevelZero());
          }

          if (Input.GetKeyDown(KeyCode.Escape) == true) { // WILL NOT WORK IN EDITOR! OK in builds.
               Application.Quit();
          }

          if (Input.GetKeyDown(KeyCode.P) == true) {
               Scene scene = SceneManager.GetActiveScene();
               if (scene.name == "Winner") {
                    ResetState(false);
               }
               else {
                    ResetState(true);
               }
               StartCoroutine(LoadLevelZero());
          }

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

          if (gameOver) {
               ResetState(true);
               StartCoroutine(LoadLevelZero());
          }

     }


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

     IEnumerator DecreaseFuel()
    {
        yield return new WaitForSeconds(1f);
        GameState.fuelAmount--;
        UpdateFuel();
        if(GameState.fuelAmount <= 0)
        {
            GameOver();
        }
        StartCoroutine(DecreaseFuel());
    }

    void UpdateFuel()
    {
        fuelText.text = "Fuel: " + GameState.fuelAmount;
    }

    public void AddFuel(int fuelValue)
    {
        GameState.fuelAmount += fuelValue;
        GameState.fuelAmount = Mathf.Clamp(GameState.fuelAmount, 0, 50);
        UpdateFuel();
    }

     IEnumerator ChangeLevel(Scene scene) {
          fadeTime = GameObject.Find("_GM").GetComponent<Fader>().BeginFade(1);
          ResetState(false);
          yield return new WaitForSeconds(fadeTime);
          SceneManager.LoadScene(scene.buildIndex + 1);
     }


     IEnumerator LoadLevelZero() {
          fadeTime = GameObject.Find("_GM").GetComponent<Fader>().BeginFade(1);
          yield return new WaitForSeconds(fadeTime);
          SceneManager.LoadScene(0);
     }


     // Boss scoring: Boss_1 = 50, Boss_2 = 60, Boss_3 = 75.
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


     private bool AllBossesAreDead() {
          for (int i = 0; i < bossCount; i++) {
               if (isDead[i] == false) {
                    return false;
               }
          }
          return true;
     }


     void UpdateScore() {
          scoreText.text = "Score: " + GameState.score;
      }


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
[4] 
[5] http://answers.unity3d.com/questions/1189512/how-do-i-check-if-all-booleans-in-an-array-are-tru.html
[6] https://stackoverflow.com/questions/5678216/all-possible-c-sharp-array-initialization-syntaxes
*/
