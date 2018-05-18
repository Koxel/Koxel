using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrafting : MonoBehaviour {

    public List<CraftingRecipe> recipes;

    public void LearnRecipe(CraftingRecipe recipe)
    {
        recipes.Add(recipe);
    }

    public void ForgetRecipe(CraftingRecipe recipe)
    {
        if (recipes.Contains(recipe))
        {
            recipes.Remove(recipe);
        }
    }
}
