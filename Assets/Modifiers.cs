using UnityEngine;

[CreateAssetMenu(fileName = "Modifiers", menuName = "Scriptable Objects/Modifiers")]
public class Modifiers : ScriptableObject
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Sprite modSprite;

    public GameObject GetPrefab(){return prefab;}
    public Sprite GetSprite(){return modSprite;}
}
