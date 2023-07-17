using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour, IEntity
{
    private bool _isAlive = true;

    public void SetAlive(bool status)
    {
        _isAlive = status;
    }

    public bool IsAlive()
    {
        return _isAlive;
    }
}
