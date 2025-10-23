//using UnityEngine;

//public class EnemySpawner : MonoBehaviour
//{
//    [System.Serializable]
//    public class EnemySpawnInfo
//    {
//        public EnemyData data;
//        public Transform spawnPoint;
//        public int level = 1;
//    }

//    [Header("Danh sách quái spawn")]
//    public EnemySpawnInfo[] enemiesToSpawn;

//    void Start()
//    {
//        foreach (var info in enemiesToSpawn)
//        {
//            SpawnEnemy(info);
//        }
//    }

//    void SpawnEnemy(EnemySpawnInfo info)
//    {
//        if (info.data == null || info.data.modelPrefab == null)
//        {
//            Debug.LogWarning("Thiếu dữ liệu hoặc prefab quái!");
//            return;
//        }

//        GameObject enemyObj = Instantiate(info.data.modelPrefab, info.spawnPoint.position, Quaternion.identity);
//        Enemy enemy = enemyObj.GetComponent<Enemy>();

//        if (enemy != null)
//        {
//            enemy.enemyData = info.data;
//            enemy.currentLevel = info.level;
//        }
//    }
//}
