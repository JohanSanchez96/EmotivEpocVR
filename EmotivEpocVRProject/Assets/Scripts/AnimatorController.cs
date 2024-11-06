using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Saludar()
    {
        anim.SetTrigger("Saludar");
    }

    public void Fallando()
    {
        anim.SetTrigger("Fallaste");
    }

    public void Felicidades()
    {
        anim.SetTrigger("Felicidades");
    }

    public void Hablar1(bool value)
    {
        anim.SetBool("Hablando1", value);
    }

    public void Hablar2(bool value)
    {
        anim.SetBool("Hablando2", value);
    }

    public void Idle2(bool value)
    {
        anim.SetBool("Idle2", value);
    }

    public void Walking(bool value)
    {
        anim.SetBool("Caminando", value);
    }
}
