using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpot : MonoBehaviour
{
    [SerializeField] private Transform guns;
    [SerializeField] private GameObject options;

    private Gun currentGun;

    private void Start()
    {
        Replace(guns.GetChild(0).gameObject);
    }

    public void Replace(GameObject gun) 
    {
        currentGun = ActivateGunWithName(gun.name);

        foreach (Transform child in options.transform) 
        {
            child.gameObject.SetActive(false);
        }

        int i = 0;
        foreach (GunData menuOption in currentGun.gunData.menuOptions) 
        {
            GameObject optionContainer = options.transform.GetChild(i).gameObject;
            optionContainer.SetActive(true);

            GameObject option = optionContainer.transform.GetChild(0).gameObject;
            CodeButton optionButton = option.GetComponent<CodeButton>();
            optionButton.onClick.RemoveAllListeners();
            optionButton.onClick.AddListener(() =>
            {
                if (!menuOption.name.Equals("None"))
                {
                    if (LevelManager.instance.MoneyCount < menuOption.shopCost) return;
                    LevelManager.instance.MoneyCount -= menuOption.shopCost;
                }
                else LevelManager.instance.MoneyCount += currentGun.gunData.totalCost / 2;

                Replace(GetGunWithDataName(menuOption.name).gameObject);
            });

            RMOption optionRMOption = option.GetComponent<RMOption>();
            optionRMOption.ReplaceIcon(menuOption.shopSprite);

            i++;
        }
    }

    public Gun ActivateGunWithName(string name) 
    {
        foreach (Transform gun in guns)
        {
            if (gun.name.Equals(name))
            {
                currentGun?.gameObject.SetActive(false);
                gun.gameObject.SetActive(true);
                return gun.GetComponent<Gun>();
            }
        }
        return null;
    }

    public Gun GetGunWithDataName(string name)
    {
        foreach (Transform gun in guns)
        {
            if (gun.GetComponent<Gun>().gunData.name.Equals(name))
            {
                return gun.GetComponent<Gun>();
            }
        }
        return null;
    }
}
