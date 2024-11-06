using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SelectPosition : MonoBehaviour
{
    public Character character;
    public List<GameObject> positions = new List<GameObject>();
    public int positionIndex;
    
    private void Start()
    {
        GameManager.Instance.SetCharacter(this);
        SetIndex();
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        transform.position = positions[positionIndex].transform.position;
        transform.rotation = positions[positionIndex].transform.rotation;
    }

    public void SetIndex()
    {
        if (character == Character.Poly)
        {
            positionIndex = GameManager.Instance.polyPositionIndex;
        }
        else if (character == Character.Alexander)
        {
            positionIndex = GameManager.Instance.alexanderPositionIndex;
        }
    }
}

public enum Character
{
    Poly, Alexander
}

