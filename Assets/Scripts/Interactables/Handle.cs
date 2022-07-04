using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    [SerializeField] Color normalColor;
    [SerializeField] Color selectedColor;
    [SerializeField] Color hoveredColor;
    [SerializeField] List<Renderer> myRenderers;

    private void Start()
    {
        ChangeColor(normalColor);
    }

    private void ChangeColor(Color newColor)
    {
        foreach(Renderer rend in myRenderers)
        {
            rend.material.color = newColor;
        }
    }

    public void OnHandleSelected(bool isSelected)
    {
        if (isSelected)
            ChangeColor(selectedColor);
        else
            ChangeColor(normalColor);
    }

    public void OnHandleHovered(bool isHovered)
    {
        if (isHovered)
            ChangeColor(hoveredColor);
        else
            ChangeColor(normalColor);
    }
}
