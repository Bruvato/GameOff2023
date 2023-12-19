using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnParticlesToPool : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        ObjectPooler.Instance.ReturnToPool("Particle", gameObject);
    }
}
