using UnityEngine;

[CreateAssetMenu(menuName = "Bomb/Bomb Stats")]
public class BombStats : ScriptableObject
{
    public float cooldown = 2f;
    public float explosionRadius = 2f;
    public float fuseTime = 3f;
    public int damage = 1;
}