using Platformer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class CannonManager : MonoBehaviour
    {
        [SerializeField] Button straightBuletShooterButton;
        [SerializeField] TextMeshProUGUI straightBulletShooterText;
        [SerializeField] Transform straightBulletShooter;
        [SerializeField] Button homingBulletShooterButton;
        [SerializeField] TextMeshProUGUI homingBulletShooterText;
        [SerializeField] Transform homingBulletShooter;

        private const string off = "OFF";
        private const string on = "ON";

        void Start()
        {
            straightBulletShooter.GetComponent<Cannon>().enabled = false;
            homingBulletShooter.GetComponent<Cannon>().enabled = false;
            
            straightBuletShooterButton.onClick.AddListener((() =>
            {
                ToggleShooter(straightBulletShooter, straightBulletShooterText);
            }));
            homingBulletShooterButton.onClick.AddListener((() =>
            {
                ToggleShooter(homingBulletShooter, homingBulletShooterText);
            }));
        }

        private void ToggleShooter(Transform shooter, TextMeshProUGUI shooterButtonText)
        {
            bool canEnableShooter;
            string operationMode;
            if (shooter.GetComponent<Cannon>().enabled)
            {
                canEnableShooter = false;
                operationMode = off;
            }
            else
            {
                canEnableShooter = true;
                operationMode = on;
            }

            shooter.GetComponent<Cannon>().enabled = canEnableShooter;
            shooterButtonText.text = operationMode;
        }
    }
}