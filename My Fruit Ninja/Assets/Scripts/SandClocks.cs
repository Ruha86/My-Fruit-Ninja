using UnityEngine;

public class SandClocks : MonoBehaviour
{
    public float SlowDuration = 3f;
    public GameObject SliceParticles;

    public void ShowSliceParticles()
    {
        Instantiate(SliceParticles, transform.position, transform.rotation);
    }
}
