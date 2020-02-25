using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public List<GameObject> houseParts;
    public List<ParticleSystem> smokeParticles;

    [NonSerialized] 
    private int currentBuildingPart;

    public int CurrentBuildingPart
    {
        get => currentBuildingPart;
        set
        {
            if (value <= 7)
                currentBuildingPart = value;
        }
    }

    public void RepairPart()
    {
        StartCoroutine(RepairPartCoroutine());
    }

    IEnumerator RepairPartCoroutine()
    {
        foreach (var smokeParticle in smokeParticles)
        {
            smokeParticle.Play();
        }

        yield return new WaitForSeconds(0.3f);
        houseParts[currentBuildingPart].SetActive(true);
    }
    
    
}
