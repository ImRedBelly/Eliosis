using UnityEngine;

public class DeathController : MonoBehaviour
{
    public static DeathController deathController;

    public GameObject deathCopy;
    GameObject emptyDeathCopy;

    public Transform checkPoint;

    void Start()
    {
        if (deathController != null)
            Destroy(gameObject);
        else
            deathController = this;
    }
    public void CreadDeadCopy()
    {
        if (emptyDeathCopy == null)
            emptyDeathCopy = Instantiate(deathCopy, new Vector2(GameManager.instance.PositionX, GameManager.instance.PositionY), Quaternion.identity);
    }
    public void LoadCheckPoint(GameObject player)
    {
        player.transform.position = checkPoint.position;
    }
}
