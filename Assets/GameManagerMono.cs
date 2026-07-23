using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManagerMono : MonoBehaviour
{

    [HideInInspector] public Action GunBattleGo;

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            GunBattleGo?.Invoke();
        }

    }
}
