using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBar : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private float percentage = 100f;
    private float ratio;
    private Vector2 sizeDelta;
    private float barWidth;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        barWidth = rectTransform.rect.width;
    }

    void Start()
    {
        sizeDelta= rectTransform.sizeDelta;
    }

    void Update()
    {
        ratio = Mathf.Clamp01((100 - percentage) * 0.01f);
        rectTransform.sizeDelta = sizeDelta - ratio * barWidth * Vector2.right;
    }
}
