using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CicleMagicUI : UIBehaviour, ILayoutGroup
{
    public float radius = 100;
    public float offsetAngle;

    protected override void OnValidate()
    {
        base.OnValidate();
        Arrange();
    }

    public void SetLayoutHorizontal()
    {
    }

    public void SetLayoutVertical()
    {
        Arrange();
    }

    void Arrange()
    {
        if (transform.childCount != 0)
        {
            
            float splitAngle = 360 / transform.childCount;
            var rect = transform as RectTransform;

            for (int elementId = 0; elementId < transform.childCount; elementId++)
            {
                var child = transform.GetChild(elementId) as RectTransform;
                float currentAngle = splitAngle * elementId + offsetAngle;
                child.anchoredPosition = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * radius;
            }
        }
    }
}
