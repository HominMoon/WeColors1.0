using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField] RectTransform lever;
    [SerializeField] Canvas canvas;
    [SerializeField, Range(10, 150)] float leverRange;

    RectTransform rectTransform;

    bool isInput;
    public Vector2 inputDirection;
    public Vector2 rotateDirection;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update() 
    {
        // Debug.Log(isInput);
        // if(isInput)
        // {
        //     PlayerMove();
        // }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        LeverMove(eventData);
        isInput = true;
    }

    //오브젝트 클릭후 드래그 하는중에 이벤트 호출, 
    //근데 클릭 유지한 상태로 안움직이면 이벤트가 안들어온다.
    public void OnDrag(PointerEventData eventData)
    {
        LeverMove(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        rotateDirection = inputDirection;
        inputDirection = Vector2.zero;
        isInput = false;
    }

    public void LeverMove(PointerEventData eventData)
    {
        var scaledAnchoredPosition = rectTransform.anchoredPosition * canvas.transform.localScale.x;

        var inputPos = eventData.position - scaledAnchoredPosition;
        var inputVector = inputPos.magnitude < leverRange ? inputPos : inputPos.normalized * leverRange;
        lever.anchoredPosition = inputVector;

        inputDirection = inputVector / leverRange; //inputVector는 너무 큰 값, 해상도에 따라 크기가 바뀌기 때문에
        rotateDirection = inputDirection;
        //Debug.Log(inputDirection.x + " " + inputDirection.y);
    }

}
