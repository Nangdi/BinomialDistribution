using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphRunner : MonoBehaviour
{
    private Coroutine fillRoutine; // 내부 코루틴 추적용
    public void AnimateFill(float targetNormalized, float maxHeight, BarGraph graph)
    {
        // 기존 코루틴 중단 (겹침 방지)
        if (fillRoutine != null)
            StopCoroutine(fillRoutine);

        fillRoutine = StartCoroutine(AnimateFillRoutine(targetNormalized, maxHeight, graph));
    }

    private IEnumerator AnimateFillRoutine(float targetNormalized, float maxHeight , BarGraph graph)
    {
        float startFill = graph.bar.fillAmount;
        float duration = 0.5f; // 애니메이션 속도
        float time = 0f;

        RectTransform textRt = graph.valueText.rectTransform;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, time / duration);
            float currentFill = Mathf.Lerp(startFill, targetNormalized, t);

            // Fill 적용
            graph.bar.fillAmount = currentFill;
            // 텍스트 위치도 같이 이동
            float yPos = maxHeight * currentFill;
            if (float.IsNaN(startFill) || float.IsNaN(targetNormalized))
            {
                Debug.LogWarning($"[GraphRunner] Invalid fill value: start={startFill}, target={targetNormalized}");
                yield break;
            }
            if (currentFill == 0)
            {
                yPos = 0;
            }
            textRt.anchoredPosition = new Vector2(textRt.anchoredPosition.x, yPos);

            yield return null;
        }

        // 마지막 정확히 맞추기
        graph.bar.fillAmount = targetNormalized;
        textRt.anchoredPosition = new Vector2(textRt.anchoredPosition.x, maxHeight * targetNormalized);
    }
}
