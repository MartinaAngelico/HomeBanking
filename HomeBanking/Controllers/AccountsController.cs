using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;

namespace HomeBanking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    //hereda los controles
    {
        private IAccountRepository _accountRepository;
        //necesitamos un repositorio 

        public AccountsController(IAccountRepository accountRepository) //constructor
        {
            _accountRepository = accountRepository;
        }

        [HttpGet] //cuando hagamos un peticion de tipo get al controlador va a responder con el sgte metodo
        public IActionResult Get()
        {
            try
            {
                var accounts = _accountRepository.GetAllAccounts(); //el GEtallClients incluye las cuentas
                //con var no especificamos el tipo de dato
                var AccountDTO = new List<AccountDTO>(); //variable DTO xq no queremos mostrar todos los datos

                foreach (Account account in accounts) //recorremoss
                {
                    var newAccountDTO = new AccountDTO //creamos nuevos cliente DTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                        Transactions = account.Transactions.Select(tr => new TransactionDTO //SELECT metodo de linq para modificar datos
                        {
                            Id = tr.Id,
                            Type = tr.Type,
                            Amount = tr.Amount,
                            Description = tr.Description,
                            Date = tr.Date,
                        }).ToList()
                    };
                    AccountDTO.Add(newAccountDTO);
                }
                return Ok(AccountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var account = _accountRepository.FindById(id);
                if (account == null)
                {
                    return NotFound(); //no encontro el cliente
                }
                var accountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Balance = account.Balance,
                    Transactions = account.Transactions.Select(tr => new TransactionDTO //SELECT metodo de linq para modificar datos
                    {
                        Id = tr.Id,
                        Type = tr.Type,
                        Amount = tr.Amount,
                        Description = tr.Description,
                        Date = tr.Date,
                    }).ToList()
                };
                return Ok(accountDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
