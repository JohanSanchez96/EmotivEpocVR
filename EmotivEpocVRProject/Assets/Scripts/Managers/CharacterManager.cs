using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterManager : Singleton<CharacterManager>
{
    public GameObject bodyCharacter;
    public ActionBasedSnapTurnProvider turnCharacter;
    public ActionBasedContinuousMoveProvider moveCharacter;

    public void SetSettupCharacterController(int index)
    {
        if (bodyCharacter != null)
        {
            switch (index)
            {
                case 0:
                    moveCharacter.enabled = turnCharacter.enabled = false;
                    Debug.Log("Move Desactived");
                    break;
                
                case 1:
                    moveCharacter.enabled = turnCharacter.enabled = true;
                    Debug.Log("Move Activate");
                    break;
            }
        }
    }
}
