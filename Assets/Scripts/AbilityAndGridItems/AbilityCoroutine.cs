using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCoroutine : MonoBehaviour
{
    public static AbilityCoroutine Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
}
