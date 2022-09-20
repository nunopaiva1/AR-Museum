using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    public Text timeText;
    public GameObject video;
    public string sentence;
    public Camera AR_cam, main_cam;
    float Timer = 0.0f;
    float Min = 0.0f;
    public int marksCount = 0;

    public Button btn_Info, btn_Instruc, btn_Sobre, btn_vidM1, btn_vidM2, btn_vidM3;
    public Text titulo, line1, line2, line3, tituloMarca, descMarca;
    public GameObject panelInfo, menu, btn_AbreFecha, mapa, btn_Mapa, painelMarca, painelParabens;

    public Button icon1, icon2, icon3;
    public Sprite iconChecked;

    private Vector3 startingPosition;

    private bool mark1Active, mark2Active, mark3Active, detected1=false, detected2=false, detected3=false;

    public Text txtScore, txtInfo1, txtInfo2, txtInfo3;
    public RawImage mk1, mk2, mk3;
    public Texture check;

    private string defaultMSG = "Procure a marca para obter informações sobre a peça correspondente!";

    public AudioSource success, complete;

    private int count = 0;

    // Use this for initialization
    void Start()
    {
        startingPosition = btn_AbreFecha.transform.position;

    }

    private void Update()
    {
        if (Timer > 9) timeText.text = "Tempo decorrido: 0" + Min + ":" + System.Math.Round(Timer, 0);
        else timeText.text = "Tempo decorrido: 0" + Min + ":0" + System.Math.Round(Timer, 0);

        if (Timer > 59)
        {
            Min += 1;
            Timer = 0;
        }
        
        Timer += Time.deltaTime;

        mark1Active = GameObject.Find("rocha_TARGET").GetComponent<DefaultTrackableEventHandler>().marca1;
        if (mark1Active && !detected1)
        {
            StartCoroutine(panelParabens("marca1"));
        }

        mark2Active = GameObject.Find("astronauta_TARGET").GetComponent<DefaultTrackableEventHandler>().marca2;
        if (mark2Active && !detected2)
        {
            StartCoroutine(panelParabens("marca2"));
        }

        mark3Active = GameObject.Find("solo_TARGET").GetComponent<DefaultTrackableEventHandler>().marca3;
        if (mark3Active && !detected3)
        {
            StartCoroutine(panelParabens("marca3"));
        }
    }

    public void showAnimation(string nomeVideo)
    {
        AR_cam.enabled = !AR_cam.enabled;
        main_cam.enabled = !main_cam.enabled;

        var someScript = GameObject.FindGameObjectWithTag("video").GetComponent<Video>();
        StartCoroutine(someScript.playAnim(nomeVideo));

        AR_cam.enabled = !AR_cam.enabled;
        main_cam.enabled = !main_cam.enabled;
    }

    public void openInfo()
    {
        titulo.text = "Informações";
        btn_Info.gameObject.SetActive(false);
        btn_Instruc.gameObject.SetActive(false);
        //btn_Sobre.gameObject.SetActive(false);
        panelInfo.SetActive(true);

    }

    public void openInstructions()
    {
        titulo.text = "Instruções";
        btn_Info.gameObject.SetActive(false);
        btn_Instruc.gameObject.SetActive(false);
        //btn_Sobre.gameObject.SetActive(false);
        line1.gameObject.SetActive(true);
        line2.gameObject.SetActive(true);
        line3.gameObject.SetActive(true);
    }

    public void reset()
    {
        titulo.text = "Menu";
        btn_Info.gameObject.SetActive(true);
        btn_Instruc.gameObject.SetActive(true);
        //btn_Sobre.gameObject.SetActive(true);
        panelInfo.SetActive(false);
        line1.gameObject.SetActive(false);
        line2.gameObject.SetActive(false);
        line3.gameObject.SetActive(false);
    }

    public void abreFechaMenu()
    {

        if (!menu.activeSelf)
        {
            menu.SetActive(true);
            btn_AbreFecha.transform.position = startingPosition;
            btn_AbreFecha.GetComponentInChildren<Text>().text = "<<";
        }
        else
        {
            fechaMenu();
        }

    }

    public void fechaMenu()
    {
        Vector3 pos = btn_AbreFecha.transform.position;
        menu.SetActive(false);
        print("O panel está fechado: " + panelInfo.activeSelf);
        pos.x = 25f;
        btn_AbreFecha.transform.position = pos;
        btn_AbreFecha.GetComponentInChildren<Text>().text = ">>";
    }

    public void Mapa()
    {
        if (!mapa.activeSelf)
        {
            mapa.SetActive(true);
            fechaMenu();
            btn_Mapa.GetComponentInChildren<Text>().text = "Ver Câmara";
        }
        else
        {
            mapa.SetActive(false);
            btn_Mapa.GetComponentInChildren<Text>().text = "Ver Mapa";

        }
    }

    public void abrePainel(string marca)
    {
        if(marca == "marca1")
        {
            if (!detected1)
            {
                if (painelMarca.activeSelf) fechaPainel();
                painelMarca.SetActive(true);
                tituloMarca.text = "Marca #1";
                descMarca.text = defaultMSG;
                btn_vidM1.gameObject.SetActive(true);
                btn_vidM2.gameObject.SetActive(false);
                btn_vidM3.gameObject.SetActive(false);

            }
            else atualizaMapa("marca1");
        }
        else if(marca == "marca2")
        {
            if (!detected2)
            {
                if (painelMarca.activeSelf) fechaPainel();
                painelMarca.SetActive(true);
                tituloMarca.text = "Marca #2";
                descMarca.text = defaultMSG;
                btn_vidM2.gameObject.SetActive(true);
                btn_vidM1.gameObject.SetActive(false);
                btn_vidM3.gameObject.SetActive(false);
            }
            else atualizaMapa("marca2");
        }
        else if(marca == "marca3")
        {
            if (!detected3)
            {
                if (painelMarca.activeSelf) fechaPainel();
                painelMarca.SetActive(true);
                tituloMarca.text = "Marca #3";
                descMarca.text = defaultMSG;
                btn_vidM3.gameObject.SetActive(true);
                btn_vidM1.gameObject.SetActive(false);
                btn_vidM2.gameObject.SetActive(false);
            }
            else atualizaMapa("marca3");

        }
    }

    public void fechaPainel()
    {
        btn_vidM1.gameObject.SetActive(false);
        btn_vidM2.gameObject.SetActive(false);
        btn_vidM3.gameObject.SetActive(false);
        painelMarca.SetActive(false);
    }

    private IEnumerator panelParabens(string marca)
    {
        count += 1;
        if (count != 3) success.Play();
        else complete.Play();
        Handheld.Vibrate();

        if (marca == "marca1")
        {
            atualizaInformacao(marca);
            icon1.image.sprite = iconChecked;
            painelParabens.SetActive(true);
            detected1 = true;
            painelParabens.GetComponentInChildren<Text>().text = "Fantástico, encontrou a Marca #1! - Rochas. Obtenha mais informações " +
                "sobre esta marca clicando sobre a mesma no mapa!";
        }

        else if (marca == "marca2")
        {
            atualizaInformacao(marca);
            icon2.image.sprite = iconChecked;
            painelParabens.SetActive(true);
            detected2 = true;
            painelParabens.GetComponentInChildren<Text>().text = "Uau, encontrou a Marca #2! - Armstrong. Obtenha mais informações " +
                "sobre esta marca clicando sobre a mesma no mapa!";
        }

        else if (marca == "marca3")
        {
            atualizaInformacao(marca);
            icon3.image.sprite = iconChecked;
            painelParabens.SetActive(true);
            detected3 = true;
            painelParabens.GetComponentInChildren<Text>().text = "Incrível, encontrou a Marca #3! - Rachadura. Obtenha mais informações " +
                "sobre esta marca clicando sobre a mesma no mapa!";
        }


        yield return new WaitForSeconds(5f);

        painelParabens.SetActive(false);

    }

    public void atualizaMapa(string marca)
    {
        if(marca == "marca1")
        {
            painelMarca.SetActive(true);
            tituloMarca.text = "#1 - Rocha";
            descMarca.text = "Esta rocha, datada de 1385, foi usada na Batalha de Aljubarrota" +
                " pelas tropas portuguesas.";
            desativaBotoes();
        }

        else if (marca == "marca2")
        {
            painelMarca.SetActive(true);
            tituloMarca.text = "#2 - Armstrong";
            descMarca.fontSize = 40;
            descMarca.text = "Nascido a 5 de agosto de 1930, Neil Armstrong foi um engenheiro aeroespacial e " +
                " astronauta norte-americano que se tornou o primeiro homem a pisar na Lua em 1969.";
            desativaBotoes();
        }
        else if (marca == "marca3")
        {
            painelMarca.SetActive(true);
            tituloMarca.text = "#3 - Rachadura de solo";
            descMarca.fontSize = 40;
            descMarca.text = "Como há 138 milhões de anos, quando o Brasil começou afastar-se do continente africano, a região conhecida como " +
                " Chifre da África deve, daqui algumas dezenas de milhões de anos, formar um novo continente.";
            desativaBotoes();
        }
    }

    private void atualizaInformacao(string marca)
    {
        if (marca == "marca1")
        {
            txtInfo1.text = "#1 Rocha";
            mk1.texture = check;
            txtScore.text = "Marcas encontradas: " + count + "/3";
        }

        else if (marca == "marca2")
        {
            txtInfo2.text = "#2 Armstrong";
            mk2.texture = check;
            txtScore.text = "Marcas encontradas: " + count + "/3";
        }

        else if (marca == "marca3")
        {
            txtInfo3.text = "#3 Rachadura";
            mk3.texture = check;
            txtScore.text = "Marcas encontradas: " + count + "/3";
        }
    }

    public void desativaBotoes()
    {
        btn_vidM1.gameObject.SetActive(false);
        btn_vidM2.gameObject.SetActive(false);
        btn_vidM3.gameObject.SetActive(false);
    }
}
