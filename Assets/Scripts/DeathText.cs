using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathText : MonoBehaviour
{
    private TextMeshPro text;
    void Start()
    {
        text = gameObject.GetComponent<TextMeshPro>();
        text.enabled = false;
        
    }

    public IEnumerator deathTextAnimate(int dieScore)
    {
        text.enabled = true;
        text.text = dieScore.ToString();
        while (true == true)
        {
            while (text.fontSize < 20)
            {
                text.fontSize = text.fontSize + 1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
        text.enabled = false;
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
