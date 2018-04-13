using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Multas_tC.Models {
   public class Agentes {

      // criar o construtor
      public Agentes() {
         ListaMultas = new HashSet<Multas>();
      }

      [Key]
      public int ID { get; set; } // chave primária

      // dados do Agente
      [Required(ErrorMessage ="O {0} é de preenchimento obrigatório")]
      [RegularExpression("[A-ZÂÁÓÉÍ][a-záéíóúàèìòùâêîôûãõçäëöüïñ]+(( | e | de | da | das | do | d'|-)[A-ZÂÁÓÉÍ][a-záéíóúàèìòùâêîôûãõçäëöüïñ]+){1,3}",
            ErrorMessage ="o {0} só aceita letras. Cada nome deve começar por uma maiúscula seguida de minúsculas...")]
      [StringLength(40,ErrorMessage ="o {0} só aceita {1} carateres.")]
      public string Nome { get; set; }

      [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
      public string Fotografia { get; set; }

      // local de trabalho do Agente
      [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
      [RegularExpression ("[A-Za-z0-9áéíóúàèìòùâêîôûãõçäëöüïñ -]+",
             ErrorMessage ="Escreva um nome v]alido de uma esquadra...")]
      public string Esquadra { get; set; }

      // Criar uma lista de multas
      // aplicadas pelo Agente
      public virtual ICollection<Multas> ListaMultas { get; set; }


   }
}
