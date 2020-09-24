﻿using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using SYSTEMCODE.Capa_de_Negocio;

namespace SYSTEMCODE.Capa_de_Vista
{
    public partial class frmLogin : Form
    {
        private bool cerrado = false;
        private bool btnIngresarPresionado = false;
        private static Usuario usuarioActual;
        private int temporizador = 3;

        public frmLogin()
        {
            InitializeComponent();
        }

        public bool Cerrado { get => cerrado; set => cerrado = value; }
        public static Usuario UsuarioActual { get => usuarioActual; set => usuarioActual = value; }

        private void labelEstadoLogin(string mensaje, bool estado)
        {
            if (!estado)
            {
                lblEstadoLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
                lblEstadoLogin.ForeColor = System.Drawing.Color.White;
                lblEstadoLogin.Text = mensaje;
            }
            else
            {
                lblEstadoLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(200)))), ((int)(((byte)(83)))));
                lblEstadoLogin.ForeColor = System.Drawing.Color.White;
                lblEstadoLogin.Text = mensaje;
            }
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            btnIngresarPresionado = true;

            if (txtUsuario.Text.Length == 0)
            {
                labelEstadoLogin("DATO OBLIGATORIO: USUARIO", false);
                txtUsuario.Focus();

                return;
            }

            if (txtClave.Text.Length == 0)
            {
                labelEstadoLogin("DATO OBLIGATORIO: CLAVE", false);
                txtClave.Focus();

                return;
            }

            try
            {
                UsuarioActual = Usuario.ValidarUsuario(txtUsuario.Text, txtClave.Text);
                if (UsuarioActual != null)
                {
                    btnIngresar.Enabled = false;
                    temporizadorAcceso.Enabled = true;
                    txtUsuario.Enabled = false;
                    txtClave.Enabled = false;
                }
                else
                {
                    labelEstadoLogin("ACCESO DENEGADO - DATOS INCORRECTOS", false);

                    txtClave.Text = "";
                    txtUsuario.Text = "";
                    txtUsuario.Focus();
                }
            }
            catch (SqlException)
            {
                labelEstadoLogin("ERROR DE CONEXIÓN - BASE DE DATOS", false);

                txtUsuario.Enabled = false;
                txtClave.Enabled = false;
                btnIngresar.Enabled = false;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            cerrado = true;
            Close();
        }

        private void temporizadorAcceso_Tick(object sender, EventArgs e)
        {
            labelEstadoLogin("ACCESO CORRECTO [" + temporizador.ToString() + "]", true);
            temporizador--;

            if (temporizador == -1)
            {
                temporizadorAcceso.Enabled = false;
                Close();
            }
        }

        private void frmLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!btnIngresarPresionado)
            {
                cerrado = true;
            }
        }
    }
}