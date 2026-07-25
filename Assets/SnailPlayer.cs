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
    public MeshRenderer ShellMaterial;
    public GameObject bandaid1;
    public GameObject bandaid2;
    private Transform _playerTransform;

    public Texture HealthyTexture_body;
    public Texture DamagedTexture1_body;
    public Texture HealthyTexture_shell;
    public Texture DamagedTexture1_shell;


    [SerializeField] private AudioManager MyAudioManager;

    private void Start()
    {
        _playerTransform = GetComponent<Transform>();
    }
    public void Move()
    {
        PlayerAnimator.SetInteger("state", 1);
    }
    public void Shoot(float time)
    {
        PlayerAnimator.SetInteger("state", 2);

        if (time > 10000)
        {
            StartCoroutine(FlashShootVFX());
        }
        else
        {
            //TODO: do whif sound, "click" no shot. 
        }
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
                ShellMaterial.material.SetTexture("_BaseMap", HealthyTexture_shell);
                bandaid1.SetActive(false);
                bandaid2.SetActive(false);
                break;
            case 1:
                BodyMaterial.material.SetTexture("_BaseMap", DamagedTexture1_body);
                ShellMaterial.material.SetTexture("_BaseMap", DamagedTexture1_shell);
                bandaid1.SetActive(true);
                bandaid2.SetActive(true);
                break;
        }
    }
}
