using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public void SpawnEnemyType(Transform[] SpawnPosition, GameObject[] SpawnEnemyGroup)
    {
        for(int a = 0; a < SpawnPosition.Length; a++)
        {
            Instantiate(SpawnEnemyGroup[Random.Range(0, SpawnEnemyGroup.Length)]
                , SpawnPosition[a].position, Quaternion.identity);

            ParticleEffectManager.Instance.PlayExplosionEffect(SpawnPosition[a].position);
        }
    }
}
