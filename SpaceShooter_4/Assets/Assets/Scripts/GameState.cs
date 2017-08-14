/**************************************************************************************************************************
* GameState.cs used to store various "global" game state variables so as to manage game play.  Called from:
*    GameController.cs
*    DestroyByContact.cs
*    FuelManagerPUP.cs
*    PUPManager.cs (Shield pickup)
*    ShieldManager.cs
***************************************************************************************************************************/
using UnityEngine;

public class GameState : MonoBehaviour {

     public static int score;
     public static int bossesNotDestroyed;
     public static int playerDestroyed;
     public static float timeToShieldDown;
     public static int fuelAmount;
}
