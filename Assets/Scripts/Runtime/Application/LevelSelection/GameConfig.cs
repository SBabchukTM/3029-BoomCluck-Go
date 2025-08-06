using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]
public class GameConfig : BaseSettings
{
    [SerializeField] private List<LevelConfig> _levelConfigs;

    public List<LevelConfig> LevelConfigs => _levelConfigs;
}
