using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectsSO kitchenObjectSO;
    public override void Interact(Player player)
    {

        if (!HasKitchenObject())
        {
            //There is no Kitchen Object 
            if (player.HasKitchenObject())
            {
                //Player is carrying something
                player.GetKitchenObject().SetKitchenObjectparent(this);
            }
            else
            {
                //player not carrying anything
            }
        }
        else
        {
            //there is a kitchen object 
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    //player is holding plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectsSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else
                {
                    //player is not carrying anything but holding a plate
                    if(GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        //conter is holding a plate
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectsSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else
            {
                //player is not  carrying anything
                GetKitchenObject().SetKitchenObjectparent(player);
            }
        }
    }
   
   
}
