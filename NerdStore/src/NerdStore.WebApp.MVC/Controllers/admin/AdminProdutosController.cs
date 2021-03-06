using Microsoft.AspNetCore.Mvc;
using NerdStore.Catalogo.Application.Services;
using NerdStore.Catalogo.Application.ViewModels;
using System;
using System.Threading.Tasks;

namespace NerdStore.WebApp.MVC.Controllers.admin
{
    public class AdminProdutosController : Controller
    {
        private readonly IProdutoAppService _produtoAppService;
        public AdminProdutosController(IProdutoAppService produtoAppService)
        {
            _produtoAppService = produtoAppService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _produtoAppService.ObterTodos());
        }

        [HttpGet]
        [Route("novo-produto")]
        public async Task<IActionResult> NovoProduto()
        {
            return View(await PopularCategorias(new ProdutoViewModel()));
        }

        [Route("novo-produto")]
        [HttpPost]
        public async Task<IActionResult> NovoProduto(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return View(await PopularCategorias(produtoViewModel));

            await _produtoAppService.AdicionarProduto(produtoViewModel);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("atualizar-produto")]
        public async Task<IActionResult> AtualizarProduto(Guid id)
        {
            return View(await PopularCategorias(await _produtoAppService.ObterPorId(id)));
        }

        [Route("atualizar-produto")]
        [HttpPost]
        public async Task<IActionResult> AtualizarProduto(Guid id, ProdutoViewModel produtoViewModel)
        {
            var produto = await _produtoAppService.ObterPorId(id);
            produtoViewModel.QuantidadeEstoque = produto.QuantidadeEstoque;

            ModelState.Remove("QuantidadeEstoque");

            if (!ModelState.IsValid) return View(await PopularCategorias(produtoViewModel));

            await _produtoAppService.AtualizarProduto(produtoViewModel);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("produtos-atualizar-estoque")]
        public async Task<IActionResult> AtualizarEstoque(Guid guid)
        {
            return View("Estoque", await _produtoAppService.ObterPorId(guid));
        }



        [Route("produtos-atualizar-estoque")]
        [HttpPost]
        public async Task<IActionResult> AtualizarEstoque(Guid id, int quantidade)
        {
            if(quantidade > 0)
            {
                await _produtoAppService.ReporEstoque(id, quantidade);
            }
            else
            {
                await _produtoAppService.DebitarEstoque(id, quantidade);
            }

            return View("Index", await _produtoAppService.ObterTodos());
        }

        private async Task<ProdutoViewModel> PopularCategorias(ProdutoViewModel produto)
        {
            produto.Categorias = await _produtoAppService.ObterCategorias();
            return produto;
        }

    }
}
