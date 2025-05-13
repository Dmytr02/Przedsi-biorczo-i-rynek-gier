using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsSave", menuName = "my/SettingsSave", order = 1)]
public class SettingsSave : ScriptableObject
{
    public float volume;
    public float FXVolume;
    public float musicVolume;
}
