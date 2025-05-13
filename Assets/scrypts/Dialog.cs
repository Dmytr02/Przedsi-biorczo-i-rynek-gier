using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialog")]
public class Dialog : ScriptableObject
{
    public List<DialogString> dialogStrings = new List<DialogString>();
}
