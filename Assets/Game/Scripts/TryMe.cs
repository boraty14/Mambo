using System.Collections;
using System.Collections.Generic;
using Game.Scripts;
using UnityEngine;

public class TryMe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SignalBus.StartGame?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
