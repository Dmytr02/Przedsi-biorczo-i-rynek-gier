using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject ContuneButton;
    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/first.save"))
        {
            ContuneButton.SetActive(true);
        }
    }

    public void OnContinue()
    {
        SaveSystem.load("first.save");
        SceneManager.LoadScene("Game");
    }

    public void onNewGmae()
    {
        SaveSystem.saveFile = new SaveFile();
        SceneManager.LoadScene("Game");
    }
}
