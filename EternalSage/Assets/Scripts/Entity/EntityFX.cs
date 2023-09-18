using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    SpriteRenderer sr;
    private Material originalMat;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] float flashTime = .2f;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }


    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        yield return new WaitForSeconds(flashTime);
        sr.material = originalMat;
    }
}
