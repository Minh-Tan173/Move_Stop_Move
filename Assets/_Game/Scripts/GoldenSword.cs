using UnityEngine;

public class GoldenSword : PoolUnit
{
    [SerializeField] private Transform goldenSwordVisual;

    [Header("Orbit")]
    [SerializeField] private float radius = 2f;
    [SerializeField] private float rotateSpeed = 180f;

    [Header("Floating")]
    [SerializeField] private float floatHeight = 0.3f;
    [SerializeField] private float floatSpeed = 3f;
    [SerializeField] private float floatBaseHeight = 0.2f;


    private float currentAngle;
    private float floatOffset;

    private void Update() {

        HandleMovement();
    }

    private void HandleMovement() {

        currentAngle += rotateSpeed * Time.deltaTime;

        float rad = currentAngle * Mathf.Deg2Rad;
        
        // Orbit Movement
        Vector3 offset = new Vector3(Mathf.Cos(rad) * radius, 0f, Mathf.Sin(rad) * radius);

        // Floating Movement
        offset.y += floatBaseHeight + Mathf.Sin((Time.time + floatOffset) * floatSpeed) * floatHeight;

        goldenSwordVisual.position = UnitTF.position + offset;
    }

    public void OnInit(Transform parent) {

        this.UnitTF.SetParent(parent);

        this.UnitTF.localPosition = new Vector3(0f, 1f, 0f);
        this.UnitTF.localRotation = Quaternion.identity;
        this.UnitTF.localScale = Vector3.one;

        currentAngle = Random.Range(0f, 360f);
        floatOffset = Random.Range(0f, 10f);
    }

}
