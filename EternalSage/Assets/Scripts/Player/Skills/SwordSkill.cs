using UnityEngine;


public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{

    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity;
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] AimController crosshair;
    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweeenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;

    private Vector2 finalDir;
    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
    }

    protected override void Update()
    {
        if (crosshair != null && crosshair.IsAiming())
        {
            Vector2 normalDir = AimDirection();
            finalDir = new Vector2(normalDir.x * launchForce.x, normalDir.y * launchForce.y);

            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweeenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        switch (swordType)
        {
            case SwordType.Regular:
                break;
            case SwordType.Bounce:
                {
                    swordGravity = bounceGravity;
                    newSwordScript.SetupBounce(true, amountOfBounce);
                }
                break;
            case SwordType.Pierce:
                break;
            case SwordType.Spin:
                break;
            default:
                break;
        }

        newSwordScript.SetupSword(finalDir, swordGravity, player);

        player.AssignNewSword(newSword);
        DotsActive(false);
    }

    #region Aim Region

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 finalPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (crosshair != null)
        {
            finalPosition = crosshair.transform.position;
        }

        //Return a vector2 wich is the direction
        return (finalPosition - playerPosition).normalized;
    }

    public void DotsActive(bool _active)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_active);
        }
    }


    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 normalDir = AimDirection();
        Vector2 position = (Vector2)player.transform.position +
                            new Vector2(normalDir.x * launchForce.x, normalDir.y * launchForce.y) *
                            t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }

    public void ResetCrosshairPosition()
    {
        crosshair.transform.position = new Vector2(player.transform.position.x + 2 * player.facingDir, player.transform.position.y);
    }

    public Vector2 GetCrosshairPos()
    {
        return crosshair.transform.position;
    }
    #endregion
}