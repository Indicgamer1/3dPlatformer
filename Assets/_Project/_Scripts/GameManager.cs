using UnityEngine;

namespace Platformer
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance{get; private set;}
        public static bool isGamePlaying{get; private set;}

        public int score{get; private set;}

        private void Awake()
        {
            isGamePlaying = true;
            if (instance == null)
                instance = this;
            else 
                Destroy(gameObject);
            
            score = 0;
        }

        public void AddScore(int scoreToAdd)
        {
            score += scoreToAdd;
        }
    }
}