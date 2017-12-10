using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(RectMask2D))]
[RequireComponent(typeof(Image))]
public class ViewComponent : MonoBehaviour
{
    public ViewHelper.Origin Origin { get; set; }

    public ScrollRect ScrollRect { get; private set; }
    public RectTransform RectTransform { get; private set; }
    public RectTransform Content { get; private set; }
    public RectMask2D RectMask2D { get; private set; }
    public Image Image { get; private set; }

    public delegate void ScrollDelegate(Vector2 _anchoredPosition);
    public event ScrollDelegate ScrollEvent;
    private void OnScroll(Vector2 _anchoredPosition) { if (ScrollEvent != null) ScrollEvent(_anchoredPosition); }

    private void Awake()
    {
        RectTransform = gameObject.GetComponent<RectTransform>();
        ScrollRect = gameObject.GetComponent<ScrollRect>();
        RectMask2D = gameObject.GetComponent<RectMask2D>();
        Image = gameObject.GetComponent<Image>();

        var go = new GameObject();
        go.name = "Content";

        Content = go.AddComponent<RectTransform>();
        Content.SetParent(RectTransform, false);

        Image.color = Color.clear;

        ScrollRect.content = Content;

        ScrollRect.onValueChanged.AddListener(OnScrollRectValueChanged);
    }

    public void SetAnchor(ViewHelper.Origin _origin)
    {
        Origin = _origin;

        ScrollRect.SetAnchor(Origin);

        SetContentRectTransform(RectTransform.rect.width, RectTransform.rect.height);
    }

    void OnScrollRectValueChanged(Vector2 _value)
    {
        OnScroll(Content.anchoredPosition);
    }

    public void SetContentRectTransform(float _deltaX, float _deltaY)
    {
        Content.SetAnchor(Origin);

        Content.sizeDelta = new Vector2(_deltaX, _deltaY);
    }
}
