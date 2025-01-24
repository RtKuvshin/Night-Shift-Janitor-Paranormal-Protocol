using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CharatcerAnimations : MonoBehaviour
{
    private const string IDLE1 = "Idle1";
    private const string IDLE2 = "Idle2";

    [SerializeField] public float idleTimeForAnimToStart = 3f;

    private Animator _animator;
    
    private float idleTimer;

    private bool isIdle = true;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        idleTimer = 0f;
    }

    private void Update()
    {
        CheckAnimationPlaying();
        HandleIdleAnimations();
        isIdle = true;
    }

    private void HandleIdleAnimations()
    {
        if (isIdle)
        {
            idleTimer += Time.deltaTime; 
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
        
            if (horizontal == 0 && vertical == 0)
            {
                if (idleTimer >= idleTimeForAnimToStart)
                {
                    PlayRandomIdleAnimation();
                    idleTimer = 0f; 
                }
            }
            else
            {
                idleTimer = 0f; 
            }
        }
    }

    private void PlayRandomIdleAnimation()
    {
        int randomIdle = Random.Range(0, 2); 
        if (randomIdle == 0)
        {
            _animator.SetTrigger(IDLE1);
        }
        else
        {
            _animator.SetTrigger(IDLE2);
        }
        isIdle = false;
    }

    private void CheckAnimationPlaying()
    {
        AnimatorStateInfo currentState = _animator.GetCurrentAnimatorStateInfo(0);
        
        if (currentState.IsName("Idle1"))
        {
            idleTimer = 0f;
        }
        else if(currentState.IsName("Idle2"))
        {
            idleTimer = 0f;
        }
    }
}
