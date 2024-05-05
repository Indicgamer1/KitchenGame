using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Player : MonoBehaviour,IKitchenObjectParent
{
    public static Player Instance { get;private set; }

    public event EventHandler OnPickSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selected_Counter;
    }

    [SerializeField] private float moveSpeed =10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    bool isWalking = false;
    private Vector3 lastInteractionDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    private void Awake()
    {
        
        if (Instance != null)
        {
            Debug.Log("There is more than one player instance");
        }
        Instance = this;
    }
    private void Start()
   {
        gameInput.OnInteraction += GameInput_OnInteraction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;

   }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteraction(object sender, System.EventArgs e)
    {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectedCounter != null)
       {
            selectedCounter.Interact(this);
       }
    }

    void Update()
    {
        HandleInteractions();
        HandleMovement();
    }
    void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetComponentVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);    
        if (moveDir != Vector3.zero)
        {
            lastInteractionDir = moveDir;
            
        }
        Debug.DrawLine(transform.position, transform.position + lastInteractionDir,Color.red);
        float interactDistance = 2f;
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position, lastInteractionDir, out hitInfo, interactDistance, counterLayerMask))
        {          
            if (hitInfo.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                if(baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);
                }             
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
    void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetComponentVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        isWalking = moveDir != Vector3.zero;
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if (!canMove)
        {
            // Can not move towards movedir
            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove =( moveDir.x<-.5f || moveDir.x> +.5f)  && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                // CanMove only on the X
                moveDir = moveDirX;
            }
            else
            {
                // Cannot Move only on the X
                // Attempt only on the Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = (moveDir.z < -.5f || moveDir.z > +.5f) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    // can only move in Z
                    moveDir = moveDirZ;
                }
                else
                {
                    //can not move in any direction
                }
            }

        }
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    public bool IsWalking()
    {
        return isWalking;
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selected_Counter = selectedCounter });
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        if(kitchenObject != null)
        {
            OnPickSomething?.Invoke(this,EventArgs.Empty);
        }
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
       return kitchenObject != null;
    }
}
