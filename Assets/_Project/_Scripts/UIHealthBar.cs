using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class UIHealthBar : MonoBehaviour
    {
        [SerializeField] Health entiyHealth;
        [SerializeField] Image healthBar;
        
        private Transform player;
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            entiyHealth.OnHealthChanged += OnHealthChanged;
        }


        private void OnHealthChanged(float healthPercentage)
        {
            healthBar.fillAmount = healthPercentage;
            print($"{transform.parent.name}: health percentage : {healthPercentage}");
        }
    }
}