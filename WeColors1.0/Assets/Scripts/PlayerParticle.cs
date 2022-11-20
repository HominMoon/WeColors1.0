using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticle : MonoBehaviour
{
    [SerializeField] ParticleSystem respawnAreaParticle;
    [SerializeField] ParticleSystem game2FinishParticle;
    [SerializeField] ParticleSystem itemParticle;
    [SerializeField] ParticleSystem damagedParticle;
    [SerializeField] ParticleSystem speedUpParticle;

    [SerializeField] AudioClip damagedSFX;
    [SerializeField] AudioClip itemSFX;

    AudioSource audioSource;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Respawn")
        {
            respawnAreaParticle.Play();
        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Finish")
        {
            game2FinishParticle.Play();
        }
        else if(other.gameObject.tag == "Item")
        {
            itemParticle.Play();

        }
        else if(other.gameObject.tag == "BossAttack")
        {
            damagedParticle.Play();
            audioSource.PlayOneShot(damagedSFX);
        }
        else if(other.gameObject.tag == "SpeedUp")
        {
            speedUpParticle.Play();
        }
    }
}
