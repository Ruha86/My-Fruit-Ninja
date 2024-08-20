using UnityEngine;

public class Heart : MonoBehaviour
{
    public int HealthForHeart = 1;
    public GameObject SliceParticles;

    public void ShowSliceParticles()
    {
        Instantiate(SliceParticles, transform.position, transform.rotation);
    }
}
