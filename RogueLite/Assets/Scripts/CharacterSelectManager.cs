using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static CharacterSelectManager instance;
    public PlayerController activePlayer;
    public CharacterSwitcher activeCharacterSelecter;
    void Awake()
    {
        instance = this;
    }

}
