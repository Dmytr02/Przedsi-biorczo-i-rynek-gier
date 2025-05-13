using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class WallCwantum : MonoBehaviour
{
    public Vector2Int id;
    public List<Vector2Int> positions;
    public int positionID = 0;
    public Console console;

    public void CangePosition()
    {
        console.CangeWallPosition(this);
    }
}
