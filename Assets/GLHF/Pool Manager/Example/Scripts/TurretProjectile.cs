using UnityEngine;

namespace GLHF.PoolManager.ExampleTurret
{
    [RequireComponent(typeof(TrailRenderer))]
    public class TurretProjectile : MonoBehaviour
    {
        private float speed;
        private float disableTimer;
        private Vector2 targetDirection;
        private TrailRenderer trailRenderer =>  GetComponent<TrailRenderer>();
        public void SetData(TurretProjectileData data)
        {
            trailRenderer.Clear();

            speed = data.Speed;
            disableTimer = data.DisableDelay;
            targetDirection = data.Direction;
            transform.position = data.StartPosition;
            trailRenderer.endColor = data.MainColor;
            trailRenderer.startColor = data.MainColor;

            gameObject.SetActive(true);
        }

        private void Update()
        {
            disableTimer -= 1 * Time.deltaTime;

            if (disableTimer <= 0)
            {
                gameObject.SetActive(false);
            }

            transform.position += (Vector3)targetDirection * Time.deltaTime * speed;
        }
    }
}