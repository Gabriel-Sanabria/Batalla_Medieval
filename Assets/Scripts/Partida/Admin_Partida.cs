using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Admin_Partida : MonoBehaviour {

	public bool PartidaIniciada;
	public bool PartidaFinalizada;

	public Admin_Casillas ScriptAdCas;
	public GameObject PanelComienzo;
	public GameObject PanelGanador;
	public Text TextoGanador;

	public Collider2D[] ColArqueros;

	private Vector2 PosMouse;

	public GameObject PanelNiebla;

	public GameObject BotonesSelecRojo;
	public GameObject BotonesSelecAzul;
	public Button[] BotonesRojo;
	public Button[] BotonesAzul;

	public GameObject PrefabCata;
	public GameObject PrefabAri;
	public GameObject PrefabTor;

	private GameObject MaquinaInst;

	private int CatasRojas, ArisRojos, TorsRojas;
	private int CatasAzules, ArisAzules, TorsAzules;

	private int[] CasillasRojasValidas = { 0, 8, 16, 26, 36, 46, 54, 1, 9, 17, 27, 37, 47, 55 };
	private int[] CasillasAzulesValidas = { 7, 15, 25, 35, 45, 53, 61, 6, 14, 24, 34, 44, 52, 60 };

	public bool Arrastrando;

	public AudioSource BocinaTrompetas;
	public AudioClip SonidoComienzo;
	public AudioClip SonidoFinal;

	public Animator AnimCampo;

	public Button BotonMenu;

	public bool Menu;

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

	void Update () {

		//Si no ha iniciado la partida, intercalar los botones de maquinas según el turno:
		if (PartidaIniciada == false) {
			if (ScriptAdCas.Turno == false) {
				BotonesSelecRojo.SetActive (true);
				BotonesSelecAzul.SetActive (false);

				PanelNiebla.SetActive (true);
				PanelNiebla.transform.position = new Vector2 (2.5f, PanelNiebla.transform.position.y);

				if (CatasRojas >= 2) {
					BotonesRojo [0].interactable = false;
				}
				if (ArisRojos >= 2) {
					BotonesRojo [1].interactable = false;
				}
				if (TorsRojas >= 2) {
					BotonesRojo [2].interactable = false;
				}

				if (CatasRojas >= 2 && ArisRojos >= 2 && TorsRojas >= 2 && CatasAzules < 2 && ArisAzules < 2 && TorsAzules < 2) {
					ScriptAdCas.Turno = !ScriptAdCas.Turno;
				}
				
				if (CatasRojas >= 2 && ArisRojos >= 2 && TorsRojas >= 2 && CatasAzules >= 2 && ArisAzules >= 2 && TorsAzules >= 2) {
					ScriptAdCas.Turno = true;
					PartidaIniciada = true;
					ActivarTropas ();
				}
			}
			else if (ScriptAdCas.Turno == true) {
				BotonesSelecRojo.SetActive (false);
				BotonesSelecAzul.SetActive (true);

				PanelNiebla.SetActive (true);
				PanelNiebla.transform.position = new Vector2 (-2.5f, PanelNiebla.transform.position.y);

				if (CatasAzules >= 2) {
					BotonesAzul [0].interactable = false;
				}
				if (ArisAzules >= 2) {
					BotonesAzul [1].interactable = false;
				}
				if (TorsAzules >= 2) {
					BotonesAzul [2].interactable = false;
				}

				if (CatasAzules >= 2 && ArisAzules >= 2 && TorsAzules >= 2 && CatasRojas < 2 && ArisRojos < 2 && TorsRojas < 2) {
					ScriptAdCas.Turno = !ScriptAdCas.Turno;
				}

				if (CatasAzules >= 2 && ArisAzules >= 2 && TorsAzules >= 2 && CatasRojas >= 2 && ArisRojos >= 2 && TorsRojas >= 2) {
					ScriptAdCas.Turno = false;
					PartidaIniciada = true;
					ActivarTropas ();
				}
			}
		} else {
			BotonesSelecRojo.SetActive (false);
			BotonesSelecAzul.SetActive (false);
			PanelNiebla.SetActive (false);
		}
			
		//Mover la maquina instanciada a la posición del mouse:
		if (PartidaIniciada == false) {
			PosMouse = Camera.main.ScreenToWorldPoint (new Vector2 (Input.mousePosition.x, Input.mousePosition.y));
			if (Arrastrando == true) {
				MaquinaInst.transform.position = PosMouse;
			}
		}

		//Desactivar a los arqueros cuando aún no empiece la partida:
		if (PartidaIniciada == false) {
			for (int i = 0; i < ColArqueros.Length; i++) {
				if(ColArqueros[i] != null)
					ColArqueros [i].enabled = false;
			}
		}

		//CONDICIONES DE VICTORIA:
		if (PartidaFinalizada == false) {
			
			//Condición de victoria equipo rojo:
			if (ScriptAdCas.CasillasOcupadasAri [63] == true || ScriptAdCas.TorresRojasClavadas >= 2 || ScriptAdCas.CabAzulesAct == 0
			   || ScriptAdCas.CabAzulesAct == 1 && ScriptAdCas.TorresAzulesClavadas == 1) {
				GanarRojo ();
				Invoke ("RegresarMenú", 5f);
				BocinaTrompetas.PlayOneShot (SonidoFinal);
				PartidaFinalizada = true;
			}

			//Condición de victoria equipo azul:
			if (ScriptAdCas.CasillasOcupadasAri [62] == true || ScriptAdCas.TorresAzulesClavadas >= 2 || ScriptAdCas.CabRojosAct == 0
			   || ScriptAdCas.CabRojosAct == 1 && ScriptAdCas.TorresRojasClavadas == 1) {
				GanarAzul ();
				Invoke ("RegresarMenú", 5f);
				BocinaTrompetas.PlayOneShot (SonidoFinal);
				PartidaFinalizada = true;
			}
		}
			
	}
		
	//Funciones para instanciar maquinas:
	public void BotónCatapultas(){
		Arrastrando = !Arrastrando;
		if (Arrastrando == true) {
			MaquinaInst = Instantiate (PrefabCata);

			SpriteRenderer RenderMaquina = MaquinaInst.GetComponent<SpriteRenderer>();
			if (ScriptAdCas.Turno == false)
				RenderMaquina.flipX = false;
			else
				RenderMaquina.flipX = true;

		} else {
			Destroy (MaquinaInst);
		}
	}
	public void BotónArietes(){
		Arrastrando = !Arrastrando;
		if (Arrastrando == true) {
			MaquinaInst = Instantiate (PrefabAri);

			SpriteRenderer RenderMaquina = MaquinaInst.GetComponent<SpriteRenderer>();
			if (ScriptAdCas.Turno == false)
				RenderMaquina.flipX = false;
			else
				RenderMaquina.flipX = true;

		} else {
			Destroy (MaquinaInst);
		}
	}
	public void BotónTorres(){
		Arrastrando = !Arrastrando;
		if (Arrastrando == true) {
			MaquinaInst = Instantiate (PrefabTor);

			SpriteRenderer RenderMaquina = MaquinaInst.GetComponent<SpriteRenderer>();
			if (ScriptAdCas.Turno == false)
				RenderMaquina.flipX = false;
			else
				RenderMaquina.flipX = true;

		} else {
			Destroy (MaquinaInst);
		}
	}

	//Función para insertar la máquina en la casilla seleccionada (al principio de la partida):
	public void InsertarMaquina(){

		//Si se está instanciando una maquina:
		if (MaquinaInst != null) {

			//Si la casilla no está ocupada por otra maquina:
			if (ScriptAdCas.CasillasOcupadasCata [ScriptAdCas.CasillaSeleccionada] == false && 
			    ScriptAdCas.CasillasOcupadasAri [ScriptAdCas.CasillaSeleccionada] == false && 
				ScriptAdCas.CasillasOcupadasTor [ScriptAdCas.CasillaSeleccionada] == false) {

				//Insertar catapultas:
				if (Arrastrando == true && MaquinaInst.GetComponent<Catapulta> () != null && ComprobarCasilla(ScriptAdCas.CasillaSeleccionada) == true) {
					MaquinaInst.transform.position = ScriptAdCas.Casillas [ScriptAdCas.CasillaSeleccionada].transform.position;
					MaquinaInst.GetComponent<Catapulta> ().enabled = true;
					MaquinaInst.GetComponent<Catapulta> ().CasillaActual = ScriptAdCas.CasillaSeleccionada;
					ScriptAdCas.CasillasOcupadasCata [ScriptAdCas.CasillaSeleccionada] = true;

					if (ScriptAdCas.Turno == false && CatasRojas < 2) {
						CatasRojas++;
					}
					if (ScriptAdCas.Turno == true && CatasAzules < 2) {
						CatasAzules++;
					}

					Arrastrando = false;
				}

				//Insertar Arietes:
				if (Arrastrando == true && MaquinaInst.GetComponent<Ariete> () != null && ComprobarCasilla(ScriptAdCas.CasillaSeleccionada) == true) {
					MaquinaInst.transform.position = ScriptAdCas.Casillas [ScriptAdCas.CasillaSeleccionada].transform.position;
					MaquinaInst.GetComponent<Ariete> ().enabled = true;
					MaquinaInst.GetComponent<Ariete> ().CasillaActual = ScriptAdCas.CasillaSeleccionada;
					ScriptAdCas.CasillasOcupadasAri [ScriptAdCas.CasillaSeleccionada] = true;

					if (ScriptAdCas.Turno == false && ArisRojos < 2) {
						ArisRojos++;
					}
					if (ScriptAdCas.Turno == true && ArisAzules < 2) {
						ArisAzules++;
					}

					Arrastrando = false;
				}

				//Insertar Torres:
				if (Arrastrando == true && MaquinaInst.GetComponent<Torre> () != null && ComprobarCasilla(ScriptAdCas.CasillaSeleccionada) == true) {
					MaquinaInst.transform.position = ScriptAdCas.Casillas [ScriptAdCas.CasillaSeleccionada].transform.position;
					MaquinaInst.GetComponent<Torre> ().enabled = true;
					MaquinaInst.GetComponent<Torre> ().CasillaActual = ScriptAdCas.CasillaSeleccionada;
					ScriptAdCas.CasillasOcupadasTor [ScriptAdCas.CasillaSeleccionada] = true;

					if (ScriptAdCas.Turno == false && TorsRojas < 2) {
						TorsRojas++;
					}
					if (ScriptAdCas.Turno == true && TorsAzules < 2) {
						TorsAzules++;
					}

					Arrastrando = false;
				}
			}

		}
	}

	//Función para comprobar si la casilla es valida para desplegar una maquina:
	bool ComprobarCasilla(int CasillaElegida){

		bool CasillaCorrecta = false;

		if (ScriptAdCas.Turno == false) {
			for (int i = 0; i < CasillasRojasValidas.Length; i++) {
				if (CasillaElegida == CasillasRojasValidas [i]) {
					CasillaCorrecta = true;
				}
			}
			if (CasillaCorrecta == true) {
				return true;
			} else {
				return false;	
			}
		} else {
			for (int i = 0; i < CasillasAzulesValidas.Length; i++) {
				if (CasillaElegida == CasillasAzulesValidas [i]) {
					CasillaCorrecta = true;
				}
			}
			if (CasillaCorrecta == true) {
				return true;
			} else {
				return false;	
			}	
		}
	}

	//Función para activar las tropas:
	void ActivarTropas(){

		//Arqueros:
		for (int i = 0; i < ColArqueros.Length; i++) {
			if(ColArqueros[i] != null)
			ColArqueros [i].enabled = true;
		}

		//Caballeros (Ambos equipos):
		for (int i = 0; i < ScriptAdCas.CaballerosRojos.Length; i++) {
			if (ScriptAdCas.CaballerosRojos [i] != null) {
				Collider2D ColCaballero = ScriptAdCas.CaballerosRojos [i].GetComponent<Collider2D> ();
				ColCaballero.enabled = true;
			}
		}
		for (int i = 0; i < ScriptAdCas.CaballerosAzules.Length; i++) {
			if (ScriptAdCas.CaballerosAzules [i] != null) {
				Collider2D ColCaballero = ScriptAdCas.CaballerosAzules [i].GetComponent<Collider2D> ();
				ColCaballero.enabled = true;
			}
		}

		//Mostrar letrero de comienzo de partida:
		ActivarLetreroComienzo();
		Invoke ("DesactivarLetreroComienzo", 2f);

	}

	//Funciones para manipular el letrero de comienzo:
	void ActivarLetreroComienzo(){
		PanelComienzo.SetActive (true);
		BocinaTrompetas.PlayOneShot (SonidoComienzo);
	}
	void DesactivarLetreroComienzo(){
		PanelComienzo.SetActive (false);
	}
		
	//FUNCIONES DE VICTORIAS:
	void GanarRojo(){
		//Desactivar arqueros:
		for (int i = 0; i < ColArqueros.Length; i++) {
			if(ColArqueros[i] != null)
				ColArqueros [i].enabled = false;
		}

		//Desactivar Caballeros de ambos equipos:
		for (int i = 0; i < ScriptAdCas.CaballerosRojos.Length; i++) {
			if (ScriptAdCas.CaballerosRojos [i] != null) {
				Collider2D ColCaballero = ScriptAdCas.CaballerosRojos [i].GetComponent<Collider2D> ();
				ColCaballero.enabled = false;
			}
		}
		for (int i = 0; i < ScriptAdCas.CaballerosAzules.Length; i++) {
			if (ScriptAdCas.CaballerosAzules [i] != null) {
				Collider2D ColCaballero = ScriptAdCas.CaballerosAzules [i].GetComponent<Collider2D> ();
				ColCaballero.enabled = false;
			}
		}

		ScriptAdCas.TextoTurno.enabled = false;
		ScriptAdCas.enabled = false;
		BotonMenu.interactable = false;

		PanelGanador.SetActive (true);
		TextoGanador.color = Color.red;
		TextoGanador.text = "¡Victoria del equipo rojo!";
	}
	void GanarAzul(){
		//Desactivar arqueros:
		for (int i = 0; i < ColArqueros.Length; i++) {
			if(ColArqueros[i] != null)
				ColArqueros [i].enabled = false;
		}

		//Desactivar Caballeros de ambos equipos:
		for (int i = 0; i < ScriptAdCas.CaballerosRojos.Length; i++) {
			if (ScriptAdCas.CaballerosRojos [i] != null) {
				Collider2D ColCaballero = ScriptAdCas.CaballerosRojos [i].GetComponent<Collider2D> ();
				ColCaballero.enabled = false;
			}
		}
		for (int i = 0; i < ScriptAdCas.CaballerosAzules.Length; i++) {
			if (ScriptAdCas.CaballerosAzules [i] != null) {
				Collider2D ColCaballero = ScriptAdCas.CaballerosAzules [i].GetComponent<Collider2D> ();
				ColCaballero.enabled = false;
			}
		}

		ScriptAdCas.TextoTurno.enabled = false;
		ScriptAdCas.enabled = false;
		BotonMenu.interactable = false;

		PanelGanador.SetActive (true);
		TextoGanador.color = Color.blue;
		TextoGanador.text = "¡Victoria del equipo azul!";
	}

	//Función para regresar al menú:
	public void RegresarMenú(){
		Menu = true;

		SceneManager.LoadScene ("Menu");
		Musica ScriptMusica = GameObject.Find ("Musica").GetComponent<Musica> ();

		if (PartidaIniciada == true) {
			ScriptMusica.BocinaMusica.Stop ();
			ScriptMusica.BocinaMusica.PlayOneShot (ScriptMusica.Canciones [0]);
		}
	}

}