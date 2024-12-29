using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Json;
using UnityEngine.InputSystem;
using System;

public class SettingsManager : MonoBehaviour
{
    public static Settings currentSettings = null;

    void Awake()
    {
        currentSettings = new Settings();
    }
}