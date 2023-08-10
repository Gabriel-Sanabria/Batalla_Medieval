using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	public Text TextoMapa;
	public GameObject ObjetoTexto;
	public string[] NombresMapas;

	public int IndexMapa;

	public Animator AnimCampo;

	void Awake(){
		//Mostrar la apariencia del ultimo mapa elegido:
		switch(PlayerPrefs.GetInt("BatMedMapa", 0)){
		case 0:
			AnimCampo.Play("Mov_Agua");
			break;
		case 1:
			AnimCampo.Play("Mov_Lava");
			break;
		case 2:
			AnimCampo.Play("Congelado");
			break;
		case 3:
			AnimCampo.Play("Mov_Pantano");
			break;
		case 4:
			AnimCampo.Play("Seco");
			break;
		}
	}

	void Start () {
		//Iniciar mostrando el nombre del mapa elegido:
		ObjetoTexto.SetActive (true);
	}

	void Update () {

		//Obtener el valor del indexMapa:
		IndexMapa = PlayerPrefs.GetInt ("BatMedMapa", 0);

		//Mostrar siempre el nombre del mapa actualmente elegido:
		TextoMapa.text = "Mapa: " + NombresMapas [IndexMapa];
	}

	//Funciones para navegar entre mapas:
	public void AntMapa(){
		ObjetoTexto.SetActive (false);
		if (IndexMapa > 0) {
			IndexMapa--;
		} else {
			IndexMapa = NombresMapas.Length-1;
		}

		//Establecer la apariencia del mapa:
		switch (IndexMapa) {
		case 0:
			AnimCampo.Play("Mov_Agua");
		    break;
		case 1:
			AnimCampo.Play("Mov_Lava");
		    break;
		case 2:
			AnimCampo.Play("Congelado");
			break;
		case 3:
			AnimCampo.Play("Mov_Pantano");
			break;
		case 4:
			AnimCampo.Play("Seco");
			break;
		}

		PlayerPrefs.SetInt ("BatMedMapa", IndexMapa);
		ObjetoTexto.SetActive (true);
	}
	public void SigMapa(){
		ObjetoTexto.SetActive (false);
		if (IndexMapa < NombresMapas.Length-1) {
			IndexMapa++;
		} else {
			IndexMapa = 0;
		}

		//Establecer la apariencia del mapa:
		switch (IndexMapa) {
		case 0:
			AnimCampo.Play("Mov_Agua");
			break;
		case 1:
			AnimCampo.Play("Mov_Lava");
			break;
		case 2:
			AnimCampo.Play("Congelado");
			break;
		case 3:
			AnimCampo.Play("Mov_Pantano");
			break;
		case 4:
			AnimCampo.Play("Seco");
			break;
		}

		PlayerPrefs.SetInt ("BatMedMapa", IndexMapa);
		ObjetoTexto.SetActive (true);
	}
	public void EmpezarMapa(){
		SceneManager.LoadScene ("Campo_Batalla");
	}

	//Función para salir del juego:
	public void SalirJuego(){
		Application.Quit ();
	}

}
