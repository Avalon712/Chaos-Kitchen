
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ChaosKitchen.UI
{
    /// <summary>
    /// 显示餐盘中装了那些菜的UI
    /// </summary>
    public sealed class PlateUI : MonoBehaviour
    {
        [SerializeField] private Transform _showRecipe;

        private Vector3 _forward;
        private void Start()
        {
            _forward = Camera.main.transform.forward;
            StartCoroutine(FaceCamera());
        }

        private IEnumerator FaceCamera()
        {
            while (true)
            {
                _showRecipe.forward = _forward;
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void ShowIcon(Sprite icon)
        {
            for (int i = 0; i < _showRecipe.childCount; i++)
            {
                Transform item = _showRecipe.GetChild(i);
                if (!item.gameObject.activeSelf)
                {
                    //显示食物的UI
                    item.gameObject.SetActive(true);
                    item.GetComponent<Image>().sprite = icon;
                    return;
                }
            }

            GameObject clone = GameObject.Instantiate(_showRecipe.GetChild(0).gameObject, _showRecipe);
            clone.GetComponent<Image>().sprite = icon;
            clone.SetActive(true);
        }

        public void Close()
        {
            for (int i = 0; i < _showRecipe.childCount; i++)
            {
                _showRecipe.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
