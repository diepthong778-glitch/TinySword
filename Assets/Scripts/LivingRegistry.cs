using UnityEngine;

public class EnemyRegistry : MonoBehaviour
{
    public static int AliveEnemies { get; private set; }

    public static void Register()
    {
        AliveEnemies++;
    }

    public static void Unregister()
    {
        AliveEnemies--;
        if (AliveEnemies < 0)
            AliveEnemies = 0;
    }
}
