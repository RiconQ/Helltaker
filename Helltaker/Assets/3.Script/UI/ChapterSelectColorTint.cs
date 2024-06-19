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
        // ��ư�� ���õǾ��� �� �ؽ�Ʈ ������ ���õ� �������� ����
        buttonText.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // ��ư�� ���õ��� �ʾ��� �� �ؽ�Ʈ ������ ���� �������� ����
        buttonText.color = normalColor;
    }
}
