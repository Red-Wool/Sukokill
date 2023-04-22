using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Player Control")]
public class PlayerControl : ScriptableObject
{
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;

    public KeyCode ability;
}
