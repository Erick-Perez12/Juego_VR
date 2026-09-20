using UnityEngine;

public static class VRTeleport
{
    public static void TeleportTo(Transform rig, Transform target, float margenY = 0.05f)
    {
        Camera head = Camera.main;
        if (rig == null || target == null || head == null) return;

        CharacterController cc = rig.GetComponentInParent<CharacterController>();
        Transform root = cc != null ? cc.transform : rig;

        Rigidbody rb = root.GetComponent<Rigidbody>();
        if (cc != null) cc.enabled = false;
        if (rb != null) { rb.velocity = Vector3.zero; rb.angularVelocity = Vector3.zero; }

        float giro = target.eulerAngles.y - head.transform.eulerAngles.y;
        root.RotateAround(head.transform.position, Vector3.up, giro);

        Vector3 offset = head.transform.position - root.position;
        offset.y = 0f;

        Vector3 nuevaPos = target.position - offset;
        nuevaPos.y = target.position.y + margenY;
        root.position = nuevaPos;

        Physics.SyncTransforms();
        if (cc != null) cc.enabled = true;
    }
}