using UnityEngine;

public class DamagePlayer : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.DealDamage();
        }

        // UIController.instance.UpdateHealtDisplay();
    }
}
