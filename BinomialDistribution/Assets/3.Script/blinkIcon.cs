using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class blinkIcon : MonoBehaviour
{

    public CanvasGroup target;   // 깜빡일 대상 (Image/Text 등)
    public float fadeTime = 0.5f; // 한 번 깜빡이는 시간 (초)
    public int loops = -1;        // -1은 무한 반복
    // Start is called before the first frame update
    void Start()
    {
        target.DOFade(0.8f, fadeTime)
              .SetLoops(loops, LoopType.Yoyo)
              .SetEase(Ease.InOutSine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
