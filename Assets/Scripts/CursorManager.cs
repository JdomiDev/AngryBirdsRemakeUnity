using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CursorManager : MonoBehaviour
{

    public Texture2D[] cursors;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot;

    // Start is called before the first frame update
    private void Start()
    {
        hotSpot = new Vector2(50, 50);
        Cursor.SetCursor(cursors[1], hotSpot, cursorMode);
    }

    private void Update()
    {
        bool True = false;
        if(SceneManager.GetActiveScene().buildIndex != 0)
        {
            foreach (var bird in new List<GameObject>(GameObject.FindGameObjectsWithTag("Bird")))
            {
                if (bird.GetComponent<Bird>().Thrown && !bird.GetComponent<Bird>().Collided)
                {
                    True = true;
                }
            }
        }
        if (Input.GetMouseButton(0) && True)
        {
            Cursor.SetCursor(cursors[3], hotSpot, cursorMode);
        }
        else if(True)
        {
            Cursor.SetCursor(cursors[0], hotSpot, cursorMode);
        }
        else if(Input.GetMouseButton(0))
        {
            Cursor.SetCursor(cursors[3], hotSpot, cursorMode);
        }
        else
        {
            Cursor.SetCursor(cursors[1], hotSpot, cursorMode);
        }
    }
}
