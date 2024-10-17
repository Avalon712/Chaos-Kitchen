using ChaosKitchen.Items;
using ChaosKitchen.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ChaosKitchen
{
    public sealed class OrderManager : MonoBehaviour
    {
        [SerializeField] private OrderUI _orderUI;
        [Range(1, 5)]
        [SerializeField] private int _maxMenuCount;
        [SerializeField] private float _interval;
        [Header("所有食谱")]
        [SerializeField] private List<Recipe> _recipes;

        private bool _isStartGame;
        private float _timer;

        /// <summary>
        /// 玩家成功制作的菜单数量
        /// </summary>
        public int SuccessNum { get; private set; }

        /// <summary>
        /// 所有随机生成的菜单
        /// </summary>
        private List<Recipe> _menu = new(5);

        public static OrderManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            EventManager.Instance.RegisterEvent(GameEvent.StartGame, StartGame);
            EventManager.Instance.RegisterEvent(GameEvent.GameOver, GameOver);
            EventManager.Instance.RegisterEvent(GameEvent.PauseGame, PauseGame);
            EventManager.Instance.RegisterEvent(GameEvent.ContinueGame, ContinueGame);
        }

        private void ContinueGame()
        {
            _isStartGame = true;
        }

        private void PauseGame()
        {
            _isStartGame = false;
        }

        private void StartGame()
        {
            _isStartGame = true;
            AutoGenerateMenu();
        }

        private void GameOver()
        {
            _isStartGame = false;
            for (int i = 0; i < _menu.Count; i++)
            {
                _orderUI.HideRecipe(_menu[i].menuName);
            }
            _menu.Clear();
            SuccessNum = 0;
        }

        private void Update()
        {
            if (_isStartGame && _maxMenuCount > _menu.Count)
            {
                _timer += Time.deltaTime;
                if (_timer >= _interval)
                {
                    _timer = 0;
                    AutoGenerateMenu();
                }
            }
        }

        private void AutoGenerateMenu()
        {
            Recipe recipe = _recipes[Random.Range(0, _recipes.Count)];
            _orderUI.ShowRecipe(recipe);
            _menu.Add(recipe);
        }

        /// <summary>
        /// 判断玩家制作的食物是否在菜单中
        /// </summary>
        public bool CheckMenu(List<KitchenObjectType> foods)
        {
            for (int i = 0; i < _menu.Count; i++)
            {
                Recipe recipe = _menu[i];
                List<KitchenObjectType> types = recipe.recipe;

                if (foods.Count == types.Count)
                {
                    for (int j = 0; j < types.Count; j++)
                    {
                        if (!foods.Contains(types[j])) { break; }
                    }

                    _orderUI.HideRecipe(_menu[i].menuName);
                    _menu.Remove(recipe);
                    SuccessNum += 1;
                    return true;
                }
            }
            return false;
        }


        private void OnDestroy()
        {
            Instance = null;
        }
    }
}
