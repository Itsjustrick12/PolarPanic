using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Settings
{
    public float masterVolume = 1f;
    public float musicVolume = -10f;
    public float soundVolume = 1f;
    public bool musicMute = false;
    public bool soundMute = false;
    public int quality = 3;
    public bool vSync = false;
}
