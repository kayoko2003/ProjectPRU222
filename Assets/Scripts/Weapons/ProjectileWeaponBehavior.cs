using UnityEngine;

public class ProjectileWeaponBehavior : MonoBehaviour
{
    protected Vector3 direction;
    public float destroyAfterSeconds;
   

    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    public void DiretionChecker(Vector3 dir)
    {
        direction = dir;
    }
}
