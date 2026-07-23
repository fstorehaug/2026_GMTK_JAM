using System;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootManager : MonoBehaviour
{
    [SerializeField] private GameManagerMono gameManager;
    [SerializeField] private KeyEnum key;

    private bool moving;

    public void Start()
    {
        gameManager.GunBattleGo += OnGunBattleGo;
    }

    public void OnGunBattleGo()
    {

    }

    private void Update()
    {
        if (key == KeyEnum.AKey)
        {
            if (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame)
            {
                Shoot();
            }

        }

        if (key == KeyEnum.LKey)
        {
            if (Keyboard.current != null && Keyboard.current.lKey.wasPressedThisFrame)
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {

    }
}

public enum KeyEnum
{
    AKey, LKey
}
