using UnityEngine;

public abstract class BoosterSO : ScriptableObject
{
    public abstract void Apply(CharacterBase character, float value);

    public abstract void Remove(CharacterBase character, float value);
}

[System.Serializable]
public class BoosterData {

    [SerializeField] private BoosterSO booster;
    [SerializeField] private float value;

    public void Apply(CharacterBase character) {
        booster.Apply(character, value);
    }

    public void Remove(CharacterBase character) {
        booster.Remove(character, value);
    }
}