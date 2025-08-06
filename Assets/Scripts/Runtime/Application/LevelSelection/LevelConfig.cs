using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Config 0", menuName = "Config/Level Config")]
public class LevelConfig : ScriptableObject
{
    public int CardPairs = 2;
    public int UniqueCards = 2;
    public float TimeTotal = 20;
    public float RememberedTime = 5;
    public int Reward = 100;
}
