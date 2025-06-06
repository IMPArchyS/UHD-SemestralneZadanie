using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DebugButton : MonoBehaviour
{
    private void Awake()
    {
        this.GetComponent<Button>().onClick.AddListener(onClick);
    }

    private void onClick()
    {
        LevelManager.instance.loadSector(true);
    }
}
