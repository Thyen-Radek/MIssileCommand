using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTags;

public class SelfDestruction : MonoBehaviour
{
    [SerializeField] 
    private float _selfDestructTime = 1.2f; // The time before the object self-destructs

    private AudioSource _myAudio;

    private GameManager _gameManager;

    private MissileLauncher _missileLauncher;

    private ShipSpawnerController _shipSpawnerController;

    void Awake()
    {
        _myAudio = GetComponent<AudioSource>();
        _gameManager = FindObjectOfType<GameManager>();
        _missileLauncher = FindObjectOfType<MissileLauncher>();
        _shipSpawnerController = GameObject.FindObjectOfType<ShipSpawnerController>();
    }

    public void RunExplosion(Vector2 position)
    {
        
        transform.position = position;
        // Instantiate the explosion prefab
        _myAudio.Play();
        Invoke("DestroyExplosion", _selfDestructTime);
    }

    private void DestroyExplosion()
    {
        _myAudio.Stop();
        _gameManager.ExplosionPool.ReturnEntity(gameObject);
    }

    // There could be 2DCollider on trigger here to destroy the object like houses and launcher


    private void OnTriggerEnter2D(Collider2D other){
        if (other.tag == MyTags.Defenders)
        {
            // Just in case two explosions hit defender at the same frame
            if (!other.gameObject.GetComponent<CityController>().IsAlive()) return;
 
            // Set if city is alive
            other.gameObject.GetComponent<CityController>().SetAlive(false);
            
            _gameManager.UpdateCityCount();

            // Instead of destroying the city, we just disable it
            other.gameObject.SetActive(false);
        } else if(other.tag == MyTags.MissileCommander){

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
}
