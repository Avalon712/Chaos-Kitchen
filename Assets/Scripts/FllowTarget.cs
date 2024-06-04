using UnityEngine;

namespace ChaosKitchen
{
    public class FllowTarget : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private void FixedUpdate()
        {
            transform.position = _target.position;
        }
    }
}

