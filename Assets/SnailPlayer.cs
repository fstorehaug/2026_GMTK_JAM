using System;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;
using TMPro;
using System.Collections;

public class SnailPlayer : MonoBehaviour
{
    public Animator PlayerAnimator;
    public GameObject PlayerShootVFX;
    public GameObject PlayerShootLine;
    public GameObject PlayerSplat;
    public SkinnedMeshRenderer BodyMaterial;
    public SkinnedMeshRenderer ShellMaterial;
    public GameObject bandaids;
    private Transform _playerTransform;

    public Texture HealthyTexture_body;
    public Texture DamagedTexture1_body;
  

    [SerializeField] private AudioManager MyAudioManager;

    private void Start()
    {
        _playerTransform = GetComponent<Transform>();
    }
    public void Move()
    {
        PlayerAnimator.SetInteger("state", 1);
    }
    public void Shoot()
    {
        PlayerAnimator.SetInteger("state", 2);
     
        StartCoroutine(FlashShootVFX());
    }

    private IEnumerator FlashShootVFX()
    {
        MyAudioManager.playAudio(0);
        MyAudioManager.playAudio(2);
        MyAudioManager.stopAudio(4);
        yield return new WaitForSeconds(0.05f);
        
        PlayerShootVFX.SetActive(true);
        PlayerShootLine.SetActive(true);

    }

    public void GetShot()
    {
        PlayerAnimator.SetInteger("state", 3);
        PlayerSplat.SetActive(true);
    }

    public void TurnAround(float turnDegrees)
    {
        
        Quaternion extraRotation = Quaternion.Euler(0, 180, 0);
        _playerTransform.rotation = extraRotation * _playerTransform.rotation;

    }

    public void updateVisuals(int i)
    {
        switch (i)
        {
            case 0:
                BodyMaterial.material.SetTexture("_BaseMap", HealthyTexture_body);
                break;
            case 1:
                BodyMaterial.material.SetTexture("_BaseMap", DamagedTexture1_body);
                break;
        }
    }
}
