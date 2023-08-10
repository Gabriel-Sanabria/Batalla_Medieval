using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Admin_Casillas : MonoBehaviour {

	public Admin_Partida ScriptAdPart;

	public Collider2D[] Casillas;

	public bool[] CasillasOcupadasRojo;
	public bool[] CasillasOcupadasAzul;

	public bool[] CasillasOcupadasCata;
	public bool[] CasillasOcupadasAri;
	public bool[] CasillasOcupadasTor;

	public int TorresRojasClavadas;
	public int TorresAzulesClavadas;

	public bool SeleccionandoCasilla;
	public bool SeleccionandoTropa;

	public bool MoviendoTropa;
	public bool MoviendoMaquina;
	public bool UsandoArquero;

	public int CasillaSeleccionada;
	public int CasillaRoca;

	public GameObject[] CaballerosRojos;
	public GameObject[] CaballerosAzules;

	public int CabRojosAct;
	public int CabAzulesAct;

	public Caballeros ScriptTropaSelect;

	public bool Turno;
	public Text TextoTurno;

	void Start(){
		//Al iniciar la partida, elegir aleatoriamente quién moverá primero:
		int ValorInicial = Mathf.RoundToInt(Random.value);
		if (ValorInicial == 0) {
			Turno = false;
		} else {
			Turno = true;
		}

		//Contar el número de caballeros iniciales:
		CaballerosRojos = GameObject.FindGameObjectsWithTag("CabRojo");
		CaballerosAzules = GameObject.FindGameObjectsWithTag("CabAzul");
		CabRojosAct = CaballerosRojos.Length;
		CabAzulesAct = CaballerosAzules.Length;
	}

	void Update(){

		//Tener siempre registro de todos los caballeros (rojos y azules) que siguen vivos:
		CaballerosRojos = GameObject.FindGameObjectsWithTag("CabRojo");
		CaballerosAzules = GameObject.FindGameObjectsWithTag("CabAzul");
		CabRojosAct = CaballerosRojos.Length;
		CabAzulesAct = CaballerosAzules.Length;

		//Seleccionar solo un caballero a la vez:
		if (SeleccionandoTropa == true) {
			foreach (GameObject CabRojo in CaballerosRojos) {
				Caballeros ScriptCaballero = CabRojo.GetComponent<Caballeros> ();
				if(ScriptCaballero != ScriptTropaSelect)
				ScriptCaballero.Seleccionado = false;
			}
			foreach (GameObject CabAzul in CaballerosAzules) {
				Caballeros ScriptCaballero = CabAzul.GetComponent<Caballeros> ();
				if(ScriptCaballero != ScriptTropaSelect)
				ScriptCaballero.Seleccionado = false;
			}
		}

		//Mostrar en pantalla el turno actual:
		if (Turno == false) {
			TextoTurno.color = Color.red;
			TextoTurno.text = "Turno:\nEquipo Rojo";
		}
		if (Turno == true) {
			TextoTurno.color = Color.blue;
			TextoTurno.text = "Turno:\nEquipo Azul";
		}
			
		//Si aún no empieza la partida:
		if (ScriptAdPart.PartidaIniciada == false) {

			//Mantener desactivadas las cajas de colisión de todos los caballeros:
			foreach (GameObject CabRojo in CaballerosRojos) {
				CabRojo.GetComponent<Collider2D> ().enabled = false;
			}
			foreach (GameObject CabAzul in CaballerosAzules) {
				CabAzul.GetComponent<Collider2D> ().enabled = false;
			}
		}
			
		//Si ya se está en partida:
		if (ScriptAdPart.PartidaIniciada == true) {

			//Intercalar cajas de colisión de cada equipo entre turnos:
			if (Turno == false && MoviendoMaquina == false) {
				TextoTurno.color = Color.red;
				TextoTurno.text = "Turno:\nEquipo Rojo";

				for (int i = 0; i < Casillas.Length - 2; i++) {
					Casillas [i].enabled = !CasillasOcupadasRojo [i];
				}
				
			} 
			if (Turno == true && MoviendoMaquina == false) {
				TextoTurno.color = Color.blue;
				TextoTurno.text = "Turno:\nEquipo Azul";

				for (int i = 0; i < Casillas.Length - 2; i++) {
					Casillas [i].enabled = !CasillasOcupadasAzul [i];
				}

			}
		
			//Si se esta moviendo una maquina, desactivar todas la cajas de colision donde haya tropas 
			//(Para evitar que el operador de la maquina ataque cuerpo a cuerpo):
			if (Turno == false && MoviendoMaquina == true) {
				for (int i = 0; i < Casillas.Length - 2; i++) {
					Casillas [i].enabled = !CasillasOcupadasRojo [i];
				}
				for (int i = 0; i < Casillas.Length - 2; i++) {
					if (CasillasOcupadasAzul [i] == true)
						Casillas [i].enabled = !CasillasOcupadasAzul [i];
				}
				
				for (int i = 0; i < Casillas.Length - 2; i++) {
					if (CasillasOcupadasCata [i] == true)
						Casillas [i].enabled = false;
				}
				for (int i = 0; i < Casillas.Length - 2; i++) {
					if (CasillasOcupadasAri [i] == true)
						Casillas [i].enabled = false;
				}
				for (int i = 0; i < Casillas.Length - 2; i++) {
					if (CasillasOcupadasTor [i] == true)
						Casillas [i].enabled = false;
				}
				
			}
			if (Turno == true && MoviendoMaquina == true) {
				for (int i = 0; i < Casillas.Length - 2; i++) {
					Casillas [i].enabled = !CasillasOcupadasAzul [i];
				}
				for (int i = 0; i < Casillas.Length - 2; i++) {
					if (CasillasOcupadasRojo [i] == true)
						Casillas [i].enabled = !CasillasOcupadasRojo [i];
				}
				
				for (int i = 0; i < Casillas.Length - 2; i++) {
					if (CasillasOcupadasCata [i] == true)
						Casillas [i].enabled = false;
				}
				for (int i = 0; i < Casillas.Length - 2; i++) {
					if (CasillasOcupadasAri [i] == true)
						Casillas [i].enabled = false;
				}
				for (int i = 0; i < Casillas.Length - 2; i++) {
					if (CasillasOcupadasTor [i] == true)
						Casillas [i].enabled = false;
				}

			}
		}


	}

}