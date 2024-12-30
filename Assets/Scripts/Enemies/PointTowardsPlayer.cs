using UnityEngine;

public class PointTowardsPlayer : MonoBehaviour
{
    Transform playerTransform;

    private void Start()
    {
        playerTransform = GameManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.paused || GameManager.instance.gameOver) return;

        Vector3 _diff = (playerTransform.position - transform.position).normalized;
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(_diff.y, _diff.x));
        //Debug.DrawRay(transform.position, transform.right, Color.green);
    }
}
