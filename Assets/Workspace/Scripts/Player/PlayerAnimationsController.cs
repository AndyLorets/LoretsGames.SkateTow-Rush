using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAnimationsController 
{
    private Animator _animator;
    private Player _player; 

    private const string anim_move_bool = "Moving";
    private const string anim_landing_trigger = "Landing";

    private string[] anims_trick_name = 
    { 
        "Trick_1", "Trick_2", "Trick_3",
        "Trick_4", "Trick_5", "Trick_6",
        "Trick_7", "Trick_8", "Trick_9"
    };
    private string[] anims_victory_name =
    {
        "Victory_1", "Victory_2", "Victory_3"
    };

    private const float normalized_transition_duration = .5f;

    public enum AnimationType
    {
        Victory, Trick
    }

    public PlayerAnimationsController(Player player, Animator animator)
    {
        this._animator = animator;
        _player = player;

        GameManager.onFinish += SetFinishAnimation;
        _player.onTrick += OnPlayerTrick; 
        _player.onDestroy += OnDestroy; 
        _player.onChangeRagdollState += SetAnimatorDeactive;
        _player.OnLanding += Landing; 
    }
    public void Moving(bool isMoving)
    {
        _animator.SetBool(anim_move_bool, isMoving);
    }
    private void PlayAnimation(AnimationType animationType)
    {
        string animName = GetAnimationName(animationType); 
        _animator.CrossFade(animName, normalized_transition_duration);
    }
    private void Landing()
    {
        _animator.SetTrigger(anim_landing_trigger);
    }
    private string GetAnimationName(AnimationType animationType)
    {
        switch (animationType)
        {
            case AnimationType.Victory:
                return anims_victory_name[Random.Range(0, anims_victory_name.Length)];
            case AnimationType.Trick:
                return anims_trick_name[Random.Range(0, anims_trick_name.Length)]; 
        }

        return "";
    }
    private void OnPlayerTrick()
    {
        PlayAnimation(AnimationType.Trick);
    }
    private void SetFinishAnimation()
    {
        PlayAnimation(AnimationType.Victory);
    }
    private void SetAnimatorDeactive(bool state)
    {
        _animator.enabled = !state;
    }
    private void OnDestroy()
    {
        GameManager.onFinish -= SetFinishAnimation;
        _player.onChangeRagdollState -= SetAnimatorDeactive;
        _player.onDestroy -= OnDestroy;
        _player.onTrick -= OnPlayerTrick;
        _player.OnLanding -= Landing;
    }
}
