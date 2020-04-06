using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public Text timerText;
    private float startTime;
    private bool finished = false;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (finished)
        {
            return;
        }
        float totalTime = Time.time - startTime;

        string minutes = ((int)totalTime / 60).ToString();
        string seconds = (totalTime % 60).ToString("f2");

        timerText.text = minutes + ":" + seconds;
    }
    public void Finished()
    {
        finished = true;
        timerText.color = Color.yellow;
    }
}
