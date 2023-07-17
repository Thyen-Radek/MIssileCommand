using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour, IEntity
{
    private int _missilesLoaded = 10;
    private int _missilesLeftCounter = 0;

    private bool _isActive = true;
    private bool _isAlive = true;

    private AudioSource _myAudio;

    private GameManager _gameManager;

    private GameObject[] _missilesLoadedObjects;

    private GameOptions _gameOptions;

    private ShipSpawnerController _shipSpawnerController;
    
    void Start()
    {
        _gameOptions = GameOptions.Instance;
        _myAudio = GetComponent<AudioSource>();
        _shipSpawnerController = GameObject.FindObjectOfType<ShipSpawnerController>();
        _missilesLoadedObjects = GameObject.FindGameObjectsWithTag("Missiles");
        _gameManager = FindObjectOfType<GameManager>();
    }

    public void PlayMissileAudio()
    {
        _myAudio.Play();
    }
     
    public void UpdateAllMissiles(bool status){
        for (int i = _missilesLeftCounter; i < _missilesLoadedObjects.Length; i++)
        {
            _missilesLoadedObjects[i].SetActive(status);
        }
    }

    public void UpdateMissilesLoaded(){
        PlayMissileAudio();
        _missilesLoaded--;
        _gameManager.UpdateMissiles(_gameManager.GetPlayerMissiles() - 1);
        _missilesLoadedObjects[_missilesLeftCounter++].SetActive(false);
        if (_missilesLoaded == 0)
        {
            if (_gameManager.GetPlayerMissiles() == 0)
            {
                _shipSpawnerController.MakeShipsFaster();
                SetActive(false);
                return;
            }
            StartCoroutine(LoadingMissiles());
        }
    }

    private IEnumerator LoadingMissiles(){
        SetActive(false);
        yield return new WaitForSeconds(1f);
        ResetMissilesLoaded();
        SetActive(true);
    }

    // When ship hit the missile launcher
    public void DestroyMissiles(){
        if (_missilesLoaded > 0){
            _gameManager.UpdateMissiles(_gameManager.GetPlayerMissiles() - _missilesLoaded);
            _missilesLoaded = 0;
            UpdateAllMissiles(false);
            if (_gameManager.GetPlayerMissiles() == 0)
            {
                _shipSpawnerController.MakeShipsFaster();
                SetAlive(false);
                gameObject.SetActive(false);
                return;
            }
            StartCoroutine(LoadingMissiles());
        }
    }

    public void SetAlive(bool status){
        _isAlive = status;
    }

    public void ResetMissiles(){
        _gameManager.UpdateMissiles(_gameOptions.PlayerMissiles);
        ResetMissilesLoaded();
    }

    public void ResetMissilesLoaded(){
        _missilesLoaded = 10;
        _missilesLeftCounter = 0;
        UpdateAllMissiles(true);
    }

    public void SetActive(bool status)
    {
        _isActive = status;
    }

    public bool IsActive()
    {
        return _isActive;
    }

    public bool IsAlive(){
        return _isAlive;
    }
}
