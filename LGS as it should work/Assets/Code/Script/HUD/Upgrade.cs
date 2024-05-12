using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Upgrade
{
    public string name;
    public Sprite icon; 
    public Action<float> effect;
}