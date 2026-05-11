using Unity.Cinemachine;
using UnityEngine;

public class CharacterLoadoutLoader : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private float hpPerLevel = 20f;
    [SerializeField] private float damagePerLevel = 2f;
    [SerializeField] private CinemachineCamera followCamera;

    private void BindCamera(Transform target)
    {
        if (followCamera == null)
        {
            followCamera = FindFirstObjectByType<CinemachineCamera>();
        }

        if (followCamera == null || target == null)
        {
            return;
        }

        followCamera.Follow = target;
        followCamera.LookAt = target;
    }

    private const string KeyCharacter = "selected_character";
    private const string KeyDamageLevelPrefix = "upgrade_damage_char_";
    private const string KeyHpLevelPrefix = "upgrade_hp_char_";

    private void Start()
    {
        if (characterPrefabs == null || characterPrefabs.Length == 0)
        {
            Debug.LogError("Character prefabs are not assigned.");
            return;
        }

        int selectedIndex = Mathf.Clamp(PlayerPrefs.GetInt(KeyCharacter, 0), 0, characterPrefabs.Length - 1);
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion spawnRotation = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

        GameObject spawnedCharacter = Instantiate(characterPrefabs[selectedIndex], spawnPosition, spawnRotation);
        CharacterBase character = spawnedCharacter.GetComponent<CharacterBase>();
        if (character == null)
        {
            Debug.LogError("Selected prefab does not have CharacterBase script.");
            return;
        }

        BindCamera(spawnedCharacter.transform);

        int hpLevel = PlayerPrefs.GetInt(KeyHpLevelPrefix + selectedIndex, 0);
        int damageLevel = PlayerPrefs.GetInt(KeyDamageLevelPrefix + selectedIndex, 0);
        character.ApplyLoadout(hpLevel, damageLevel, hpPerLevel, damagePerLevel);
    }
}
