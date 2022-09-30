using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [Range(0f,1f)] float  movementFactor = 0f;

    [SerializeField] float movementSpeed;
    [SerializeField] [Range(0,1)] float returningSpeed;

    bool isGrowing;
    bool stopper;
    private float waitTime = 0.5f;
    
    void Start()
    {
        startingPosition = transform.position; //current position
    }

    void Update()
    {
        // 0 -> 1은 빨리 증가하고, 1에서 잠시 대기 후 0 -> 1까지는 천천히 이동하도록 구현
        // 프레스기 느낌나게

        //movementFactor의 값이 0에서 1까지 일정하게 증가
        //돌아올때도 일정, 하지만 느리게

        //의도하지 않은 동작: stop이 동작하지 않는 경우가 있다
        // 만약 waitTime보다 moveMentFactor가 한쪽 끝으로 이동이 빠른경우 stop이 실행되지 않고있다. -> 타이밍이 안맞음
        

        if(isGrowing && !stopper)
        {
            movementFactor += movementSpeed * Time.deltaTime;
        }
        else if(!isGrowing & !stopper)
        {
            movementFactor -= movementSpeed * Time.deltaTime * returningSpeed;
        }
        
        if(movementFactor >= 1)
        {
            isGrowing = false;
            stopper = true;
            StartCoroutine(Stop());
        }
        else if(movementFactor <= 0)
        {
            isGrowing = true;
            stopper = true;
            StartCoroutine(Stop());
        }

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(waitTime);
        stopper = false;
    }
}
