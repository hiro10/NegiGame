using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public NegikoController negiko;

    public Text scoreText;

    public LifePanel lifePanel;


    // Update is called once per frame
    void Update()
    {
        int score = CalcScore();
        scoreText.text = "SCORE : " + score + "m";

        // ライフパネルの更新
        lifePanel.UpdateLife(negiko.Life());

        // ゲームオーバー処理
        if(negiko.Life() <= 0)
        {
            //これ以降のupdateは止める
            enabled = false;

            if(PlayerPrefs.GetInt("HightScore")<score)
            {
                PlayerPrefs.SetInt("HightScore", score);
            }

            // 2秒後にRetrurnTitleを呼ぶ
            Invoke("ReturnToTitle", 2.0f);
        }
    }

    int CalcScore()
    {
        return (int)negiko.transform.position.z;
    }

    private void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }
}
