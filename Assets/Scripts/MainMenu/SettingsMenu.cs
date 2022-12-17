using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public AudioController AudioController;
    private void OnDisable()
    {
        AudioController.ApplyChanges();
    }
}
