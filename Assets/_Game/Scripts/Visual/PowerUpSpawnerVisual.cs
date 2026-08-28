using System.Collections;
using UnityEngine;

public class PowerUpSpawnerVisual : MonoBehaviour
{
    [Header("Ref")]
    [SerializeField] private PowerUpSpawner powerUpSpawner;
    [SerializeField] private ParticleSystem particleSystem;

    [Header("Setup Visual")]
    [SerializeField, Range(0, 1)] private float scaleSize;

    private Transform visualTransform;
    private Transform VisualTransform => visualTransform == null ? visualTransform = this.transform : visualTransform;

    private void PlayPartical() {

        particleSystem.Stop();
        particleSystem.Clear();
        particleSystem.Play();
    }

    public void OnInit() {

        VisualTransform.localScale = Vector3.one * scaleSize;
        PlayPartical();
    }

    public IEnumerator IEShrink(float shrinkDuration) {

        float elapsed = 0f;

        Vector3 startScale = VisualTransform.localScale;

        while (elapsed < shrinkDuration) {

            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / shrinkDuration);
            float easeT = AnimationEase.EaseInOut(t);

            VisualTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, easeT);

            yield return null;
        }

        VisualTransform.localScale = Vector3.zero;
    }
}
