using UnityEngine;
using Cinemachine;
public class SectorCameraManager : MonoBehaviour
{ 
    [SerializeField] private float movementSensitivity;
    [SerializeField] private float scrollSensitivity;
    [SerializeField] private float fovMax;
    [SerializeField] private float fovMin;
    [SerializeField] private PolygonCollider2D bounds;
    private Transform cameraSystem;
    private float targetFov = 35;
    private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        this.virtualCamera = GetComponent<CinemachineVirtualCamera>();
        this.cameraSystem = new GameObject("CameraSystem").transform;
        this.virtualCamera.Follow = this.cameraSystem;
    }
    private void Update()
    {
        this.cameraDragMovement();
        this.cameraZoom();
    }
    private void cameraDragMovement()
    {
        Vector3 moveDir = new Vector3(0, 0, 0);

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * this.movementSensitivity * Time.deltaTime;
        Vector3 newPosition = this.cameraSystem.transform.position + movement;

        bool canMoveHorizontally = this.bounds.bounds.Contains(new Vector3(newPosition.x, this.cameraSystem.transform.position.y, 0f));
        bool canMoveVertically = this.bounds.bounds.Contains(new Vector3(this.cameraSystem.transform.position.x, newPosition.y, 0f));

        if (canMoveHorizontally)
            this.cameraSystem.transform.position = new Vector3(newPosition.x, this.cameraSystem.transform.position.y, 0f);

        if (canMoveVertically)
            this.cameraSystem.transform.position = new Vector3(this.cameraSystem.transform.position.x, newPosition.y, 0f);
    }
    private void cameraZoom()
    {
        if(Input.mouseScrollDelta.y > 0)
            this.targetFov -= 5;
        if(Input.mouseScrollDelta.y < 0)
            this.targetFov += 5;

        
        this.targetFov = Mathf.Clamp(targetFov, fovMin, fovMax);
        this.virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(this.virtualCamera.m_Lens.FieldOfView, this.targetFov, Time.deltaTime * this.scrollSensitivity);
    }
}
