using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class StartController : MonoBehaviour {

	// Use this for initialization
    public Transform Block;
    void Awake()
    {
    }
    void Start () {
        SocialManager.GetInstance().Start();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	}

    void FixedUpdate()
    {
        if (Block)
            Block.transform.position -= new Vector3(Time.deltaTime * 1, 0, 0);
    }

    public void StartGame()
    {
        Application.LoadLevel("Main");
    }
}
