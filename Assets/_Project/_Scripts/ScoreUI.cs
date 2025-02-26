using System;
using KBCore.Refs;
using TMPro;
using System.Collections;
using UnityEngine;

namespace Platformer
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI scoreText;

        private void Start() => UpdateScore();

        public void UpdateScore()
        {
            StartCoroutine(UpdateScoreNextFrame());
        }

        IEnumerator UpdateScoreNextFrame()
        {
            yield return null;
            int score = GameManager.instance.score;
            scoreText.text = score.ToString();
        }
    }
}