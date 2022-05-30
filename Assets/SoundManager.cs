using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Usable;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _kalashSound;
    [SerializeField] private AudioSource _pistolSound;
    [SerializeField] private AudioSource _shotgunSound;
    [SerializeField] private AudioSource _minigunSound;

    private void PlayKalash() => _kalashSound.Play();
    private void PlayPistol() => _pistolSound.Play();
    private void PlayShotgun() => _shotgunSound.Play();
    private void PlayMinigun() => _minigunSound.Play();

    public void PlaySound(WeaponType _type)
    {
        switch (_type)
        {
            case WeaponType.Kalash:
            {
                PlayKalash();
                break;
            }
            case WeaponType.Minigun:
            {
                PlayMinigun();
                break;
            }
            case WeaponType.Shotgun:
            {
                PlayShotgun();
                break;
            }
            case WeaponType.Pistol:
                PlayPistol();
                break;
        }
    }
}
