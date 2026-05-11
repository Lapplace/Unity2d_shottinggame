using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SelectionChar : MonoBehaviour
{
    private int index;
    [SerializeField] private GameObject[] chars;
    [SerializeField] private TextMeshProUGUI charName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        index = 0;
        SelectChars();
    }
    public void OnSelectBtnClick()
    {
        SceneManager.LoadScene(1);
    }
    public void OnRightBtnClick()
    {
        if (index > 0) index--;
        SelectChars();
    }
    public void OnLeftBtnClick()
    {
        if (index < chars.Length - 1) index++;
        SelectChars();
    }
    public void SelectChars()
    {
        for (int i = 0; i < chars.Length; i++)
        {
            if (i == index)
            {
                chars[i].GetComponent<SpriteRenderer>().color = Color.white;
                chars[i].GetComponent<Animator>().enabled = true;
                charName.text = chars[i].name;
            }
            else
            {
                chars[i].GetComponent<SpriteRenderer>().color = Color.black;
                chars[i].GetComponent<Animator>().enabled = false;
            }
        }
    }
}