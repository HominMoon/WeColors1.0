using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviourPun
{
    //요약: 가상 카메라를 플레이어에게 세팅해준다.

    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    Scene scene;

    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();

        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        if (!photonView.IsMine) { return; }

        cinemachineVirtualCamera.Follow = transform;
        var transposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        cinemachineVirtualCamera.transform.rotation = Quaternion.Euler(45,0,0);
        transposer.m_FollowOffset = new Vector3(0, 6, -6);

        if(scene.name == "Game2") { return; }

        if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            cinemachineVirtualCamera.transform.rotation = Quaternion.Euler(45,180,0);
            //transform.rotation = Quaternion.Euler(0,180,0); // 플레이어2의 방향을 180도 돌린다
            transposer.m_FollowOffset = new Vector3(0, 6, 6); //플레이어 RigidBody Rotate의 y값도 고정했기 때문에 카메라 직접 돌려줘야함
        }

    }
}
