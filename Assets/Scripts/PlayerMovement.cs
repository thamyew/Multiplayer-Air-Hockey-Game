using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    bool wasJustClicked;
    bool canMove;

    Rigidbody2D rb;
    Vector2 startingPosition;
    private ScoreScript score;

    public Transform BoundaryHolder;
    Boundary playerBoundary;
    Collider2D playerCollider;

    PhotonView view;

    struct Boundary
    {
        public float Top, Bottom, Left, Right;
        public Boundary(float top, float bottom, float left, float right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = rb.position;
        score = FindObjectOfType<ScoreScript>();
        playerCollider = GetComponent<Collider2D>();
        
        if (PhotonNetwork.IsMasterClient) BoundaryHolder = GameObject.FindGameObjectsWithTag("RedBoundaryHolder")[0].transform;
        else BoundaryHolder = GameObject.FindGameObjectsWithTag("BlueBoundaryHolder")[0].transform;

        view = GetComponent<PhotonView>();

        playerBoundary = new Boundary(BoundaryHolder.GetChild(0).position.y,
                                      BoundaryHolder.GetChild(1).position.y,
                                      BoundaryHolder.GetChild(2).position.x,
                                      BoundaryHolder.GetChild(3).position.x);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!score.finish) {
            if (view.IsMine)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    if (wasJustClicked)
                    {
                        wasJustClicked = false;

                        if (playerCollider.OverlapPoint(mousePos))
                        {
                            canMove = true;
                        }

                        else
                        {
                            canMove = false;
                        }
                    }

                    if (canMove)
                    {
                        Vector2 clampedMousePos = new Vector2(Mathf.Clamp(mousePos.x, playerBoundary.Left, playerBoundary.Right),
                                                            Mathf.Clamp(mousePos.y, playerBoundary.Bottom, playerBoundary.Top));
                        rb.MovePosition(clampedMousePos);
                    }
                }

                else
                {
                    wasJustClicked = true;
                }
            }
        }
    }

    public void ResetPosition()
    {
        rb.position = startingPosition;
    }

}
