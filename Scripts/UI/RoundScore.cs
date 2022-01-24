using System.Collections;
using TMPro;
using UnityEngine;

public class RoundScore : MonoBehaviour
{

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private float animationTime = 1.5f;

    private void OnEnable()
    {
        StartCoroutine(TextAnima());
    }

    private IEnumerator TextAnima()
    {
        scoreText.SetText("");
        int curScore = 0;
        int completeScore = GameStat.currentScore;
        float curTime = 0;

        yield return new WaitForSeconds(.7f);

        while (curTime <= animationTime)
        {
            
            scoreText.SetText(Mathf.Lerp(curScore, completeScore, curTime / animationTime).ToString("0"));
            curTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        scoreText.SetText(completeScore.ToString("0"));

        yield return null;
    }

}
