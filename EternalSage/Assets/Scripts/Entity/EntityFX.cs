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

    private void RedColorBlink()
    {
        if (sr.color != Color.white) 
        {
            sr.color = Color.white;
        }
        else 
        {
            sr.color = Color.red;
        }
    }

    private void CancelRedBlink()
    {
        CancelInvoke();
        sr.color = Color.white;
    }
}