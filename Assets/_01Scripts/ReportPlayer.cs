using UnityEngine;

public class ReportPlayer : MonoBehaviour, IPlayarable
{   
    [SerializeField] private ThePlayer player;
    public ThePlayer GetPlayer()
    {
        return player;
    }
}