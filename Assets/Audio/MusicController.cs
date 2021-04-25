using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;
    public AudioManager audioManager;

    [Range(0, 1)]
    public float musicTensionLevel;
    public AudioSource[] sources;
    public MusicalTensionProfile[] tensionProfiles;
    public float targetTensionLevel;

    Sequence currentTransition;

    private void Start()
    {
        /*
        LerpToLevelRaw(0.5f, 0);
        GameManager.instance.lobbyReadyEvent += () => LerpToLevelRaw(0.5f, 1f);
        GameManager.instance.displayPlayersEvent += () => LerpToLevelRaw(1f, 1f);
        GameManager.instance.turnStartedEvent += () => LerpToLevelRaw(0.5f, 1f);

        SettingsUI.settingsUpEvent += () => LerpToLevelRaw(0, 1f);
        SettingsUI.settingsDownEvent += () => LerpToLevelRaw(0.5f, 1f);
        */

    }

    private void Update()
    {
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].volume = GetLayerVolume(musicTensionLevel, i) * audioManager.getScaledMusicVolume();
        }
    }

    public int GetCurrentLevel()
    {
        return Mathf.FloorToInt(targetTensionLevel);
    }

    public void LerpToNextLevel()
    {
        float val = Mathf.Min((GetCurrentLevel() + 1.0f) / tensionProfiles.Length, 1);
        LerpToLevelRaw(val, 1);
    }

    public void LerpToLastLevel()
    {
        print((GetCurrentLevel() - 1.0f) / tensionProfiles.Length);
        float val = Mathf.Max((GetCurrentLevel() - 1.0f) / tensionProfiles.Length, 0);
        LerpToLevelRaw(val, 1.5f);
    }

    public void LerpToLevel(int value, float speed)
    {
        targetTensionLevel = value;
        LerpToLevelRaw(value / (float)tensionProfiles.Length, speed);
    }

    public void LerpToLevelRaw(float value, float speed)
    {
        currentTransition.Kill();
        currentTransition = DOTween.Sequence();
        currentTransition.Append(DOTween.To(() => musicTensionLevel, x => musicTensionLevel = x, value, speed));
    }

    public void SetTensionLevelRaw(float value)
    {
        currentTransition.Kill();
        musicTensionLevel = value;
    }

    public void SetTensionLevel(int level)
    {
        currentTransition.Kill();
        level = Mathf.Clamp(level, 0, sources.Length);
        musicTensionLevel = level / (float)tensionProfiles.Length;
    }

    private float GetLayerVolume(float tensionValue, int layer)
    {
        tensionValue *= (tensionProfiles.Length); // dunno what is wrong with my math here. this causes and extra value at 7. however it works better so, yeah
        int floor = Mathf.Clamp(Mathf.FloorToInt(tensionValue), 0, tensionProfiles.Length - 1);
        int ciel = Mathf.Clamp(Mathf.CeilToInt(tensionValue), 0, tensionProfiles.Length - 1);
        return Mathf.Lerp(tensionProfiles[floor].layerVolume[layer], tensionProfiles[ciel].layerVolume[layer], tensionValue - floor);
    }
}

[Serializable]
public struct MusicalTensionProfile
{
    public string name;
    [Range(0, 1)]
    public float[] layerVolume;
}