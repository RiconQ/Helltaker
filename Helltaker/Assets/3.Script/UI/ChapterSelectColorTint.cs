using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChapterSelectColorTint : MonoBehaviour
{
    private Text buttonText;
    private Color normalColor = Color.gray;
    private Color selectedColor = Color.white;

    private void Start()
    {
        buttonText = GetComponentInChildren<Text>();
        buttonText.color = Color.gray;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = selectedColor;
    }
    public void OnSelected(BaseEventData eventData)
    {
        // 버튼이 선택되었을 때 텍스트 색상을 선택된 색상으로 변경
        buttonText.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // 버튼이 선택되지 않았을 때 텍스트 색상을 원래 색상으로 변경
        buttonText.color = normalColor;
    }
}
