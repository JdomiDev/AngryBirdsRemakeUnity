using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class StatsCounter : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI fpsText;

    // Start is called before the first frame update
    void Start()
    {
        fpsText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            changeState();
        }
        if(fpsText.enabled == true)
        {
            fpsText.text = (1f / Time.unscaledDeltaTime).ToString();
        }
    }

    void changeState()
    {
        if(fpsText.enabled == true)
        {
            fpsText.enabled = false;
        }
        else
        {
            fpsText.enabled = true;
        }
    }
}
