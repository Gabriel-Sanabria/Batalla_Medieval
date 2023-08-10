using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapulta : MonoBehaviour {

	public Admin_Casillas ScriptAdCas;
	public bool Usando;
	private GameObject Operador;
	public Caballeros ScriptOperador;

	public int CasillaActual;

	public SpriteRenderer RenderCata;

	public AudioSource BocinaCata;
	public AudioClip SonidoRoca;

	public GameObject Botones;
	private bool DejandoMaquina;

	public Animator AnimCata;

	public GameObject SpawnDerRoca;
	public GameObject SpawnIzqRoca;
	private GameObject SpawnAUsar;

	private bool CataSeleccionada;

	private bool ComprobarInicio = true;

	void Start(){
		//Al inicio, encontrar al admin e las casillas:
		ScriptAdCas = GameObject.Find ("Casillas").GetComponent<Admin_Casillas> ();
	}

	void Update () {

		//Detectar al inicio quien es el operador de la maquina:
		if (ComprobarInicio == true) {
			for (int i = 0; i < 14; i++) {
				if (ScriptAdCas.CaballerosRojos [i].GetComponent<Caballeros> ().CasillaActual == CasillaActual) {
					Operador = ScriptAdCas.CaballerosRojos [i];
					ScriptOperador = Operador.GetComponent<Caballeros> ();
					ScriptOperador.UsandoCata = true;
					ScriptOperador.ScriptCata = this;
				}
			}
			for (int i = 0; i < 14; i++) {
				if (ScriptAdCas.CaballerosAzules [i].GetComponent<Caballeros> ().CasillaActual == CasillaActual) {
					Operador = ScriptAdCas.CaballerosAzules [i];
					ScriptOperador = Operador.GetComponent<Caballeros> ();
					ScriptOperador.UsandoCata = true;
					ScriptOperador.ScriptCata = this;
				}
			}
			ComprobarInicio = false;
		}

		//Si se detecta que un caballero esta en la misma casilla que ésta catapulta, asignar al caballero como el operador de esta maquina:
		if (ScriptAdCas.ScriptTropaSelect != null && ScriptAdCas.CasillasOcupadasCata [ScriptAdCas.ScriptTropaSelect.CasillaActual] == true
			&& ScriptAdCas.MoviendoTropa == false && DejandoMaquina == false && ScriptAdCas.ScriptTropaSelect.CasillaActual == CasillaActual) {
			Operador = ScriptAdCas.ScriptTropaSelect.gameObject;
			ScriptOperador = ScriptAdCas.ScriptTropaSelect;
			ScriptOperador.UsandoCata = true;
			ScriptOperador.ScriptCata = this;
		}

		//Si hay un operador en la maquina, asignar que la maquina esta en uso:
		if (Operador == null) {
			Usando = false;
		} else if(Operador != null){
			Usando = true;
		}
			
		//Mientras se este usando esta maquina, moverse a donde se mueva el operador:
		if(Usando == true)
			transform.position = Operador.transform.position;

		//Definir a donde se orientará la maquina en base al equipo del operador:
		if (Usando == true && ScriptOperador.Equipo == "Rojo") {
			RenderCata.flipX = false;
			SpawnAUsar = SpawnDerRoca;
		}
		if (Usando == true && ScriptOperador.Equipo == "Azul") {
			RenderCata.flipX = true;
			SpawnAUsar = SpawnIzqRoca;
		}
			
		//Activar los botones de acciones al seleccionar al caballero:
		if (Usando == true && ScriptOperador.Equipo == "Rojo" && ScriptAdCas.Turno == false && ScriptAdCas.MoviendoTropa == false && ScriptOperador.Seleccionado == true 
		|| Usando == true && ScriptOperador.Equipo == "Azul" && ScriptAdCas.Turno == true && ScriptAdCas.MoviendoTropa == false && ScriptOperador.Seleccionado == true) {
			Botones.SetActive (true);
			ScriptAdCas.MoviendoMaquina = true;
			CataSeleccionada = true;
		}
		//Desactivar los botones de acciones:
		if (Usando == true && CataSeleccionada == true) {
			if (ScriptOperador.Equipo == "Rojo" && ScriptAdCas.Turno == true || ScriptOperador.Equipo == "Azul" && ScriptAdCas.Turno == false || ScriptOperador.Seleccionado == false) {
				Botones.SetActive (false);
				ScriptAdCas.MoviendoMaquina = false;
				CataSeleccionada = false;
			}
		}

		//Reiniciar la variable "DejandoMaquina" (Para que pueda volver a ser utilizada después sin problemas):
		if (ScriptOperador != null) {
			if (ScriptOperador.Equipo == "Rojo" && ScriptAdCas.Turno == true || ScriptOperador.Equipo == "Azul" && ScriptAdCas.Turno == false) {
				DejandoMaquina = false;
			}
		}


		//Si no hay operador pero se es golpeada esta casilla por otra roca, destruir igualmente:
		if(Usando == false && CasillaActual == ScriptAdCas.CasillaRoca){
			Autodestruir ();
		}
		
	}


	//Funciones para realizar acciones con la maquina:
	public void DejarMaquina(){
		ScriptAdCas.MoviendoMaquina = false;
		Botones.SetActive (false);
		Operador = null;
		DejandoMaquina = true;
		ScriptOperador.UsandoCata = false;
		ScriptOperador.ScriptCata = null;
		Usando = false;
	}
	public void Disparar(){
		AnimCata.Play ("Catapulta_Tiro");

		ScriptAdCas.SeleccionandoTropa = false;
		ScriptAdCas.MoviendoTropa = true;
		//ScriptAdCas.MoviendoMaquina = false;
		ScriptOperador.Moviendose = false;
		ScriptOperador.Seleccionado = false;

		Invoke ("AparecerRoca", 0.35f);
	}

	//Funciones para recrear la animación y efectos del lanzamiento de roca:
	void AparecerRoca(){
		SpawnAUsar.SetActive (true);
		Invoke ("SonidoGolpe", 0.35f);
		Invoke ("DesaparecerRoca", 0.7f);
	}
	void DesaparecerRoca(){
		ScriptAdCas.MoviendoTropa = false;
		ScriptAdCas.Turno = !ScriptAdCas.Turno;
		SpawnAUsar.SetActive (false);
	}
	void SonidoGolpe(){
		BocinaCata.PlayOneShot (SonidoRoca);
	}

	//Función para destruir esta maquina:
	public void Autodestruir(){
		ScriptAdCas.CasillasOcupadasCata [CasillaActual] = false;
		Destroy (gameObject);
	}
		
}