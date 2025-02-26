using UnityEngine;
using UnityEngine.UI;

public  abstract class Shooter : MonoBehaviour
{
    [SerializeField] protected Transform barrel;
    [SerializeField] protected Transform barrelMouth;
    [SerializeField] protected Transform turret;
    [SerializeField] protected Transform bulletPrefab;
    [SerializeField] protected float bulletSpeed;

    [Header("Shooter Setting")] 
    [SerializeField] Image ReloadingBar;
    [SerializeField] protected float range = 30f;
    [SerializeField] float turretRotationSpeed = 100f;
    [SerializeField] float barrelRotationSpeed = 100f;
    [SerializeField] float barrelTopDownRange = 80f;
    [SerializeField] float offset = -40f;
    [SerializeField] float attackDelay = 2f;

    protected Transform player;
    protected Vector3 targetDirection;
    protected Vector3 rotatedForward;
    protected ParticleSystem shootParticle;

    protected CountdownTimer attackCoolDownTimer;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Target").transform;
        if(player == null)
            Debug.LogError("Can't find player");
        
        shootParticle = GetComponentInChildren<ParticleSystem>();
        attackCoolDownTimer = new CountdownTimer(attackDelay);
    }

    protected void Update()
    {
        attackCoolDownTimer.Tick(Time.deltaTime);
        targetDirection = player.position - barrel.position;
        HandleShooterRotation();

        if (CanShoot())
        {
            Shoot();
        }

        UpdateReloadBar();
    }

    private void UpdateReloadBar()
    {
        ReloadingBar.fillAmount = attackCoolDownTimer.Progress;
    }

    protected void HandleShooterRotation()
    {
        if(targetDirection.magnitude > range) return;
        
        RotateTurret(targetDirection);
        if (CanBarrelRotate())
        {
            RotateBarrel(targetDirection);
        }
        else
        {
           RotateTurret(Vector3.forward);
           RotateBarrel(rotatedForward);
        }
    }

    protected void RotateTurret(Vector3 targetDirection) 
    {
        Vector3 XZtargetDirection = new Vector3(targetDirection.x, 0f, targetDirection.z);
        Quaternion targetRotation = Quaternion.LookRotation(XZtargetDirection);
        turret.rotation = Quaternion.RotateTowards(turret.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
        
        //print("turret Rotating");
    }

    protected bool CanBarrelRotate()
    {
        rotatedForward = Quaternion.AngleAxis(offset, turret.right) * turret.forward; //vector which will determine limit
        float angleToPlayer = Vector3.Angle(targetDirection, rotatedForward);
            
        //Debug
        Debug.DrawLine(barrel.position, barrel.position + turret.up * range, Color.green);
        Debug.DrawLine(barrel.position, barrel.position + turret.forward * range, Color.red);
        Debug.DrawLine(barrel.position, barrel.position + turret.right * range, Color.blue);
        Debug.DrawLine(barrel.position, barrel.position + rotatedForward * range, Color.yellow);
        Debug.DrawLine(barrel.position, barrel.position + targetDirection * range, Color.yellow);

        if (angleToPlayer <= barrelTopDownRange / 2f)
        {
            //print($"Can rotate barrel {angleToPlayer}");
            return true;
        }
        else
        {
            //print($"Can't rotate barrel {angleToPlayer}");
            return false;
        }
    }

    public void RotateBarrel(Vector3 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        barrel.rotation = Quaternion.RotateTowards(barrel.rotation, targetRotation, barrelRotationSpeed * Time.deltaTime);
        
        //print("barrel Rotating");
    }

    protected bool CanShoot()
    {
        if(targetDirection.magnitude > range) return false;
        if (attackCoolDownTimer.IsRunning || !CanBarrelRotate()) return false;

        
        return true;
    }

    protected abstract void Shoot();
    
    private void OnDrawGizmos()
    {
        if(player != null)
            Gizmos.DrawLine(barrel.position, player.position);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
