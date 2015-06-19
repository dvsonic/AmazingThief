using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainUIController : MonoBehaviour {

    public Text tfScore;
    public GameObject PausePanel;
    public GameObject PauseUI;
    public Text tfCountDown; 
	// Use this for initialization
	void Start () {
        GameData.score = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (tfScore)
            tfScore.text = GameData.score.ToString();
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (tfCountDown.enabled)
        {
            int passTime = Mathf.FloorToInt(Time.realtimeSinceStartup - realTime);
            int left = 3 - passTime;
            if(left>0)
                tfCountDown.text = left.ToString();
            else
            {
                Time.timeScale = 1;
                PausePanel.SetActive(false);
                tfCountDown.enabled = false;
            }
        }
	}

    public void PauseOrResume()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        PauseUI.SetActive(true);
        tfCountDown.enabled = false;
        realTime = 0;
    }

    private float realTime;
    void Resume()
    {
        realTime = Time.realtimeSinceStartup;
        tfCountDown.enabled = true;
        PauseUI.SetActive(false);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
        Canvas canvas = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
        if (canvas)
            canvas.SendMessage("DestroyAD", SendMessageOptions.DontRequireReceiver);
        GameData.score = 0;
        Application.LoadLevel("Start");
    }


    


}
