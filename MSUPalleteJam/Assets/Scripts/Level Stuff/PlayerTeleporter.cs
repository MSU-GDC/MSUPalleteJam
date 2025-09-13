using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    [SerializeField] private Transform _dungeonStart;

    public void TeleportPlayer(int teleportid)
    {
        TeleportPlayer((TELEPORTID_e)teleportid);
    }

    public void TeleportPlayer(TELEPORTID_e teleportid)
    {
        switch (teleportid)
        {
            case TELEPORTID_e.TP_DUNGEONSTART:
                Player.Singleton.transform.position = _dungeonStart.position;
                break;

            case TELEPORTID_e.TP_NONE:
            default:
                break;
        }
        Player.Singleton.PlayerRigidbody.linearVelocity = Vector2.zero;
    }


}
public enum TELEPORTID_e
{
    TP_NONE = 0,
    TP_DUNGEONSTART = 1
}
