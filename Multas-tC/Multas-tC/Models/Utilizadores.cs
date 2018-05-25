using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Multas_tC.Models {
   public class Utilizadores {
      /// <summary>
      /// adicionar atributos específicos de um utilizador
      /// - nome
      /// - apelido
      /// - data de nascimento
      /// </summary>
      /// 
      public int ID { get; set; }
      public string NomeProprio { get; set; }
      public string Apelido { get; set; }
      public DateTime? DataNascimento { get; set; }

      ///*********************************************
      /// o próximo atributo serve como chave forasteira
      /// para ligar os objetos desta classe à tabela de autenticação
      public string UserName { get; set; }

   }
}