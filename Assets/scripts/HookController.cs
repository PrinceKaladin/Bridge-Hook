using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class HookController : MonoBehaviour
{
    public int fishScore = 1;

    [Header("Auto cleanup")]
    public float maxLifeTime = 8f; // если ничего не поймал Ч исчезнет, чтобы можно было кидать снова

    Rigidbody2D rb;
    FisherCastController owner;
    bool finished;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(FisherCastController fisher)
    {
        owner = fisher;
        finished = false;

        // на вс€кий
        CancelInvoke();
        Invoke(nameof(Timeout), maxLifeTime);
    }

    public void Launch(Vector2 impulse)
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.AddForce(impulse, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;

        if (other.CompareTag("Fish"))
        {
            finished = true;
            CancelInvoke();

            // начисл€ем очки
            if (GameManager.I != null)
                GameManager.I.AddScore(fishScore);

            // говорим спавнеру: этот объект поймали Ч надо заспавнить новый позже
            var anchor = other.GetComponent<SpawnAnchor>();
            if (anchor != null && anchor.point != null && SpawnManager.I != null)
            {
                SpawnManager.I.OnCaughtAtPoint(anchor.point);
            }
            // убрать пойманный объект
            Destroy(other.gameObject);

            EndHook();
        }
        else if (other.CompareTag("Trash"))
        {
            finished = true;
            CancelInvoke();

            // проигрыш
            EndHook();

            // грузим сцену buildIndex = 3
            SceneManager.LoadScene(3);
        }
    }

    void Timeout()
    {
        if (finished) return;
        finished = true;
        EndHook();
    }

    void EndHook()
    {
        // разрешаем рыбаку бросать снова
        if (owner != null) owner.ClearHook();
        Destroy(gameObject);
    }
}
