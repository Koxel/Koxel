using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour {
    public Transform SlotsParent;
    public GameObject SlotPrefab;
    public int rowWidth = 5;
    public int minRows = 4;
    public bool isSetup;
    public int createdRows = 0;

    public void SetupSlots()
    {
        if (isSetup)
            return;

        for (int row = 0; row < minRows; row++)
        {
            AddRow();
        }
        isSetup = true;
    }

    public void AddRow()
    {
        for (int slot = 0; slot < rowWidth; slot++)
        {
            Instantiate(SlotPrefab, SlotsParent);
        }
        createdRows++;
    }

    public virtual void UpdateUI()
    {
        
    }
}
