using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float speed = 2f;
    [Networked] TickTimer _lifetime { get; set; }
    [SerializeField] float _lifeTimeToDeath;

    public void Init() // On BeforeSpawned
    {
        _lifetime = TickTimer.CreateFromSeconds(Runner, _lifeTimeToDeath);
    }
    public override void Spawned()
    {
        base.Spawned();
    }

    public override void FixedUpdateNetwork()
    {
        if (_lifetime.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
        else
        {
            transform.position += transform.forward * speed * Runner.DeltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.TryGetComponent(out HealthSystem _hs))
        {
            _hs.DoDamage(20);
        }

        Runner.Despawn(Object);
    }
}
