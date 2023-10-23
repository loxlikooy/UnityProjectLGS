using UnityEngine;

namespace Code.Script
{
    public abstract class Weapon : MonoBehaviour
    {
        public float damage = 10f;

        public abstract void Attack();
    }
}