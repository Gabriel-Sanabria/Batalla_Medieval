using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torre : MonoBehaviour {

	public Admin_Casillas ScriptAdCas;
	public bool Usando;
	private GameObject Operador;
	public Caballeros ScriptOperador;

	public int CasillaActual;

	public SpriteRenderer RenderTor;
	public Animator AnimTorre;

	public GameObject Botones;

	private bool DejandoMaquina;

	private bool TorSeleccionada;
	private bool TorreClavada;

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
					ScriptOperador.UsandoTor = true;
					ScriptOperador.ScriptTor = this;
				}
			}
			for (int i = 0; i < 14; i++) {
				if (ScriptAdCas.CaballerosAzules [i].GetComponent<Caballeros> ().CasillaActual == CasillaActual) {
					Operador = ScriptAdCas.CaballerosAzules [i];
					ScriptOperador = Operador.GetComponent<Caballeros> ();
					ScriptOperador.UsandoTor = true;
					ScriptOperador.ScriptTor = this;
				}
			}
			ComprobarInicio = false;
		}

		//Si se detecta que un caballero esta en la misma casilla que ésta torre, asignar al caballero como el operador de esta maquina:
		if (ScriptAdCas.ScriptTropaSelect != null && ScriptAdCas.CasillasOcupadasTor [ScriptAdCas.ScriptTropaSelect.CasillaActual] == true
			&& ScriptAdCas.MoviendoTropa == false && DejandoMaquina == false  && ScriptAdCas.ScriptTropaSelect.CasillaActual == CasillaActual) {
			Operador = ScriptAdCas.ScriptTropaSelect.gameObject;
			ScriptOperador = ScriptAdCas.ScriptTropaSelect;
			ScriptOperador.UsandoTor = true;
			ScriptOperador.ScriptTor = this;
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
			RenderTor.flipX = false;
		}
		if (Usando == true && ScriptOperador.Equipo == "Azul") {
			RenderTor.flipX = true;
		}

		//Activar el botón de acción al seleccionar al caballero:
		if (Usando == true && ScriptOperador.Equipo == "Rojo" && ScriptAdCas.Turno == false && ScriptAdCas.MoviendoTropa == false && ScriptOperador.Seleccionado == true 
			|| Usando == true && ScriptOperador.Equipo == "Azul" && ScriptAdCas.Turno == true && ScriptAdCas.MoviendoTropa == false && ScriptOperador.Seleccionado == true) {
			Botones.SetActive (true);
			ScriptAdCas.MoviendoMaquina = true;
			TorSeleccionada = true;
		}
		//Desactivar el botón de acción:
		if (Usando == true && TorSeleccionada == true) {
			if (ScriptOperador.Equipo == "Rojo" && ScriptAdCas.Turno == true || ScriptOperador.Equipo == "Azul" && ScriptAdCas.Turno == false || ScriptOperador.Seleccionado == false) {
				Botones.SetActive (false);
				ScriptAdCas.MoviendoMaquina = false;
				TorSeleccionada = false;
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

		//Si se llega a la orilla del rio siendo el equipo rojo:
		if (ScriptOperador != null && ScriptOperador.Equipo == "Rojo" && TorreClavada == false) {
			switch (CasillaActual) {
			case 7:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;	
			break;
			case 15:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;
			break;
			case 25:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;
			break;
			case 45:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;
			break;
			case 53:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;
			break;
			case 61:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;
			break;
			}
		}

		//Si se llega a la orilla del rio siendo el equipo azul:
		if (ScriptOperador != null && ScriptOperador.Equipo == "Azul" && TorreClavada == false) {
			switch (CasillaActual) {
			case 0:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;	
			break;
			case 8:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;
			break;
			case 16:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;
			break;
			case 36:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;
			break;
			case 46:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;
			break;
			case 54:
				AnimTorre.Play ("Torre_Puente");
				Invoke ("DesactivarTorre", 2f);
				TorreClavada = true;
			break;
			}
		}
	
	}

	//Funciones para realizar acciones con la maquina:
	public void DejarMaquina(){
		ScriptAdCas.MoviendoMaquina = false;
		Botones.SetActive (false);
		Operador = null;
		DejandoMaquina = true;
		ScriptOperador.UsandoTor = false;
		ScriptOperador.ScriptTor = null;
		Usando = false;
	}
	public void Autodestruir(){
		ScriptAdCas.CasillasOcupadasTor [CasillaActual] = false;
		Destroy (gameObject);
	}

	void DesactivarTorre(){
		if (ScriptOperador.Equipo == "Rojo") {
			ScriptAdCas.TorresRojasClavadas++;
			ScriptAdCas.CasillasOcupadasAzul [CasillaActual] = true;
		}
		if (ScriptOperador.Equipo == "Azul") {
			ScriptAdCas.TorresAzulesClavadas++;
			ScriptAdCas.CasillasOcupadasRojo [CasillaActual] = true;
		}
		//Operador.tag = "Untagged";
		ScriptOperador.Col.enabled = false;
		ScriptOperador.enabled = false;
		this.enabled = false;
	}

}