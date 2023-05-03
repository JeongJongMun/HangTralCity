using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEffect : MonoBehaviour
{

    AudioSource walkingEffect;

    private void Awake()
    {
        walkingEffect = GetComponent<AudioSource>();
    }

    private void Update()
    {
        WalkingSound();
    }

    void WalkingSound()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!walkingEffect.isPlaying)
            {
                walkingEffect.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!walkingEffect.isPlaying)
            {
                walkingEffect.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!walkingEffect.isPlaying)
            {
                walkingEffect.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!walkingEffect.isPlaying)
            {
                walkingEffect.Play();
            }
        }
        else
        {
            if (walkingEffect.isPlaying)
            {
                walkingEffect.Stop();
            }
        }    
    }
}
