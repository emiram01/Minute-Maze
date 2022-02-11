using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator _bodyAnim;
    [SerializeField] private Animator _armsAnim;

    private int _grounded;
    private int _walkingForward;
    private int _walkingBack;
    private int _walkingLeft;
    private int _walkingRight;
    private int _running;
    private int _crouching;

    private void Start()
    {
        _grounded = Animator.StringToHash("Grounded");
        _walkingForward = Animator.StringToHash("WalkingForward");
        _walkingBack = Animator.StringToHash("WalkingBack");
        _walkingLeft = Animator.StringToHash("WalkingLeft");
        _walkingRight = Animator.StringToHash("WalkingRight");
        _running = Animator.StringToHash("Running");
        _crouching = Animator.StringToHash("Crouching");
    }

    private void AnimateBoth(int state, bool b)
    {
        _bodyAnim.SetBool(state, b);
        _armsAnim.SetBool(state, b);
    }

    public void Grounded()
    {
        AnimateBoth(_grounded, true);
    }

    public void NotGrounded()
    {
        AnimateBoth(_grounded, false);
    }

    public void WalkingForward()
    {
        AnimateBoth(_walkingForward, true);
        AnimateBoth(_walkingBack, false);
        AnimateBoth(_walkingLeft, false);
        AnimateBoth(_walkingRight, false);
    }

    public void WalkingBack()
    {
        AnimateBoth(_walkingForward, false);
        AnimateBoth(_walkingBack, true);
        AnimateBoth(_walkingLeft, false);
        AnimateBoth(_walkingRight, false);
    }

    public void WalkingLeft()
    {
        AnimateBoth(_walkingForward, false);
        AnimateBoth(_walkingBack, false);
        AnimateBoth(_walkingLeft, true);
        AnimateBoth(_walkingRight, false);
    }

    public void WalkingRight()
    {;
        AnimateBoth(_walkingForward, false);
        AnimateBoth(_walkingBack, false);
        AnimateBoth(_walkingLeft, false);
        AnimateBoth(_walkingRight, true);
    }

    public void Idle()
    {
        AnimateBoth(_walkingForward, false);
        AnimateBoth(_walkingBack, false);
        AnimateBoth(_walkingLeft, false);
        AnimateBoth(_walkingRight, false);
    }

    public void TurningLeft()
    {
        _bodyAnim.SetBool(_walkingLeft, true);
    }

    public void TurningRight()
    {
        _bodyAnim.SetBool(_walkingRight, true);
    }

    public void Running()
    {
        AnimateBoth(_walkingForward, false);
        AnimateBoth(_running, true);
    }

    public void NotRunning()
    {
        AnimateBoth(_running, false);
    }

    public void Crouching()
    {
        AnimateBoth(_crouching, true);
    }

    public void NotCrouching()
    {
        AnimateBoth(_crouching, false);
    }
}