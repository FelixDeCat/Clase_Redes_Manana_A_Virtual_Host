using Fusion;
using Fusion.Addons.Physics;
using TMPro;
using UnityEngine;

public class MyPlayer : NetworkBehaviour
{
    [SerializeField] bool isFirePressed;
    [SerializeField] float speed = 100f;

    [SerializeField] float jumpForce = 5f;

    [SerializeField] NetworkRigidbody3D rig;

    [SerializeField] Transform ShootPoint;
    [SerializeField] Bullet bullet;

    [SerializeField] Transform rot_root;

    [SerializeField] float lookLerpQuant = 0.05f;

    [SerializeField] ParticleSystem shootParticle;
    [SerializeField] ParticleSystem shootParticleRay;

    Vector3 move = Vector3.zero;
    float yAxis = 0;

    [Networked, OnChangedRender(nameof(OnChangeNickeName))] public NetworkString<_16> nickname { get; set; }

    [SerializeField] TextMeshProUGUI myNickName;

    public override void Spawned()
    {
        nickname = PlayerPrefs.GetString("nickname");
    }

    void OnChangeNickeName()
    {
        RPC_OnChangeName();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_OnChangeName()
    {
        if (Runner.LocalPlayer != Object.InputAuthority)
        {
            myNickName.text = nickname.ToString();
        }
    }






    public override void FixedUpdateNetwork() /// T I C K
    {
        /// Prediction: Es cuando el cliente, sigue ejecutando su codigo, pero espera un snapshot para ser corregido
        /// Rollback: Es cuando el cliente recibe un Snapshot incorrecto, "No hay reconciliacion", y este resimula hasta el snapshotCorrecto

        /// Host    100 101 102 103
        /// Cli     100 101 102 103


        if (GetInput(out NetworkPlayerInputData data))
        {
            // transform.position = transform.position + data.direction * Runner.DeltaTime * speed;

            yAxis = rig.Rigidbody.linearVelocity.y;
            move = data.direction.normalized * Runner.DeltaTime * speed; 
            move.y = yAxis;

            rig.Rigidbody.linearVelocity = move;

            Vector3 toLook = data.dirToLook;

            rot_root.rotation = Quaternion.Lerp(rot_root.rotation, Quaternion.LookRotation(toLook), lookLerpQuant);

            if (HasStateAuthority)
            {
                if (data.buttons.IsSet(NetworkPlayerInputData.IsFirePressed0))
                {

                    RPC_ShootParticle();
                    Debug.Log("Fire 1");
                    Runner.Spawn(
                        bullet,
                        ShootPoint.position,
                        ShootPoint.rotation,
                        onBeforeSpawned:
                        (r,o) =>
                        {
                            o.GetComponent<Bullet>().Init();
                        }
                        );
                }

                if (data.buttons.IsSet(NetworkPlayerInputData.IsFirePressed1))
                {
                    LagCompensatedHit hit;

                    RPC_ShootParticleRay();

                    Debug.Log("Fire 2");
                    if (Runner.LagCompensation.Raycast(ShootPoint.position, ShootPoint.forward, float.MaxValue, Object.InputAuthority, out hit))
                    {
                        if (hit.Hitbox.Root.TryGetComponent(out HealthSystem enemyhealth))
                        {
                            enemyhealth.DoDamage(30);
                        }
                    }
                }
            }

            if (data.buttons.IsSet(NetworkPlayerInputData.IsJumping))
            {
                rig.Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }

            
        }
    }


    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_ShootParticle()
    {
        shootParticle.Play();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_ShootParticleRay()
    {
        shootParticleRay.Play();
    }
}