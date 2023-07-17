using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTags;

public class ShipController : MonoBehaviour
{
    private bool _isAlive = true;
    
    private Vector2 _targetPosition;

    private GameManager _gameManager;

    private Vector3[] _defendersPositions;

    private ShipSpawnerController _shipSpawnerController;

    private MissileLauncher _missileLauncher;

    void Awake()
    {
        _missileLauncher = FindObjectOfType<MissileLauncher>();
        _gameManager = FindObjectOfType<GameManager>();
        _shipSpawnerController = FindObjectOfType<ShipSpawnerController>();
        _defendersPositions = _shipSpawnerController.GetDefendersPositions();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _shipSpawnerController.GetShipSpeed() * Time.deltaTime); 
    }

    public void SetDefenderPositions(Vector3[] positions)
    {
        _defendersPositions = positions;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Just in case you send two missiles near to the object and ship catch both of them at the same frame
        if (!_isAlive) return;

        if (other.tag == MyTags.Defenders)
        {
            EntityExplode();
            // Just in case two ships hit defender at the same frame
            if (!other.gameObject.GetComponent<CityController>().IsAlive()) return;
 
            // Set if city is alive
            other.gameObject.GetComponent<CityController>().SetAlive(false);
            
            _gameManager.UpdateCityCount();

            // Instead of destroying the city, we just disable it
            other.gameObject.SetActive(false);

        } else if (other.tag == MyTags.Explosions){
            EntityExplode();

            // Update score
            _gameManager.UpdateScore(ScoreType.Ship);

        } else if (other.tag == MyTags.Terrain){
            EntityExplode();

        } else if (other.tag == MyTags.MissileCommander){
            EntityExplode();

            _missileLauncher.SetActive(false);
            if (_gameManager.GetPlayerMissiles() > 0){
                _missileLauncher.DestroyMissiles();
                return;
            }
            
            _shipSpawnerController.MakeShipsFaster();
            _missileLauncher.SetAlive(false);

            // Instead of destroying the missile commander, we just disable it
            other.gameObject.SetActive(false);
        }
    }

    private void EntityExplode()
    {
        _isAlive = false;

        SelfDestruction explosion = _gameManager.ExplosionPool.GetEntity().GetComponent<SelfDestruction>();
        explosion.RunExplosion(transform.position);

        // Instead of destroying the ship, we return it to the pool
        _gameManager.ShipPool.ReturnEntity(gameObject);

        _gameManager.UpdateShipsLeft(-1);
    }

    private void SplitShip()
    {
        float yValue = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).y;
        if (!_isAlive || transform.position.y < yValue) return;

        _gameManager.UpdateShipsLeft(1);

        ShipController ship = _gameManager.ShipPool.GetEntity().GetComponent<ShipController>();
        ship.RunShip(transform.position);
    }

    public void RunShip(Vector2 position)
    {
        transform.position = position;

        _isAlive = true;
        // get random target 
        _targetPosition = _defendersPositions[Random.Range(0, _defendersPositions.Length)];
        // calculate vector from ship to target
        Vector2 direction = _targetPosition - (Vector2)transform.position;
        // normalize vector
        direction.Normalize();

        _targetPosition += direction * 1f;
        
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _targetPosition - (Vector2)transform.position);

        float randomTime = Random.Range(0.1f, 100f) / _shipSpawnerController.GetShipSpeed();
        Invoke("SplitShip", randomTime);
    }
}
