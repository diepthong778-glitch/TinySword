using UnityEngine;

public class StairTrigger : MonoBehaviour
{
    public float stairSlope = 0.5f; // how steep
    public bool rightIsUp = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.EnterStairs(stairSlope, rightIsUp);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.ExitStairs();
        }
    }
}
