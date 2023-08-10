using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arqueros : MonoBehaviour {

	public string Equipo;
	public int NumArquero;

	public Admin_Casillas ScriptAdCas;
	public Admin_Arqueros ScriptAdArq;

	public GameObject SeñalSeleccionado;

	void Update(){
		//Desactivar al arquero automaticamente si se selecciona otra tropa:
		if (ScriptAdCas.SeleccionandoTropa == true) {
			ScriptAdArq.ArquerosActivos [NumArquero] = false;
		}	

		//Activar señal de seleccionado:
		if (ScriptAdCas.SeleccionandoTropa == false && ScriptAdArq.Disparando == false && ScriptAdArq.ArquerosActivos [NumArquero] == true) {
			SeñalSeleccionado.SetActive (true);
		}
		//Desactivar señal de seleccionado al seleccionar otra tropa distinta de este arquero:
		if (ScriptAdCas.SeleccionandoTropa == true || ScriptAdArq.Disparando == true
		|| NumArquero == 0 && ScriptAdArq.ArquerosActivos[1] == true || NumArquero == 1 && ScriptAdArq.ArquerosActivos[0] == true
		|| NumArquero == 0 && ScriptAdArq.ArquerosActivos[0] == false || NumArquero == 1 && ScriptAdArq.ArquerosActivos[1] == false) {
			SeñalSeleccionado.SetActive (false);
		}
		if (ScriptAdArq.ArqueroDisparando == gameObject) {
			Destroy (SeñalSeleccionado);
		}
	}
		
	//Seleccionar o deseleccionar este arquero al dar click sobre él según sea el turno:
	void OnMouseDown(){
		if (Equipo == "Rojo" && ScriptAdCas.Turno == false && ScriptAdCas.MoviendoTropa == false && ScriptAdCas.UsandoArquero == false
		|| Equipo == "Azul" && ScriptAdCas.Turno == true && ScriptAdCas.MoviendoTropa == false && ScriptAdCas.UsandoArquero == false) {
			ScriptAdArq.ArquerosActivos [NumArquero] = !ScriptAdArq.ArquerosActivos [NumArquero];
			if (ScriptAdArq.ArquerosActivos [NumArquero] == true) {
				ScriptAdArq.ScriptArqueroSelect = this;
			}
			if (ScriptAdArq.ArquerosActivos [NumArquero] == false) {
				ScriptAdArq.ScriptArqueroSelect = null;
			}
		}
	}
}