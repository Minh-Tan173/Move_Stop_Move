using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Obstacle : MonoBehaviour
{
    [Header("Material")]
    [SerializeField] private Material baseMAT;
    [SerializeField] private Material fadeMAT;

    [Header("")]
    [SerializeField] private MeshRenderer meshRenderer;

    private Material material;
    private Coroutine currentIE;

    //private IEnumerator FadeCoroutine() {
    //    float duration = 0.3f;
    //    float time = 0f;

    //    while (time < duration) {
    //        time += Time.deltaTime;

    //        float alpha = Mathf.Lerp(1f, 0.3f, time / duration);
    //        material.SetFloat("_AlphaValue", alpha);

    //        yield return null;
    //    }
    //}

    public void OnFadeMAT() {

        meshRenderer.material = fadeMAT;
    }

    public void OnBaseMAT() {

        Debug.Log("Turn on base mat");

        meshRenderer.material = baseMAT;
    }
}
