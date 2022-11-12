using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class PlayerRotate : MonoBehaviourPun
{
    //요약: 사용자의 조이스틱 direction을 받아와서 플레이어 오브젝트에 적용한다
    //주의! 플레이어 오브젝트 하위 메시에 적용되고 있음! -> 마주보는 게임에서 사용
    //해결 필요: 조이스틱 direction이 0이되면 캐릭터가 정면만 본다.

    JoyStick joyStick;
    Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        joyStick = GameObject.Find("JoyStick").GetComponent<JoyStick>();

        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!photonView.IsMine)
        {
            return;
        }

        float xInput = joyStick.rotateDirection.x;
        float zInput = joyStick.rotateDirection.y;

        if(PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            xInput = -xInput;
            zInput = -zInput;
        }

        transform.forward = new Vector3(xInput, 0, zInput).normalized;
        
        if(scene.name == "Game2" && PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            transform.forward = new Vector3(xInput, 0, zInput).normalized;
        }
        else if(scene.name == "Game2" && PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            transform.forward = new Vector3(-xInput, 0, -zInput).normalized;
        }
    }
}
