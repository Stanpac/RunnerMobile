using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AdaptToSafeArea : MonoBehaviour
{
    private RectTransform _tRect;

    private void Awake()
    {
        _tRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        _tRect.offsetMin = new Vector2(Screen.safeArea.xMin, Screen.safeArea.yMin);
        _tRect.sizeDelta = new Vector2(Screen.safeArea.width, Screen.safeArea.height);
    }
}
