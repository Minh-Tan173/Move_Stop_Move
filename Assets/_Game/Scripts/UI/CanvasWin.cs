using UnityEngine;

public class CanvasWin : UICanvas
{
    [SerializeField] private ParticleSystem particleSystem;

    public override void SetUp() {

        PlayPartical();
    }

    private void PlayPartical() {

        StopPartical();
        particleSystem.Play();

        Invoke(nameof(StopPartical), 2.5f);

    }

    private void StopPartical() {
        particleSystem.Stop();
        particleSystem.Clear();
    }
}
