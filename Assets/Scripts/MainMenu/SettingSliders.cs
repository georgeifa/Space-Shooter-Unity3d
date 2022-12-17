using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine;
using TMPro; 

[RequireComponent(typeof(Slider))]
public class SettingSliders : MonoBehaviour
{
    
    Slider slider
    {
        get { return GetComponent<Slider>(); }
    }

    [Header("Volume Name")]
    [Tooltip("This is the name of the exposed parameter")]
    [SerializeField]
    private string volumeName = "Enter volume name here";
    
    [Header("Volume Label")]
    [SerializeField]
    private TextMeshProUGUI volumeLabel;


    private void Start()
    {
        ResetSliderValue();

        UpdateValueOnChange(slider.value);

        slider.onValueChanged.AddListener(delegate
        {
            UpdateValueOnChange(slider.value);
        });
    }
    public void UpdateValueOnChange(float value)
    {
        if(volumeLabel.text != null)
            volumeLabel.text = Mathf.Round(value * 100f).ToString() + "%";

        if (Settings.profile)
        {
            Settings.profile.SetAudioLevels(volumeName, value);
        }
    }

    public void ResetSliderValue()
    {
        if (Settings.profile)
        {
            float volume = Settings.profile.GetAudioLevels(volumeName);

            UpdateValueOnChange(volume);
            slider.value = volume;
        }
    }
}
