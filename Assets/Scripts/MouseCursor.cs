using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{  
    private GameManager _gameManager;

    private MissileLauncher _missileLauncher; // The missile launcher

    void Start()
    {
        Cursor.visible = false; // Hide the cursor
        _gameManager = FindObjectOfType<GameManager>();
        _missileLauncher = FindObjectOfType<MissileLauncher>();
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position
    
        transform.position = new Vector3(mousePosition.x, mousePosition.y, -5); // Set the cursor position

        if (Input.GetMouseButtonDown(0) && _missileLauncher.IsActive() && !_gameManager.IsRoundOver()) // If the left mouse button is pressed
        {   
            PlayerMissileController missile = _gameManager.MissilePool.GetEntity().GetComponent<PlayerMissileController>();
            missile.RunMissile();

            _missileLauncher.UpdateMissilesLoaded();
        }
    }
}
