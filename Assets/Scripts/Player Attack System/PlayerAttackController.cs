using UnityEngine;
using Game.ProgressionSystem;
public class PlayerAttackController : MonoBehaviour, IUpgradeable
{
    public WaveAttack attackPrefab;
    private NoteManager _noteManager;

    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float attackSpeed = 2f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private float attackSpread = 40f;
    

    void Start()
    {
        _noteManager = GetComponent<NoteManager>();
    }

    public void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            TryTriggerAttack(_noteManager.GetNextNote());
        }
    }
    public bool TryTriggerAttack(Note note){
        if (note == null){
            return false;
        }

        note.Expire();
        var attack = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        attack.sourceObject = gameObject;


        float qualityMultiplier = Mathf.Max(0.5f + Mathf.Pow(note.GetPercentageToHitTime(), 2f), 1f);
        attack.InitializeAttack(attackDamage * qualityMultiplier, attackSpeed * qualityMultiplier, attackRange * qualityMultiplier, attackSpread * qualityMultiplier, GetMouseAngle());

        

        return true;
    }

    private float GetMouseAngle(){
        var mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPos.z = 0f;
        return Mathf.Atan2(worldPos.y - transform.position.y, worldPos.x - transform.position.x) * Mathf.Rad2Deg;
    }

    public void Upgrade(UpgradeType upgradeType){
        switch(upgradeType){
            case UpgradeType.AttackDamage:
                attackDamage += 1f;
                break;
            case UpgradeType.AttackSpeed:
                attackSpeed += 1f;
                break;
            case UpgradeType.AttackRange:
                attackRange += 1f;
                break;
            case UpgradeType.AttackSpread:
                attackSpread += 10f;
                break;
        }
    }
}
