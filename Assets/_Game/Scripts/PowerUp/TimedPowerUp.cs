using UnityEngine;

public class TimedPowerUp : PowerUpBase
{
    [Header("TimedPowerUp Setup")]
    [SerializeField] private float duration;

    private float elapsedDuration;
    private bool isPowerUpInteract = false;

    private void Update() {


        if (!isPowerUpInteract) { return; }

        elapsedDuration += Time.deltaTime;

        if (elapsedDuration >= duration) {

            isPowerUpInteract = false;
            ReleaseBooster();
        }
    }

    public override void ApplyBoosterProgress(CharacterBase character) {

        base.ApplyBoosterProgress(character);

        elapsedDuration = 0;

        isPowerUpInteract = true;
    }
}
