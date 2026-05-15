using UnityEngine;
using System.Collections;

public class HunterCharacter : CharacterBase
{
    [Header("Hunter Skill - Sprint")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private float sprintDuration = 2f;
    [SerializeField] private float sprintCooldown = 5f;

    private bool isSprinting;
    private float nextSprintTime;

    private void Update()
    {
        if (Input.GetKeyDown(sprintKey) && Time.time >= nextSprintTime && !isSprinting)
        {
            StartCoroutine(SprintRoutine());
        }
    }

    private IEnumerator SprintRoutine()
    {
        isSprinting = true;
        nextSprintTime = Time.time + sprintCooldown;
        player.SetSpeedMultiplier(sprintMultiplier);
        yield return new WaitForSeconds(sprintDuration);
        player.SetSpeedMultiplier(1f);
        isSprinting = false;
    }
}
