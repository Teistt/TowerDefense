using UnityEngine;

//Little script for enemie's health bar to constantly face camera

public class LookAtCam : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
