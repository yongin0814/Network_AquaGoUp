using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Game Data", menuName = "Game Data")]
public class GameData : ScriptableObject
{
    public Object gameScene;
    public Sprite gameScreenShot;
    public string gameName;
    [TextArea(3, 8)]
    public string gameExplain;
}