using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject", fileName = "KitchenObjectsSO")]
public class KitchenObjectsSO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
