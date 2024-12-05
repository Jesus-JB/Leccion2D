using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    public int currentHealth, maxHealth;

    public float Invicibilidad;
    public float ContadorInvisibilidad;

    private SpriteRenderer TheSR;

    public GameObject deathEffect;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;

        TheSR = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        if (ContadorInvisibilidad > 0)
        {
            ContadorInvisibilidad -= Time.deltaTime;

            if (ContadorInvisibilidad <= 0)
            {
                TheSR.color = new Color(TheSR.color.r, TheSR.color.g, TheSR.color.b, 1f);
            }
        }
    }

    public void DealDamage()
    {
        if (ContadorInvisibilidad <= 0)
        {
            currentHealth--;
            PlayerController.instance.anim.SetTrigger("Hurt");

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                Instantiate(deathEffect, PlayerController.instance.transform.position, PlayerController.instance.transform.rotation);

                LevelManager.instance.RespawnPlayer();
            }
            else
            {
                ContadorInvisibilidad = Invicibilidad;
                TheSR.color = new Color(TheSR.color.r, TheSR.color.g, TheSR.color.b, .5f);

                PlayerController.instance.KnockBack();
            }

            // UIController.instance.UpdateHealtDisplay();
        }
    }
    // public void HealPlayer()
    // {
    //     currentHealth++;

    //     if (currentHealth > maxHealth)
    //     {
    //         currentHealth = maxHealth;
    //     }

    //     UIController.instance.UpdateHealtDisplay();
    // }
}
