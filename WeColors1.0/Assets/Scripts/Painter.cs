using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Painter : MonoBehaviourPun
{
    [SerializeField] ParticleSystem paintParticle;
    [SerializeField] AudioClip paintSFX;

    AudioSource audioSource;

    private void Start()
    {
        if (!photonView.IsMine) { return; }
        
        audioSource.GetComponent<AudioSource>();
        ParticleSystem.MainModule settings = paintParticle.main;

        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            settings.startColor = Color.red;
        }
        else if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
        {
            settings.startColor = Color.blue;
        }
    }

    private void OnEnable()
    {
        paintParticle.Play();
        audioSource.PlayOneShot(paintSFX);
    }
}
