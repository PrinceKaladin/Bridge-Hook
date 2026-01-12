using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    GameObject current;
    float respawnAt = -1f;

    public bool HasObject => current != null;

    public void SetCurrent(GameObject go)
    {
        current = go;
        respawnAt = -1f;

        // чтобы HookController смог найти точку через GetComponent<SpawnPoint>()
        // сам SpawnPoint висит на пустом объекте, а рыба/мусор Ч отдельный объект,
        // поэтому св€зь делаем через добавление "€кор€" компонентом на спавн-объект:
        var anchor = go.GetComponent<SpawnAnchor>();
        if (anchor == null) anchor = go.AddComponent<SpawnAnchor>();
        anchor.point = this;
    }

    public void ClearCurrent()
    {
        current = null;
    }

    public void Schedule(float delay)
    {
        respawnAt = Time.time + delay;
    }

    public bool IsReadyToRespawn()
    {
        return !HasObject && respawnAt > 0f && Time.time >= respawnAt;
    }
}

// маленький компонент У€ родом из этой точкиФ
public class SpawnAnchor : MonoBehaviour
{
    public SpawnPoint point;
}
