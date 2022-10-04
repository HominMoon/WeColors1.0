using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    //요약: 플레이어를 조이스틱의 벡터를 받아와 이동시킨다.
    //수정 필요: 플레이어의 충돌에 대한 내용이 같이 쓰여 있으므로 분리 필요

    Rigidbody rd;
    Transform tr;

    int playerNum = PhotonNetwork.LocalPlayer.ActorNumber;

    [SerializeField] float speed = 5f;
    [SerializeField] float jumpPower = 5f;
    GameObject joyStick;
    Vector2 playerDirection;

    bool isJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rd = GetComponent<Rigidbody>();
        joyStick = GameObject.Find("JoyStick");
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        playerDirection = joyStick.GetComponent<JoyStick>().inputDirection;
        Move(playerDirection);
    }

    public void Move(Vector2 inputDirection)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        bool isMove = inputDirection.magnitude != 0;

        Debug.Log(isMove);

        if (isMove)
        {
            float xVal = inputDirection.x * speed * Time.deltaTime;
            float zVal = inputDirection.y * speed * Time.deltaTime;

            transform.Translate(xVal, 0, zVal);
        }
    }

    public void Jump()
    {
        if (!photonView.IsMine || isJump == true)
        {
            return;
        }

        isJump = true;
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * jumpPower, ForceMode.Impulse); 
    }

    void OnCollisionEnter(Collision other)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        isJump = false;

        GameObject collidedGameObject = other.gameObject;

        if(other.gameObject.tag == "Cube")
        {
            collidedGameObject.GetComponent<CubeColor>().ChangeColor(playerNum);
        }

    }

    public void PlayerDash()
    {
        speed *= 2f;
        StartCoroutine(DashEnd());
    }

    IEnumerator DashEnd()
    {
        yield return new WaitForSeconds(5f);
        speed /= 2f;
    }

}
