using Fusion;
using UnityEngine;

public class MyPlayer : NetworkBehaviour
{
    [SerializeField] bool isFirePressed;
    [SerializeField] float speed = 5f;


    public override void FixedUpdateNetwork() // Lado del Host
    {
        //var input = GetInput<NetworkPlayerInputData>();

        // if (input != null)
        // {
                // input.Value.isFirePressed
        // }

        if (GetInput(out NetworkPlayerInputData data))
        {
            transform.position = transform.position + data.direction * Runner.DeltaTime * speed;

            isFirePressed = data.isFirePressed;

            
        }
    }
}
