using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(OnChange_Health))] int health { get; set; }
    [SerializeField] int maxLife = 100;

    [SerializeField] Image lifebar;
    [SerializeField] Image bkg;

    public override void Spawned()
    {
        health = maxLife;
    }

    void OnChange_Health()
    {
        if (health == maxLife || health == 0)
        {
            lifebar.enabled = false;
            bkg.enabled = false;
        }
        else
        {
            bkg.enabled = true;
            lifebar.enabled = true;
            lifebar.fillAmount = (float)health / maxLife;

        }
    }

    public void DoDamage(int val) // host
    {
        if (val < health)
        {
            health -= val;
        }
        else
        {
            health = 0;
        }

        if (health <= 0)
        {
            Death();
        }
    }

    void Death()
    {
        if (!HasInputAuthority) // solamente los objetos en los peers que ESPECIFICAMENTE no son Host
        {
            Runner.Disconnect(Object.InputAuthority);
        }

        Runner.Despawn(Object);
    }

}
