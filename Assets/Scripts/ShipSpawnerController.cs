using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameTags;

public class ShipSpawnerController : MonoBehaviour
{
    private bool _setFaster = false;
    
    private float _shipSpeed = 0f;

    // App values, not to be changed
    private float _minX = 0;
    private float _maxX = Screen.width;
    private float _paddingTop = 25f;
    private float _ySpawnPoint = 0f;

    private GameOptions _gameOptions;
    
    private Vector3[] _defendersPositions;

    private GameManager _gameManager;

    void Awake()
    {    
        
        _gameOptions = GameOptions.Instance;
        _shipSpeed = _gameOptions.ShipSpeed;
        GameObject[] defenders = GameObject.FindGameObjectsWithTag(MyTags.Defenders);
        _defendersPositions = new Vector3[defenders.Length + 1];
        _gameManager = FindObjectOfType<GameManager>();
        Vector3 missileCommanderPosition = GameObject.FindGameObjectWithTag(MyTags.MissileCommander).transform.position;
        _defendersPositions[0] = missileCommanderPosition;
        for (int i = 1; i < defenders.Length + 1; i++)
        {
            _defendersPositions[i] = defenders[i-1].transform.position;
        }
        _ySpawnPoint = Screen.height + _paddingTop;

    }

    void Update(){
        // _setFaster is only once true when we run out of missiles so it is still kinda efficient
        if(_setFaster && _gameManager.MissilePool.AllObjectsInPool() && _gameManager.ExplosionPool.AllObjectsInPool()){
            _setFaster = false;
            _shipSpeed = _gameManager.GetShipSpeed() + 2f;
        }
    }

    private IEnumerator SpawnShipsCoroutine()
    {   
        int numberOfShips;
        for (int i = 0; i < _gameOptions.PhasesCount; i++)
        {
            numberOfShips = Random.Range(_gameOptions.MinShipsPerPhase, _gameOptions.MaxShipsPerPhase);
            FindObjectOfType<GameManager>().UpdateShipsLeft(numberOfShips);
            for (int j = 0; j < numberOfShips; j++)
            {
                float randomX = Random.Range(_minX, _maxX);
                Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector2(randomX, _ySpawnPoint));
                ShipController ship = _gameManager.ShipPool.GetEntity().GetComponent<ShipController>();
                ship.RunShip(spawnPosition);
            }
            yield return new WaitForSeconds(_gameOptions.SpawnRate / _shipSpeed);
        }
        for (int j = 0; j < 4; j++)
        {
            float randomX = Random.Range(_minX, _maxX);
            Vector2 spawnPosition = Camera.main.ScreenToWorldPoint(new Vector2(randomX, _ySpawnPoint));
            ShipController ship = _gameManager.ShipPool.GetEntity().GetComponent<ShipController>();
            ship.RunShip(spawnPosition);
        }
    }

    public void MakeShipsFaster(){
        _setFaster = true;
    }
    
    public void StartLevel()
    {
        StartCoroutine(SpawnShipsCoroutine());
    }

    public Vector3[] GetDefendersPositions()
    {
        return _defendersPositions;
    }

    public float GetShipSpeed()
    {
        return _shipSpeed;
    }

    public void SetShipSpeed(float speed)
    {
        _shipSpeed = speed;
    }
}
