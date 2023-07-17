using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private TextMeshProUGUI _levelText;

    [SerializeField]
    private TextMeshProUGUI _missilesText;

    [SerializeField]
    private GameObject _roundPanel;

    [SerializeField]
    private TextMeshProUGUI _leftMissilesBonusText;

    [SerializeField]
    private TextMeshProUGUI _leftHousesBonusText;

    [SerializeField]
    private TextMeshProUGUI _totalBonusText;

    [SerializeField]
    private TextMeshProUGUI _nextRoundText;

    [SerializeField]
    private TextMeshProUGUI _pointsMultiplierText;

    [SerializeField]
    private TextMeshProUGUI _highScoreText;

    public void UpdateScoreText(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLevelText(int level)
    {
        _levelText.text = "Level: " + level;
    }

    public void UpdateMissilesText(int missiles)
    {
        _missilesText.text = "Missiles: " + missiles;
    }

    public void SetPanelStatus(bool status)
    {
        _roundPanel.SetActive(status);
    }

    public void UpdateLeftMissilesBonusText(int missiles)
    {
        _leftMissilesBonusText.text = "Missiles left: " + missiles;
    }

    public void UpdateLeftHousesBonusText(int houses)
    {
        _leftHousesBonusText.text = "Houses left: " + houses;
    }

    public void UpdateTotalBonusText(int bonus)
    {
        _totalBonusText.text = "Total bonus: " + bonus;
    }

    public void UpdateNextRoundText(int seconds)
    {
        _nextRoundText.text = "Next round in: " + seconds;
    }

    public void UpdatePointsMultiplierText(int multiplier)
    {
        _pointsMultiplierText.text = "Points: " + multiplier + "x";
    }

    public void UpdateHighScoreText(int score)
    {
        _highScoreText.text = "High Score: " + score;
    }
}
