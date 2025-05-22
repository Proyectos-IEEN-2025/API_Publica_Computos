using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace API_Computos_Publica.Utils
{
    public static class Commons
    {
        public static string Pendiente = "Pendiente";
        public static string Aceptado = "Aceptado";
        public static string Rechazado = "Rechazado";

        #region Bodegas
        public static string BodegaAbierta = "Bodega Abierta";
        public static string BodegaCerrada = "Bodega Cerrada";

        public static string Apertura = "Apertura";
        public static string Cierre = "Cierre";
        #endregion

        #region Mensajes

        public static string MensajeError = "Ha ocurrido un error inesperado. Por favor, inténtelo de nuevo.";
        public static string MensajeOk = "Proceso realizado con éxito.";
        #endregion

        public static string _Encriptar(string Valor)
        {
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(Valor);
            var result = Convert.ToBase64String(encryted);
            return result;
        }

        public static string _Desencriptar(string Valor)
        {
            byte[] decryted = Convert.FromBase64String(Valor);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            var result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }

        public static string MaskEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return email;

            var parts = email.Split('@');
            if (parts.Length != 2)
                return email;

            var name = parts[0];
            var domain = parts[1];

            if (name.Length <= 2)
                return email;

            var masked = name.Substring(0, 2) + new string('*', name.Length - 2);
            return $"{masked}@{domain}";
        }

        public static string _GetRandomPassword(int Length)
        {
            const string charsMay = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string CharnMin = "abcdefghijklmnopqrstuvwxyz";
            const string CharNum = "1234567890";
            const string CharExt = "@#$%";

            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            int indexM = rnd.Next(charsMay.Length);
            sb.Append(charsMay[indexM]);

            for (int i = 0; i < Length - 3; i++)
            {
                int index = rnd.Next(CharnMin.Length);
                sb.Append(CharnMin[index]);
            }

            int indexNum = rnd.Next(CharNum.Length);
            sb.Append(CharNum[indexNum]);

            int indexExt = rnd.Next(CharExt.Length);
            sb.Append(CharExt[indexExt]);

            return sb.ToString();
        }

        public static string CalificacionATexto(float calificacion)
        {
            // Definir arrays para los números y sus representaciones textuales
            string[] unidades = { "", "Uno", "Dos", "Tres", "Cuatro", "Cinco", "Seis", "Siete", "Ocho", "Nueve", "Diez" };

            // Separar la parte entera y decimal de la calificación
            var Calificacion_Texto = calificacion.ToString();
            var Array_Calificaciones = Calificacion_Texto.Split('.');
            var Entero = Convert.ToInt32(Array_Calificaciones[0]);
            int Decimal = 0;
            string Numero_Decimal_Texto = "";
            if (Array_Calificaciones.Length == 2)
            {
                var Array_Decimales = Array_Calificaciones[1];
                switch (Array_Decimales.Length)
                {
                    case 1:
                        Decimal = Convert.ToInt32(Array_Calificaciones[1]);
                        Numero_Decimal_Texto = NumeroALetras(Decimal);
                        break;
                    case 2:

                        if (Array_Decimales[0] == '0' && Array_Decimales[1] == '0')
                        {
                            Numero_Decimal_Texto = "";
                        }
                        else
                        {
                            if (Array_Decimales[0] == '0')
                            {

                                Numero_Decimal_Texto = $"cero {NumeroALetras(Convert.ToInt32(Array_Decimales[1].ToString()))}";
                            }
                            else
                            {
                                Numero_Decimal_Texto = NumeroALetras(Convert.ToInt32(Array_Calificaciones[1]));
                            }
                        }
                        break;
                    case 3:
                        if (Array_Decimales[0] == '0' && Array_Decimales[1] == '0' && Array_Decimales[2] == '0')
                        {
                            Numero_Decimal_Texto = "";
                        }
                        else
                        {
                            if (Array_Decimales[0] == '0')
                            {

                                Numero_Decimal_Texto = "cero";

                                if (Array_Decimales[1] == '0')
                                {
                                    Numero_Decimal_Texto += " cero";
                                    Numero_Decimal_Texto += $" {NumeroALetras(Convert.ToInt32(Array_Decimales[2].ToString()))}";

                                }
                                else
                                {
                                    Numero_Decimal_Texto += $" {NumeroALetras(Convert.ToInt32(Array_Calificaciones[1]))}";
                                }

                            }
                            else
                            {
                                Numero_Decimal_Texto += NumeroALetras(Convert.ToInt32(Array_Calificaciones[1]));
                            }
                        }
                        break;
                }
            }

            var Numero_Entero_Texto = unidades[Entero];
            if (Array_Calificaciones.Length == 2 && Numero_Decimal_Texto != "")
            {
                return $"{Numero_Entero_Texto} punto {Numero_Decimal_Texto}";
            }
            else
            {
                return Numero_Entero_Texto;
            }
        }

        public static string NumeroALetras(this int numberAsString)
        {

            var res = NumeroALetras(Convert.ToDouble(numberAsString));
            return res;
        }
        public static int CalcularEdad(DateTime fechaNacimiento)
        {
            DateTime fechaActual = DateTime.Today;
            int edad = fechaActual.Year - fechaNacimiento.Year;

            // Ajustar la edad si el cumpleaños aún no ha ocurrido en el año actual
            if (fechaNacimiento.Date > fechaActual.AddYears(-edad))
            {
                edad--;
            }

            return edad;
        }

        private static string NumeroALetras(double value)
        {
            string num2Text; value = Math.Truncate(value);
            if (value == 0) num2Text = "CERO";
            else if (value == 1) num2Text = "UNO";
            else if (value == 2) num2Text = "DOS";
            else if (value == 3) num2Text = "TRES";
            else if (value == 4) num2Text = "CUATRO";
            else if (value == 5) num2Text = "CINCO";
            else if (value == 6) num2Text = "SEIS";
            else if (value == 7) num2Text = "SIETE";
            else if (value == 8) num2Text = "OCHO";
            else if (value == 9) num2Text = "NUEVE";
            else if (value == 10) num2Text = "DIEZ";
            else if (value == 11) num2Text = "ONCE";
            else if (value == 12) num2Text = "DOCE";
            else if (value == 13) num2Text = "TRECE";
            else if (value == 14) num2Text = "CATORCE";
            else if (value == 15) num2Text = "QUINCE";
            else if (value < 20) num2Text = "DIECI" + NumeroALetras(value - 10);
            else if (value == 20) num2Text = "VEINTE";
            else if (value < 30) num2Text = "VEINTI" + NumeroALetras(value - 20);
            else if (value == 30) num2Text = "TREINTA";
            else if (value == 40) num2Text = "CUARENTA";
            else if (value == 50) num2Text = "CINCUENTA";
            else if (value == 60) num2Text = "SESENTA";
            else if (value == 70) num2Text = "SETENTA";
            else if (value == 80) num2Text = "OCHENTA";
            else if (value == 90) num2Text = "NOVENTA";
            else if (value < 100) num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " Y " + NumeroALetras(value % 10);
            else if (value == 100) num2Text = "CIEN";
            else if (value < 200) num2Text = "CIENTO " + NumeroALetras(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) num2Text = NumeroALetras(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) num2Text = "QUINIENTOS";
            else if (value == 700) num2Text = "SETECIENTOS";
            else if (value == 900) num2Text = "NOVECIENTOS";
            else if (value < 1000) num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);
            else if (value == 1000) num2Text = "MIL";
            else if (value < 2000) num2Text = "MIL " + NumeroALetras(value % 1000);
            else if (value < 1000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value % 1000);
                }
            }
            else if (value == 1000000)
            {
                num2Text = "UN MILLON";
            }
            else if (value < 2000000)
            {
                num2Text = "UN MILLON " + NumeroALetras(value % 1000000);
            }
            else if (value < 1000000000000)
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);
                }
            }
            else if (value == 1000000000000) num2Text = "UN BILLON";
            else if (value < 2000000000000) num2Text = "UN BILLON " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            else
            {
                num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0)
                {
                    num2Text = num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);
                }
            }
            return num2Text;
        }

        public static string _CerosIzquiera(string Numero, int ceros)
        {
            string Frmt = "";
            for (int i = 0; i < ceros; i++)
            {
                Frmt += "0";
            }
            return Frmt + Numero;
        }

        #region Métodos de Conversión Segura
        public static int ConvertirInt(object valor)
        {
            return valor != null && int.TryParse(valor.ToString(), out int result) ? result : 0;
        }

        public static double ConvertirDouble(object valor)
        {
            return valor != null && double.TryParse(valor.ToString(), out double result) ? result : 0.0;
        }

        public static string ConvertirString(object valor)
        {
            return valor?.ToString().Trim() ?? string.Empty;
        }

        public static DateTime ConvertirDateTime(object valor)
        {
            if (valor == null || valor == DBNull.Value)
                return DateTime.MinValue; // Retorna un valor por defecto si el objeto es nulo

            if (DateTime.TryParse(valor.ToString(), out DateTime resultado))
                return resultado;

            return DateTime.MinValue; // Si la conversión falla, devuelve una fecha mínima
        }


        public static bool ConvertirBool(object valor)
        {
            return valor != null && bool.TryParse(valor.ToString(), out bool result) && result;
        }

        #endregion
    }
}
