using System;
using UnityEngine;

[Serializable]
public class Upgrade
{
    public string name;
    public Sprite icon; // Предполагается, что вы добавили Sprite в редакторе Unity
    public Action effect;
}