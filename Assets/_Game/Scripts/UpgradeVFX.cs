using System.Linq.Expressions;
using UnityEngine;

public class UpgradeVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem upgradeVFX;
    [SerializeField] private float baseSize;

    private Transform vfxTransform;
    private Transform VFXTransform => vfxTransform == null ? vfxTransform = this.transform : vfxTransform;

    private void SetActive(bool isShow) {

        if (isShow) {

            VFXTransform.gameObject.SetActive(true);
        }
        else {
            VFXTransform.gameObject.SetActive(false);
        }
    }

    private void UpdateSize(float scale) {

        VFXTransform.localScale = Vector3.one * baseSize * scale;
    }

    private void ResetVFX() {

        upgradeVFX.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void StopVFX() {

        upgradeVFX.Stop();

        SetActive(false);
    }

    public void PlayVFX(float vfxDuration, float scale) {

        UpdateSize(scale);
        SetActive(true);

        ResetVFX();
        upgradeVFX.Play();

        Invoke(nameof(StopVFX), vfxDuration);
    }
}
