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
        // tìm đối tượng cine trên scene , hàm này sẽ có tác dụng ghim cam follow character.
    }
    // Các hằng số để lưu trữ trong PlayerPrefs
    private const string KeyCharacter = "selected_character";
    private const string KeyDamageLevelPrefix = "upgrade_damage_char_";
    private const string KeyHpLevelPrefix = "upgrade_hp_char_";
    private const string KeyBaseDamagePrefix = "char_base_damage_";
    private const string KeyBaseHpPrefix = "char_base_hp_";
    private const string KeyMoveSpeedPrefix = "char_move_speed_";

    private void Start()
    {
        if (characterPrefabs == null || characterPrefabs.Length == 0)
        {
            Debug.LogError("Character prefabs are not assigned.");
            return;
        }

        int selectedIndex = Mathf.Clamp(PlayerPrefs.GetInt(KeyCharacter, 0), 0, characterPrefabs.Length - 1);   // lấy ra vị trí char đã lưu trong PlayerPrefs, nếu không có thì mặc định là 0 (char đầu tiên)
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : transform.position;
        Quaternion spawnRotation = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity; // rotation
        // 2 dòng lấy vị trí sinh của char, nếu spawnPoint được gán thì lấy vị trí và góc quay của nó, nếu không thì lấy vị trí và góc quay mặc định của đối tượng đã gắn scrips
        GameObject spawnedCharacter = Instantiate(characterPrefabs[selectedIndex], spawnPosition, spawnRotation,spawnPoint); // con của spawnPoint để dễ dàng quản lý vị trí sinh của char
        CharacterBase character = spawnedCharacter.GetComponent<CharacterBase>(); // gắn scrips CharacterBase vào char
        if (character == null)
        {
            Debug.LogError("Selected prefab does not have CharacterBase script.");
            return;
        }

        BindCamera(spawnedCharacter.transform);

        int hpLevel = PlayerPrefs.GetInt(KeyHpLevelPrefix + selectedIndex, 0);
        int damageLevel = PlayerPrefs.GetInt(KeyDamageLevelPrefix + selectedIndex, 0);

        float baseHp = PlayerPrefs.GetFloat(KeyBaseHpPrefix + selectedIndex, 100f);
        float baseDamage = PlayerPrefs.GetFloat(KeyBaseDamagePrefix + selectedIndex, 10f);
        float moveSpeed = PlayerPrefs.GetFloat(KeyMoveSpeedPrefix + selectedIndex, 5f);

        character.ApplyLoadoutWithBase(baseHp, baseDamage, moveSpeed, hpLevel, damageLevel, hpPerLevel, damagePerLevel);
    }
}
