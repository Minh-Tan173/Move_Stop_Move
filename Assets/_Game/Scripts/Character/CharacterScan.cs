using System.Collections.Generic;
using UnityEngine;

public class CharacterScan : MonoBehaviour
{
    [SerializeField] private CharacterBase character;
    [SerializeField] private LayerMask charLayer;

    [Header("Scan Behavior")]
    [SerializeField] private float duration = 0.2f;

    private Collider[] charCollArray = new Collider[20];

    private float elapsedTime;

    public void OnInit() {

        elapsedTime = 0f;
    }

    public void OnDespawn() {

    }

    private void Update() {

        if (!character.CanScanTarget()) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration) {

            elapsedTime -= duration;

            ScanTargetProgress();
        }
    }

    private void ScanTargetProgress() {

        charCollArray = Physics.OverlapSphere(character.UnitTF.position, character.GetTrueAttackRange(), charLayer);

        CharacterBase target = GetNearestTarget(charCollArray);
        if (target != null) {

            character.SetAttackTarget(target);
        }
    }

    private CharacterBase GetNearestTarget(Collider[] collArray) {

        CharacterBase nearestTarget = null ;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider coll in collArray) {

            CharacterBase target = LevelCache<Collider, CharacterBase>.GetValueWithKey(coll);

            if (!character.CanSelectAttackTarget(target)) continue;

            float sqrDistance = (target.UnitTF.position - character.UnitTF.position).sqrMagnitude;

            if (sqrDistance <= nearestDistance) {

                nearestDistance = sqrDistance;
                nearestTarget = target;
            }

        }

        return nearestTarget;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {

        if (character == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(character.UnitTF.position, character.GetTrueAttackRange());
    }
#endif
}
