using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleController : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI hightScoreText;

    private void Start()
    {
        hightScoreText.text = "HIGHT SCORE :" + PlayerPrefs.GetInt("HeightScore") + "m";
    }
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("Main");
    }
}
