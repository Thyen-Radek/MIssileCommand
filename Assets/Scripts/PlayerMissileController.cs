using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMissileController : MonoBehaviour
{
    private Vector2 _targetPosition; // The direction of the missile
    private Vector2 _missileLauncherPosition;

    private GameOptions _gameOptions;

    private GameObject _tower;

    private GameManager _gameManager;

    void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _gameOptions = GameOptions.Instance;
        _tower = GameObject.Find("Tower");
        _missileLauncherPosition = _tower.transform.position;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _targetPosition, _gameOptions.PlayerMissileSpeed * Time.deltaTime); // Move the missile
        transform.position = new Vector3(transform.position.x, transform.position.y, 4);

        if (transform.position.x == _targetPosition.x && transform.position.y == _targetPosition.y) // If the missile reaches the mouse position
        {
            SelfDestruction explosion = _gameManager.ExplosionPool.GetEntity().GetComponent<SelfDestruction>();
            explosion.RunExplosion(transform.position);

            // Instead of destroying the missile, return it to the missile pool
            _gameManager.MissilePool.ReturnEntity(gameObject);
        }
    }

    public void RunMissile(){
        transform.position = _missileLauncherPosition;
        _targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // set missile launcher pos
        float maxY = 1f;
        if (_targetPosition.y < _missileLauncherPosition.y + maxY)
        {
            _targetPosition.y = _missileLauncherPosition.y + maxY;
        }

        // rotation
        Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, _targetPosition - _missileLauncherPosition);
        _tower.transform.rotation = lookRotation * Quaternion.Euler(0, 0, -90);
        transform.rotation = lookRotation * Quaternion.Euler(0, 0, 90);
    }
}
