using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType {

    AddSpeed,
    AddAttackRange,
    SecretAbility
}

public class PowerUpBase : PoolUnit
{
    [Header("Power Up Type")]
    [SerializeField] private PowerUpType powerUpType;

    [Header("Ref")]
    [SerializeField] private Transform powerUpVisual;
    [SerializeField] private TrailRenderer trailVFX;

    [Header("Launch Behavior")]
    [SerializeField] private float launchDuration = 0.8f;
    [SerializeField] private float arcHeight = 3f;
    [SerializeField] private float minLaunchDistance = 2f;

    [Header("Booster Apply")]
    [SerializeField] private List<BoosterData> boosterList;

    private CharacterBase currentCharInteract;
    private bool canInteract;

    private PowerUpSpawner spawner;

    private void OnTriggerEnter(Collider other) {

        if (!canInteract) { return; }

        CharacterBase character = LevelCache<Collider, CharacterBase>.GetValueWithKey(other);
        if (character == null) { return; }

        if (currentCharInteract == null) {
            // If can interact with character

            if (character.IsHavingThisPowerUp(powerUpType)) {

                PowerUpBase oldPowerUp = character.GetPowerUp(powerUpType);
                oldPowerUp.ReleaseBooster();
            }

            canInteract = false;

            SetActiveVisual(false);
            ApplyBoosterProgress(character);

            spawner.UnRegisterPowerUp(this);
        }
    }

    private IEnumerator IELaunchTo(Vector3 startPos, Vector3 targetPos) {

        ShowTrail();

        float elapsed = 0f;

        while (elapsed < launchDuration) {

            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / launchDuration);

            float moveT = AnimationEase.EaseOutCubic(t);

            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, moveT);

            float height = 4f * arcHeight * t * (1f - t);

            currentPos.y += height;

            UnitTF.position = currentPos;

            yield return null;
        }

        this.UnitTF.position = targetPos;

        canInteract = true;

        HideTrail();
    }

    private void SetActiveVisual(bool isActive) {
        powerUpVisual.gameObject.SetActive(isActive);
    }

    private void ShowTrail() {

        trailVFX.Clear();
        trailVFX.emitting = true;

        //trailVFX.sharedMaterial = meshRenderer.material;

        trailVFX.enabled = true;
    }

    private void HideTrail() {

        trailVFX.enabled = false;
        trailVFX.Clear();
        trailVFX.emitting = false;
    }

    protected void ApplyBoosterFor(CharacterBase character) {
        
        foreach (BoosterData booster in boosterList) {

            booster.Apply(character);
        }
    }

    protected void RemoveBoosterFor(CharacterBase character) {

        foreach (BoosterData booster in boosterList) {

            booster.Remove(character);
        }
    }

    public void ReleaseBooster() {

        if (currentCharInteract != null) {

            RemoveBoosterFor(currentCharInteract);

            currentCharInteract.UnregisterPowerUp(powerUpType);

            currentCharInteract = null;
        }

        SimplePool.Despawn(this);
    }

    public void OnInit(PowerUpSpawner powerSpawner) {

        SetActiveVisual(true);

        spawner = powerSpawner;
        currentCharInteract = null;
        canInteract = false;

        // Movement
        if (LevelManager.Instance.GetCurrentLeveL().TryGetRandomSpawnPoint(out Vector3 targetPos)) {

            float distanceToSpawner = (targetPos - spawner.UnitTF.position).sqrMagnitude;
            if (distanceToSpawner <= minLaunchDistance * minLaunchDistance) {

                targetPos = new Vector3(targetPos.x + 0.5f, targetPos.y, targetPos.z + 0.5f);
            }

            StartCoroutine(IELaunchTo(spawner.UnitTF.position, targetPos));
        }
        
    }

    public virtual void ApplyBoosterProgress(CharacterBase character) {

        currentCharInteract = character;

        ApplyBoosterFor(character);

        character.RegisterPowerUp(powerUpType, this);
    }
}
