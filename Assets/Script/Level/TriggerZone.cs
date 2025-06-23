using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public bool oneShot = false;
    private bool alreadyEntered = false;
    private bool alreadyExited = false;

    public string collisionTag;

    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alreadyEntered)
        {
            return;
        }

        if (!string.IsNullOrEmpty(collisionTag) && !collision.CompareTag(collisionTag))
        {
            return;
        }

        OnTriggerEnter?.Invoke();

        if (oneShot)
        {
            alreadyEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (alreadyExited)
        {
            return;
        }

         if (!string.IsNullOrEmpty(collisionTag) && !collision.CompareTag(collisionTag))
        {
            return;
        }

        OnTriggerExit?.Invoke();

        if (oneShot)
        {
            alreadyExited = true;
        }
    }
}
