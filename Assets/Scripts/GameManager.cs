using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using GameTags;

public class GameManager : MonoBehaviour, IObserver
{
    private int _score = 0;
    private int _level = 1;
    private int _playerMissiles = 0;
    private int _shipsLeft = 4;
    private int _cityCount = 0;
    private int _pointsMultiplier = 1;
    private int _citiesRecovered = 0;
    private int _highScore = 0;

    private bool _isRoundOver = false;

    private float _shipSpeed = 0f;

    [SerializeField]
    private UIController _userInterface;

    private GameObject _missileCommander; // The missile commander prefab

    private MissileLauncher _missileLauncher; // The missile launcher script

    private ShipSpawnerController _shipSpawnerController;

    private GameOptions _gameOptions;

    private GameObject[] _cities;

    // The Object Pooling
    public Pool MissilePool;
    public Pool ShipPool;
    public Pool ExplosionPool;

    void Start()
    {
        MissilePool = GameObject.Find("MissilePool").GetComponent<Pool>();
        ShipPool = GameObject.Find("ShipPool").GetComponent<Pool>();
        ExplosionPool = GameObject.Find("ExplosionPool").GetComponent<Pool>();

        _gameOptions = GameOptions.Instance;
        _shipSpeed = _gameOptions.ShipSpeed;
        _cities = GameObject.FindGameObjectsWithTag(MyTags.Defenders);

        _missileCommander = GameObject.Find(MyTags.MissileCommander);
        _missileLauncher = _missileCommander.GetComponent<MissileLauncher>();

        _shipSpawnerController = GameObject.FindObjectOfType<ShipSpawnerController>();
        _highScore = SaveLoadManager.LoadScore();
        _userInterface.UpdateHighScoreText(_highScore);
        _cityCount = _cities.Length;

        _userInterface.UpdateScoreText(_score);
        _userInterface.UpdateLevelText(_level);
        _userInterface.UpdatePointsMultiplierText(_pointsMultiplier);
        UpdateMissiles(_gameOptions.PlayerMissiles);

        _shipSpawnerController.StartLevel();
    }

    void Update()
    {
        if(_cityCount == 0 && !_isRoundOver){
            _isRoundOver = true;
            if (_score > _highScore)
            {
                SaveLoadManager.SaveScore(_score);
            }
            SceneManager.LoadScene("GameOver");
        } else if (_shipsLeft == 0 && !_isRoundOver)
        {
            _isRoundOver = true;    
            StartCoroutine(EndOfLevel());   
        }
    }

    public void UpdateMissiles(int missiles)
    {
        _playerMissiles = missiles;
        _userInterface.UpdateMissilesText(_playerMissiles);
    }

    public void UpdateLevel(int level)
    {
        _level = level;
        _userInterface.UpdateLevelText(_level);
    }

    public void UpdatePointsMultiplier()
    {
        if (_level % 2 == 1)
        {
            _pointsMultiplier++;
        }
        _userInterface.UpdatePointsMultiplierText(_pointsMultiplier);
    }

    // At first i wanted to use observer with that method but it the end i even didn't need that
    public void UpdateScore(ScoreType scoreType, int score = 0)
    {
        switch (scoreType)
        {
            case ScoreType.Ship:
                _score += _gameOptions.ShipPoints * _pointsMultiplier;
                break;
            case ScoreType.Bonus:
                _score += score;
                break;
        }
        _userInterface.UpdateScoreText(_score);
    }

    private IEnumerator EndOfLevel()
    {
        yield return new WaitForSeconds(0.5f);

        _userInterface.UpdateNextRoundText(3);
        _userInterface.SetPanelStatus(true);

        // Calculate bonuses
        int missilesBonus = _playerMissiles * _gameOptions.MissileLeftPoints * _pointsMultiplier;
        int housesBonus = _cityCount * _gameOptions.HousePoints * _pointsMultiplier;
        int totalBonus = missilesBonus + housesBonus;

        // Update UI
        _userInterface.UpdateLeftMissilesBonusText(missilesBonus);
        _userInterface.UpdateLeftHousesBonusText(housesBonus);
        _userInterface.UpdateTotalBonusText(totalBonus);

        // Countdown counter
        for (int i = 2; i >= 0; i--)
        {
            yield return new WaitForSeconds(1f);
            _userInterface.UpdateNextRoundText(i);
            
        }
        yield return new WaitForSeconds(0.5f);

        _userInterface.SetPanelStatus(false);

        // Update game status
        UpdateLevel(_level + 1);
        _missileLauncher.ResetMissiles();
        UpdateScore(ScoreType.Bonus, totalBonus);
        if (_pointsMultiplier < 6){
            UpdatePointsMultiplier();
        }

        // House spawning
        if (_cityCount < 6 && _score / 10000 > _citiesRecovered){
            // Potentially it could be optimized by storing objects or indexes in queueue whenever one is destroyed and then just pop it out
            _cityCount++;
            _citiesRecovered++;
            for (int i = 0; i < _cities.Length; i++){
                CityController city = _cities[i].GetComponent<CityController>();
                if (!city.IsAlive()){
                    // Set city status if is destroyed
                    city.SetAlive(true);
                    _cities[i].SetActive(true);
                    break;
                }
            }

        }

        // It is for last phase of the round
        _shipsLeft = 4;

        // Update ship speed
        _shipSpeed *= _gameOptions.ShipSpeedMultiplier;
        _shipSpawnerController.SetShipSpeed(_shipSpeed);
        _isRoundOver = false;
        _missileLauncher.SetActive(true);

        if (!_missileLauncher.IsAlive())
        {
            _missileLauncher.SetAlive(true);
            
            // Instead of instantiating it, we just activate it
            _missileCommander.SetActive(true);
            // Instantiate(_missileCommander, new Vector3(0, -3.7f, 0), Quaternion.identity);
        }
        
        _shipSpawnerController.StartLevel();
    }

    public float GetShipSpeed()
    {
        return _shipSpeed;
    }

    public bool IsRoundOver()
    {
        return _isRoundOver;
    }
    
    public void UpdateShipsLeft(int shipsLeft)
    {
        _shipsLeft += shipsLeft;
    }

    public int GetPlayerMissiles()
    {
        return _playerMissiles;
    }

    public void UpdateCityCount()
    {
        _cityCount--;
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
