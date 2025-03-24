using System;
using System.Windows;
using System.Windows.Controls;

namespace Calculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private double valorActual = 0;
        private string operacionPendiente = "";
        private bool iniciarNuevoNumero = true;

        private void Button_Num_Click(object sender, RoutedEventArgs e)
        {
            Button boton = (Button)sender;
            string digito = boton.Content.ToString();

            if (iniciarNuevoNumero)
            {
                if (digito == ",")
                    txtPanel.Text = "0,"; // Si es una coma, agregam "0," en lugar de borrar todo
                else
                    txtPanel.Text = digito;

                iniciarNuevoNumero = false;
            }
            else
            {
                // Evitar múltiples ceros a la izquierda
                if (txtPanel.Text == "0" && digito != ",")
                    txtPanel.Text = digito;
                else if (digito == "," && !txtPanel.Text.Contains(",")) // Permitir solo un decimal
                    txtPanel.Text += digito;
                else if (digito != ",") // Agregar solo si no es una coma ya repetida
                    txtPanel.Text += digito;
            }
        }

        private void BotonRetroceso_Click(object sender, RoutedEventArgs e)
        {
            string textoActual = txtPanel.Text;// Obtener el texto actual de la pantalla

            if (textoActual.Length > 0)// Verificar que haya algo para borrar
            {
                textoActual = textoActual.Substring(0, textoActual.Length - 1); // Remover el último carácter

                if (string.IsNullOrEmpty(textoActual) || textoActual == "-")
                {
                    txtPanel.Text = "0"; // Si se elimina todo, mostrar 0
                    iniciarNuevoNumero = true;
                }
                else txtPanel.Text = textoActual; // Actualizar la pantalla con el nuevo texto
            }
            else
            {
                txtPanel.Text = "0";  // Si no hay nada en la pantalla, mostrar 0
                iniciarNuevoNumero = true;
            }
        }

        private void BotonC_Click(object sender, RoutedEventArgs e)
        {
            // Resetear todo
            txtPanel.Text = "0";
            txtOperacion.Text = "";
            valorActual = 0;
            operacionPendiente = "";
            iniciarNuevoNumero = true;
        }

        private void BotonCE_Click(object sender, RoutedEventArgs e)
        {
            txtPanel.Text = "0"; // Borrar solo la entrada actual
            iniciarNuevoNumero = true;
        }

        private void BotonSigno_Click(object sender, RoutedEventArgs e)
        {
            if (txtPanel.Text != "0") 
            {
                if (txtPanel.Text.StartsWith("-"))
                    txtPanel.Text = txtPanel.Text.Substring(1);
                else
                    txtPanel.Text = "-" + txtPanel.Text;
            }
        }

        private void BotonOperacion_Click(object sender, RoutedEventArgs e)
        {
            // Si ya hay una operación pendiente, ejecutarla primero
            if (!string.IsNullOrEmpty(operacionPendiente) && !iniciarNuevoNumero)
            {
                Button_Igual_Click(null, null);
            }

            valorActual = Convert.ToDouble(txtPanel.Text); // Guarda el valor actual del panel dentro de la variable, convertido a double

            Button boton = (Button)sender;
            operacionPendiente = boton.Content.ToString(); // Guarda en la var el contenido del btn (+, -, * o /) para luego convertirlo en una operación

            txtOperacion.Text = valorActual.ToString() + " " + operacionPendiente; // Muestra la operación en el panel de operaciones

            iniciarNuevoNumero = true; // Hacer esto para que el programa sepa que se va a ingresar un nuevo numero
        }

        private void Button_Igual_Click(object sender, RoutedEventArgs e)
        {
            double segundoValor = Convert.ToDouble(txtPanel.Text); // Guarda el segundo valor en una variable
            double resultado = 0; // Setea el resultado en 0

            switch (operacionPendiente)
            {
                case "+":
                    resultado = valorActual + segundoValor;
                    break;
                case "-":
                    resultado = valorActual - segundoValor;
                    break;
                case "✕":
                    resultado = valorActual * segundoValor;
                    break;
                case "÷":
                    if (segundoValor != 0) resultado = valorActual / segundoValor;
                    else
                    { MessageBox.Show("Soy calculadora, no hago magia ✨"); return; }
                    break;
                case "%":
                    if (segundoValor != 0)
                        resultado = valorActual % segundoValor;
                    else
                    {
                        MessageBox.Show("No se puede calcular el módulo con 0");
                        return;
                    }
                    break;

            }

            // Actualizar el TextBlock para mostrar la operación completa
            txtOperacion.Text = valorActual.ToString() + " " + operacionPendiente + " " + segundoValor.ToString() + " =";

            txtPanel.Text = Math.Round(resultado, 2).ToString(); // Muestra el resultado en el panel, redondea resultado a 2 decimales
            iniciarNuevoNumero = true;
        }

    }
}
