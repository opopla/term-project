using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public int stageIndex;
    public int Health;
    public static string endingtime;

    public PlayerMove player;
    public MonsterMove monster;
    public GameObject[] Stages;
    public GameObject menuset;
    public GameObject rankset;
    
    public Image[] UIhealth;
    public Text UIstages;
    public Text timeText;
    public string SceneLoad;
    private float time;

    public void Update()
    {
        time += Time.deltaTime;
        timeText.text = time.ToString();
        timeText.text = string.Format("{0:N2}", time);

        if (Input.GetButtonDown("Cancel"))
        {
            if (menuset.activeSelf)
                menuset.SetActive(false);
            else
                menuset.SetActive(true);
        }
    }

    public void NextStage()
    {
        //Change Stage
        if (stageIndex < Stages.Length-1)
        { 
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
            UIstages.text = "STAGE " + (stageIndex + 1);
        }
        else // Game Clear
        {
            endingtime = time.ToString();
            //Player Control Lock
            Time.timeScale = 0;
            //Result UI
            Debug.Log("게임 클리어!");
            rankset.SetActive(true);
            //Restart Button UI

        }
    }

    public void HealthDown()
    {
        if (Health > 1)
        {
            Health--;
            UIhealth[Health].color = new Color(0, 0, 0, 0.4f);
        }
        else
        {
            UIhealth[0].color = new Color(0, 0, 0, 0.4f);
            //Player Die Effect
            player.OnDie();
            //Result UI
            Debug.Log("죽었습니다.");
            //Retry Button UI
            Invoke("Restart", 1);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Player Reposition
            if (Health > 1)
            {
                PlayerReposition();
            }
                

            //Health Down
           
            HealthDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, -1);
        player.VelocityZero();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void HealthUp()
    {
        Health++;
        if (Health == 2)
            UIhealth[1].color = new Color(255, 255, 255, 255);
        else if (Health == 3)
            UIhealth[2].color = new Color(255, 255, 255, 255);
    }

    public void BackMain()
    {
        SceneManager.LoadScene(SceneLoad);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    
}