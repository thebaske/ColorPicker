using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Upgradables;

public class MoneyStack : MonoBehaviour
{
    [SerializeField] private GameObject money;
    [SerializeField] private float stackFrequency = 1f;
    private float stackFrequencyCounter;
    [SerializeField] private float stackDistance;
    [SerializeField] private int stackCounter;
    [SerializeField] private Transform stackObject;
    private bool stackMOney;
    Vector3 oldPosition;
    [SerializeField] private UpgradableValueInt cargoPayoutValue;
    private MoneyInternalStackAnim moneyAnim;
    
    public void StartStacking()
    {
        stackMOney = true;
    }
    void Update()
    {
        if (!stackMOney) return;
        
        if (Vector3.Distance(transform.position, oldPosition) > stackFrequency)
        {
            oldPosition = transform.position;
            StackMoney();
        }
    }

    void StackMoney()
    {
        if (stackCounter < 50)
        {
            GameObject mnyTmp = Instantiate(money, stackObject.position.WithAddY(stackCounter * stackDistance),
                stackObject.localRotation, stackObject);
            mnyTmp.transform.parent = stackObject;
            mnyTmp.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            mnyTmp.transform.localPosition = new Vector3(0f, stackCounter * stackDistance, 0f);
            moneyAnim = mnyTmp.GetComponent<MoneyInternalStackAnim>();
        }
        else
        {
            if (moneyAnim != null)
            {
                moneyAnim.TriggerAnimWorker();
            }
        }
        stackCounter++;
    }
}
