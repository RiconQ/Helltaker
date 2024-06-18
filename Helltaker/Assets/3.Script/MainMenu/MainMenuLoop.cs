using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuLoop : MonoBehaviour
{
    //[SerializeField] private float width;
    //[SerializeField] private RectTransform rect;
    //
    //private void Start()
    //{
    //    width = transform.GetComponent<BoxCollider2D>().size.x/100;
    //    rect = this.GetComponent<RectTransform>();
    //}
    //
    //
    //private void Update()
    //{
    //    Debug.Log(rect.position.x);
    //    if (rect.position.x >= width)
    //    {
    //        //offset -> vector2 좌표를 만들어서 현재 transform에 더함
    //        Vector2 offset = new Vector2(width * 2, 0);
    //        rect.position = (Vector2)transform.position - offset;
    //    }
    //}
    [SerializeField] private RawImage _img;
    [SerializeField] private float _x, _y;

    // Update is called once per frame
    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
    }
}
