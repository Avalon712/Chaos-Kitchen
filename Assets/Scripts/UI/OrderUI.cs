using ChaosKitchen.Items;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosKitchen.UI
{
    public sealed class OrderUI : MonoBehaviour
    {
        [SerializeField] private Transform _showMenus;

        public void ShowRecipe(Recipe recipe)
        {
            for (int i = 0; i < _showMenus.childCount; i++)
            {
                Transform menu = _showMenus.GetChild(i);
                if (!menu.gameObject.activeSelf)
                {
                    menu.gameObject.SetActive(true);
                    ShowRecipeInfo(recipe, menu);
                    return;
                }
            }
        }

        private void ShowRecipeInfo(Recipe recipe, Transform menu)
        {
            TMP_Text menuNameTxt = menu.GetComponentInChildren<TMP_Text>();
            menuNameTxt.text = recipe.menuName;

            List<KitchenObjectType> icons = recipe.recipe;
            Transform menus = menu.GetChild(0);

            for (int i = 0; i < menus.childCount; i++)
            {
                menus.GetChild(i).gameObject.SetActive(i < icons.Count);
                if (i < icons.Count)
                {
                    menus.GetChild(i).GetComponent<Image>().sprite = KitchenManager.Instance.GetIcon(icons[i]);
                }
            }
        }

        public void HideRecipe(string menuName)
        {
            for (int i = 0; i < _showMenus.childCount; i++)
            {
                Transform menu = _showMenus.GetChild(i);
                if (menu.gameObject.activeSelf)
                {
                    TMP_Text menuNameTxt = menu.GetComponentInChildren<TMP_Text>();
                    if (menuNameTxt.text == menuName)
                    {
                        Transform meunIcons = menu.GetChild(0);
                        for (int j = 0; j < meunIcons.childCount; j++)
                        {
                            meunIcons.GetChild(j).gameObject.SetActive(false);
                        }
                        menu.gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
}
