using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberBehaviour : CreatureBehaviour {
    public bool isInRange, isTextVisible, isInteractable, isFollowing, isInFollowingRange;
    private GameObject _UIElement;
    public KeyCode interactKey;
    public FloatingTextController thanksText;
    private float _delayBeforeShowingText;
    private GameObject _player;
    private float _followSpeed;
    public Animator animator;
    private PlayerMovement _movement;
    public MoveToTile mtm;
    public Vector2 move;
    private bool wasSaved;
    
    public override void Initialize(int HP){
        isInteractable = false;
        isInRange = false;
        isFollowing = false;
        wasSaved = false;
        isTextVisible = false;
        _delayBeforeShowingText = 1.3f;
        _player = GameManager.PlayerObj();
        _followSpeed = 6.5f;
        _movement = _player.GetComponent<PlayerMovement>();
        mtm = GetComponent<MoveToTile>();
    }

    public override void UpdateBehaviour() {
        isInRange = GameManager.IsNearby(transform.position);

        if(isInRange && !isFollowing && isInteractable){
            isTextVisible = true;
            TextUIManager.UpdateInteractableText(true);
        }

        if(isTextVisible && !isInRange){
            isTextVisible = false;
            TextUIManager.UpdateInteractableText(false);
        }
        
        if(isInteractable && isInRange && Input.GetKeyDown(interactKey) && !wasSaved){
            StartFollow();
            isTextVisible = false;
            TextUIManager.UpdateInteractableText(false);
        }

        if(isFollowing){
            isInFollowingRange = GameManager.IsInFollowingRange(transform.position);

            if(GameManager.InFight()){
                StopFollowing();

            } else if(_movement.move.sqrMagnitude < 0.1){
                if(isInFollowingRange){
                    StopMovement();
                } else {
                    FollowPlayer();
                }
            } else {
                FollowPlayer();
            }
        }

        if(wasSaved && !isFollowing){
            Vector3 direction = (mtm.targetPosition - transform.position).normalized;
            move.x = Mathf.Clamp(direction.x, -1f, 1f);
            move.y = Mathf.Clamp(direction.y, -1f, 1f);

            animator.SetFloat("Horizontal", move.x);
            animator.SetFloat("Vertical", move.y);
            animator.SetFloat("MovementSpeed", move.sqrMagnitude);
        }
    }

    private IEnumerator ShowThanksTextCoroutine() {
        yield return new WaitForSeconds(_delayBeforeShowingText);
        SayThanks();

    }

    public void Escaped() {
        isInteractable = true;
        Debug.Log("ally escaped");
        StartCoroutine(ShowThanksTextCoroutine());
    }

    private void SetUpHealthBar(){
        _UIElement = HealthBarManager.CreateHealthBar(gameObject);
    }

    private void StartFollow(){
        isInteractable = false;
        isFollowing = true;
        wasSaved = true;
        GameManager.Rescue(gameObject);
        SetUpHealthBar();
        Debug.Log("following started");
    }

    private void SayThanks(){
        thanksText = TextUIManager.GetFTC();
        Vector3 textPos = transform.position;
        textPos.x *= 25f;
        textPos.y *= 60f;
        Debug.Log(textPos);
        thanksText.Activate(textPos);
        thanksText.ShowText("Oh.. Thanks");
    }

    private void FollowPlayer() {
        Vector2 direction = _player.transform.position - transform.position;
        move = _movement.move;
        animator.SetFloat("Horizontal", move.x);
        animator.SetFloat("Vertical", move.y);
        animator.SetFloat("MovementSpeed", Mathf.Clamp(direction.sqrMagnitude, 0.1f, 1f));

        transform.position = Vector2.MoveTowards(this.transform.position, _player.transform.position, _followSpeed * Time.deltaTime);
    }

    private void StopMovement() {
        move.x = 0;
        move.y = 0;
        animator.SetFloat("Horizontal", move.x);
        animator.SetFloat("Vertical", move.y);
        animator.SetFloat("MovementSpeed", 0);
    }

    private void StopFollowing() {
        StopMovement();
        isFollowing = false;
    }

    public override void TakeDmg(int DMGvalue){
        Entity currentEntity = _current.GetComponent<Entity>();
        currentEntity.HP.Take(DMGvalue);

        if(currentEntity.HP.GetValue() <= 0){
            currentEntity.Death();
            BattleManager.GoNext();
        }
        
        UpdateHP(currentEntity);
    }

    public override void AddHeal(int HEALvalue){
        Entity currentEntity = _current.GetComponent<Entity>();
        currentEntity.HP.Add(HEALvalue);
        UpdateHP(currentEntity);
    }

    public void UpdateHP(Entity entity){
        HealthBarManager.UpdateHPFor(entity);
        FightUIManager.UpdateHPFor(entity);
    }
}
