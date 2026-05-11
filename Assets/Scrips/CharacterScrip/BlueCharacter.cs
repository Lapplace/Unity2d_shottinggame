using UnityEngine;
using System.Collections;

public class BlueCharacter : CharacterBase
{
    [Header("Blue Skill - Time Freeze")]
    [SerializeField] private KeyCode freezeKey = KeyCode.F;
    [SerializeField] private float freezeDuration = 2f;
    [SerializeField] private float freezeCooldown = 8f;

    private bool isFreezing;
    private float nextFreezeTime;

    private void Update()
    {
        if (Input.GetKeyDown(freezeKey) && Time.unscaledTime >= nextFreezeTime && !isFreezing)
        {
            StartCoroutine(FreezeTimeRoutine());
        }
    }

    private IEnumerator FreezeTimeRoutine()
    {
        isFreezing = true;
        nextFreezeTime = Time.unscaledTime + freezeCooldown;

        Time.timeScale = 0f;
        player.SetUseUnscaledMovement(true);
        player.SetAnimatorUseUnscaledTime(true);
        gun.SetUseUnscaledTime(true);

        yield return new WaitForSecondsRealtime(freezeDuration);

        gun.SetUseUnscaledTime(false);
        player.SetUseUnscaledMovement(false);
        player.SetAnimatorUseUnscaledTime(false);
        Time.timeScale = 1f;
        isFreezing = false;
    }
}
