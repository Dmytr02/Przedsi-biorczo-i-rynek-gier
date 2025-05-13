using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PouseManuManager : MonoBehaviour
{
    [SerializeField] private GameObject pouseManu;
    [SerializeField] private GameObject pouseSettingManu;
    public static bool isPoused = false;

    private void Start()
    {
        Time.timeScale = 1;
        isPoused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pouse();
        }
    }

    public void pouse()
    {
        pouseSettingManu.SetActive(false);
        pouseManu.SetActive(!pouseManu.activeSelf);
        if (pouseManu.activeSelf)
        {
            isPoused = true;
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            isPoused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OnMainMenu()
    {
        SaveSystem.save("first.save");
        SceneManager.LoadScene("SampleScene");
    }
}
