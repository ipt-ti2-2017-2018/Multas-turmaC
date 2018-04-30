using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Multas_tC.Models;

namespace Multas_tC.Controllers {
   public class AgentesController : Controller {

      //cria um objeto privado que 'referencia' a BD
      private MultasDb db = new MultasDb();

      // GET: Agentes
      public ActionResult Index() {

         // (LINQ) db.Agentes.ToList() --> 
         // em SQL: SELECT * FROM Agentes ORDER BY Nome
         // lista de agentes, presentes na BD

         var listaAgentes = db.Agentes.OrderBy(a => a.Nome).ToList();

         return View(listaAgentes);
      }







      // GET: Agentes/Details/5
      /// <summary>
      /// Apresenta numa listagem os dados de um agente
      /// </summary>
      /// <param name="id"> identifica o nº do agente a pesquisar </param>
      /// <returns></returns>
      public ActionResult Details(int? id) {

         // int? id  ---> o '?' informa que o parâmetro é de preenchimento
         //               facultativo

         // caso não haja ID, nada é feito
         if(id == null) {
            // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return RedirectToAction("Index");
         }

         // pesquisa os dados do Agente, cujo ID foi fornecido 
         Agentes agentes = db.Agentes.Find(id);

         // valida se foi encontrado algum Agente
         // se não foi encontrado, nada faz
         if(agentes == null) {
            // return HttpNotFound();
            return RedirectToAction("Index");
         }

         // apresenta na View os dados do Agente
         return View(agentes);
      }

      // GET: Agentes/Create
      public ActionResult Create() {
         return View();
      }

      // POST: Agentes/Create
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Create([Bind(Include = "Nome,Esquadra")] Agentes agente,
                                  HttpPostedFileBase carregaFotografia) {
         // gerar o ID do novo Agente
         int novoID = 0;
         if(db.Agentes.Count() != 0) {
            novoID = db.Agentes.Max(a => a.ID) + 1;
         }
         else {
            novoID = 1;
         }
         agente.ID = novoID; // atribuir o ID deste Agente
         // **************************************************
         // outra hipótese de validar a atribuição de ID
         // try { }
         // catch(Exception) { }
         // **************************************************

         // var auxiliar
         string nomeFicheiro = "Agente_" + novoID + ".jpg";
         string caminho = "";

         /// primeiro q tudo, há que garantir q existe imagem
         if(carregaFotografia != null) {
            // a imagem existe
            agente.Fotografia = nomeFicheiro;
            // definir o nome do ficheiro e o seu caminho
            caminho = Path.Combine(Server.MapPath("~/imagens/"), nomeFicheiro);
         }
         else {
            // não foi submetida uma imagem

            // gerar mensagem de erro, para elucidar o utilizador do erro
            ModelState.AddModelError("", "Não foi inserida uma imagem.");

            // redirecionar o utilizador para a View,
            // para que ele corriga os dados
            return View(agente);
         }
         /// formatar o tamanho da imagem ---> fazer em casa
         /// será q o ficheiro é uma imagem? ---> fazer em casa

         // ModelState.IsValid --> confronta os dados recebidos
         // como o modelo, para verificar se 
         // o que recebeu é o que deveria ter sido recebido
         if(ModelState.IsValid) {
            try {
               // adiciona o Agente à estrutura de dados
               db.Agentes.Add(agente);
               // efetuam um COMMIT à BD
               db.SaveChanges();
               // guardar o ficheio no disco rígido
               carregaFotografia.SaveAs(caminho);

               // redireciona o utilizador para a página do início
               return RedirectToAction("Index");
            }
            catch(Exception) {
               ModelState.AddModelError("", "Ocorreu um erro na criação do Agente '" + agente.Nome + "'.");
            }
         }

         // se aqui cheguei, é pq alguma coisa correu mal...
         // devolvo os dados do Agente à View
         return View(agente);
      }

      // GET: Agentes/Edit/5
      public ActionResult Edit(int? id) {
         if(id == null) {
            // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return RedirectToAction("Index");
         }
         Agentes agentes = db.Agentes.Find(id);
         if(agentes == null) {
            // return HttpNotFound();
            return RedirectToAction("Index");
         }
         return View(agentes);
      }



      // POST: Agentes/Edit/5
      // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
      // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
      /// <summary>
      /// Editar os dados de um Agente
      /// </summary>
      /// <param name="agente">dados do agente a editar</param>
      /// /// <param name="uploadFoto"> ficheiro com a nova fotografia (opcional) do Agente (.jpg) </param>
      /// <returns></returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult Edit([Bind(Include = "ID,Nome,Fotografia,Esquadra")] Agentes agente, HttpPostedFileBase uploadFoto) {
         /// a primeira ação a executar neste método é ajustar o nome da variável de entrada.
         /// 'agentes' é um nome criado automaticamente e reflete o nome da classe,
         /// mas como está no plural não é adequado, pois os dados referem-se a apenas um Agente

         /// como se pretende editar os dados de um Agente,
         /// tem de haver a hipótese de se editar a fotografia dele.
         /// Por esse motivo, é necessário adicionar aos parâmetros de entrada do método
         /// uma variável do tipo HttpPostedFileBase para receber o ficheiro da imagem.
         /// É igualmente necessário adicionar, na View, um objeto do tipo <input type="file" />
         /// para possibilitar a escolha da imagem a efetuar upload.
         /// O nome da variável no método do controller e na view tem de ser o mesmo.
         /// 
         /// De igual forma, é necessário alterar a forma como se configura o objeto do tipo <form />,
         /// responsável por enviar os dados do browser para o servidor,
         /// adicionando-lhe o parâmetro 'enctype = "multipart/form-data" '



         if(ModelState.IsValid) {
            // update
            db.Entry(agente).State = EntityState.Modified;
            // COMMIT
            db.SaveChanges();

            return RedirectToAction("Index");
         }
         return View(agente);
      }

      // GET: Agentes/Delete/5
      public ActionResult Delete(int? id) {
         if(id == null) {
            // return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            return RedirectToAction("Index"); // redireciona para a página de início
         }
         Agentes agentes = db.Agentes.Find(id);

         if(agentes == null) {
            // return HttpNotFound();
            return RedirectToAction("Index");
         }
         return View(agentes);
      }



      // POST: Agentes/Delete/5
      [HttpPost, ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public ActionResult DeleteNewMethod(int id) {
         Agentes agente = db.Agentes.Find(id);

         try {
            db.Agentes.Remove(agente);
            db.SaveChanges();
            return RedirectToAction("Index");
         }
         catch(Exception) {
            ModelState.AddModelError("", string.Format("aconteceu um erro com a eliminação do Agente '{0}', porque há multas associadas a ele.", agente.Nome));
         }
         // se aqui chego, é pq alguma coisa correu mal
         return View(agente);
      }

      protected override void Dispose(bool disposing) {
         if(disposing) {
            db.Dispose();
         }
         base.Dispose(disposing);
      }
   }
}
