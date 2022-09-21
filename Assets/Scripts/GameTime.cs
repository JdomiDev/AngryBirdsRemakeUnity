using System.Collections;
using UnityEngine;

public class GameTime : MonoBehaviour
{
    [HideInInspector]
    public static int gameTime;
    void Start()
    {
        gameTime = 0;
        StartCoroutine(gameTimeAdd());
    }

    private IEnumerator gameTimeAdd()
    {
        yield return new WaitForSeconds(1f);
        gameTime++;
    }
}
