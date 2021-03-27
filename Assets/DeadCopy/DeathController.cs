using UnityEngine;

public class DeathController : MonoBehaviour
{
    public static DeathController deathController;

    public GameObject deathCopy;

    void Start()
    {
        if (deathController != null)
            Destroy(gameObject);
        else
            deathController = this;

        if (GameManager.instance.countDeath > 0)
            Instantiate(deathCopy, new Vector2(GameManager.instance.PositionX, GameManager.instance.PositionY), Quaternion.identity);
    }
}
