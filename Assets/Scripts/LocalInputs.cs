using Fusion;
using UnityEngine;

public class LocalInputs : NetworkBehaviour
{
    public static LocalInputs instance;

    NetworkPlayerInputData inputData;

    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            instance = this;
            inputData = new NetworkPlayerInputData();
            Debug.Log("Cliente Local Input Instancia creada");
            return;
        }

        this.enabled = false;
    }

    Vector3 dir = Vector3.zero;
    bool isFirePressed = false;

    private void Update() // solo para levantar inputs
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        Debug.Log("Direction: " + dir);

        if (Input.GetButtonDown("Fire1")) isFirePressed = true;

        //inputData.isFirePressed |= Input.GetButtonDown("Fire1");
    }

    public NetworkPlayerInputData GetInputData()
    {
        inputData = new NetworkPlayerInputData(dir, isFirePressed);

        isFirePressed = false;

        return inputData;
    }
}
