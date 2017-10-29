using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ListViewHelper 
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
                    _rectTransform.anchorMin = new Vector2(0.5f, 1f);
                    _rectTransform.anchorMax = new Vector2(0.5f, 1f);
                    _rectTransform.pivot = new Vector2(0.5f, 1f);
                }
                break;
            case Origin.Bottom:
                {
                    _rectTransform.anchorMin = new Vector2(0.5f, 0f);
                    _rectTransform.anchorMax = new Vector2(0.5f, 0f);
                    _rectTransform.pivot = new Vector2(0.5f, 0f);
                }
                break;
            case Origin.Left:
                {
                    _rectTransform.anchorMin = new Vector2(0f, 0.5f);
                    _rectTransform.anchorMax = new Vector2(0f, 0.5f);
                    _rectTransform.pivot = new Vector2(0f, 0.5f);
                }
                break;
            case Origin.Right:
                {
                    _rectTransform.anchorMin = new Vector2(1f, 0.5f);
                    _rectTransform.anchorMax = new Vector2(1f, 0.5f);
                    _rectTransform.pivot = new Vector2(1f, 0.5f);
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
                _scrollRect.vertical = true;
                _scrollRect.horizontal = false;
                break;
            case Origin.Left:
            case Origin.Right:
                _scrollRect.vertical = false;
                _scrollRect.horizontal = true;
                break;
        }
    }
}
