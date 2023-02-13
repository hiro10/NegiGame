using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        lifePanel.UpdateLife(negiko.Life());
    }

    int CalcScore()
    {
        return (int)negiko.transform.position.z;
    }
}
