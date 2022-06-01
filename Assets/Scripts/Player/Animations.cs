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

    public void SetDeadStatus(bool dead)
    {
        _animator.SetBool("Dead", dead);
    }
}
