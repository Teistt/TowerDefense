using UnityEngine;

/*
 * Camera controls
 * 
 */

public class CameraController : MonoBehaviour
{

    private bool dragging = false;
    public float camSpeed=1f;
    private Vector3 lastPosition = Vector3.zero;
    //private Vector3 mouseDragPos=Vector3.zero;

    //private bool canMove = true;

    public float panSpeed = 30f;
    public float panBorder = 20f;

    public float rotSpeed = 30f; 

    public float scrollSpeed=5f;
    public float mouseSensitivity;

    private float minX = 2f;
    private float maxX = 70f;
    private float minY = 10f;
    private float maxY = 80f;
    private float minZ = -25f;
    private float maxZ = 60f;

    void Update()
    {
        if (GameManager.gameIsOver)
        {
            this.enabled = false;
            return;
        }
        /*
        //pour l'inspector, permet d'activer ou désactiver les commandes
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canMove = !canMove;
        }

        if (!canMove)
        {
            return;
        }
        */
        #region MOVEMENT
        Vector3 pos = transform.position;

        if (Input.GetMouseButtonDown(1))
        {
            dragging = true;
            
            lastPosition = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(1))
        {
            //Debug.Log("drag false");
            dragging = false;
            lastPosition = Vector3.zero;
        }

        if (dragging)
        {
            Vector3 delta = Input.mousePosition - lastPosition;
            pos.x+=-delta.x* mouseSensitivity;
            pos.z += -delta.y * mouseSensitivity;
            lastPosition = Input.mousePosition;
        }

        else
        {
            //Déplacement haut
            if (Input.GetKey(KeyCode.Z) || Input.mousePosition.y >= Screen.height - panBorder)
            {
                pos += Vector3.forward * Time.deltaTime * panSpeed;
                //transform.Translate(Vector3.forward * Time.deltaTime * panSpeed, Space.World);
            }

            //Déplacement bas
            if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= panBorder)
            {
                pos += Vector3.back * Time.deltaTime * panSpeed;
                //transform.Translate(Vector3.back * Time.deltaTime * panSpeed, Space.World);
            }

            //Déplacement gauche
            if (Input.GetKey(KeyCode.Q) || Input.mousePosition.x <= panBorder)
            {
                pos += Vector3.left * Time.deltaTime * panSpeed;
                //transform.Translate(Vector3.left * Time.deltaTime * panSpeed, Space.World);
            }

            //Déplacement droite
            if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width - panBorder)
            {
                pos += Vector3.right * Time.deltaTime * panSpeed;
                //transform.Translate(Vector3.right * Time.deltaTime * panSpeed, Space.World);
            }

            //zoom
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            

            //multiplication vénère car valeur molette très faible
            pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime;
        }

        

        //Bloquaque caméra en x y z
        
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);


        //if(transform.position!=pos) transform.Translate(pos,0);
        transform.position = pos;
        #endregion

        #region ROTATION
        // JE VEUX TOURNER AUTOUR DE Y DANS WORLD SPACE
        //Déplacement haut
        if (Input.GetKey(KeyCode.A))
        {

            transform.Rotate(Vector3.down, rotSpeed * Time.deltaTime, Space.World);
        }

        //Déplacement bas
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime, Space.World);
        }
        #endregion
    }
}
