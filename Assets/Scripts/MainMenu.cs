using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string SceneLoad;
    public GameObject rankback;
    public Image[] UIKnight;

    public void LoadScene()
    {
        //UIKnight[Health].color = new Color(0, 0, 0, 0.4f);
        SceneManager.LoadScene(SceneLoad);
    }

    public void isExit()
    {
        Application.Quit();
    }

    public void Ranking()
    {
        rankback.SetActive(true);
    }

    public void RankingBack()
    {
        rankback.SetActive(false);
    }
}
