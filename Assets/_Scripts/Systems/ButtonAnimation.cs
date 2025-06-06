using UnityEngine;
using UnityEngine.Events;
public class ButtonAnimation : MonoBehaviour
{
    [SerializeField] private UnityEvent anim;
    [SerializeField] private string animationName;
    private void Awake()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("UIAnimated");

        foreach (GameObject obj in taggedObjects)
        {
            anim.AddListener(() => obj.GetComponent<Animator>().Play(animationName));
        }   
    }

    public void callAnimation() 
    {
        anim.Invoke();
    }
}
