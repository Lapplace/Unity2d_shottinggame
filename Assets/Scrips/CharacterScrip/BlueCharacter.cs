using UnityEngine;
using System.Collections;

public class BlueCharacter : CharacterBase
{
    [Header("Blue Skill - Time Freeze")]
    [SerializeField] private KeyCode freezeKey = KeyCode.F;
    [SerializeField] private float freezeDuration = 2f;
    [SerializeField] private float freezeCooldown = 8f;
    [SerializeField][Range(0.01f, 0.2f)] private float frozenTimeScale = 0.05f;
    private bool isFreezing;
    private float nextFreezeTime;
    private float previousTimeScale = 1f;
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

        previousTimeScale = Time.timeScale;
        Time.timeScale = frozenTimeScale;

        player.SetUseUnscaledMovement(true);
        player.SetAnimatorUseUnscaledTime(true);
        gun.SetUseUnscaledTime(true);

        yield return new WaitForSecondsRealtime(freezeDuration);

        gun.SetUseUnscaledTime(false);
        player.SetUseUnscaledMovement(false);
        player.SetAnimatorUseUnscaledTime(false);
        Time.timeScale = previousTimeScale;
        isFreezing = false;
    }
}
