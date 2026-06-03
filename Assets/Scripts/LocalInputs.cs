using ExitGames.Client.Photon.StructWrapping;
using Fusion;
using UnityEngine;

public class LocalInputs : NetworkBehaviour
{
    public static LocalInputs instance_for_input_auth;

    NetworkPlayerInputData inputData;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            instance_for_input_auth = this;
            inputData = new NetworkPlayerInputData();
            Debug.Log("Cliente Local Input Instancia creada");
            return;
        }

        this.enabled = false;
    }

    Vector3 dir = Vector3.zero;
    Vector3 dirLook = Vector3.zero;
    bool isFirePressed1 = false;
    bool isFirePressed2 = false;
    bool isJumping = false;

    [SerializeField] LayerMask viewMask;

    private void Update() // solo para levantar inputs
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        Debug.Log("Direction: " + dir);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, viewMask))
        {
            dirLook = hit.point - transform.position;
        }


        isFirePressed1 |= Input.GetButtonDown("Fire1");
        isFirePressed2 |= Input.GetButtonDown("Fire2");

        isJumping |= Input.GetButtonDown("Jump");
    }

    public NetworkPlayerInputData GetInputData() // en el tick del servidor, solo cuando el lo quiera pasar a buscar
    {
        inputData = new NetworkPlayerInputData();

        inputData.direction = dir;
        inputData.dirToLook = dirLook;
        inputData.buttons.Set(NetworkPlayerInputData.IsFirePressed0, isFirePressed1);
        inputData.buttons.Set(NetworkPlayerInputData.IsFirePressed1, isFirePressed2);
        inputData.buttons.Set(NetworkPlayerInputData.IsJumping, isJumping);

        isFirePressed1 = false;
        isFirePressed2 = false;
        isJumping = false;

        return inputData;
    }
}
