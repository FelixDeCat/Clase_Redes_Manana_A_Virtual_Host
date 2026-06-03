using Fusion;
using Fusion.Addons.Physics;
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

    Vector3 move = Vector3.zero;
    float yAxis = 0;
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
            }
            

            if (data.buttons.IsSet(NetworkPlayerInputData.IsFirePressed1))
            {
                Debug.Log("Fire 1");
                // ejecuto habilidad
            }

            if (data.buttons.IsSet(NetworkPlayerInputData.IsJumping))
            {
                rig.Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            }

            
        }
    }
}
