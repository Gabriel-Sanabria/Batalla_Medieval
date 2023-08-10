using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ariete : MonoBehaviour {

	public Admin_Casillas ScriptAdCas;
	public bool Usando;
	private GameObject Operador;
	public Caballeros ScriptOperador;

	public int CasillaActual;

	public SpriteRenderer RenderAri;

	public GameObject Botones;

	private bool DejandoMaquina;

	private bool AriSeleccionado;
	private bool ActivandoPuentes;

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
					ScriptOperador.UsandoAri = true;
					ScriptOperador.ScriptAri = this;
				}
			}
			for (int i = 0; i < 14; i++) {
				if (ScriptAdCas.CaballerosAzules [i].GetComponent<Caballeros> ().CasillaActual == CasillaActual) {
					Operador = ScriptAdCas.CaballerosAzules [i];
					ScriptOperador = Operador.GetComponent<Caballeros> ();
					ScriptOperador.UsandoAri = true;
					ScriptOperador.ScriptAri = this;
				}
			}
			ComprobarInicio = false;
		}

		//Si se detecta que un caballero esta en la misma casilla que éste ariete, asignar al caballero como el operador de esta maquina:
		if (ScriptAdCas.ScriptTropaSelect != null && ScriptAdCas.CasillasOcupadasAri [ScriptAdCas.ScriptTropaSelect.CasillaActual] == true
			&& ScriptAdCas.MoviendoTropa == false && DejandoMaquina == false  && ScriptAdCas.ScriptTropaSelect.CasillaActual == CasillaActual) {
			Operador = ScriptAdCas.ScriptTropaSelect.gameObject;
			ScriptOperador = ScriptAdCas.ScriptTropaSelect;
			ScriptOperador.UsandoAri = true;
			ScriptOperador.ScriptAri = this;
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
			RenderAri.flipX = false;
		}
		if (Usando == true && ScriptOperador.Equipo == "Azul") {
			RenderAri.flipX = true;
		}

		//Activar el botón de acción al seleccionar al caballero:
		if (Usando == true && ScriptOperador.Equipo == "Rojo" && ScriptAdCas.Turno == false && ScriptAdCas.MoviendoTropa == false && ScriptOperador.Seleccionado == true
			|| Usando == true && ScriptOperador.Equipo == "Azul" && ScriptAdCas.Turno == true && ScriptAdCas.MoviendoTropa == false && ScriptOperador.Seleccionado == true) {
			Botones.SetActive (true);
			ScriptAdCas.MoviendoMaquina = true;
			AriSeleccionado = true;
		}
		//Desactivar el botón de acción:
		if (Usando == true && AriSeleccionado == true) {
			if (ScriptOperador.Equipo == "Rojo" && ScriptAdCas.Turno == true || ScriptOperador.Equipo == "Azul" && ScriptAdCas.Turno == false || ScriptOperador.Seleccionado == false) {
				Botones.SetActive (false);
				ScriptAdCas.MoviendoMaquina = false;
				AriSeleccionado = false;
			}
		}

		//Reiniciar la variable "DejandoMaquina" (Para que pueda volver a ser utilizada después sin problemas):
		if (ScriptOperador != null) {
			if (ScriptOperador.Equipo == "Rojo" && ScriptAdCas.Turno == true || ScriptOperador.Equipo == "Azul" && ScriptAdCas.Turno == false) {
				DejandoMaquina = false;
			}
		}
			
		//Activar los puentes en base a si el ariete está seleccionado y el equipo del operador:
		if (AriSeleccionado == true && ActivandoPuentes == false) {

			if (Usando == true && ScriptAdCas.Turno == false && ScriptOperador.Equipo == "Rojo") {
				ScriptAdCas.Casillas [63].enabled = true;
			}
			if (Usando == true && ScriptAdCas.Turno == true && ScriptOperador.Equipo == "Azul") {
				ScriptAdCas.Casillas [62].enabled = true;
			}
			ActivandoPuentes = true;
		}

		//Desactivar los puentes:
		if(AriSeleccionado == false && ActivandoPuentes == true || Usando == false && ActivandoPuentes == true){

			if(ScriptOperador.Equipo == "Rojo" && ScriptAdCas.ScriptTropaSelect.CasillaActual == CasillaActual
			|| ScriptAdCas.ScriptTropaSelect != ScriptOperador && ScriptAdCas.ScriptTropaSelect.UsandoAri == false){
				ScriptAdCas.Casillas [63].enabled = false;
			}
			if(ScriptOperador.Equipo == "Azul" && ScriptAdCas.ScriptTropaSelect.CasillaActual == CasillaActual
			|| ScriptAdCas.ScriptTropaSelect != ScriptOperador && ScriptAdCas.ScriptTropaSelect.UsandoAri == false){
				ScriptAdCas.Casillas [62].enabled = false;
			}
			ActivandoPuentes = false;
		}
			
	}
		
	//Funciones para realizar acciones con la maquina:
	public void DejarMaquina(){
		ScriptAdCas.MoviendoMaquina = false;
		Botones.SetActive (false);
		Operador = null;
		DejandoMaquina = true;
		ScriptOperador.UsandoAri = false;
		ScriptOperador.ScriptAri = null;
		Usando = false;
	}
}