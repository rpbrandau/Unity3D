using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPUPManager : MonoBehaviour
{

    private GameController gameController;
    public GameObject explosion;
    public int fuelValue;

    // Use this for initialization
    void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController', script.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Boss")
            || other.CompareTag("Boss_1") || other.CompareTag("Boss_2") || other.CompareTag("Boss_3")
            || other.CompareTag("Boss_4") || other.CompareTag("Boss_5") || other.CompareTag("Boss_6")
            || other.CompareTag("PlayerBolt") || other.CompareTag("Untagged"))
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.CompareTag("Player"))
        {
            gameController.AddFuel(fuelValue);
            Destroy(gameObject);
        }

    }

}
