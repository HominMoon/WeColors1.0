using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{
    //요약: 플레이어를 조이스틱의 벡터를 받아와 이동시킨다.
    //수정 필요: 플레이어의 충돌에 대한 내용이 같이 쓰여 있으므로 분리 필요


    private static PlayerMovement instance = null;

    private void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject); 
        }  
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static PlayerMovement Instance
    {
        get
        {
            if(instance == null)
            {
                return null;
            }
            return instance;
        }
    }


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
