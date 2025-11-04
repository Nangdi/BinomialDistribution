using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GraphScreenTransition : MonoBehaviour
{
    enum Mode 
    {
        Total,
        Today
    }



    [SerializeField]
    private BarGraphController graphController;
    [SerializeField]
    private CanvasGroup total;
    [SerializeField]
    private CanvasGroup today;


    [Header("Transition Settings")]
    public float targetTime = 120f;   // 2분마다 전환
    public float fadeDuration = 1.5f; // 페이드 시간

    private float currentLapse = 0f;
    private Mode mode = Mode.Total;
    private Coroutine transitionRoutine;
    void Start()
    {
        TryGetComponent(out graphController);
        total.alpha = 1f;
        today.alpha = 0f;
        total.interactable = true;
        today.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentLapse += Time.deltaTime;
        if(currentLapse >= targetTime)
        {
            currentLapse = 0;
            Transition();
        }
    }
    public void Transition()
    {
        // 중복 실행 방지
        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        // 모드 전환
        mode = (mode == Mode.Total) ? Mode.Today : Mode.Total;
        List<BarGraph> graphs = (mode == Mode.Total) ? graphController.totalBarGraphs : graphController.barGraphs;
        graphController.MotionReset(graphs);

        // 코루틴 실행
        transitionRoutine = StartCoroutine(TransitionRoutine());
    }

    private IEnumerator TransitionRoutine()
    {
        CanvasGroup fadeOut = (mode == Mode.Total) ? today : total;
        CanvasGroup fadeIn = (mode == Mode.Total) ? total : today;

        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;

            fadeOut.alpha = Mathf.Lerp(1f, 0f, t);
            fadeIn.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // 마지막 보정
        fadeOut.alpha = 0f;
        fadeIn.alpha = 1f;

        // 인터랙션 제어 (필요시)
        fadeOut.interactable = false;
        fadeIn.interactable = true;
        graphController.UpdateGraphs();
    }
}
