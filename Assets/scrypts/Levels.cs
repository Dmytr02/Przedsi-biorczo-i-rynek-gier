using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "my/Level", order = 1)]
public class Levels : ScriptableObject
{
    public Vector2Int player;
    public Vector2Int[] gridWalls;
    public Vector2Int[] gridBoxs;
    public Vector2Int[] gridTargets;
    public int stepCount;
    public AudioClip stepSound;
    public AudioClip musicSound;
    public List<WallList> CwantumWalls;
}

[Serializable]
public class WallList
{
    public List<Vector2Int> walls;
    public Vector2Int id;
}
