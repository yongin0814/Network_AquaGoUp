using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPManager : MonoBehaviour
{
    public static HPManager instance;
    void Awake()
    {
        HPManager.instance = this;
    }

    int hp;
    int maxHP = 4;
    public Slider sliderHP;
    public int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            sliderHP.value = hp;
        }
    }

    void Start()
    {
        sliderHP.maxValue = maxHP;
        HP = maxHP;
    }

}
