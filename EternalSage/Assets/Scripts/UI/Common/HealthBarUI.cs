using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    private Entity entity;
    private EntityStats stats;
    private RectTransform rectTransform;
    private Slider slider;


    private void Start()
    {
        stats = GetComponentInParent<EntityStats>();
        entity = GetComponentInParent<Entity>();
        rectTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();

        entity.OnFliped += FlipUI;
        stats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealthValue();
        slider.value = stats.currentHelth;
    }


    private void FlipUI() => rectTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        entity.OnFliped -= FlipUI;
        stats.onHealthChanged -= UpdateHealthUI;
    }
}
