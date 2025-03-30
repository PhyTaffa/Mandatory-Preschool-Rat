using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float Distance = 2.5f;
    private GameObject player;
    private Vector2 direction;
    private Vector3 Direction = new Vector3();



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        direction = player.GetComponent<Movement>().ReturnMovement();

        if (player == null)
        {
            return;
        }

        Vector3 RayOrigin = player.transform.position;

        if (direction.x != 0 || direction.y != 0)
        {
            Direction = direction;
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            int interactableLayerMask = 1 << LayerMask.NameToLayer("Interactable");

            RaycastHit2D HitInformation = Physics2D.Raycast(RayOrigin, Direction, Distance, interactableLayerMask);

            if (HitInformation.collider)
            {
                IInteractable interactableObject = HitInformation.collider.GetComponent<IInteractable>();
                if (interactableObject != null)
                {
                    interactableObject.Interact(player);
                }
            }
        }

    }
}
