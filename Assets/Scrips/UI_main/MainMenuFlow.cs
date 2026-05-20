using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuFlow : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject panelMenuMain;
    [SerializeField] private GameObject panelCharSelection;
    [SerializeField] private GameObject panelStage;
    [SerializeField] private GameObject panelUpgrade;
    [SerializeField] private GameObject panelOptions;
    [SerializeField] private GameObject selectChar;

    [Header("Character")]
    [SerializeField] private GameObject[] characterPreviewObjects;
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private string[] characterNames;

    [Header("Stage")]
    [SerializeField] private string[] stageSceneNames;

    [Header("Upgrade")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text damageLevelText;
    [SerializeField] private TMP_Text hpLevelText;
    [SerializeField] private TMP_Text upgradeCharacterText;
    [SerializeField] private TMP_Text upgradeMessageText;
    [SerializeField] private TMP_Text damageValueText;
    [SerializeField] private TMP_Text hpValueText;
    [SerializeField] private int damageUpgradeCost = 50;
    [SerializeField] private int hpUpgradeCost = 50;
    [SerializeField] private CharacterData[] characterBaseStats;
    [SerializeField] private float damagePerLevel = 2f;
    [SerializeField] private float hpPerLevel = 20f;

    private int currentCharacterIndex;
    private int upgradeCharacterIndex;

    private const string KeyCharacter = "selected_character";
    private const string KeyStage = "selected_stage";
    private const string KeyCoin = "coin";
    private const string KeyDamageLevelPrefix = "upgrade_damage_char_";
    private const string KeyHpLevelPrefix = "upgrade_hp_char_";
    private const string KeyBaseDamagePrefix = "char_base_damage_";
    private const string KeyBaseHpPrefix = "char_base_hp_";
    private const string KeyMoveSpeedPrefix = "char_move_speed_";


    private int GetUpgradeCharacterIndex()
    {
        return Mathf.Clamp(upgradeCharacterIndex, 0, Mathf.Max(0, characterPreviewObjects.Length - 1));
    }

    private string GetDamageKey(int characterIndex) => KeyDamageLevelPrefix + characterIndex;
    private string GetHpKey(int characterIndex) => KeyHpLevelPrefix + characterIndex;

    private string GetBaseDamageKey(int characterIndex) => KeyBaseDamagePrefix + characterIndex;
    private string GetBaseHpKey(int characterIndex) => KeyBaseHpPrefix + characterIndex;
    private string GetMoveSpeedKey(int characterIndex) => KeyMoveSpeedPrefix + characterIndex;

    private void InitializeCharacterStatsPrefs()
    {
        if (characterBaseStats == null)
        {
            return;
        }

        int count = Mathf.Min(characterBaseStats.Length, characterPreviewObjects.Length);
        for (int i = 0; i < count; i++)
        {
            CharacterData stat = characterBaseStats[i];
            if (stat == null)
            {
                continue;
            }

            string hpKey = GetBaseHpKey(i);
            string damageKey = GetBaseDamageKey(i);
            string moveSpeedKey = GetMoveSpeedKey(i);

            if (!PlayerPrefs.HasKey(hpKey))
            {
                PlayerPrefs.SetFloat(hpKey, stat.baseHp);
            }

            if (!PlayerPrefs.HasKey(damageKey))
            {
                PlayerPrefs.SetFloat(damageKey, stat.baseDamage);
            }

            if (!PlayerPrefs.HasKey(moveSpeedKey))
            {
                PlayerPrefs.SetFloat(moveSpeedKey, stat.moveSpeed);
            }
        }

        PlayerPrefs.Save();
    }

    private float GetBaseDamage(int characterIndex)
    {
        return PlayerPrefs.GetFloat(GetBaseDamageKey(characterIndex), 10f);
    }

    private float GetBaseHp(int characterIndex)
    {
        return PlayerPrefs.GetFloat(GetBaseHpKey(characterIndex), 100f);
    }

    private void Start()
    {
        InitializeCharacterStatsPrefs();
        currentCharacterIndex = Mathf.Clamp(PlayerPrefs.GetInt(KeyCharacter, 0), 0, characterPreviewObjects.Length - 1);
        upgradeCharacterIndex = Mathf.Clamp(PlayerPrefs.GetInt(KeyCharacter, 0), 0, Mathf.Max(0, characterPreviewObjects.Length - 1));
        ShowMainMenu();
        RefreshCharacterPreview();
        RefreshUpgradeUI();
        ClearUpgradeMessage();
    }

    public void OnClickNewGame()
    {
        OpenPanel(panelCharSelection);
    }

    public void OnClickUpgrade()
    {
        OpenPanel(panelUpgrade);
        upgradeCharacterIndex = Mathf.Clamp(PlayerPrefs.GetInt(KeyCharacter, 0), 0, Mathf.Max(0, characterPreviewObjects.Length - 1));
        RefreshUpgradeUI();
        ClearUpgradeMessage();
    }

    public void OnClickOptions()
    {
        OpenPanel(panelOptions);
    }

    public void OnClickExit()
    {
        Application.Quit();
        Debug.Log("Exit game requested.");
    }

    public void BackToMainMenu()
    {
        upgradeCharacterIndex = Mathf.Clamp(PlayerPrefs.GetInt(KeyCharacter, 0), 0, Mathf.Max(0, characterPreviewObjects.Length - 1));
        ShowMainMenu();
    }

    public void OnCharacterLeft()
    {
        currentCharacterIndex--;
        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = characterPreviewObjects.Length - 1;
        }
        RefreshCharacterPreview();
    }

    public void OnCharacterRight()
    {
        currentCharacterIndex++;
        if (currentCharacterIndex >= characterPreviewObjects.Length)
        {
            currentCharacterIndex = 0;
        }
        RefreshCharacterPreview();
    }

    public void OnCharacterSelect()
    {
        PlayerPrefs.SetInt(KeyCharacter, currentCharacterIndex);
        PlayerPrefs.Save();
        OpenPanel(panelStage);
    }

    public void OnSelectStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stageSceneNames.Length)
        {
            Debug.LogError($"Stage index {stageIndex} is out of range.");
            return;
        }

        PlayerPrefs.SetInt(KeyStage, stageIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(stageSceneNames[stageIndex]);
    }


    public void OnUpgradeCharacterLeft()
    {
        upgradeCharacterIndex--;
        if (upgradeCharacterIndex < 0)
        {
            upgradeCharacterIndex = Mathf.Max(0, characterPreviewObjects.Length - 1);
        }
        RefreshUpgradeUI();
        ClearUpgradeMessage();
    }

    public void OnUpgradeCharacterRight()
    {
        upgradeCharacterIndex++;
        if (upgradeCharacterIndex >= characterPreviewObjects.Length)
        {
            upgradeCharacterIndex = 0;
        }
        RefreshUpgradeUI();
        ClearUpgradeMessage();
    }

    public void UpgradeDamage()
    {
        int coin = PlayerPrefs.GetInt(KeyCoin, 0);
        if (coin < damageUpgradeCost)
        {
            SetUpgradeMessage($"Not enough coin. Need {damageUpgradeCost}.");
            return;
        }

        coin -= damageUpgradeCost;
        int characterIndex = GetUpgradeCharacterIndex();
        string damageKey = GetDamageKey(characterIndex);
        int level = PlayerPrefs.GetInt(damageKey, 0) + 1;
        PlayerPrefs.SetInt(KeyCoin, coin);
        PlayerPrefs.SetInt(damageKey, level);
        PlayerPrefs.Save();
        RefreshUpgradeUI();
        SetUpgradeMessage("Damage upgraded successfully!");
    }

    public void UpgradeHp()
    {
        int coin = PlayerPrefs.GetInt(KeyCoin, 0);
        if (coin < hpUpgradeCost)
        {
            SetUpgradeMessage($"Not enough coin. Need {hpUpgradeCost}.");
            return;
        }

        coin -= hpUpgradeCost;
        int characterIndex = GetUpgradeCharacterIndex();
        string hpKey = GetHpKey(characterIndex);
        int level = PlayerPrefs.GetInt(hpKey, 0) + 1;
        PlayerPrefs.SetInt(KeyCoin, coin);
        PlayerPrefs.SetInt(hpKey, level);
        PlayerPrefs.Save();
        RefreshUpgradeUI();
        SetUpgradeMessage("HP upgraded successfully!");
    }

    private void ShowMainMenu()
    {
        OpenPanel(panelMenuMain);
    }

    private void OpenPanel(GameObject target)
    {
        panelMenuMain.SetActive(target == panelMenuMain);
        panelCharSelection.SetActive(target == panelCharSelection);
        panelStage.SetActive(target == panelStage);
        panelUpgrade.SetActive(target == panelUpgrade);
        panelOptions.SetActive(target == panelOptions);
        selectChar.SetActive(target == panelCharSelection);
    }

    private void RefreshCharacterPreview()
    {
        for (int i = 0; i < characterPreviewObjects.Length; i++)
        {
            characterPreviewObjects[i].SetActive(i == currentCharacterIndex);
        }

        if (characterNameText != null && characterNames.Length > currentCharacterIndex)
        {
            characterNameText.text = characterNames[currentCharacterIndex];
        }
    }

    private void SetUpgradeMessage(string message)
    {
        if (upgradeMessageText != null)
        {
            upgradeMessageText.text = message;
        }
    }

    private void ClearUpgradeMessage()
    {
        SetUpgradeMessage(string.Empty);
    }

    private void RefreshUpgradeUI()
    {
        int characterIndex = GetUpgradeCharacterIndex();

        if (coinText != null)
        {
            coinText.text = $"Coin: {PlayerPrefs.GetInt(KeyCoin, 0)}";
        }

        if (upgradeCharacterText != null)
        {
            string displayName = (characterNames != null && characterNames.Length > characterIndex) ? characterNames[characterIndex] : $"Char {characterIndex}";
            upgradeCharacterText.text = $"Upgrade: {displayName}";
        }

        int damageLevel = PlayerPrefs.GetInt(GetDamageKey(characterIndex), 0);
        int hpLevel = PlayerPrefs.GetInt(GetHpKey(characterIndex), 0);

        if (damageLevelText != null)
        {
            damageLevelText.text = $"Damage Lv: {damageLevel}";
        }

        if (hpLevelText != null)
        {
            hpLevelText.text = $"HP Lv: {hpLevel}";
        }

        if (damageValueText != null)
        {
            float totalDamage = GetBaseDamage(characterIndex) + (damageLevel * damagePerLevel);
            damageValueText.text = $"Damage: {totalDamage:0.#}";
        }

        if (hpValueText != null)
        {
            float totalHp = GetBaseHp(characterIndex) + (hpLevel * hpPerLevel);
            hpValueText.text = $"HP: {totalHp:0.#}";
        }
    }
}
