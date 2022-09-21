using System.Collections;
using UnityEngine;
using TMPro;
public class ScoreText : MonoBehaviour
{

    [SerializeField] TextMeshPro scoreText;


    public void animateStart(int Score ,int Size, Color32 Color, Color32 OutlineColor)
    {
        StartCoroutine(animate(Score, Size ,Color, OutlineColor));
    }
    private IEnumerator animate(int Score, int Size, Color32 Color, Color32 OutlineColor)
    {
        scoreText.text = Score.ToString();
        scoreText.color = Color;
        scoreText.fontSize = Size;
        scoreText.outlineColor = OutlineColor;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(CountUpToTarget(scoreText,1f));


    }

    private IEnumerator CountUpToTarget(TextMeshPro label, float duration)
    {

        float current = 0;

        float targetVal = label.fontSize;


        while (targetVal > current)
        {
            if (label.fontSize <= 0) break;

            current = (targetVal / (duration / Time.deltaTime));
            current = Mathf.Clamp(current, 0, targetVal);

            label.fontSize = label.fontSize-current;
            yield return null;
        }

        Destroy(gameObject);
    }
}
