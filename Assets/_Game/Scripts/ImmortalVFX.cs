using System.Linq.Expressions;
using UnityEngine;

public class ImmortalVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem immortal;
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

        immortal.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void StopVFX() {

        immortal.Stop();

        SetActive(false);
    }

    public void PlayVFX(float vfxDuration, float scale) {

        UpdateSize(scale);
        SetActive(true);

        ResetVFX();
        immortal.Play();

        Invoke(nameof(StopVFX), vfxDuration);
    }
}
