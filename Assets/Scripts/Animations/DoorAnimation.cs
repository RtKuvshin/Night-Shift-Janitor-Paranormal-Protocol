using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    private Animator _animator;
    private bool isAnimating = false; 
    private bool isDoorOpen = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnUsed()
    {
        if (!isAnimating)
        {
            if (!isDoorOpen)
            {
                _animator.SetTrigger("Opened");
                isDoorOpen = true;
            }
            else if (isDoorOpen)
            {
                _animator.SetTrigger("Closed");
                isDoorOpen = false;
            } 
            isAnimating = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnUsed();
            isAnimating = false;
        }
    }
}