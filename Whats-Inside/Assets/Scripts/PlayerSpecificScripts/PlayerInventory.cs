using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public Dictionary<ItemSO, int> IngredientsHeld = new Dictionary<ItemSO, int>();

    private void Update()
    {
        if (!IngredientsHeld.ContainsKey(HandleCameraMovement.Interaction.GetClickedItem()))
        {
            IngredientsHeld.Add(HandleCameraMovement.Interaction.GetClickedItem(), 1);
        } else
        {
            IngredientsHeld[HandleCameraMovement.Interaction.GetClickedItem()] += 1;
        }
    }
}
