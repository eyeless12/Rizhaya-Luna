using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public Animator _animator;

    public void SetRunAnimation(float condition)
    {
        _animator.SetBool("Running", condition != 0);
    }
}
