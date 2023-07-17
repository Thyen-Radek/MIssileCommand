using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour
{
    // Singleton pattern
    private static GameOptions _instance;

    // Score values
    public int ShipPoints = 25;
    public int HousePoints = 200;
    public int MissileLeftPoints = 5;

    // Player values
    public int PlayerMissiles = 30;
    public float PlayerMissileSpeed = 5f;

    // Enemy ship values
    public float ShipSpeedMultiplier = 1.2f;
    public float ShipSpeed = 0.6f;
   
    // Ship spawner values
    public int MaxShipsPerPhase = 5;
    public int PhasesCount = 4;
    public int MinShipsPerPhase = 3;
    public float SpawnRate = 4f;

    public static GameOptions Instance
    {
        get 
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameOptions>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("GameOptions");
                    _instance = container.AddComponent<GameOptions>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogError("More than one GameOptions in the scene");
        }
        // DontDestroyOnLoad(this.gameObject);
    }
}
