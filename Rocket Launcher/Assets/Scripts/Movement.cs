using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem rocketJetParticle;
    [SerializeField] ParticleSystem rightThrusterParticle;
    [SerializeField] ParticleSystem leftThrusterParticle;
    [SerializeField] ParticleSystem midThrusterParticle;

    Rigidbody myRigidbody;
    AudioSource audioSource;



    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();       
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }

    }

    void StartThrusting()
    {
        myRigidbody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        if (!rocketJetParticle.isPlaying)
        {
            rocketJetParticle.Play();
        }
    }

    void StopThrusting()
    {
        audioSource.Stop();
        rocketJetParticle.Stop();
    }


    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            PlayLeftParticle();

        }
        else if (Input.GetKey(KeyCode.D))
        {
            PlayRightParticle();
        }
        else
        {
            StopParticles();
        }
    }

    void PlayLeftParticle()
    {
        ApplyRotation(rotationSpeed);
        if (!rightThrusterParticle.isPlaying)
        {
            rightThrusterParticle.Play();
        }
    }

    void PlayRightParticle()
    {
        ApplyRotation(-rotationSpeed);
        if (!midThrusterParticle.isPlaying)
        {
            midThrusterParticle.Play();
        }
        if (!leftThrusterParticle.isPlaying)
        {
            leftThrusterParticle.Play();
        }
    }

    void StopParticles()
    {
        rightThrusterParticle.Stop();
        leftThrusterParticle.Stop();
        midThrusterParticle.Stop();
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        myRigidbody.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        myRigidbody.freezeRotation = false;
    }
}
