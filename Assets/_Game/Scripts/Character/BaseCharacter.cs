using System;
using System.Collections;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    [SerializeField] protected Transform charTF;

    [Header("Attack Behavior")]
    [SerializeField] protected float attackDelayTime;

    private Coroutine IEAttack;

    private IEnumerator AttackCoroutine(Action callback) {

        float elapsedTime = 0f;

        while (elapsedTime <= attackDelayTime) {

            if (IsMoving()) {
                // While attack, if character is moving
                yield break;
            }

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        callback?.Invoke();
    }

    public virtual void OnInit() {

    }

    public virtual void OnDespawn() {

    }

    #region Skin Method
    public void ChangeWeapon() {

    }

    public void ChangePants() {

    }
    #endregion

    public void Attack() {

        if (IEAttack != null) {
            StopCoroutine(IEAttack);
        }

        IEAttack = StartCoroutine(AttackCoroutine(Throw));
    }

    public void Throw() {

    }

    public virtual bool IsMoving() {
        Debug.LogError("TRIGGER BASE CHARACTER!!");
        return true;
    }
}
