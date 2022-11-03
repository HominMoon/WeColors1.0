using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

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
    GameObject platformUnderPlayer;
    Vector3 distanceofPlatform;

    bool isJump = false;
    bool isAttack = false;
    public bool isMove = false;
    bool isOnPlatform = false;

    public int playerHealth = 3;

    [SerializeField] GameObject painter;

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
        OnPlatformMovement();
    }

    private void OnPlatformMovement()
    {
        if(!isOnPlatform || isMove) { return; }

        transform.position = platformUnderPlayer.transform.position - distanceofPlatform;
    }

    public void Move(Vector2 inputDirection)
    {
        if (!photonView.IsMine)
        {
            return;
        }

        isMove = inputDirection.magnitude != 0;

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

    public void Attack()
    {
        if (!photonView.IsMine || isAttack == true)
        {
            return;
        }

        isAttack = true;
        Paint();
    }

    public void Paint()
    {
        StartCoroutine(RPaint());
    }

    IEnumerator RPaint()
    {
        painter.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        isAttack = false;
        painter.SetActive(false);
    }

    public void PlayerRelease()
    {
        Rigidbody m_Rigidbody;
        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.constraints = RigidbodyConstraints.None;
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX |
                                RigidbodyConstraints.FreezeRotationY |
                                RigidbodyConstraints.FreezeRotationZ;
    }

    public void PlayerStop()
    {
        Rigidbody m_Rigidbody;
        m_Rigidbody = GetComponent<Rigidbody>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "BossAttack" && photonView.IsMine)
        {
            playerHealth--;
        }
    }

    void OnCollisionStay(Collision other)
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

        if(other.gameObject.tag == "Platform")
        {
            if(!isMove)
            {
                transform.position = transform.position;
            }

            platformUnderPlayer = other.gameObject;
            isOnPlatform = true;
            distanceofPlatform = platformUnderPlayer.transform.position - transform.position;
        }

    }

    void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "Platform")
        {
            isOnPlatform = false;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (!photonView.IsMine)
        {
            return;
        }
        if(other.gameObject.tag == "MZone")
        {
            isJump = false;
            GetComponent<Rigidbody>().AddForce(Vector3.up * MoonGravity.Instance.gravityForce , ForceMode.Impulse);
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
