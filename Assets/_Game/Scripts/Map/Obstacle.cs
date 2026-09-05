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

    public void OnFadeMAT() {

        meshRenderer.sharedMaterial = fadeMAT;
    }

    public void OnBaseMAT() {


        meshRenderer.sharedMaterial = baseMAT;
    }
}
