using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProvaSemestral.Models;
using System.Data.SqlClient;

namespace ProvaSemestral.Controllers
{
    public class UsuarioController : Controller
    {

        [HttpPost]
        public void CriarUsuario([FromBody] Usuario usuario) {

            if (string.IsNullOrWhiteSpace(usuario.Nome) == true)
            {
                throw new ArgumentException("Nome");
            }
            if (string.IsNullOrWhiteSpace(usuario.Email) == true)
            {
                throw new ArgumentException("Email");
            }

            using (SqlConnection conn = Database.GetConn())
            {
                using (SqlCommand command = new SqlCommand(@"
                    INSERT INTO Usuario40006(Nome, Email)
                    VALUES(@Nome, @Email)", conn))
                {
                    command.Parameters.AddWithValue("@Nome", usuario.Nome);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.ExecuteNonQuery();
                }
            }
        }

        [HttpGet]
        public void ExcluirUsuario(int id) {
            using (SqlConnection conn = Database.GetConn())
            {
                using (SqlCommand command = new SqlCommand(@"
                    DELETE FROM Usuario40006
                    WHERE IdUsuario = @IdUsuario", conn))
                {
                    command.Parameters.AddWithValue("@IdUsuario", id);
                    command.ExecuteNonQuery();
                }
            }


        }

        [HttpGet]
        public Usuario ObterUsuarioPorId(int id) {
            using (SqlConnection conn = Database.GetConn())
            {
                using (SqlCommand command = new SqlCommand(@"
                    SELECT Nome, Email FROM Usuario40006
                    WHERE IdUsuario = @IdUsuario", conn))
                {

                    command.Parameters.AddWithValue("@IdUsuario", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Usuario usuario = null;

                        if (reader.Read() == true)
                        {
                                usuario = new Usuario()
                                {
                                usuario.IdUsuario = reader.GetInt32(0),
                                usuario.Nome = reader.GetString(1),
                                usuario.Email = reader.GetString(2)
                                };                          
                        }
                        return usuario;
                    }
                }
            }
        }

        [HttpPost]
        public void IncluirPreferenciasDoUsuario([FromBody] Preferencia[] preferencias) {
            for (int i = 0; i < preferencias.Length; i++)
            {
                if (preferencias[i].IdUsuario < 0)
                {
                    throw new ArgumentException("IdUsuario " + i);
                }
                if (preferencias[i].Valor < 0)
                {
                    throw new ArgumentException("Valor " + i);
                }
            }

            using (SqlConnection conn = Database.GetConn()){                           
                for (int i = 0; i < preferencias.Length; i++)
                {
                    using (SqlCommand command = new SqlCommand(@"
                        INSERT INTO Preferencia40006 (IdUsuario,  Valor)
                        VALUES (@IdUsuario, @Valor)", conn))
                    {
                        command.Parameters.AddWithValue("@IdUsuario", preferencias[i].IdUsuario);
                        command.Parameters.AddWithValue("@Valor", preferencias[i].Valor);
                        command.ExecuteNonQuery();
                    }
                }
            }

        }

        [HttpGet]
        public void ExcluirPreferenciasDoUsuario(int idUsuario) {
            using (SqlConnection conn = Database.GetConn())
            {
                using (SqlCommand command = new SqlCommand(@"
                    DELETE FROM Preferencia40006
                    WHERE IdUsuario = @IdUsuario", conn))
                {
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    command.ExecuteNonQuery();
                }
            }
        }

        [HttpGet]
        public List<Preferencia> ObterPreferenciasDoUsuario(int idUsuario) {
            using (SqlConnection conn = Database.GetConn())
            {
                using (SqlCommand command = new SqlCommand(@"
                    SELECT IdPreferencia, IdUsuario, Valor FROM Preferencia40006
                    WHERE IdUsuario = @IdUsuario", conn))
                {
                    command.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<Preferencia> lista = new List<Preferencia>();

                        while (reader.Read() == true)
                        {
                            lista.Add(new Preferencia()
                            {
                                IdPreferencia = reader.GetInt32(0),
                                IdUsuario = reader.GetInt32(1),
                                Valor = reader.GetInt32(2)
                            });
                        }
                        return lista;
                    }
                }
            }

        }


    }
}
