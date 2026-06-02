using Fusion;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] float speed = 2f;
    [Networked] TickTimer _lifetime { get; set; }
    [SerializeField] float _lifeTimeToDeath;

    public void Init()
    {
        _lifetime = TickTimer.CreateFromSeconds(Runner, _lifeTimeToDeath);
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
}
