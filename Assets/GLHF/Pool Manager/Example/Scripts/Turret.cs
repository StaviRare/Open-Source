using UnityEngine;

namespace GLHF.PoolManager.ExampleTurret
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Color32 mainColor = Color.white;

        [Header("Fire Rate")]
        [SerializeField] private float timerMin = 0;
        [SerializeField] private float timerMax = 5;

        [Header("Projectile Data")]
        [SerializeField] private float speed = 5;
        [SerializeField] private float disableDelay = 5;

        [Header("Pool Data")]
        [SerializeField] private string bulletPoolName = "Projectiles";
        [SerializeField] private int poolSize = 10;
        [SerializeField] private bool poolCanExpand = false;
        [SerializeField] private GameObject projectilePrefab;
        
        private float timer;
        private PoolManager poolManager;
        private SpriteRenderer spriteRenderer => GetComponent<SpriteRenderer>();

        private void Start()
        {
            poolManager = PoolManager.Instance;
            var poolManagerExists = poolManager != null;

            if (poolManagerExists)
            {
                CreatePool();
            }
        }

        private void Update()
        {
            SpawnCounter();
        }

        private void OnValidate()
        {
            spriteRenderer.color = mainColor;
        }

        private void CreatePool()
        {
            var poolConfig = new PoolConfig()
            {
                poolCanExpand = poolCanExpand,
                poolName = bulletPoolName,
                poolPrefab = projectilePrefab,
                poolSize = poolSize
            };

            poolManager.CreatePool(poolConfig);
        }

        private void SpawnCounter()
        {
            timer -= (1 * Time.deltaTime);

            if (timer <= 0)
            {
                TrySpawning();
                SpawnCounterReset();
            }
        }

        private void SpawnCounterReset()
        {
            timer = Random.Range(timerMin, timerMax);
        }

        private void TrySpawning()
        {
            if (poolManager != null)
            {
                var bulletObj = poolManager.GetPoolObjectByName(bulletPoolName);

                if (bulletObj != null)
                {
                    var projectile = bulletObj.GetComponent<TurretProjectile>();
                    var projectileData = new TurretProjectileData()
                    {
                        Speed = speed,
                        DisableDelay = disableDelay,
                        StartPosition = this.transform.position,
                        Direction = Random.insideUnitCircle.normalized,
                        MainColor = mainColor
                    };

                    projectile.SetData(projectileData);
                }
            }
        }
    }
}