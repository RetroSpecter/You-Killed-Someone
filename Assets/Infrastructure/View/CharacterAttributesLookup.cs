using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactrVisuals", menuName = "CharacterVisualLookup", order = 1)]
public class CharacterAttributesLookup : ScriptableObject
{
    [SerializeField] private CharacterAttributes[] characterAttributes;
    private Dictionary<string, CharacterAttributes> characterMap;

    public CharacterAttributes GetCharacterAttributes(string name) {
        if (characterMap == null) {
            InitCharacterMap();
        }

        if (!characterMap.ContainsKey(name)) {
            Debug.LogError(name + " is not listed here" );
        }

        return characterMap[name];
    }

    private void InitCharacterMap() {
        characterMap = new Dictionary<string, CharacterAttributes>();
        foreach (CharacterAttributes ca in characterAttributes)
            characterMap[ca.name] = ca;
    }
}

[Serializable]
public struct CharacterAttributes
{
    public string name;
    public Color color;
    public SoundEffectProfile soundPing;
}

