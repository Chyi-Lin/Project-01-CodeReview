using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioValueOptionUI : MonoBehaviour
{
    [System.Serializable]
    public struct ValueBoxBar
    {
        public float value;
        public Image image;
    }

    [SerializeField]
    private string functionName;

    [SerializeField]
    private GameSettingKey valueKey;

    [SerializeField]
    private GameSettingKey valueMuteKey;

    [SerializeField]
    private ToggleButton toggleButton;

    [SerializeField]
    private int startValueIndex = 1;

    [SerializeField]
    private ValueBoxBar[] valueBoxBars;

    [SerializeField]
    private Color lightColor, darkColor;

    [SerializeField]
    private AudioMixerGroup targetAudioGroup;

    private int currentIndex;
    private int isMute = 0;

    private void OnEnable()
    {
        toggleButton.OnToggleEvent += Mute;
    }

    private void OnDisable()
    {
        toggleButton.OnToggleEvent -= Mute;
    }

    private void Start()
    {
        if (functionName.Equals(""))
            return;

        currentIndex = PlayerPrefs.GetInt(valueKey.ToString(), startValueIndex);
        isMute = PlayerPrefs.GetInt(valueMuteKey.ToString(), 0);

        SetVolumeAndValueBox(currentIndex, isMute);
        toggleButton.SetToggle(isMute == 0 ? false : true);
    }

    private void SetVolumeAndValueBox(int index, int mute)
    {
        targetAudioGroup.audioMixer.SetFloat(functionName, valueBoxBars[index].value);

        for (int i = 0; i < valueBoxBars.Length; i++)
        {
            if(i <= index)
                valueBoxBars[i].image.color = lightColor;
            else
                valueBoxBars[i].image.color = darkColor;
        }

        if(mute == 1)
        {
            targetAudioGroup.audioMixer.SetFloat(functionName, valueBoxBars[0].value);

            if (valueMuteKey == GameSettingKey.GAME_SETTING_BGM_VOLUME_MUTE)
                GameAudioManager.Instance.audioSource.enabled = false;
        }
        else
        {
            if (valueMuteKey == GameSettingKey.GAME_SETTING_BGM_VOLUME_MUTE)
                GameAudioManager.Instance.audioSource.enabled = true;
        }

        PlayerPrefs.SetInt(valueKey.ToString(), index);
        PlayerPrefs.SetInt(valueMuteKey.ToString(), mute);
        PlayerPrefs.Save();
    }

    public void VolumeUp()
    {
        if (currentIndex + 1 >= valueBoxBars.Length)
            return;
        else
            currentIndex++;

        SetVolumeAndValueBox(currentIndex, isMute);
    }

    public void VolumeDown()
    {
        if (currentIndex - 1 < 0)
            return;
        else
            currentIndex--;

        SetVolumeAndValueBox(currentIndex, isMute);
    }

    public void Mute(bool toggle)
    {
        if (toggle)
            isMute = 1;
        else
            isMute = 0;

        SetVolumeAndValueBox(currentIndex, isMute);
    }

}
