using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform = null;

    private float initialWidth = 0f;
    private float setWidth = 0f;
    private void Awake()
    {
        initialWidth = rectTransform.rect.width;
    }
    public void ModifyPercent(float percentValue)
    {
        setWidth = ( percentValue / 100 ) * initialWidth;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, setWidth);
    }
}
