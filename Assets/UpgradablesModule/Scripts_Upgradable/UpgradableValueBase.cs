using DataSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Upgradables
{
    public abstract class UpgradableValueBase : ScriptableObject, ISaveDataHolderJson
    {
        [SerializeField]protected int Level;
        [SerializeField][ShowIf("canMaxOut")]protected int numberOfLevels;
        [SerializeField] protected bool canMaxOut;
        [SerializeField]protected IntValue currencyInt;
        public UnityAction OnUpgradeSuccessfull;
        public bool writeValue;
        public bool writelevel;
        [SerializeField] private ValuesCurve valueProgression;
        [SerializeField] private ValuesCurve priceProgression;
        
        public float GetFloatValueAtLevel(int level)
        {
            if (IsMaxedOut())
            {
                level = numberOfLevels;
            }
            return valueProgression.GetValueForLevel(level);
        }
        
        public float GetCurrentValueFloat()
        {
            if (IsMaxedOut())
            {
                Level = numberOfLevels;
            }
            return valueProgression.GetValueForLevel(Level);
        }
        public int GetCurrentValueInt()
        {
            if (IsMaxedOut())
            {
                Level = numberOfLevels;
            }
            return valueProgression.GetValueForLevle(Level);
        }

        public int GetIntValueAtLevel(int level)
        {
            if (IsMaxedOut())
            {
                level = numberOfLevels;
            }
            return valueProgression.GetValueForLevle(level);
        }

        public int GetPriceAtLevel(int level)
        {
            if (IsMaxedOut())
            {
                level = numberOfLevels;
            }
            return priceProgression.GetValueForLevle(level);
        }

        public int GetCurrentPrice()
        {
            if (IsMaxedOut())
            {
                Level = numberOfLevels;
            }
            return priceProgression.GetValueForLevle(Level);
        }

        public float GetMaxValue()
        {
            if (canMaxOut)
            {
                return valueProgression.GetValueForLevle(numberOfLevels);
            }
            else
            {
                return GetCurrentValueFloat();
            }
        }
        public bool IsMaxedOut()
        {
            if (canMaxOut)
            {
                return Level >= numberOfLevels;
            }
            else
            {
                return false;
            }
        }
        public int GetNextLevel()
        {
            if (IsMaxedOut())
            {
                Level = numberOfLevels - 1;
            }
            return Level + 1;
        }

        public void TryUpgrade()
        {
            if (Upgrade())
            {
                Utility.DebugText("Upgrade successfull");
            }
            else
            {
                Utility.DebugText("Upgrade failed");
            }
        }
        bool Upgrade()
        {
            bool upgradeSuccessfull;
            if (CanUpgrade())
            {
                currencyInt.MyValue -= GetPriceAtLevel(Level);
                Level++;
                upgradeSuccessfull = true;
                OnUpgradeSuccessfull?.Invoke();
            }
            else
            {
                upgradeSuccessfull = false;
            }

            return upgradeSuccessfull;
        }

        [Button("Manual Upgrade")]
        public void ManualForcedUpgrade()
        {
            Level += 1;
            OnUpgradeSuccessfull?.Invoke();
        }

        public bool CanUpgrade()
        {
            float value = GetCurrentPrice();
            if (currencyInt.MyValue - value >= 0)
            {
                if (canMaxOut)
                {
                    if (!IsMaxedOut())
                    {
                        return true;    
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else
                {
                    return true;
                }
            }
            else
            {
                // Utility.DebugText($"Cant upgrade not enough currency {currencyInt.MyValue} cost {value}");
                return false;
            }
        }
        public SaveDataHolder GetMyData()
        {
            IntBasic basicInt = new IntBasic(this.name, Level);
            SaveDataHolder sdh = new SaveDataHolder(this.name, JsonUtility.ToJson(basicInt));
            return sdh;
        }

        public void SetMyData(SaveDataHolder loadedData)
        {
            IntBasic loadOut = JsonUtility.FromJson<IntBasic>(loadedData.jsondata);
            Level = loadOut.Value;
        }

        public void ResetMyData()
        {
            Level = 0;
        }
    }
}