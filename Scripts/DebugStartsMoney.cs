using UnityEngine;

public sealed class DebugStartMoney : MonoBehaviour
{
    [SerializeField] private GameSession session;
    [SerializeField] private int targetMoney = 20000;

    private void Start()
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
        if (session == null || session.Money == null)
            return;

        int current = session.Money.Money;
        int delta = targetMoney - current;

        if (delta > 0)
            session.Money.Add(delta);
#endif
    }
}