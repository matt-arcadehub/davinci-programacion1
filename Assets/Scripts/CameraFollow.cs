using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target to Follow")]
    [SerializeField] private Transform target;

    [Header("Follow Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Lookahead Settings")]
    [SerializeField] private float lookaheadDistance = 2.5f;
    [SerializeField] private float lookaheadSpeed = 3f;

    private Vector3 currentLookahead;
    private Vector3 lastTargetPosition;

    private void Start()
    {
        if (target != null)
        {
            lastTargetPosition = target.position;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 moveDirection = (target.position - lastTargetPosition) / Time.deltaTime;
        lastTargetPosition = target.position;
        Vector3 targetLookahead = Vector3.zero;

        if (moveDirection.sqrMagnitude > 0.1f)
        {
            targetLookahead = moveDirection.normalized * lookaheadDistance;
        }

        currentLookahead = Vector3.Lerp(currentLookahead, targetLookahead, lookaheadSpeed * Time.deltaTime);
        Vector3 desiredPosition = target.position + offset + currentLookahead;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }
}
