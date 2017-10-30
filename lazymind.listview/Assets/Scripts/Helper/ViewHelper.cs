using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ViewHelper 
{
    public enum Origin
    {
        Top,
        Bottom,
        Left,
        Right,
    }

    public static void SetAnchor(this RectTransform _rectTransform, Origin _origin)
    {
        switch (_origin)
        {
            case Origin.Top:
                {
                    _rectTransform.anchorMin = new Vector2(0f, 1f);
                    _rectTransform.anchorMax = new Vector2(0f, 1f);
                    _rectTransform.pivot = new Vector2(0f, 1f);
                }
                break;
            case Origin.Bottom:
                {
                    _rectTransform.anchorMin = Vector2.zero;
                    _rectTransform.anchorMax = Vector2.zero;
                    _rectTransform.pivot = Vector2.zero;
                }
                break;
            case Origin.Left:
                {
                    _rectTransform.anchorMin = new Vector2(0f, 1f);
                    _rectTransform.anchorMax = new Vector2(0f, 1f);
                    _rectTransform.pivot = new Vector2(0f, 1f);
                }
                break;
            case Origin.Right:
                {
                    _rectTransform.anchorMin = Vector2.one;
                    _rectTransform.anchorMax = Vector2.one;
                    _rectTransform.pivot = Vector2.one;
                }
                break;
        }
    }

    public static void SetAnchor(this ScrollRect _scrollRect, Origin _origin)
    {
        switch (_origin)
        {
            case Origin.Top:
            case Origin.Bottom:
                {
                    _scrollRect.vertical = true;
                    _scrollRect.horizontal = false;    
                }
                break;
            case Origin.Left:
            case Origin.Right:
                {
                    _scrollRect.vertical = false;
                    _scrollRect.horizontal = true;    
                }
                break;
        }
    }
}
