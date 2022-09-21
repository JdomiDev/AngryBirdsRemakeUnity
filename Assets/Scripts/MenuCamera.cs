using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = 0.01f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x + 0.0125f, gameObject.transform.position.y);
    }
}
