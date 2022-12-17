using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    private Profiles m_profiles;

    [SerializeField]
    private List<SettingSliders> VolumeSliders = new List<SettingSliders>();

    private void Awake()
    {
        if (m_profiles != null)
            m_profiles.SetProfile(m_profiles);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Settings.profile && Settings.profile.audioMixer != null)
            Settings.profile.GetAudioLevels();
    }

    public void ApplyChanges()
    {
        if (Settings.profile && Settings.profile.audioMixer != null)
            Settings.profile.SaveAudioLevels();
    }

    //Not In Use.. For Reusabiilty and expansion
    public void CancelChanges()
    {
        if (Settings.profile)
            Settings.profile.GetAudioLevels();  


        for (int i = 0; i< VolumeSliders.Count; i++)
        {
            VolumeSliders[i].ResetSliderValue();
        }
    }
}
