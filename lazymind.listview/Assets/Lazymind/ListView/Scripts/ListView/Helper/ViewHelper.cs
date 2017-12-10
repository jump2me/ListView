﻿using UnityEngine;
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
        var topLeft = new Vector2(0f, 1f);
        var bottomLeft = Vector2.zero;
        var topRight = Vector2.one;
        var bottomRight = new Vector2(1f, 0f);

        switch (_origin)
        {
            case Origin.Top:
                {
                    _rectTransform.anchorMin = topLeft;
                    _rectTransform.anchorMax = topLeft;
                    _rectTransform.pivot = topLeft;
                }
                break;
            case Origin.Bottom:
                {
                    _rectTransform.anchorMin = bottomLeft;
                    _rectTransform.anchorMax = bottomLeft;
                    _rectTransform.pivot = bottomLeft;
                }
                break;
            case Origin.Left:
                {
                    _rectTransform.anchorMin = topLeft;
                    _rectTransform.anchorMax = topLeft;
                    _rectTransform.pivot = topLeft;
                }
                break;
            case Origin.Right:
                {
                    _rectTransform.anchorMin = topRight;
                    _rectTransform.anchorMax = topRight;
                    _rectTransform.pivot = topRight;
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
